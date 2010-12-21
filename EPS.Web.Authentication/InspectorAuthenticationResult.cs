using System;
using System.Security.Principal;

namespace EPS.Web.Authentication
{
    public class InspectorAuthenticationResult
    {
        public bool Success { get; set; }
        public IPrincipal Principal { get; set; }
    }
}
