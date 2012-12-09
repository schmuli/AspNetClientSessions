using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Schmulik.AspNetClientSession.Tests
{
    [TestClass]
    public class CookieHelperTests
    {
        [TestMethod]
        public void CookieHelper_WhenNoClientSessionCookie_ReturnNull()
        {
            var context = new PartialHttpContext();

            var cookieHelper = new CookieHelper("test cookie");

            var result = cookieHelper.Gets(context);

            Assert.IsNull(result);
            Assert.AreEqual(0, context.Request.Cookies.Count);
        }

        [TestMethod]
        public void CookieHelper_WhenClientSessionCookieExists_ReturnContent()
        {
            var content = "some content";

            var context = new PartialHttpContext();
            context.Request.Cookies.Add(new HttpCookie("test cookie", content));

            var helper = new CookieHelper("test cookie");
            var result = helper.Gets(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void CookieHelper_InsertClientSessionCookie()
        {
            var content = "some content";

            var context = new PartialHttpContext();

            var helper = new CookieHelper("test cookie");
            helper.Set(context, content);

            var result = context.Response.Cookies.Get("test cookie");

            Assert.AreEqual(result.Value, content);
        }
    }

    class PartialHttpContext : HttpContextBase
    {
        private HttpRequestBase _request;
        private HttpResponseBase _response;

        public override HttpRequestBase Request
        {
            get
            {
                return _request ?? (_request = new PartialHttpRequest());
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _response ?? (_response = new PartialHttpResponse());
            }
        }
    }

    class PartialHttpRequest : HttpRequestBase
    {
        private HttpCookieCollection _cookies;

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies ?? (_cookies = new HttpCookieCollection());
            }
        }
    }

    class PartialHttpResponse : HttpResponseBase
    {
        private HttpCookieCollection _cookies;

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies ?? (_cookies = new HttpCookieCollection());
            }
        }
    }

}