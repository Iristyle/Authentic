using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace EPS.Web.Authentication
{
    /// <summary>   The value returned from a <see cref="T:EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator"/>. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    [SuppressMessage("Gendarme.Rules.Maintainability", "AvoidLackOfCohesionOfMethodsRule", Justification = "This is a simple placeholder type used for generating return values from inspectors")]
    public class AuthenticationResult
    {
        /// <summary>   Initializes a new instance of the InspectorAuthenticationResult class. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="success">      true if the operation was a success, false if it failed. </param>
        /// <param name="principal">    The IPrincipal. </param>
        /// <param name="errorMessage"> Message describing the error. </param>
        public AuthenticationResult(bool success, IPrincipal principal, string errorMessage)
        {
            Success = success;
            Principal = principal;
            ErrorMessage = errorMessage;
        }

        /// <summary>   Gets a value indicating whether the authentication was a success. </summary>
        /// <value> true if success, false if not. </value>
        public bool Success { get; private set; }
        
        /// <summary>   Gets the principal to be used for a successful authentication event. </summary>
        /// <value> The principal. </value>
        public IPrincipal Principal { get; private set; }
        
        /// <summary>   Gets a message describing any error that may have occurred. </summary>
        /// <value> A message describing the error. </value>
        public string ErrorMessage { get; private set; }
    }
}
