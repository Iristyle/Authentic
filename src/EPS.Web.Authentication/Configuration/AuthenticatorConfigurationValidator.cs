using System.Diagnostics.CodeAnalysis;
using EPS.Reflection;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Security;
using EPS.Web.Authentication.Utility;
using FluentValidation;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>	
	/// Authenticator configuration validator, responsible for validating a <see cref="T:IAuthenticatorConfiguration"/>, whether it is code-
	/// based or .config file based. 
	/// </summary>
	/// <remarks>	ebrown, 4/20/2011. </remarks>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "While AuthenticatorConfigurationValidator is an IEnumerable of validation rules, collection is not an appropriate name")]
	public class AuthenticatorConfigurationValidator :
		AbstractValidator<IAuthenticatorConfiguration>
	{
		/// <summary>
		/// Initializes a new instance of the AuthenticatorConfigurationValidator class.
		/// </summary>
		public AuthenticatorConfigurationValidator()
		{
			RuleFor(config => config.Name).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
			RuleFor(config => config.Authenticator).Cascade(CascadeMode.StopOnFirstFailure).NotNull()
				.Must(authenticator =>
				{
					return (!typeof(IAuthenticator<>).IsGenericInterfaceAssignableFrom(authenticator.GetType()));
				});
			//TODO: 4-8-2011 -- this needs to be moved to the validator associated with the concrete .net config system based implementation of the interface
			/*
			RuleFor(config => config.Factory).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must((config, factoryName) =>
				{
					if (null == config.Factory) { return false; }
					//TODO: need to push out better error messages

					//{
					//    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] cannot be found - check configuration settings", config.Factory ?? string.Empty));
					//}

					if (!typeof(IHttpContextInspectingAuthenticatorFactory<>).IsGenericInterfaceAssignableFrom(config.Factory.GetType()))
					{
						return false;
						//throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must implement interface {1} - check configuration settings", config.Factory ?? string.Empty, typeof(IHttpContextInspectingAuthenticatorFactory<>).Name));
					}

					var c = config.Factory.GetType().GetConstructor(Type.EmptyTypes);
					if (null == c)
					{
						return false;
						//throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "The factory type specified [{0}] must have a parameterless constructor - check configuration settings", config.Factory ?? string.Empty));
					}
                    

					return true;
				});
			 * */

			//TODO: write test code to verify if we need to catch exceptions or not
			RuleFor(config => config.RoleProviderName)
				.Must(roleProviderName => null != RoleProviderHelper.GetProviderByName(roleProviderName))
				.When(config => !string.IsNullOrWhiteSpace(config.RoleProviderName));

			RuleFor(config => config.ProviderName)
				.Must(providerName => null != MembershipProviderLocator.GetProvider(providerName))
				.When(config => !string.IsNullOrWhiteSpace(config.ProviderName));


			RuleFor(config => config.PrincipalBuilder).NotNull();
			//TODO: 4-8-2011 -- this checking must be moved to the class that validates the .net configuration system specific implementations
			/*
			 * RuleFor(config => config.PrincipalBuilderFactory).NotNull();
		.Must((config, principalBuilderFactory) =>
		{
			if (null == config.PrincipalBuilderFactory)
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
		*/
		}
	}
}