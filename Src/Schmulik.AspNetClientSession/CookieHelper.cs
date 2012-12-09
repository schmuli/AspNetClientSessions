using System.Linq;
using System.Web;

namespace Schmulik.AspNetClientSession
{
    /// <summary>
    /// A helper class for retrieving and inserting HTTP cookies.
    /// </summary>
    public class CookieHelper
    {
        private readonly string _cookieName;

        public CookieHelper(string cookieName)
        {
            _cookieName = cookieName;
        }

        /// <summary>
        /// Gets the contents of the HTTP cookie with the name specified.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>The contents of the HTTP cookie</returns>
        public string Gets(HttpContextBase context)
        {
            return Gets(context.Request.Cookies);
        }

        private string Gets(HttpCookieCollection cookies)
        {
            var containsCookie = cookies.AllKeys.Contains(_cookieName);
            if (!containsCookie)
            {
                return null;
            }

            var cookie = cookies.Get(_cookieName);
            return cookie != null ? cookie.Value : null;
        }

        /// <summary>
        /// Sets a HTTP cookie with the name specified.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content">The contents to add to the cookie</param>
        public void Set(HttpContextBase context, string content)
        {
            Set(context.Response.Cookies, content);
        }

        private void Set(HttpCookieCollection cookies, string content)
        {
            // Could set MaxAge, to allow persistent cookies
            var cookie = new HttpCookie(_cookieName, content)
            {
                HttpOnly = true
            };

            cookies.Set(cookie);
        }
    }
}
