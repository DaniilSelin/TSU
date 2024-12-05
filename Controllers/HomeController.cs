using laba12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace laba12.Controllers
{
    public class OperationViewModel
    {
        public Numbers Numbers { get; set; }
        public IEnumerable<SelectListItem> Operations { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly IEnumerable<Operations> operations = new List<Operations>
        {
            new Operations("add", "+"),
            new Operations("sub", "-"),
            new Operations("mult", "*"),
            new Operations("div", "/")
        };

        private string CalculateResult(int num1, int num2, string operation)
        {
            return operation switch
            {
                "add" => (num1 + num2).ToString(),
                "sub" => (num1 - num2).ToString(),
                "mult" => (num1 * num2).ToString(),
                "div" => num2 != 0 ? (num1 / num2).ToString() : "ERROR",
                _ => "ERROR",
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManualSingle()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManualSeparate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManualSeparateResult()
        {
            int num1 = int.Parse(Request.Form["num1"]);
            int num2 = int.Parse(Request.Form["num2"]);
            string operation = Request.Form["operation"];

            Numbers model = new Numbers
            {
                num1 = num1,
                num2 = num2,
                operation = operation switch
                {
                    "add" => "+",
                    "sub" => "-",
                    "mult" => "*",
                    "div" => "/",
                    _ => "?"
                },
                result = CalculateResult(num1, num2, operation)
            };

            return View("Result", model);
        }

        public IActionResult ModelParameters()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ModelSeparate()
        {
            var viewModel = new OperationViewModel
            {
                Numbers = new Numbers(),
                Operations = operations.Select(op => new SelectListItem
                {
                    Text = op.Value,
                    Value = op.Text
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ModelSeparate(Numbers model)
        {
            model.result = CalculateResult(model.num1, model.num2, model.operation);
            model.operation = model.operation switch
            {
                "add" => "+",
                "sub" => "-",
                "mult" => "*",
                "div" => "/",
                _ => "?"
            };

            return View("Result2", model);
        }

        [HttpGet, HttpPost]
        public IActionResult Result(int num1, int num2, string operation)
        {
            Numbers model = new Numbers
            {
                num1 = num1,
                num2 = num2,
                operation = operation switch
                {
                    "add" => "+",
                    "sub" => "-",
                    "mult" => "*",
                    "div" => "/",
                    _ => "?"
                },
                result = CalculateResult(num1, num2, operation)
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
