using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pluralsight.Owin.Demo.Hosting;

namespace Pluralsight.Owin.Demo.Tests
{
    [TestClass]
    public class OwinTests
    {
        [TestMethod]
        public async Task Owin_returns_200_on_request_to_root()
        {
            var statusCode = await CallServer(async x =>
            {
                var response = await x.GetAsync("/");
                return response.StatusCode;
            });
            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            //using (var server = TestServer.Create<Startup>())
            //{
            //    var response = await server.HttpClient.GetAsync("/");
            //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //}
        }

        [TestMethod]
        public async Task Owin_returns_hello_world_on_request_to_root()
        {
            var body = await CallServer(async x => {
                var response = await x.GetAsync("/");
                return await response.Content.ReadAsStringAsync();
            });
            Assert.AreEqual("Hello World", body);
            //using (var server = TestServer.Create<Startup>())
            //{
            //    var response = await server.HttpClient.GetAsync("/");
            //    var body = await response.Content.ReadAsStringAsync();
            //    Assert.AreEqual("Hello World", body);
            //}
        }

        [TestMethod]
        public async Task Owin_returns_correct_contenttype_on_request_to_jpg()
        {
            var contenttype = await CallServer(async x =>
            {
                var response = await x.GetAsync("/test image 2.jpg");
                return response.Content.Headers.ContentType.MediaType;

            });
            Assert.AreEqual("image/jpeg", contenttype);
            //using (var server = TestServer.Create<Startup>())
            //{
            //    var response = await server.HttpClient.GetAsync("/test image 2.jpg");
            //    var contenttype = response.Content.Headers.ContentType.MediaType;
            //    Assert.AreEqual("image/jpg", contenttype);
            //}
        }

        [TestMethod]
        private async Task<T> CallServer<T>(Func<HttpClient, Task<T>> callback)
        {
            using (var server = TestServer.Create<Startup>())
            {
                return await callback(server.HttpClient);
            }
        }
    }
}
