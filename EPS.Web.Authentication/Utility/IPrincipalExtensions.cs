using System;
using System.Security.Principal;
using EPS.Web.Authentication.Security;
using EPS.Windows.Security;

namespace EPS.Web.Authentication.Utility
{
    /// <summary>   A simple set of extension methods on top of <see cref="T:System.Security.Principal"/> that perform utility functions,
    /// 			such as checking whether the user has an email address. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public static class IPrincipalExtensions
    {
        /// <summary>   An IPrincipal extension method that querys if an 'IPrincipal' has an email address. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="principal">    The principal to act on. </param>
        /// <returns>   true if email address, false if not. </returns>
        public static bool HasEmailAddress(this IPrincipal principal)
        {
            if (null == principal) { throw new ArgumentNullException("principal"); }

            return !string.IsNullOrEmpty(GetEmailAddress(principal));
        }

        /// <summary>   An IPrincipal extension method that gets an email address. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="principal">    The principal to act on. </param>
        /// <returns>   The email address. </returns>
        public static string GetEmailAddress(this IPrincipal principal)
        {
            if (null == principal) { throw new ArgumentNullException("principal"); }

            /*
            if (principal is OnyxPrincipal)
                return ((OnyxPrincipal)principal).Identity.UserProperties.Email;
            else 
            */
            if (principal is WindowsPrincipal)
                return ((WindowsIdentity)principal.Identity).GetUserEmailAddress();
            else if (principal is FixedProviderRolePrincipal)
                return string.Empty;
            else if (principal is GenericPrincipal)
                return string.Empty;

            return string.Empty;
        }
    }
}