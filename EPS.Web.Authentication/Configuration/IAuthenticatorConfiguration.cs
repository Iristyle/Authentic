using System.Diagnostics.CodeAnalysis;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>   Interface for http context inspecting authenticator configuration element. </summary>
	/// <remarks>   ebrown, 3/28/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Design", "ConsiderAddingInterfaceRule", Justification = "IFailureHandlerConfiguration and IAuthenticatorConfiguration do not share commonality")]
	public interface IAuthenticatorConfiguration
	{
		/// <summary>   Gets or sets the name of the role provider used to validate the principal. </summary>
		/// <value> The name of the role provider. </value>
		string RoleProviderName { get; set; }

		/// <summary>   Gets or sets the human-friendly name / key for this inspector. </summary>
		/// <value> The name. </value>
		string Name { get; set; }

		/// <summary>   Gets or sets the authenticator instance. </summary>
		/// <value> The authenticator. </value>
		IAuthenticator Authenticator { get; }

		/// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
		/// <value> true if require SSL, false if not. </value>
		bool RequireSsl { get; set; }

		/// <summary>   
		/// Get or sets the name of the MembershipProvider to be used.  By default no membership provider is used as it may be just as costly as
		/// extracting a principal. If a simpler membership provider exists that can provide a faster validation of user credentials than a full
		/// IPrincipal extraction, then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured
		/// MembershipProvider for the system. 
		/// </summary>
		/// <value> The name of the provider. </value>
		string ProviderName { get; set; }

		/// <summary>   Gets or sets the principal builder instance. </summary>
		/// <value> 
		/// The builder instance.
		/// </value>
		IPrincipalBuilder PrincipalBuilder { get; }
	}
}