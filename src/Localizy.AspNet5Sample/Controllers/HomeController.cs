using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Localizy.AspNet5Sample.Localizations;
using Microsoft.AspNetCore.Mvc;

namespace Localizy.AspNet5Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocalizationProvider _localizationProvider;

        public HomeController(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        public IActionResult Index()
        {
            ViewBag.GermanText = _localizationProvider.GetText(L.Home.APPLICATION_USES, new CultureInfo("de"));
            return View();
        }

        public class PostModel
        {
            [Required]
            public string Name { get; set; }
        }

        [HttpPost]
        public IActionResult Post(PostModel inputModel)
        {
            var state = ModelState;
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
