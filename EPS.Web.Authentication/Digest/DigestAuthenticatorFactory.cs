using System.Diagnostics.CodeAnalysis;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>   An implementation of a simple digest authentication inspecting authenticator factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class DigestAuthenticatorFactory :
        AuthenticatorFactoryBase<IDigestAuthenticatorConfiguration>
    {
        #region  Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Digest.DigestAuthenticator"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IAuthenticator{IAuthenticatorConfigurationElement}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new authenticator instance. </returns>
        public override IAuthenticator<IDigestAuthenticatorConfiguration> Construct(IDigestAuthenticatorConfiguration config)
        {
            return new DigestAuthenticator(config);
        }
        #endregion
    }
}