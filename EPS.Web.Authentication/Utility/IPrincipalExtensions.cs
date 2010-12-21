using System;
using System.Security.Principal;
using EPS.Windows.Security;
using EPS.Web.Authentication.Security;

namespace EPS.Web.Authentication.Utility
{
    public static class IPrincipalExtensions
    {
        public static bool HasEmailAddress(this IPrincipal principal)
        {
            return !string.IsNullOrEmpty(GetEmailAddress(principal));
        }

        public static string GetEmailAddress(this IPrincipal principal)
        {
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
