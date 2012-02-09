using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Abstractions
{
    /// <summary>   Interface that defines how to construct instances of a principal builder. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IPrincipalBuilderFactory
    {
        /// <summary>   Constructs the class instance which can build an IPrincipal from incoming HTTP context. </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   The <see cref="T:EPS.Web.Abstractions.IPrincipalBuilder"/> instance. </returns>
        IPrincipalBuilder Construct(IAuthenticatorConfiguration config);
    }
}
