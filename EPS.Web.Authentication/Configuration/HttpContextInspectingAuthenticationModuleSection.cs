using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.Security;
using EPS.Reflection;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    /*
     * <EPS.Web.Authentication>
    <httpContextAuthentication enabled="true">
      <!--  failureHandlerFactory="EPS.Web.Authentication.BasicAuthenticationFailureHandlerFactory"
            customFailureHandlerConfigurationSection="EPS.Web.Authentication/basicAuthenticationFailure" -->
      <inspectors>
        <add name="corporateBasicAuth" factory="EPS.Web.Authentication.BasicAuthenticationInspectingAuthenticatorFactory"
             requireSsl="true"
             principalBuilderFactory="EPS.Web.Authentication.Corporate.Basic.BasicCredentialsOnyxPrincipalBuilder"
             customConfigurationSection="EPS.Web.Authentication/corporateAuthentication" />
        <add name="corporateTokenAuth" factory="EPS.Web.Authentication.Corporate.Token.TokenCookieInspectingAuthenticatorFactory"
             requireSsl="true" customConfigurationSection="EPS.Web.Authentication/corporateAuthentication" />
        <add name="onyxAuth" factory="EPS.Web.Authentication.Onyx.OnyxCookieInspectingAuthenticatorFactory" requireSsl="false"
             customConfigurationSection="EPS.Web.Authentication/onyxAuthentication" />
      </inspectors>
    </httpContextAuthentication>
    <basicAuthenticationFailure realm="EPS Hub Services" requireSsl="true" />
    */

    /// <summary>   The top-level ConfigurationSection used to setup the Http context inspecting authenticators. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class HttpContextInspectingAuthenticationModuleSection : ConfigurationSection
    {
        private HttpContextInspectingAuthenticationFailureConfigurationSection customFailureConfigurationSection = null;

        /// <summary>   Inspects the configuration specified and throws errors where configuration is invaild. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when there are any number of configuration errors. </exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "roleManager", Justification = "Name of configuration element / attribute"),
        SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "httpContextAuthentication", Justification = "Name of configuration element / attribute")]
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            if (Enabled && Roles.Enabled)
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "To enable custom HTTP Header Authentication with the <httpContextAuthentication> and the enabled=\"true\" setting, the <roleManager> must be set to enabled=\"false\""));

            if (Enabled && Inspectors.Count == 0)
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "There must be at least one inspector in the <inspectors> section under the <httpContextAuthentication> configuration element"));

            if (!string.IsNullOrEmpty(FailureHandlerFactoryName))
            {
                var t = Type.GetType(FailureHandlerFactoryName);
                if (null == t)
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] cannot be found - check configuration settings", FailureHandlerFactoryName));

                if (!typeof(IHttpContextInspectingAuthenticationFailureHandlerFactory<>).IsGenericInterfaceAssignableFrom(t))
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must implement interface {1} - check configuration settings", FailureHandlerFactoryName, typeof(IHttpContextInspectingAuthenticationFailureHandlerFactory<>).Name));

                var c = t.GetConstructor(Type.EmptyTypes);
                if (null == c)
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must have a parameterless constructor - check configuration settings", FailureHandlerFactoryName));

                //TODO: 5-17-2010 - custom configuration section checking is deferred until runtime as it hasn't been deserialized yet... i don't think
            }
        }

        /// <summary>   Gets or sets a value indicating whether the authentication module is enabled. </summary>
        /// <value> true if enabled, false if not. </value>
        [ConfigurationProperty("enabled", DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }

        /* TODO: I see no reason to use this
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

        /// <summary>   Gets or sets the name of the failure handler factory. </summary>
        /// <value> The name of the failure handler factory. </value>
        [ConfigurationProperty("failureHandlerFactory", DefaultValue = "", IsRequired = false)]
        public string FailureHandlerFactoryName
        {
            get { return (string)base["failureHandlerFactory"]; }
            set { base["failureHandlerFactory"] = value; }
        }

        /// <summary>   Gets the name of the custom failure handler configuration section. </summary>
        /// <value> The name of the custom failure handler configuration section. </value>
        [ConfigurationProperty("customFailureHandlerConfigurationSection", DefaultValue = "")]
        public string CustomFailureHandlerConfigurationSectionName
        {
            get { return (string)this["customFailureHandlerConfigurationSection"]; }
        }

        /// <summary>   Gets the custom failure handler configuration section. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when there are configuration errors. </exception>
        /// <returns>   The custom failure handler configuration section. </returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "customFailureHandlerConfigurationSection", Justification = "Name of configuration element / attribute"), 
        SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is not suitable for a property as configuration is inspected and exceptions may be thrown")]
        public HttpContextInspectingAuthenticationFailureConfigurationSection GetCustomFailureHandlerConfigurationSection()
        {
            //TOOD: 5-5-2010 -- unfortunately this defers this error until runtime rather than config parse time
            //see if we can fix that
            if (null == customFailureConfigurationSection && !string.IsNullOrEmpty(CustomFailureHandlerConfigurationSectionName))
            {
                try
                {
                    customFailureConfigurationSection = (HttpContextInspectingAuthenticationFailureConfigurationSection)CurrentConfiguration.GetSection(CustomFailureHandlerConfigurationSectionName);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The custom configuration section specified by \"customFailureHandlerConfigurationSection\" [{0}] must exist - check configuration settings", CustomFailureHandlerConfigurationSectionName), ex);
                }
            }
            
            return customFailureConfigurationSection;
        }

        /// <summary>   Gets the collection of defined inspectors. </summary>
        /// <value> The inspectors. </value>
        [ConfigurationProperty("inspectors", IsRequired = true)]
        [ConfigurationCollection(typeof(HttpContextInspectingAuthenticatorConfigurationElementCollection))]
        public HttpContextInspectingAuthenticatorConfigurationElementCollection Inspectors
        {
            get { return (HttpContextInspectingAuthenticatorConfigurationElementCollection)base["inspectors"]; }
        }
    }
}
