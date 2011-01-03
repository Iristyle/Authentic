using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   
    /// Basic authentication header inspector configuration element that defines which MembershipProvider to use for authentication and which
    /// PrincipalBuilderFactory to use (class that implements <see cref="T:
    /// EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class BasicAuthenticationHeaderInspectorConfigurationElement : HttpContextInspectingAuthenticatorConfigurationElement
    {
        private IBasicAuthPrincipalBuilderFactory principalBuilderFactoryInstance = null;

        /// <summary>   
        /// By default no membership provider is used as it may be just as costly as extracting a principal. If a simpler membership provider
        /// exists that can provide a faster validation of user credentials than a full IPrincipal extraction, then it makes sense to use a
        /// membershipProvider.  Specify 'default' to use the default configured MembershipProvider for the system. 
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
        /// EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory"/>. 
        /// </value>
        [ConfigurationProperty("principalBuilderFactory", DefaultValue="")]
        public string PrincipalBuilderFactory
        {
            get { return (string)this["principalBuilderFactory"]; }
            set { this["principalBuilderFactory"] = value; }
        }

        /// <summary>   Validates the deserialized configuration values. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when the PrincipalBuilderFactory is improperly configured. </exception>
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            if (!string.IsNullOrEmpty(PrincipalBuilderFactory))
            {
                var type = Type.GetType(PrincipalBuilderFactory);
                if (null == type)
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] cannot be found - check configuration settings", PrincipalBuilderFactory ?? string.Empty));

                if (!typeof(IBasicAuthPrincipalBuilderFactory).IsAssignableFrom(type))                
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must implement interface {1} - check configuration settings", PrincipalBuilderFactory ?? string.Empty, typeof(IBasicAuthPrincipalBuilderFactory).Name));

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (null == constructor)
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must have a parameterless constructor - check configuration settings", PrincipalBuilderFactory ?? string.Empty));
            }
        }

        /// <summary>   
        /// Gets the principal builder factory instance implementing <see cref="T:
        /// EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory"/>, and uses that to create instances of <see cref="T:
        /// EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilder"/> given the specified configuration. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <returns>   The principal builder instance or null if the PrincipalBuilderFactory property is not properly configured. </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is not suitable for a property as it returns new IBasicAuthPrincipalBuilder instances each time")]
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
