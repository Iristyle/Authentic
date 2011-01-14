using System;
using System.Security.Principal;
using System.Web.Security;

namespace EPS.Web.Authentication.Security
{
    /// <summary>   
    /// A very simle IPrincipal implementation that supports attaching an arbitrary IIdentity and RoleProvider to determine role membership.. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class FixedProviderRolePrincipal : IPrincipal
    {
        private readonly RoleProvider _roleProvider;
        private readonly IIdentity _identity;
        
        /// <summary>   Constructor an instance of the principal. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="roleProvider"> The role provider. </param>
        /// <param name="identity">     The identity. </param>
        public FixedProviderRolePrincipal(RoleProvider roleProvider, IIdentity identity)
        {
            this._roleProvider = roleProvider;
            this._identity = identity;
        }

        /// <summary>   Determines whether the current principal belongs to the specified role. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="role"> The name of the role for which to check membership. </param>
        /// <returns>   true if the current principal is a member of the specified role; otherwise, false. </returns>
        public bool IsInRole(string role)
        {
            return _roleProvider.IsUserInRole(_identity.Name, role);
        }

        /// <summary>   Gets the identity of the current principal. </summary>
        /// <value> The <see cref="T:System.Security.Principal.IIdentity" /> object associated with the current principal. </value>
        public IIdentity Identity
        {
            get { return this._identity; }
        }
    }
}