using System;
using System.Diagnostics.CodeAnalysis;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>   A class that supports code-based configuration. </summary>
	/// <remarks>   ebrown, 4/8/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Maintainability", "AvoidLackOfCohesionOfMethodsRule", Justification = "DTOs by design aren't particularly cohesive")]
	[SuppressMessage("Gendarme.Rules.Design", "ConsiderAddingInterfaceRule", Justification = "IFailureHandlerConfiguration and IAuthenticatorConfiguration do not share commonality")]
	public class AuthenticatorConfiguration :
		IAuthenticatorConfiguration
	{
		private string _roleProviderName;
		private bool roleProviderNameInitialized;
		private bool _requireSsl;
		private bool requireSslInitialized;
		private string _providerName;
		private bool providerNameInitialized;

		/// <summary>
		/// Initializes a new instance of the AuthenticatorConfiguration class.
		/// </summary>
		public AuthenticatorConfiguration(string name, IAuthenticator authenticator, IPrincipalBuilder principalBuilder)
		{
			//TODO: 4-8-2011 -- create a FleutnValidator that matches up with this and call it here
			Name = name;
			Authenticator = authenticator;
			PrincipalBuilder = principalBuilder;
		}

		/// <summary>   Gets or sets the name of the role provider used to validate the principal. </summary>
		/// <value> The name of the role provider. </value>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RoleProviderName", Justification = "Legit compound terms")]
		public string RoleProviderName
		{
			get { return _roleProviderName; }
			set
			{
				if (roleProviderNameInitialized)
				{
					throw new NotSupportedException("RoleProviderName has already been initialized");
				}
				else
				{
					_roleProviderName = value;
					roleProviderNameInitialized = true;
				}
			}
		}

		/// <summary>   Gets or sets the human-friendly name / key for this inspector. </summary>
		/// <value> The name. </value>
		public string Name { get; protected set; }

		/// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
		/// <value> true if require SSL, false if not. </value>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RequireSsl", Justification = "Legit compound terms")]
		public bool RequireSsl
		{
			get { return _requireSsl; }
			set
			{
				if (requireSslInitialized)
				{
					throw new NotSupportedException("RequireSsl has already been initialized");
				}
				else
				{
					_requireSsl = value;
					requireSslInitialized = true;
				}
			}
		}

		/// <summary>   
		/// Get or sets the name of the MembershipProvider to be used.  By default no membership provider is used as it may be just as costly as
		/// extracting a principal. If a simpler membership provider exists that can provide a faster validation of user credentials than a full
		/// IPrincipal extraction, then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured
		/// MembershipProvider for the system. 
		/// </summary>
		/// <value> The name of the provider. </value>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ProviderName", Justification = "Legit compound terms")]
		public string ProviderName
		{
			get { return _providerName; }
			set
			{
				if (providerNameInitialized)
				{
					throw new NotSupportedException("ProviderName has already been initialized");
				}
				else
				{
					_providerName = value;
					providerNameInitialized = true;
				}
			}
		}

		/// <summary>   Gets or sets the authenticator instance. </summary>
		/// <value> The authenticator. </value>
		public IAuthenticator Authenticator { get; protected set; }

		/// <summary>   Gets or sets the principal builder instance. </summary>
		/// <value> The builder instance. </value>
		public IPrincipalBuilder PrincipalBuilder { get; protected set; }
	}
}