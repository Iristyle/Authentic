using System;
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
        public void Init(HttpApplication context)            
        {            
            WrapAttach(() => context.AcquireRequestState += (sender, e) => OnAcquireRequestState(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.AuthenticateRequest += (sender, e) => OnAuthenticateRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.AuthorizeRequest += (sender, e) => OnAuthorizeRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.BeginRequest += (sender, e) => OnBeginRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.EndRequest += (sender, e) => OnEndRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.Error += (sender, e) => OnError(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.LogRequest += (sender, e) => OnLogRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.MapRequestHandler += (sender, e) => OnMapRequestHandler(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostAcquireRequestState += (sender, e) => OnPostAcquireRequestState(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostAuthenticateRequest += (sender, e) => OnPostAuthenticateRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostAuthorizeRequest += (sender, e) => OnPostAuthorizeRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostLogRequest += (sender, e) => OnPostLogRequest(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostMapRequestHandler += (sender, e) => OnPostMapRequestHandler(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostReleaseRequestState += (sender, e) => OnPostReleaseRequestState(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostRequestHandlerExecute += (sender, e) => OnPostRequestHandlerExecute(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostResolveRequestCache += (sender, e) => OnPostResolveRequestCache(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PostUpdateRequestCache += (sender, e) => OnPostUpdateRequestCache(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PreRequestHandlerExecute += (sender, e) => OnPreRequestHandlerExecute(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PreSendRequestContent += (sender, e) => OnPreSendRequestContent(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.PreSendRequestHeaders += (sender, e) => OnPreSendRequestHeaders(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.ReleaseRequestState += (sender, e) => OnReleaseRequestState(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.ResolveRequestCache += (sender, e) => OnResolveRequestCache(new HttpContextWrapper(((HttpApplication)sender).Context)));
            WrapAttach(() => context.UpdateRequestCache += (sender, e) => OnUpdateRequestCache(new HttpContextWrapper(((HttpApplication)sender).Context)));
        }

        //HACK: 9-17-2010 
        //1.  I don't know how to determine if we're running on integrated pipeline -- which some of the above handlers require
        // -- hence the need for this try / catch
        //2.  Would like to use some more succint code like the commented function below -- but can't find a way to call it!
        //http://stackoverflow.com/questions/623716/how-can-i-pass-an-event-to-a-function-in-c
        private void WrapAttach(Action attachment)
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
        public void Dispose()
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
