using System;
using System.Web.Routing;
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
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="routeCollection">  The applications RouteCollection. </param>
        /// <param name="configuration">    A IRoutingConfigurationSection to read settings from.  This interface is implemented by
        ///                                 <see cref="T:EPS.Web.RoutingConfigurationSection"/> and can be retrieved by
        ///                                 <code><![CDATA[new ConfigurationManagerWrapper().GetSection<RoutingConfigurationSection>(RoutingConfigurationSection.ConfigurationPath)]]></code>
        ///                                 or can be faked for testing purposes. </param>
        public static void Register(RouteCollection routeCollection, IRoutingConfigurationSection configuration)
        {
            if (null == configuration) { throw new ArgumentNullException("configuration"); }

            if (configuration.Enabled)
            {
                //RouteTable.Routes.RouteExistingFiles = true;
                //RouteTable.Routes.RedirectPermanently("home/{foo}.aspx", "~/home/{foo}");
                //http://haacked.com/archive/2008/12/15/redirect-routes-and-other-fun-with-routing-and-lambdas.aspx
                // The {*} instructs the route to match all content after the first slash (including extra slashes)                
                foreach (var s in configuration.PermanentRedirects.GetUrlMap())
                    routeCollection.RedirectPermanently(s.Key, s.Value);
            }
        }
    }
}