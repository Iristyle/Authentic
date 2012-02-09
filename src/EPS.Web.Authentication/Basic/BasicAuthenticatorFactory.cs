using System.Diagnostics.CodeAnalysis;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
	/// <summary>   An implementation of a simple Basic authentication inspecting authenticator factory. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class BasicAuthenticatorFactory :
		AuthenticatorFactoryBase<IBasicAuthenticatorConfiguration>
	{
		#region IAuthenticatorFactory Members
		/// <summary>   
		/// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.BasicAuthenticator"/>, returning a
		/// <see cref="T:
		/// EPS.Web.Authentication.Abstractions.IAuthenticator{IAuthenticatorConfiguration}"/> . 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		/// <returns>   A new authenticator instance. </returns>
		public override IAuthenticator<IBasicAuthenticatorConfiguration> Construct(IBasicAuthenticatorConfiguration config)
		{
			return new BasicAuthenticator(config);
		}
		#endregion
	}
}