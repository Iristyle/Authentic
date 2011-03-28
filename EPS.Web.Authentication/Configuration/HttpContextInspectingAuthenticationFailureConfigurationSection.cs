using System;
using System.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Http context inspecting authentication failure configuration section. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class HttpContextInspectingAuthenticationFailureConfigurationSection : 
        ConfigurationSection, 
        IHttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Sets the <see cref="T:System.Configuration.ConfigurationElement" /> object to its initial state.
        /// 			Necessary to mimic the deserialization process from the collection of these guys. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        public new void Init()
        {
            base.Init();
        }

        /// <summary>   
        /// Reads XML from the configuration file.  Calls base class implementation.  Necessary to mimic the deserialization process from the
        /// collection of these guys. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="reader">                   The <see cref="T:System.Xml.XmlReader" /> that reads from the configuration file. </param>
        /// <param name="serializeCollectionKey">   true to serialize only the collection key properties; otherwise, false. </param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">  The element to read is locked.- or -An attribute of the
        ///                                                                                 current node is not recognized.- or -The lock status
        ///                                                                                 of the current node cannot be determined. </exception>
        public new void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }

        /// <summary>   
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization. This allows the use of types derived from
        /// HttpContextInspectingAuthenticationFailureConfigurationSection. Adds new ConfigurationProperty to the base class properties. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="name">     The name of the unrecognized attribute. </param>
        /// <param name="value">    The value of the unrecognized attribute. </param>
        /// <returns>   Always returns true (unknown attribute is encountered while deserializing) </returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), value);
            Properties.Add(property);
            base[property] = value;
            //this.Parameters[name] = value;
            return true;
        }

        /// <summary>   Gets or sets a value indicating whether SSL is required. </summary>
        /// <value> true if require SSL, false if not. </value>
        [ConfigurationProperty("requireSsl", DefaultValue = false, IsRequired = false)]
        public bool RequireSsl
        {
            get { return (bool)this["requireSsl"]; }
            set { base["requireSsl"] = value; }
        }
    }
}