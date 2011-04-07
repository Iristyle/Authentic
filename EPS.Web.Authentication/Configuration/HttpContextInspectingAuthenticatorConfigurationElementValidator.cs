using System;
using EPS.Reflection;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Security;
using EPS.Web.Authentication.Utility;
using FluentValidation;

namespace EPS.Web.Authentication.Configuration
{
    public class HttpContextInspectingAuthenticatorConfigurationElementValidator :
        AbstractValidator<IHttpContextInspectingAuthenticatorConfigurationElement>
    {
        /// <summary>
        /// Initializes a new instance of the HttpContextInspectingAuthenticatorConfigurationElementValidator class.
        /// </summary>
        public HttpContextInspectingAuthenticatorConfigurationElementValidator()
        {
            RuleFor(config => config.Name).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(config => config.Factory).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must((config, factoryName) =>
                {
                    var t = Type.GetType(config.Factory);
                    if (null == t) { return false; }
                    //TODO: need to push out better error messages

                    //{
                    //    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] cannot be found - check configuration settings", config.Factory ?? string.Empty));
                    //}

                    if (!typeof(IHttpContextInspectingAuthenticatorFactory<>).IsGenericInterfaceAssignableFrom(t))
                    {
                        return false;
                        //throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must implement interface {1} - check configuration settings", config.Factory ?? string.Empty, typeof(IHttpContextInspectingAuthenticatorFactory<>).Name));
                    }

                    var c = t.GetConstructor(Type.EmptyTypes);
                    if (null == c)
                    {
                        return false;
                        //throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must have a parameterless constructor - check configuration settings", config.Factory ?? string.Empty));
                    }

                    return true;
                });
            RuleFor(config => config.CustomConfigurationSectionName).Must(customConfigName =>
            {
                //TODO: make sure the named config section exists -- look at our config abstractions
                return false;

            }).When(config => !string.IsNullOrWhiteSpace(config.CustomConfigurationSectionName));

            //TODO: write test code to verify if we need to catch exceptions or not
            RuleFor(config => config.RoleProviderName)
                .Must(roleProviderName => null != RoleProviderHelper.GetProviderByName(roleProviderName))
                .When(config => !string.IsNullOrWhiteSpace(config.RoleProviderName));

            RuleFor(config => config.ProviderName)
                .Must(providerName => null != MembershipProviderLocator.GetProvider(providerName))
                .When(config => !string.IsNullOrWhiteSpace(config.ProviderName));


            RuleFor(config => config.PrincipalBuilderFactory).Must((config, principalBuilderFactory) =>
                {
                    var type = Type.GetType(config.PrincipalBuilderFactory);
                    if (null == type)
                    {
                        return false;
                        //throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] cannot be found - check configuration settings", config.PrincipalBuilderFactory ?? string.Empty));
                    }

                    if (!typeof(IPrincipalBuilderFactory).IsAssignableFrom(type))
                    {
                        return false;
                        //throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must implement interface {1} - check configuration settings", config.PrincipalBuilderFactory ?? string.Empty, typeof(IPrincipalBuilderFactory).Name));
                    }

                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (null == constructor)
                    {
                        return false;
                        //throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The principalBuilderFactory type name specified [{0}] must have a parameterless constructor - check configuration settings", config.PrincipalBuilderFactory ?? string.Empty));
                    }
                    return true;
                }).When(config => !string.IsNullOrWhiteSpace(config.PrincipalBuilderFactory));
        }
    }
}