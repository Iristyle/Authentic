using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using EPS.Reflection;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   The basic configuration definition for a HttpContextBase inspecting authenticator configuration element. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class HttpContextInspectingAuthenticatorConfigurationElement : 
        ConfigurationElement, 
        IHttpContextInspectingAuthenticatorConfigurationElement
    {
        private IPrincipalBuilderFactory principalBuilderFactoryInstance;

        //private IHttpContextInspectingAuthenticatorFactory<HttpContextInspectingAuthenticatorConfigurationElement> factoryInstance = null;
        //private ConfigurationSection customConfigurationSection;        

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
        public new void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }

        /// <summary>   Called after deserialization has completed, verifying specified configuration information. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when the Factory or PrincipalBuilderFactory are improperly configured. </exception>        
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "principalBuilderFactory", Justification = "Name of configuration element / attribute")]
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            //simple verification of info supplied in config -- not used for anything (yet)
            var t = Type.GetType(Factory);
            if (null == t)
            {
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] cannot be found - check configuration settings", Factory ?? string.Empty));
            }

            if (!typeof(IHttpContextInspectingAuthenticatorFactory<>).IsGenericInterfaceAssignableFrom(t))
            {
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must implement interface {1} - check configuration settings", Factory ?? string.Empty, typeof(IHttpContextInspectingAuthenticatorFactory<>).Name));
            }

            var c = t.GetConstructor(Type.EmptyTypes);
            if (null == c)
            {
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must have a parameterless constructor - check configuration settings", Factory ?? string.Empty));
            }

            if (!string.IsNullOrEmpty(PrincipalBuilderFactory))
            {
                var type = Type.GetType(PrincipalBuilderFactory);
                if (null == type)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] cannot be found - check configuration settings", PrincipalBuilderFactory ?? string.Empty));
                }

                if (!typeof(IPrincipalBuilderFactory).IsAssignableFrom(type))
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must implement interface {1} - check configuration settings", PrincipalBuilderFactory ?? string.Empty, typeof(IPrincipalBuilderFactory).Name));
                }

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (null == constructor)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must have a parameterless constructor - check configuration settings", PrincipalBuilderFactory ?? string.Empty));
                }
            }
        }

        /// <summary>   
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization. This allows the use of types derived from
        /// HttpContextInspectingAuthenticatorConfigurationElement. Adds new ConfigurationProperty to the base class properties. 
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

        /// <summary>   Gets or sets the name of the role provider used to validate the principal. </summary>
        /// <value> The name of the role provider. </value>
        [ConfigurationProperty("roleProviderName", IsRequired = false, DefaultValue = "")]
        public string RoleProviderName
        {
            get { return (string)this["roleProviderName"]; }
            set { this["roleProviderName"] = value; }
        }

        /// <summary>   Gets or sets the human-friendly name / key for this inspector. </summary>
        /// <value> The name. </value>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>   Gets or sets the FullName of the factory class Type. </summary>
        /// <value> The factory. </value>
        [ConfigurationProperty("factory", IsRequired = true)]
        public string Factory
        {
            get { return (string)this["factory"]; }
            set { this["factory"] = value; }
        }

        /// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
        /// <value> true if require SSL, false if not. </value>
        [ConfigurationProperty("requireSsl", DefaultValue = true)]
        public bool RequireSsl
        {
            get { return (bool)this["requireSsl"]; }
            set { this["requireSsl"] = value; }
        }

        /// <summary>   Gets the name of the custom configuration section that will be loaded and passed on to clients of this class. </summary>
        /// <value> The name of the custom configuration section. </value>
        [ConfigurationProperty("customConfigurationSection", DefaultValue = "")]
        public string CustomConfigurationSectionName
        {
            get { return (string)this["customConfigurationSection"]; }
        }

        /// <summary>   
        /// Get or sets the name of the MembershipProvider to be used.  By default no membership provider is used as it may be just as costly as
        /// extracting a principal. If a simpler membership provider exists that can provide a faster validation of user credentials than a full
        /// IPrincipal extraction, then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured
        /// MembershipProvider for the system. 
        /// </summary>
        /// <value> The name of the provider. </value>
        [ConfigurationProperty("providerName", DefaultValue = "")]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
            set { base["providerName"] = value; }
        }

        /// <summary>   Gets or sets the principal builder factory Type name. </summary>
        /// <value> 
        /// The FullName for the type of the principal builder factory -- i.e. the class that implements <see cref="T:
        /// EPS.Web.Abstractions.IPrincipalBuilderFactory"/>. 
        /// </value>
        [ConfigurationProperty("principalBuilderFactory", DefaultValue = "")]
        public string PrincipalBuilderFactory
        {
            get { return (string)this["principalBuilderFactory"]; }
            set { this["principalBuilderFactory"] = value; }
        }

        /// <summary>   
        /// Gets the principal builder factory instance implementing <see cref="T:
        /// EPS.Web.Abstractions.IPrincipalBuilderFactory"/>, and uses that to create instances of <see cref="T:
        /// EPS.Web.Abstractions.IPrincipalBuilder"/> given the specified configuration. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <returns>   The principal builder instance or null if the PrincipalBuilderFactory property is not properly configured. </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is not suitable for a property as it returns new IBasicAuthPrincipalBuilder instances each time")]
        public IPrincipalBuilder GetPrincipalBuilder()
        {
            if (string.IsNullOrEmpty(PrincipalBuilderFactory))
            {
                return null;
            }

            if (null == principalBuilderFactoryInstance)
            {
                principalBuilderFactoryInstance = Activator.CreateInstance(Type.GetType(PrincipalBuilderFactory)) as IPrincipalBuilderFactory;
            }

            return principalBuilderFactoryInstance.Construct(this);
        }

        //TODO: 3-28-2011 -- consider moving this out to its own class
        /*
        /// <summary>   Gets the custom configuration section. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when there are configuration errors. </exception>
        /// <returns>   The custom configuration section. </returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "configurationSection", Justification = "Name of configuration element / attribute"), 
        SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is not suitable for a property as configuration is inspected and exceptions may be thrown")]
        public ConfigurationSection GetCustomConfigurationSection()
        {
            //TOOD: 5-5-2010 -- unfortunately this defers this error until runtime rather than config parse time
            //see if we can fix that
            if (null == customConfigurationSection && !string.IsNullOrEmpty(CustomConfigurationSectionName))
            {
                try
                {
                    customConfigurationSection = CurrentConfiguration.GetSection(CustomConfigurationSectionName);
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The custom configuration section specified by \"configurationSection\" [{0}] must exist - check configuration settings", CustomConfigurationSectionName), ex);
                }
            }

            return customConfigurationSection;
        }
        */

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