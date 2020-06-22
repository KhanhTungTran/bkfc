using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace bkfc.Controllers
{
    public class HelloWorldController : Controller
    {
        // Every public method in a controller is callable as an HTTP endpoint.
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }
        // The preceding code calls the controller's View method. It uses a view template to generate an HTML response

        // GET: /HelloWorld/Welcome/
        // requires using System.Text.Encodings.Web;
        // The MVC model binding system automatically maps the named parameters from the query string in the address bar to parameters in your method.
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello" + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}