using System;
using System.Web;

using Petecat.DependencyInjection;

namespace Petecat.HttpServer
{
    public class HttpRedirectModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(HttpRedirectModule_BeginRequest);
        }

        private void HttpRedirectModule_BeginRequest(object sender, EventArgs e)
        {
            var context = (sender as HttpApplicationBase).Context;

            var redirect = DependencyInjector.GetObject<IHttpApplicationConfigurer>().ApplyHttpRedirect(context.Request.Url.AbsoluteUri);
            if (redirect != null)
            {
                context.Response.Redirect(redirect);
            }
        }

        public void Dispose()
        {
        }
    }
}
