using System.Web.Mvc;

namespace Interactive.HateBin.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Approved")]
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
