using WebApplication3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Numbers _numbers;

        public HomeController(ILogger<HomeController> logger, Numbers numbers)
        {
            _logger = logger;
            _numbers = numbers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PassUsingModel()
        {
            Numbers model = new Numbers();
            Random random = new Random();
            model.num1 = random.Next(0, 11);
            model.num2 = random.Next(0, 11);
            Calc_Model(model);
            return View(model);
        }

        public IActionResult PassUsingViewData()
        {
            Random random = new Random();
            int num1 = random.Next(0, 11);
            int num2 = random.Next(0, 11);
            ViewData["Numbers"] = new Numbers()
            {
                num1 = num1,
                num2 = num2,
                add = Convert.ToString(num1 + num2),
                sub = Convert.ToString(num1 - num2),
                mult = Convert.ToString(num1 * num2),
                div = num2 != 0 ? Convert.ToString(num1 / num2) : "ERROR"
            };
            return View();
        }

        public IActionResult PassUsingViewBag()
        {
            Random random = new Random();
            int num1 = random.Next(0, 11);
            int num2 = random.Next(0, 11);
            ViewBag.Numbers = new Numbers()
            {
                num1 = num1,
                num2 = num2,
                add = Convert.ToString(num1 + num2),
                sub = Convert.ToString(num1 - num2),
                mult = Convert.ToString(num1 * num2),
                div = num2 != 0 ? Convert.ToString(num1 / num2) : "ERROR"
            };
            return View();
        }

        public IActionResult AccessServiceDirectly()
        {
            Random random = new Random();
            int num1 = random.Next(0, 11);
            int num2 = random.Next(0, 11);
            _numbers.num1 = num1;
            _numbers.num2 = num2;
            _numbers.add = Convert.ToString(num1 + num2);
            _numbers.sub = Convert.ToString(num1 - num2);
            _numbers.mult = Convert.ToString(num1 * num2);
            _numbers.div = num2 != 0 ? Convert.ToString(num1 / num2) : "ERROR";
            ViewData["Numbers"] = _numbers;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        private void Calc_Model(Numbers model)
        {
            model.add = Convert.ToString(model.num1 + model.num2);
            model.sub = Convert.ToString(model.num1 - model.num2);
            model.mult = Convert.ToString(model.num1 * model.num2);
            model.div = model.num2 != 0 ? Convert.ToString(model.num1 / model.num2) : "ERROR";
        }
    }
}
