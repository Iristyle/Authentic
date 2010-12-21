using System;
using System.Security.Principal;
using System.Web.Security;

namespace EPS.Web.Authentication.Security
{
    public class FixedProviderRolePrincipal : IPrincipal
    {
        private RoleProvider roleProvider;
        private IIdentity identity;
        public FixedProviderRolePrincipal(RoleProvider roleProvider, IIdentity identity)
        {
            this.roleProvider = roleProvider;
            this.identity = identity;
        }

        public bool IsInRole(string role)
        {
            return roleProvider.IsUserInRole(identity.Name, role);
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }
    }
}
