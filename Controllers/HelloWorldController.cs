using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace bkfc.Controllers
{
    public class HelloWorldController: Controller
    {
        // GET: /HelloWorld/
        public string Index()
        {
            return "This is my default action...";
        }

        // GET: /HelloWorld/Welcome/
        public string Welcome() {
            return "This is the welcome action method";
        }
    }
}