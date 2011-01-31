using System;
using System.Web;
using System.Web.Routing;

namespace EPS.Web.Routing
{
    /// <summary>   Unique identifier constraint that can be used as a routing constraint to ensure the parameter is a Guid. </summary>
    /// <remarks>   ebrown, 1/28/2011. </remarks>
    public class GuidConstraint : IRouteConstraint
    {
        /// <summary>   Determines whether the URL parameter contains a valid Guid value for this constraint. </summary>
        /// <remarks>   ebrown, 1/28/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when httpContext, route, parameterName, or values is null. </exception>
        /// <exception cref="ArgumentException">        Thrown when parameterName is whitespace or empty. </exception>
        /// <param name="httpContext">      An object that encapsulates information about the HTTP request -- unused, but checked for null. </param>
        /// <param name="route">            The object that this constraint belongs to -- unused, but checked for null. </param>
        /// <param name="parameterName">    The name of the parameter that is being checked.  Must not be null or empty. </param>
        /// <param name="values">           An object that contains the parameters for the URL. Must not not be null</param>
        /// <param name="routeDirection">   An object that indicates whether the constraint check is being performed when an incoming request is
        ///                                 being handled or when a URL is being generated.  Unused. </param>
        /// <returns>   true if the URL parameter contains a valid value; otherwise, false. </returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }
            if (route == null) { throw new ArgumentNullException("route"); }
            if (parameterName == null) { throw new ArgumentNullException("parameterName"); }
            if (string.IsNullOrWhiteSpace(parameterName)) { throw new ArgumentException("must not be empty", "parameterName"); }
            if (values == null) { throw new ArgumentNullException("values"); }

            if (values.ContainsKey(parameterName))
            {
                var parameter = values[parameterName];
                if (parameter is Guid && (Guid)parameter != Guid.Empty) return true;

                string stringValue = parameter as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;
                    return Guid.TryParse(stringValue, out guidValue) && (guidValue != Guid.Empty);
                }
            }
            return false;
        }
    }
}