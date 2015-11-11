using System.Web.Mvc;

namespace Interactive.HateBin.Controllers
{
    [Authorize(Roles = "Approved")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {                 
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
