using Owin.Demo.Middleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy.Owin;
using Nancy;
using System.Web.Http;

namespace Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseDebugMiddleware(new DebugMiddlewareOptions
            {
                OnIncomingRequest = (ctx) =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    ctx.Environment["DebugStopWatch"] = watch;
                },
                OnOutgoingRequest = (ctx) =>
                {
                    var watch = (Stopwatch)ctx.Environment["DebugStopWatch"];
                    watch.Stop();
                    Debug.WriteLine("Request took: " + watch.ElapsedMilliseconds + " ms");
                }
            });
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new Microsoft.Owin.PathString("/Auth/Login")
            });
            app.UseFacebookAuthentication(new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
            {
                AppId = "1498507743777935",
                AppSecret = "ba49d8db081c6c6e1c1decc6519c94f",
                SignInAsAuthenticationType = "ApplicationCookie"
            });
            app.UseTwitterAuthentication(new Microsoft.Owin.Security.Twitter.TwitterAuthenticationOptions
            {
                ConsumerKey ="",
                ConsumerSecret = "",
                SignInAsAuthenticationType = "ApplicationCookie",
                BackchannelCertificateValidator = null
            });
            app.Use(async (ctx, next) =>
            {
                if (ctx.Authentication.User.Identity.IsAuthenticated)
                    Debug.WriteLine("User: " + ctx.Authentication.User.Identity.Name);
                else
                    Debug.WriteLine("User Not Authenticated");
                await next();
            });
            var conf = new HttpConfiguration();
            conf.MapHttpAttributeRoutes();
            app.UseWebApi(conf);
            app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); });
            //app.UseNancy();
            //app.UseNancy(config =>
            //{
            //    config.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            //});
            //app.Use(async (ctx, next) =>
            //{
            //    await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            //});
        }
    }
}