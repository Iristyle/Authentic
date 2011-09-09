using System;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
	/// <summary>	Basic authenticator configuration.  </summary>
	/// <remarks>	9/2/2011. </remarks>
	public class BasicAuthenticatorConfiguration
		: AuthenticatorConfiguration, IBasicAuthenticatorConfiguration
	{
		/// <summary>	Initializes a new instance of the AuthenticatorConfiguration class. </summary>
		/// <remarks>	9/2/2011. </remarks>
		/// <param name="name">			   	The name. </param>
		/// <param name="authenticator">   	The authenticator. </param>
		/// <param name="principalBuilder">	The principal builder. </param>
		public BasicAuthenticatorConfiguration(string name, IAuthenticator authenticator, IPrincipalBuilder principalBuilder)
			: base(name, authenticator, principalBuilder)
		{ }
	}
}