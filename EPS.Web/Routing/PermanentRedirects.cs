using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Routing;
using EPS.Configuration.Abstractions;
using EPS.Web.Configuration;

namespace EPS.Web.Routing
{
    /// <summary>   A static helper class that is used to register a set of permanent route redirections. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class PermanentRedirects
    {
        /// <summary>   
        /// This must be called by client code somewhere near the start of the application loading -- i.e. Application_Load to register permanent
        /// redirects within the applications RouteCollection. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="routeCollection">      The applications RouteCollection. </param>
        /// <param name="configurationManager"> An optional IConfigurationManager to read settings from.  If left unspecified or null, the
        ///                                     executing application ConfigurationManager is used. </param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This code is only used server-side internally where we control source languages - default params are perfectly acceptable")]
        public static void Register(RouteCollection routeCollection, IConfigurationManager configurationManager = null)
        {
            if (null == configurationManager)
                configurationManager = new ConfigurationManagerWrapper();

            RoutingConfigurationSection config = configurationManager.GetSection<RoutingConfigurationSection>(RoutingConfigurationSection.ConfigurationPath);

            if (config.Enabled)
            {
                //RouteTable.Routes.RouteExistingFiles = true;
                //RouteTable.Routes.RedirectPermanently("home/{foo}.aspx", "~/home/{foo}");
                //http://haacked.com/archive/2008/12/15/redirect-routes-and-other-fun-with-routing-and-lambdas.aspx
                // The {*} instructs the route to match all content after the first slash (including extra slashes)                
                foreach (var s in config.PermanentRedirects.GetUrlMap())
                    routeCollection.RedirectPermanently(s.Key, s.Value);
            }
        }
    }
}
