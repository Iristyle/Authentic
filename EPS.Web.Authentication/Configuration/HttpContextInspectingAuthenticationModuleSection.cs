using System;
using System.Configuration;
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
             requireSSL="true"
             principalBuilderFactory="EPS.Web.Authentication.Corporate.Basic.BasicCredentialsOnyxPrincipalBuilder"
             customConfigurationSection="EPS.Web.Authentication/corporateAuthentication" />
        <add name="corporateTokenAuth" factory="EPS.Web.Authentication.Corporate.Token.TokenCookieInspectingAuthenticatorFactory"
             requireSSL="true" customConfigurationSection="EPS.Web.Authentication/corporateAuthentication" />
        <add name="onyxAuth" factory="EPS.Web.Authentication.Onyx.OnyxCookieInspectingAuthenticatorFactory" requireSSL="false"
             customConfigurationSection="EPS.Web.Authentication/onyxAuthentication" />
      </inspectors>
    </httpContextAuthentication>
    <basicAuthenticationFailure realm="EPS Hub Services" requireSSL="true" />
    */

    public class HttpContextInspectingAuthenticationModuleSection : ConfigurationSection
    {
        //for building / checking the type name of the failure handler
        //private IHttpContextInspectingAuthenticationFailureHandlerFactory<HttpContextInspectingAuthenticationFailureConfigurationSection> factoryInstance = null;
        private IHttpContextInspectingAuthenticationFailureHandlerFactory factoryInstance = null;
        private HttpContextInspectingAuthenticationFailureConfigurationSection customFailureConfigurationSection = null;

        protected override void PostDeserialize()
        {
            base.PostDeserialize();


            if (Enabled && Roles.Enabled)
                throw new ConfigurationErrorsException("To enable custom HTTP Header Authentication with the <httpContextAuthentication> and the enabled=\"true\" setting, the <roleManager> must be set to enabled=\"false\"");

            if (Enabled && Inspectors.Count == 0)
                throw new ConfigurationErrorsException("There must be at least one inspector in the <inspectors> section under the <httpContextAuthentication> configuration element");

            if (!string.IsNullOrEmpty(FailureHandlerFactoryName))
            {
                var t = Type.GetType(FailureHandlerFactoryName);
                if (null == t)
                    throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] cannot be found - check configuration settings", FailureHandlerFactoryName));

                if (!typeof(IHttpContextInspectingAuthenticationFailureHandlerFactory<>).IsGenericInterfaceAssignableFrom(t))
                    throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] must implement interface {1} - check configuration settings", FailureHandlerFactoryName, typeof(IHttpContextInspectingAuthenticationFailureHandlerFactory<>).Name));

                var c = t.GetConstructor(Type.EmptyTypes);
                if (null == c)
                    throw new ConfigurationErrorsException(String.Format("The factory type specified [{0}] must have a parameterless constructor - check configuration settings", FailureHandlerFactoryName));

                //TODO: 5-17-2010 - custom configuration section checking is deferred until runtime as it hasn't been deserialized yet... i don't think
            }
        }


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

        [ConfigurationProperty("failureHandlerFactory", DefaultValue = "", IsRequired = false)]
        public string FailureHandlerFactoryName
        {
            get { return (string)base["failureHandlerFactory"]; }
            set { base["failureHandlerFactory"] = value; }
        }

        public IHttpContextInspectingAuthenticationFailureHandler GetFailureHandler()
        {
            if (string.IsNullOrEmpty(FailureHandlerFactoryName))
                return null;

            if (null == factoryInstance)
                factoryInstance = (IHttpContextInspectingAuthenticationFailureHandlerFactory)Activator.CreateInstance(Type.GetType(FailureHandlerFactoryName));

            return factoryInstance.Construct(CustomFailureHandlerConfigurationSection);
        }


        [ConfigurationProperty("customFailureHandlerConfigurationSection", DefaultValue = "")]
        public string CustomFailureHandlerConfigurationSectionName
        {
            get { return (string)this["customFailureHandlerConfigurationSection"]; }
        }

        public HttpContextInspectingAuthenticationFailureConfigurationSection CustomFailureHandlerConfigurationSection
        {
            get
            {
                //TOOD: 5-5-2010 -- unfortunately this defers this error until runtime rather than config parse time
                //see if we can fix that
                if (null == customFailureConfigurationSection && !string.IsNullOrEmpty(CustomFailureHandlerConfigurationSectionName))
                    try
                    {
                        customFailureConfigurationSection = (HttpContextInspectingAuthenticationFailureConfigurationSection)CurrentConfiguration.GetSection(CustomFailureHandlerConfigurationSectionName);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(String.Format("The custom configuration section specified by \"configurationSection\" [{0}] must exist - check configuration settings", CustomFailureHandlerConfigurationSectionName), ex);
                    }

                return customFailureConfigurationSection;
            }
        }      

        [ConfigurationProperty("inspectors", IsRequired = true)]
        [ConfigurationCollection(typeof(HttpContextInspectingAuthenticatorConfigurationElementCollection))]
        public HttpContextInspectingAuthenticatorConfigurationElementCollection Inspectors
        {
            get { return (HttpContextInspectingAuthenticatorConfigurationElementCollection)base["inspectors"]; }
        }
    }

}
