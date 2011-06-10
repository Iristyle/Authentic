using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.Web;
using EPS.Annotations;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;

namespace EPS.Web.Authentication.Digest
{
	/// <summary>   A failure handler that sends out a basic authentication WWW-Authenticate header if authentication fails. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class DigestFailureHandler :
		FailureHandlerBase<IDigestFailureHandlerConfiguration>
	{
		PrivateHashEncoder privateHashEncoder;

		/// <summary>   Initializes a new instance of the DigestFailureHandler class given configuration values. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		public DigestFailureHandler(IDigestFailureHandlerConfiguration config)
			: base(config)
		{
			//TODO: 4-6-2011 -- find a better way to hook up the validation logic here so that it matches up with the config class
			if (null == config) { throw new ArgumentNullException("config"); }
			string privateKey = config.PrivateKey;
			if (null == privateKey) { throw new ArgumentNullException("config", "IDigestAuthenticatorConfiguration.PrivateKey is null"); }
			if (null == config.Realm) { throw new ArgumentNullException("config", "IDigestAuthenticatorConfiguration.Realm is null"); }
			if (string.IsNullOrWhiteSpace(privateKey)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.PrivateKey must not be whitespace", "config"); }
			if (privateKey.Length < 8) { throw new ArgumentException("IDigestAuthenticatorConfiguration.PrivateKey must be at least 8 characters", "config"); }
			if (string.IsNullOrWhiteSpace(config.Realm)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.Realm must not be whitespace", "config"); }
			if (config.NonceValidDuration < TimeSpan.FromSeconds(20)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.NonceValidDuration must be at least 20 seconds", "config"); }
			if (config.NonceValidDuration > TimeSpan.FromMinutes(60)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.NonceValidDuration must be less than 60 minutes", "config"); }

			privateHashEncoder = new PrivateHashEncoder(privateKey);
		}

		#region IFailureHandler Members
		/// <summary>   
		/// Executes the authentication failure action, sending a WWW-Authenticate header with the configured realm out through the context and
		/// setting the HTTP status code to 401 (Unauthorized). 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <param name="context">          The incoming context. </param>
		/// <param name="inspectorResults"> The set of failed inspector results. </param>
		/// <returns>   Null -- no IPrincipal is returned as the response is completed after sending the authenticate header. </returns>
		public override IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
		{
			if (null == context) { throw new ArgumentNullException("context"); }

			//http://en.wikipedia.org/wiki/Digest_access_authentication
			//not sure that 'auth-int' is supported across the board, so stick with just 'auth'
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

			string stale = "FALSE";
			//attempt to extract credentials from header -- assuming they might be stale
			DigestHeader digestHeader;
			HttpRequestBase request = context.Request;
			if (HttpDigestAuthHeaderParser.TryExtractDigestHeader(request.HttpMethod.ToEnumFromEnumValue<HttpMethodNames>(), request.Headers["Authorization"], out digestHeader)
				&& NonceManager.Validate(digestHeader.Nonce, request.UserHostAddress, privateHashEncoder)
				&& NonceManager.IsStale(digestHeader.Nonce, Configuration.NonceValidDuration))
			{
				stale = "TRUE";
			}

			context.Response.AddHeader("WWW-Authenticate", String.Format(CultureInfo.InvariantCulture,
				"Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\", stale={3}, algorithm=MD5, qop=\"{4}\"",
				Configuration.Realm, NonceManager.Generate(request.UserHostAddress, privateHashEncoder), Opaque.Current(),
				stale, DigestQualityOfProtectionType.Authentication.ToEnumValueString()));

			//this is a guard since we can't effectively mock CompleteRequest in tests
			var application = context.ApplicationInstance;
			if (null != application)
			{
				application.CompleteRequest();
			}

			return null;
		}
		#endregion
	}
}