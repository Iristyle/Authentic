using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace EPS.Web.Abstractions
{
    /// <summary>   A unit-testing friendly base class that implements <see cref="T:System.Web.IHttpModule"/>.
    /// 			Based on concepts from:
    /// 			<a href="http://weblogs.asp.net/rashid/archive/2009/03/12/unit-testable-httpmodule-and-httphandler.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public abstract class HttpModuleBase : IHttpModule
    {
        /// <summary>   Initializes a module and prepares it to handle requests. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Not as complex as it might appear")]
        public void Init(HttpApplication context)            
        {
            WrapAttach(() => context.AcquireRequestState += (sender, e) => OnAcquireRequestState(ToWrapper(sender)));
            WrapAttach(() => context.AuthenticateRequest += (sender, e) => OnAuthenticateRequest(ToWrapper(sender)));
            WrapAttach(() => context.AuthorizeRequest += (sender, e) => OnAuthorizeRequest(ToWrapper(sender)));
            WrapAttach(() => context.BeginRequest += (sender, e) => OnBeginRequest(ToWrapper(sender)));
            WrapAttach(() => context.EndRequest += (sender, e) => OnEndRequest(ToWrapper(sender)));
            WrapAttach(() => context.Error += (sender, e) => OnError(ToWrapper(sender)));
            WrapAttach(() => context.LogRequest += (sender, e) => OnLogRequest(ToWrapper(sender)));
            WrapAttach(() => context.MapRequestHandler += (sender, e) => OnMapRequestHandler(ToWrapper(sender)));
            WrapAttach(() => context.PostAcquireRequestState += (sender, e) => OnPostAcquireRequestState(ToWrapper(sender)));
            WrapAttach(() => context.PostAuthenticateRequest += (sender, e) => OnPostAuthenticateRequest(ToWrapper(sender)));
            WrapAttach(() => context.PostAuthorizeRequest += (sender, e) => OnPostAuthorizeRequest(ToWrapper(sender)));
            WrapAttach(() => context.PostLogRequest += (sender, e) => OnPostLogRequest(ToWrapper(sender)));
            WrapAttach(() => context.PostMapRequestHandler += (sender, e) => OnPostMapRequestHandler(ToWrapper(sender)));
            WrapAttach(() => context.PostReleaseRequestState += (sender, e) => OnPostReleaseRequestState(ToWrapper(sender)));
            WrapAttach(() => context.PostRequestHandlerExecute += (sender, e) => OnPostRequestHandlerExecute(ToWrapper(sender)));
            WrapAttach(() => context.PostResolveRequestCache += (sender, e) => OnPostResolveRequestCache(ToWrapper(sender)));
            WrapAttach(() => context.PostUpdateRequestCache += (sender, e) => OnPostUpdateRequestCache(ToWrapper(sender)));
            WrapAttach(() => context.PreRequestHandlerExecute += (sender, e) => OnPreRequestHandlerExecute(ToWrapper(sender)));
            WrapAttach(() => context.PreSendRequestContent += (sender, e) => OnPreSendRequestContent(ToWrapper(sender)));
            WrapAttach(() => context.PreSendRequestHeaders += (sender, e) => OnPreSendRequestHeaders(ToWrapper(sender)));
            WrapAttach(() => context.ReleaseRequestState += (sender, e) => OnReleaseRequestState(ToWrapper(sender)));
            WrapAttach(() => context.ResolveRequestCache += (sender, e) => OnResolveRequestCache(ToWrapper(sender)));
            WrapAttach(() => context.UpdateRequestCache += (sender, e) => OnUpdateRequestCache(ToWrapper(sender)));
        }

        private static HttpContextWrapper ToWrapper(object sender)
        {
            return new HttpContextWrapper(((HttpApplication)sender).Context);
        }
        //HACK: 9-17-2010 
        //1.  I don't know how to determine if we're running on integrated pipeline -- which some of the above handlers require
        // -- hence the need for this try / catch
        //2.  Would like to use some more succint code like the commented function below -- but can't find a way to call it!
        //http://stackoverflow.com/questions/623716/how-can-i-pass-an-event-to-a-function-in-c
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The purpose of this call is to eat any exceptions that may occur as we're wrapping an existing HttpContext")]
        private static void WrapAttach(Action attachment)
        {
            try { attachment(); }
            catch (Exception) { }
        }

        //TODO: 9-17-2010 -- something like this should work.. but doesn't!
        /*
        private void SafelyAttachHandler(ref EventHandler toAttach, Action<HttpContextBase> handler)
        {            
            toAttach += (sender, e) => handler(new HttpContextWrapper(((HttpApplication)sender).Context));
        }
        */
        /// <summary>   Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />. </summary>
        void IHttpModule.Dispose()
        {
        }

        /// <summary>   Executes the acquire request state action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnAcquireRequestState(HttpContextBase context) { }
        /// <summary>   Executes the authenticate request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnAuthenticateRequest(HttpContextBase context) { }
        /// <summary>   Executes the authorize request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnAuthorizeRequest(HttpContextBase context) { }
        /// <summary>   Executes the begin request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnBeginRequest(HttpContextBase context) { }
        /// <summary>   Executes the end request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnEndRequest(HttpContextBase context) { }
        /// <summary>   Executes the error action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnError(HttpContextBase context) { }
        /// <summary>   Executes the log request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnLogRequest(HttpContextBase context) { }
        /// <summary>   Executes the map request handler action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnMapRequestHandler(HttpContextBase context) { }
        /// <summary>   Executes the post acquire request state action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostAcquireRequestState(HttpContextBase context) { }
        /// <summary>   Executes the post authenticate request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostAuthenticateRequest(HttpContextBase context) { }
        /// <summary>   Executes the post authorize request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostAuthorizeRequest(HttpContextBase context) { }
        /// <summary>   Executes the post log request action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostLogRequest(HttpContextBase context) { }
        /// <summary>   Executes the post map request handler action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostMapRequestHandler(HttpContextBase context) { }
        /// <summary>   Executes the post release request state action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostReleaseRequestState(HttpContextBase context) { }
        /// <summary>   Executes the post request handler execute action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostRequestHandlerExecute(HttpContextBase context) { }
        /// <summary>   Executes the post resolve request cache action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostResolveRequestCache(HttpContextBase context) { }
        /// <summary>   Executes the post update request cache action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPostUpdateRequestCache(HttpContextBase context) { }
        /// <summary>   Executes the pre request handler execute action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPreRequestHandlerExecute(HttpContextBase context) { }
        /// <summary>   Executes the pre send request content action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPreSendRequestContent(HttpContextBase context) { }
        /// <summary>   Executes the pre send request headers action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnPreSendRequestHeaders(HttpContextBase context) { }
        /// <summary>   Executes the release request state action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnReleaseRequestState(HttpContextBase context) { }
        /// <summary>   Executes the resolve request cache action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnResolveRequestCache(HttpContextBase context) { }
        /// <summary>   Updates the user interface for the request cache action. </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events
        ///                         common to all application objects within an ASP.NET application. </param>
        public virtual void OnUpdateRequestCache(HttpContextBase context) { }
    }
}
