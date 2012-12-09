using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schmulik.AspNetClientSession.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            // Simple Example
            // Gets the session object, and if the 'FirstName' property is null,
            // i.e. the session doesn't yet exist, creates the properties and
            // and returns a simple response.
            // On the next request, when the session already exists, it just reads
            // the 'FirstName' property already set on the previous request.

            dynamic session = HttpContext.Session();
            if (session.FirstName == null)
            {
                session.FirstName = "Schmulik";
            }

            return Content("This is a test of client session, FirstName: " +
                           session.FirstName);
        }
    }
}
