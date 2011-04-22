using System;
using EPS.Web.Authentication.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Authentication.Basic.Configuration
{
	/// <summary>   Interface for basic authentication header inspector configuration element. </summary>
	/// <remarks>   ebrown, 3/28/2011. </remarks>
	[SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "We might later expand this interface in addition to the fact that it fits with our architecture")]
	public interface IBasicAuthenticatorConfiguration :
			IAuthenticatorConfiguration
	{ }
}
