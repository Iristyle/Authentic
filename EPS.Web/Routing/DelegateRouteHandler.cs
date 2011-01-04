using System;
using System.Web;
using System.Web.Routing;

namespace EPS.Web.Routing
{
    /// <summary>   A route handler that allows specifying a delegate in lambda form.
    /// 			Code inspired by Haacked:
    /// 			<a href="http://haacked.com/archive/2008/12/15/redirect-routes-and-other-fun-with-routing-and-lambdas.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class DelegateRouteHandler : IRouteHandler
    {
        /// <summary>   Constructor that accepts a delegate handler. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="action">   The action. </param>
        /// <example>new DelegateRouteHandler(context => GetRedirectHandler(context, targetUrl, true))</example>
        public DelegateRouteHandler(Func<RequestContext, IHttpHandler> action)
        {
            HttpHandlerAction = action;
        }

        /// <summary>   Gets the delegate that processes the RequestContext with an IHttpHandler. </summary>
        /// <value> The delegate. </value>
        public Func<RequestContext, IHttpHandler> HttpHandlerAction { get; private set; }

        /// <summary>   Uses our delegate handler action to return an IHttpHandler to process an incoming RequestContext. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is invalid. </exception>
        /// <param name="requestContext">   An object that encapsulates information about the request. </param>
        /// <returns>   An IHttpHandler that processes the request. </returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var action = HttpHandlerAction;
            if (action == null)
            {
                throw new InvalidOperationException("No action specified");
            }

            return action(requestContext);
        }
    }
}
