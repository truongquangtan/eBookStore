using Microsoft.AspNetCore.Mvc;

namespace BookStoreWebApp.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
