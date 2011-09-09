using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
	/// <summary>
	/// Simple principal builder that doesn't accept configuration and can be used to provide an inline anonymous function to build
	/// principals.
	/// </summary>
	/// <remarks>	9/2/2011. </remarks>
	public class SimplePrincipalBuilder : IPrincipalBuilder
	{
		private readonly Func<HttpContextBase, MembershipUser, string, string, IPrincipal> _constructor;

		/// <summary>	Initializes a new instance of the SimplePrincipalBuilder class. </summary>
		/// <remarks>	9/2/2011. </remarks>
		/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
		/// <param name="constructor">	An inspection function that accepts the context, an optional membership user, username and password in
		/// 							that order. </param>
		public SimplePrincipalBuilder(Func<HttpContextBase, MembershipUser, string, string, IPrincipal> constructor)
		{
			if (null == constructor) { throw new ArgumentNullException("constructor"); }

			_constructor = constructor;
		}

		/// <summary>	Gets the human-friendly name. </summary>
		/// <value>	The name, which is always SimplePrincipalBuilder. </value>
		public string Name
		{
			get { return "SimplePrincipalBuilder"; }
		}

		/// <summary>
		/// Gets the configuration defining the <see cref="T:System.Web.Security.MembershipProvider"/> and <see cref="T:
		/// EPS.Web.Abstractions.IPrincipalBuilderFactory"/> to use.
		/// </summary>
		/// <value>	Always returns null. </value>
		public IAuthenticatorConfiguration Configuration
		{
			get { return null; }
		}

		/// <summary>	Construct an IPrincipal given a username and password / MembershipUser (context thrown in for good measure). </summary>
		/// <remarks>	9/2/2011. </remarks>
		/// <param name="context">		 	The context. </param>
		/// <param name="membershipUser">	The membership user extracted if there was a MembershipProvider specified through configuration.  May
		/// 								be null. </param>
		/// <param name="userName">		 	The username. </param>
		/// <param name="password">		 	The password. </param>
		/// <returns>	An IPrincipal if the given credentials could be authenticated, otherwise null. </returns>
		public IPrincipal ConstructPrincipal(HttpContextBase context, MembershipUser membershipUser, string userName, string password)
		{
			return _constructor(context, membershipUser, userName, password);
		}
	}
}