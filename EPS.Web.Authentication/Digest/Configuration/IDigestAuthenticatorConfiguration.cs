using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Digest.Configuration
{
	/// <summary>   Interface for digest authentication header inspector configuration element. </summary>
	/// <remarks>   ebrown, 3/28/2011. </remarks>
	public interface IDigestAuthenticatorConfiguration :
			IAuthenticatorConfiguration
	{
		/// <summary>   Gets or sets the realm of the digest response on an outgoing 401 challenge. </summary>
		/// <value> The realm. </value>
		string Realm { get; }

		/// <summary>   Gets or sets the Private Key value used when generating nonce values. </summary>
		/// <value> The private key. </value>
		string PrivateKey { get; }

		/// <summary>   Gets or sets the timespan that a nonce is valid. </summary>
		/// <value> The duration that the nonce is valid for. </value>
		TimeSpan NonceValidDuration { get; set; }
	}
}