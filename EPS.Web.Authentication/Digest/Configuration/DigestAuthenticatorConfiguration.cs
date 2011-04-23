using System;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Digest.Configuration
{
	/// <summary>	The configuration class responsible for managing the settings for the digest authentication handler. </summary>
	/// <remarks>	ebrown, 4/21/2011. </remarks>
	public class DigestAuthenticatorConfiguration :
		AuthenticatorConfiguration, IDigestAuthenticatorConfiguration
	{
		private TimeSpan _nonceValidDuration = new TimeSpan(0, 5, 0);

		/// <summary>
		/// Initializes a new instance of the DigestAuthenticatorConfiguration class.
		/// </summary>
		public DigestAuthenticatorConfiguration(string name, IAuthenticator authenticator,
			IPrincipalBuilder principalBuilder, string realm, string privateKey) :
			base(name, authenticator, principalBuilder)
		{
			//TODO: 4-8-2011 cook up an AbstractValidator class that verifies this goop
			Realm = realm;
			PrivateKey = privateKey;
		}

		/// <summary>   Gets or sets the realm of the digest response on an outgoing 401 challenge. </summary>
		/// <value> The realm. </value>
		public string Realm { get; private set; }

		/// <summary>   Gets or sets the Private Key value used when generating nonce values. </summary>
		/// <value> The private key. </value>
		public string PrivateKey { get; private set; }

		/// <summary>   Gets or sets the timespan that a nonce is valid. </summary>
		/// <value> The duration that the nonce is valid for. </value>
		public TimeSpan NonceValidDuration
		{
			get { return _nonceValidDuration; }
			set { _nonceValidDuration = value; }
		}
	}
}