using System.Diagnostics.CodeAnalysis;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   Interface that defines how to construct instances of a basic authorization principal builder. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    [SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "This interface might be implemented by third parties")]
    public interface IBasicAuthPrincipalBuilderFactory
    {
        /// <summary>   Constructs the class instance which can build an IPrincipal from incoming HTTP context. </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   The IBasicAuthPrincipalBuilder instance. </returns>
        IBasicAuthPrincipalBuilder Construct(AuthenticationHeaderInspectorConfigurationElement config);
    }
}
