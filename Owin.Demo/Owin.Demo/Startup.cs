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
            var conf = new HttpConfiguration();
            conf.MapHttpAttributeRoutes();
            app.UseWebApi(conf);
            //app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); });
            //app.UseNancy();
            app.UseNancy(config =>
            {
                config.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            });
            app.Use(async (ctx, next) =>
            {
                await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            });
        }
    }
}