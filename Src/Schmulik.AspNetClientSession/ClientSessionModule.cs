using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Schmulik.AspNetClientSession;

// Ensures the Module is registered on application start.
[assembly : WebActivator.PreApplicationStartMethod(typeof(ClientSessionModule), "Start")]

namespace Schmulik.AspNetClientSession
{
    /// <summary>
    /// The HTTP Module that is alerted on pre/post request.
    /// </summary>
    public class ClientSessionModule : IHttpModule
    {
        /// <summary>
        /// Tracks initialization, so event registration only happens once.
        /// </summary>
        private bool _hasInitialized;

        /// <summary>
        /// Registers callbacks for pre/post requests, on application initialization.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            if (_hasInitialized)
            {
                return;
            }
            _hasInitialized = true;

            context.PreRequestHandlerExecute += OnPreRequest;
            context.PreSendRequestHeaders += OnPostRequest;
        }

        /// <summary>
        /// Passes the application to the ClientSession.
        /// </summary>
        /// <param name="sender">The HTTP Application</param>
        /// <param name="e"></param>
        private static void OnPreRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;

            if (application != null)
            {
                ClientSession.ProcessPreRequest(application);
            }
        }

        /// <summary>
        /// Passes the application to the ClientSession.
        /// </summary>
        /// <param name="sender">The HTTP Application.</param>
        /// <param name="e"></param>
        private static void OnPostRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;

            if (application != null)
            {
                ClientSession.ProcessPostRequest(application);
            }
        }

        // ReSharper disable UnusedMember.Local
        /// <summary>
        /// Used by the PreApplicationStartupMethod Attribute, to register
        /// the ASP.NET Module.
        /// </summary>
        private static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(ClientSessionModule));
        }
        // ReSharper restore UnusedMember.Local

        public void Dispose()
        {
        }
    }
}
