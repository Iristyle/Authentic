using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{

    public class BasicAuthenticationHeaderInspectorConfigurationElement : HttpContextInspectingAuthenticatorConfigurationElement
    {
        private IBasicAuthPrincipalBuilderFactory principalBuilderFactoryInstance = null;

        /// <summary>
        /// By default no membership provider is used as it may be just as costly as extracting a principal.
        /// If a simpler membership provider exists that can provide a faster validation of user credentials than a full IPrincipal extraction,
        /// then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured MembershipProvider for the system.
        /// </summary>
        [ConfigurationProperty("providerName", DefaultValue = "")]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
            set { base["providerName"] = value; }
        }

        [ConfigurationProperty("principalBuilderFactory", DefaultValue="")]
        public string PrincipalBuilderFactory
        {
            get { return (string)this["principalBuilderFactory"]; }
            set { this["principalBuilderFactory"] = value; }
        }

        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            if (!string.IsNullOrEmpty(PrincipalBuilderFactory))
            {
                var type = Type.GetType(PrincipalBuilderFactory);
                if (null == type)
                    throw new ConfigurationErrorsException(String.Format("The principalBuilderFactory type name specified [{0}] cannot be found - check configuration settings", PrincipalBuilderFactory ?? string.Empty));

                if (!typeof(IBasicAuthPrincipalBuilderFactory).IsAssignableFrom(type))                
                    throw new ConfigurationErrorsException(String.Format("The principalBuilderFactory type name specified [{0}] must implement interface {1} - check configuration settings", PrincipalBuilderFactory ?? string.Empty, typeof(IBasicAuthPrincipalBuilderFactory).Name));

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (null == constructor)
                    throw new ConfigurationErrorsException(String.Format("The principalBuilderFactory type name specified [{0}] must have a parameterless constructor - check configuration settings", PrincipalBuilderFactory ?? string.Empty));
            }
        }

        public IBasicAuthPrincipalBuilder GetPrincipalBuilder()
        {
            if (string.IsNullOrEmpty(PrincipalBuilderFactory))
                return null;

            if (null == principalBuilderFactoryInstance)
            {
                principalBuilderFactoryInstance = Activator.CreateInstance(Type.GetType(PrincipalBuilderFactory)) as IBasicAuthPrincipalBuilderFactory;
            }

            return principalBuilderFactoryInstance.Construct(this);
        }
    }
}
