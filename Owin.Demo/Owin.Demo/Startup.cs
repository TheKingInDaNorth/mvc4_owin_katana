using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) => {
                Debug.WriteLine("Incoming request:" + ctx.Request.Path);
                await next();
                Debug.WriteLine("Outgoing request:" + ctx.Request.Path);
            });
            app.Use(async (ctx, next) => {
                await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            });
        }
    }
}