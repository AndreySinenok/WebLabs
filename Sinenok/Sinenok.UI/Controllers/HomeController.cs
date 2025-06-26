using Microsoft.AspNetCore.Mvc;
using Sinenok.UI.Models;
using System.Diagnostics;

namespace Sinenok.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
