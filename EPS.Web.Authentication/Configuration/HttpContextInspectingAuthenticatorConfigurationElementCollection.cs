using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using EPS.Configuration;
using EPS.Reflection;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    //http://www.codeproject.com/KB/dotnet/mysteriesofconfiguration3.aspx
    public class HttpContextInspectingAuthenticatorConfigurationElementCollection : 
        ConfigurationElementCollection<string, HttpContextInspectingAuthenticatorConfigurationElement>
    {
        private static Dictionary<string, HttpContextInspectingAuthenticatorConfigurationElement> derivedElements = new Dictionary<string, HttpContextInspectingAuthenticatorConfigurationElement>();

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override string ElementName
        {
            get { return "inspectors"; }
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            //verify that this element is OK and has a valid factory spec'd
            if (AddElementName == elementName)
            {
                string factoryTypeName = reader.GetAttribute("factory");
                Type factoryType = Type.GetType(factoryTypeName, true, true);
                if (null == factoryType)
                    throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] cannot be found - check configuration settings", factoryTypeName ?? string.Empty));

                var genericTypeParameter = typeof(IHttpContextInspectingAuthenticatorFactory<>).GetGenericInterfaceTypeParameters(factoryType).ToList();
                if (genericTypeParameter.Count == 0)
                    throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] must implement interface {1} - check configuration settings", factoryTypeName ?? string.Empty, typeof(IHttpContextInspectingAuthenticatorFactory<>).Name));

                //this automatically throws when there's no default parameterless constructor
                //just create an instance and see what happens ;0
                var testInstance = Activator.CreateInstance(factoryType);

                //Step 1 -- create the *real* configuration elementName type we need here, caching it, 
                //let the config system deserialize the same xml chunk
                //and it will pop in a standard HttpContextInspectingAuthenticatorConfigurationElement
                //which we will swap out later (by key)
                Type specializedConfigurationElementType = genericTypeParameter[0];
                var configElement = (HttpContextInspectingAuthenticatorConfigurationElement)specializedConfigurationElementType
                    .GetConstructor(Type.EmptyTypes).Invoke(null);
                configElement.Init();
                configElement.DeserializeElement(reader, false);                
                derivedElements.Add(configElement.Name, configElement);
            }
            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        //HACK: this is really hacky, and there might be another way, but the config system is *weird*
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            //Step 2 -- config system recreates elements and rebuilds the collection for some reason
            //when it asks by name / key, we give it a cached one
            if (derivedElements.ContainsKey(elementName))
                return derivedElements[elementName];
            
            return base.CreateNewElement(elementName);
        }

        public override string GetElementKey(HttpContextInspectingAuthenticatorConfigurationElement element)
        {
            return element.Name;
        }

        //use the specified type names to create actual inspector instances
        public IEnumerable<IHttpContextInspectingAuthenticator> GetInspectors()
        {
            return this.OfType<HttpContextInspectingAuthenticatorConfigurationElement>().Select(i => i.GetInspector());
        }
    }
}
