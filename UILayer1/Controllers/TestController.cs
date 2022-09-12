using Microsoft.AspNetCore.Mvc;

namespace UILayer.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
