using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using EPS.Security.Cryptography;
using EPS.Text;

namespace EPS.Web
{
	/// <summary>   Represents an <a href="http://tools.ietf.org/html/rfc2617#section-3.2.2">RFC 2617</a> digest header. </summary>
	/// <remarks>   ebrown, 3/28/2011. </remarks>
	public class DigestHeader
	{
		/// <summary>   Gets or sets the verb used during the request. </summary>
		/// <value> The HTTP verb. </value>
		public HttpMethodNames Verb { get; set; }
		/// <summary>   Gets or sets the name of the user. </summary>
		/// <value> The name of the user. </value>
		public string UserName { get; set; }
		/// <summary>   Gets or sets the realm. </summary>
		/// <value> The realm. </value>
		public string Realm { get; set; }
		/// <summary>   Gets or sets the nonce. </summary>
		/// <value> The nonce. </value>
		public string Nonce { get; set; }
		/// <summary>   Gets or sets URI of the document. </summary>
		/// <value> The uri. </value>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "The Uri in a digest auth request is not necessarily complete")]
		public string Uri { get; set; }
		/// <summary>   Gets or sets the quality of protection. </summary>
		/// <value> The quality of protection. </value>
		public DigestQualityOfProtectionType QualityOfProtection { get; set; }
		/// <summary>   Gets or sets the request counter. </summary>
		/// <value> The request counter. </value>
		public int? RequestCounter { get; set; }
		/// <summary>   Gets or sets the client nonce. </summary>
		/// <value> The client nonce. </value>
		public string ClientNonce { get; set; }
		/// <summary>   Gets or sets the response. </summary>
		/// <value> The response. </value>
		public string Response { get; set; }
		/// <summary>   Gets or sets the opaque. </summary>
		/// <value> The opaque. </value>
		public string Opaque { get; set; }

		/// <summary>	Matches credentials. </summary>
		/// <remarks>	ebrown, 3/28/2011. </remarks>
		/// <exception cref="ArgumentNullException">	Thrown when realm, nonce or password are null. </exception>
		/// <exception cref="NotImplementedException">	Thrown when the current DigestHeader is set to 'auth-int', which is presently
		/// 											unsupported. </exception>
		/// <exception cref="NotSupportedException">	Thrown when the DigestHeaders HTTP method is unrecognized or not supported. </exception>
		/// <param name="realm">	The realm. </param>
		/// <param name="opaque">	The opaque. </param>
		/// <param name="password">	The password. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		public bool MatchesCredentials(string realm, string opaque, string password)
		{
			if (null == realm) { throw new ArgumentNullException("realm"); }
			if (null == password) { throw new ArgumentNullException("password"); }
			if (DigestQualityOfProtectionType.AuthenticationWithIntegrity == QualityOfProtection) { throw new NotImplementedException("auth-int is not currently supported"); }
			if (!Enum.IsDefined(typeof(HttpMethodNames), Verb)) { throw new NotSupportedException("The verb specified is not valid"); }


			var encoding = Encoding.GetEncoding("ISO-8859-1");
			using (var algorithm = MD5.Create())
			{
				//client to server opaque must match
				if (Opaque != opaque) { return false; }
				//client to server realm must match
				if (realm != Realm) { return false; }

				//valid for auth, auth-int and unspecified
				string hash1 = HashHelpers.SafeHash(algorithm,
					encoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", UserName, Realm, password)));
				//valid for auth and unspecified
				string hash2 = HashHelpers.SafeHash(algorithm,
					encoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Verb.ToEnumValueString(), Uri)));

				if (QualityOfProtection == DigestQualityOfProtectionType.Authentication)
				{
					return Response == HashHelpers.SafeHash(algorithm,
						encoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2:00000000.##}:{3}:{4}:{5}", hash1, Nonce,
						RequestCounter, ClientNonce, QualityOfProtection.ToEnumValueString(), hash2)));
				}

				return Response == HashHelpers.SafeHash(algorithm,
						encoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", hash1, Nonce, hash2)));
			}
		}
	}
}