using System;
using System.Configuration;
using System.Xml;
using EPS.Reflection;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    public class HttpContextInspectingAuthenticatorConfigurationElement : ConfigurationElement
    {
        //private IHttpContextInspectingAuthenticatorFactory<HttpContextInspectingAuthenticatorConfigurationElement> factoryInstance = null;
        private ConfigurationSection customConfigurationSection = null;
        private IHttpContextInspectingAuthenticatorFactory factoryInstance;

        //these two methods are needed to mimic the deserialization process from the collection of these guys
        public new void Init()
        {
            base.Init();
        }

        public new void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }

        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            //simple verification of info supplied in config -- not used for anything (yet)
            var t = Type.GetType(Factory);
            if (null == t)
                throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] cannot be found - check configuration settings", Factory ?? string.Empty));

            if (!typeof(IHttpContextInspectingAuthenticatorFactory<>).IsGenericInterfaceAssignableFrom(t))
                throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] must implement interface {1} - check configuration settings", Factory ?? string.Empty, typeof(IHttpContextInspectingAuthenticatorFactory<>).Name));

            var c = t.GetConstructor(Type.EmptyTypes);
            if (null == c)
                throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] must have a parameterless constructor - check configuration settings", Factory ?? string.Empty));
        }

        //this allows us to use types derived from HttpContextInspectingAuthenticatorConfigurationElement
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {                    
            ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), value);
            Properties.Add(property);
            base[property] = value;            
            //this.Parameters[name] = value;
            return true;
        }

        [ConfigurationProperty("roleProviderName", IsRequired = false, DefaultValue = "")]
        public string RoleProviderName
        {
            get { return (string)this["roleProviderName"]; }
            set { this["roleProviderName"] = value; }
        }

        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("factory", IsRequired = true)]
        public string Factory
        {
            get { return (string)this["factory"]; }
            set { this["factory"] = value; }
        }

        public IHttpContextInspectingAuthenticator GetInspector()
        {
            //TODO: 5-21-2010 -- theres a bug here -- dynamic runtime can't find the appropriate Construct method for some reason
            //it *should* work, but doesn't
            //HACK: rather than deal with some funky reflection code, we just use dynamic, since we know there is a Construct here
            //dynamic factoryInstance = Activator.CreateInstance(Type.GetType(Factory));
            
            //http://stackoverflow.com/questions/266115/pass-an-instantiated-system-type-as-a-type-parameter-for-a-generic-class
            //var t = typeof(IHttpContextInspectingAuthenticatorFactory<>).MakeGenericType(this.GetType());

            if (null == factoryInstance)
                factoryInstance = (IHttpContextInspectingAuthenticatorFactory)Activator.CreateInstance(Type.GetType(Factory));
            return factoryInstance.Construct(this);
        }

        [ConfigurationProperty("requireSSL", DefaultValue = true)]
        public bool RequireSSL
        {
            get { return (bool)this["requireSSL"]; }
            set { this["requireSSL"] = value; }
        }

        [ConfigurationProperty("customConfigurationSection", DefaultValue = "")]
        public string CustomConfigurationSectionName
        {
            get { return (string)this["customConfigurationSection"]; }
        }

        public ConfigurationSection CustomConfigurationSection
        {
            get 
            {
                //TOOD: 5-5-2010 -- unfortunately this defers this error until runtime rather than config parse time
                //see if we can fix that
                if (null == customConfigurationSection && !string.IsNullOrEmpty(CustomConfigurationSectionName))
                    try
                    {
                        customConfigurationSection = CurrentConfiguration.GetSection(CustomConfigurationSectionName);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(String.Format("The custom configuration section specified by \"configurationSection\" [{0}] must exist - check configuration settings", CustomConfigurationSectionName), ex);
                    }

                return customConfigurationSection; 
            }
        }

        /*
        [ConfigurationProperty("cachingEnabled", DefaultValue = true)]
        public bool CachingEnabled
        {
            get { return (bool)this["cachingEnabled"]; }
            set { base["cachingEnabled"] = value; }
        }

        [ConfigurationProperty("cachingDuration", DefaultValue = 15)]
        [IntegerValidator(MinValue = 1, MaxValue = 60)]
        public int CachingDuration
        {
            get { return (int)this["cachingDuration"]; }
            set { base["cachingDuration"] = value; }
        }
        */
    }
}
