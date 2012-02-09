using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Xml;
using EPS.Configuration;
using EPS.Reflection;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    //http://www.codeproject.com/KB/dotnet/mysteriesofconfiguration3.aspx
    /// <summary>   Collection of http context inspecting authenticator configuration elements. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticatorConfigurationElementDictionary : 
        ConfigurationElementDictionary<string, AuthenticatorConfigurationElement>
    {
        private static readonly Dictionary<string, AuthenticatorConfigurationElement> derivedElements 
            = new Dictionary<string, AuthenticatorConfigurationElement>();

        /// <summary>   Gets the type of the collection -- and AddRemoveClearMap. </summary>
        /// <value> The type of the collection. </value>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>   Gets the name of the config element - "inspectors". </summary>
        /// <value> "inspectors" - the name of the element. </value>
        protected override string ElementName
        {
            get { return "inspectors"; }
        }

        /// <summary>   Executes the deserialize unrecognized element action. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when the factory types specified are not correct or cannot be instantiated. </exception>
        /// <param name="elementName">  Name of the element. </param>
        /// <param name="reader">       The reader. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (null == reader) { throw new ArgumentNullException("reader"); }

            //verify that this element is OK and has a valid factory spec'd
            if (AddElementName == elementName)
            {
                string factoryTypeName = reader.GetAttribute("factory");
                Type factoryType = Type.GetType(factoryTypeName, true, true);
                if (null == factoryType)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] cannot be found - check configuration settings", factoryTypeName ?? string.Empty));
                }

                var genericTypeParameter = typeof(IAuthenticatorFactory<>).GetGenericInterfaceTypeParameters(factoryType).ToList();
                if (genericTypeParameter.Count == 0)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must implement interface {1} - check configuration settings", factoryTypeName ?? string.Empty, typeof(IAuthenticatorFactory<>).Name));
                }

                //this automatically throws when there's no default parameterless constructor
                //just create an instance and see what happens ;0
                Activator.CreateInstance(factoryType);

                //principalBuilderFactory is not required, but we can validate it if specified
                string principalBuilderFactoryTypeName = reader.GetAttribute("principalBuilderFactory");
                if (!string.IsNullOrWhiteSpace(principalBuilderFactoryTypeName))
                {
                    Type principalBuilderFactoryType = Type.GetType(principalBuilderFactoryTypeName, true, true);
                    if (null != principalBuilderFactoryType)
                    {
                        Type principalBuilderFactoryInterface = typeof(IPrincipalBuilderFactory);
                        if (!principalBuilderFactoryInterface.IsAssignableFrom(principalBuilderFactoryType))
                        {
                            throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, 
                                "The principal builder factory type specified [{0}] must implement interface {1} - check configuration settings", 
                                principalBuilderFactoryTypeName ?? string.Empty, principalBuilderFactoryInterface.Name));
                        }

                        //this automatically throws when there's no default parameterless constructor
                        //just create an instance and see what happens ;0
                        Activator.CreateInstance(principalBuilderFactoryType);
                    }
                }

                //Step 1 -- create the *real* configuration elementName type we need here, caching it, 
                //let the config system deserialize the same xml chunk
                //and it will pop in a standard HttpContextInspectingAuthenticatorConfigurationElement
                //which we will swap out later (by key)
                Type specializedConfigurationElementType = genericTypeParameter[0];
                var configElement = (AuthenticatorConfigurationElement)specializedConfigurationElementType
                    .GetConstructor(Type.EmptyTypes).Invoke(null);
                configElement.Init();
                configElement.DeserializeElement(reader, false);
                derivedElements.Add(configElement.Name, configElement);
            }
            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        //HACK: this is really hacky, and there might be another way, but the config system is *weird*
        /// <summary>   Creates a new element. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="elementName">  Name of the element. </param>
        /// <returns>   An existing element if one exists with the given named, or else calls base class CreateNewElement. </returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            //Step 2 -- config system recreates elements and rebuilds the collection for some reason
            //when it asks by name / key, we give it a cached one
            if (derivedElements.ContainsKey(elementName))
                return derivedElements[elementName];
            
            return base.CreateNewElement(elementName);
        }

        /// <summary>   Gets the elements key, in this case, its name. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="element">  The configuration element. </param>
        /// <returns>   The element key. </returns>
        public override string GetElementKey(AuthenticatorConfigurationElement element)
        {
            if (null == element) { throw new ArgumentNullException("element"); }

            return element.Name;
        }        
    }
}