using System;
using System.Globalization;
using System.Text;

namespace EPS.Web.Authentication.Digest
{
	/// <summary>   
	/// This class is responsible for generating a unique nonce based on a client IP address and our PrivateHashEncoder implementation. 
	/// </summary>
	/// <remarks>   ebrown, 4/8/2011. </remarks>
	public static class NonceManager
	{
		internal static Func<DateTime> Now = () => { return DateTime.UtcNow; };
		private static Encoding encoding = Encoding.ASCII;

		/// <summary>   
		/// Generates a base64 encoded nonce combining the current time, and another hashed value that contains a hash of the time, ip address
		/// and a private key. Milliseconds:PrivateHash where PrivateHash = Milliseconds:IP:PrivateKey. 
		/// </summary>
		/// <remarks>   ebrown, 4/6/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
		/// <param name="ipAddress">            The IP address. </param>
		/// <param name="privateHashEncoder">   The private hash encoder. </param>
		/// <returns>   A base64 nonce value representing time:privateValue. </returns>
		public static string Generate(string ipAddress, PrivateHashEncoder privateHashEncoder)
		{
			if (null == ipAddress) { throw new ArgumentNullException("ipAddress"); }
			if (string.IsNullOrWhiteSpace(ipAddress)) { throw new ArgumentException("must not be empty", "ipAddress"); };
			if (null == privateHashEncoder) { throw new ArgumentNullException("privateHashEncoder"); }

			string dateTimeInMilliSecondsString = (Now() - DateTime.MinValue)
				.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
			string privateHash = privateHashEncoder.Encode(dateTimeInMilliSecondsString, ipAddress);
			return Convert.ToBase64String(encoding.GetBytes(
				string.Format(CultureInfo.InvariantCulture, "{0}:{1}", dateTimeInMilliSecondsString, privateHash)));
		}

		/// <summary>   Given a nonce value, determines if it correctly applies to the given IP address. </summary>
		/// <remarks>   ebrown, 4/6/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
		/// <param name="nonce">                The nonce. </param>
		/// <param name="ipAddress">            The IP address. </param>
		/// <param name="privateHashEncoder">   The private hash encoder. </param>
		/// <returns>   true if it succeeds, false if it fails. </returns>
		public static bool Validate(string nonce, string ipAddress, PrivateHashEncoder privateHashEncoder)
		{
			if (null == nonce) { throw new ArgumentNullException("nonce"); }
			if (string.IsNullOrWhiteSpace(nonce)) { throw new ArgumentException("must not be empty", "nonce"); };

			if (null == ipAddress) { throw new ArgumentNullException("ipAddress"); }
			if (string.IsNullOrWhiteSpace(ipAddress)) { throw new ArgumentException("must not be empty", "ipAddress"); };

			if (null == privateHashEncoder) { throw new ArgumentNullException("privateHashEncoder"); }

			string[] decodedParts = GetDecodedParts(nonce);
			string md5EncodedString = privateHashEncoder.Encode(decodedParts[0], ipAddress);
			return string.CompareOrdinal(decodedParts[1], md5EncodedString) == 0;
		}

		/// <summary>   Query if 'nonce' is stale based on an allowed number of seconds. </summary>
		/// <remarks>   ebrown, 4/6/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
		/// <param name="nonce">        The nonce. </param>
		/// <param name="staleTimeout"> The stale time out expressed as a TimeSpan. </param>
		/// <returns>   true if stale, false if not. </returns>
		public static bool IsStale(string nonce, TimeSpan staleTimeout)
		{
			if (null == nonce) { throw new ArgumentNullException("nonce"); }
			if (string.IsNullOrWhiteSpace(nonce)) { throw new ArgumentException("must not be empty", "nonce"); };

			string[] decodedParts = GetDecodedParts(nonce);
			DateTime dateTimeFromNonce = NonceTimestampParser.Parse(decodedParts[0]);
			return (dateTimeFromNonce + staleTimeout) < Now();
		}

		private static string[] GetDecodedParts(string nonce)
		{
			return encoding.GetString(Convert.FromBase64String(nonce))
				.Split(':');
		}
	}
}