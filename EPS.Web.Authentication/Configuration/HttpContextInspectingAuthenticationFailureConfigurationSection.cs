using System;
using System.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    public class HttpContextInspectingAuthenticationFailureConfigurationSection : ConfigurationSection
    {
        //these two methods are needed to mimic the deserialization process from the collection of these guys
        public new void Init()
        {
            base.Init();
        }

        public new void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }

        //this allows us to use types derived from HttpContextInspectingAuthenticationFailureConfigurationSection
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), value);
            Properties.Add(property);
            base[property] = value;
            //this.Parameters[name] = value;
            return true;
        }

        [ConfigurationProperty("requireSSL", DefaultValue = false, IsRequired = false)]
        public bool RequireSSL
        {
            get { return (bool)this["requireSSL"]; }
            set { base["requireSSL"] = value; }
        }
    }
}
