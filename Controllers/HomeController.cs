using laba13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace laba13.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Quiz()
        {
            var model = GenerateRandomNumbers();
            SaveToSession(model);

            ViewBag.Result = false;
            return View(model);
        }

        [HttpPost]
        public IActionResult Quiz(Numbers model, string submitButton)
        {
            var answerList = GetFromSession();

            var lastQuestion = answerList[^1];
            model.enter ??= lastQuestion.enter;
            model.success = (lastQuestion.result == model.enter);

            lastQuestion.enter = model.enter;
            lastQuestion.success = model.success;

            SaveToSession(answerList);

            if (submitButton == "Finish")
            {
                ViewBag.AnswerList = answerList;
                ViewBag.CorrectAnswers = answerList.Count(a => a.success);
                ViewBag.Result = true;
            }
            else
            {
                var newModel = GenerateRandomNumbers();
                SaveToSession(newModel);
                ViewBag.Result = false;
                model = newModel;
            }

            return View("Quiz", model);
        }

        public IActionResult QuizResult()
        {
            var answerList = GetFromSession();
            ViewBag.AnswerList = answerList;
            ViewBag.CorrectAnswers = answerList.Count(a => a.success);
            ViewBag.Result = true;

            return View("Quiz");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Numbers GenerateRandomNumbers()
        {
            var rand = new Random();
            var operations = new List<string> { "+", "-", "*", "/" };

            var model = new Numbers
            {
                num1 = rand.Next(0, 11),
                num2 = rand.Next(1, 11),
                operation = operations[rand.Next(operations.Count)]
            };

            model.result = model.operation switch
            {
                "+" => (model.num1 + model.num2).ToString(),
                "-" => (model.num1 - model.num2).ToString(),
                "*" => (model.num1 * model.num2).ToString(),
                "/" => model.num2 != 0 ? (model.num1 / model.num2).ToString() : "ERROR",
                _ => "ERROR"
            };

            model.enter = "0";
            model.success = false;

            return model;
        }

        private void SaveToSession(Numbers model)
        {
            var answerList = GetFromSession();
            answerList.Add(model);
            HttpContext.Session.SetString("AnswerList", JsonSerializer.Serialize(answerList));
        }

        private void SaveToSession(List<Numbers> answerList)
        {
            HttpContext.Session.SetString("AnswerList", JsonSerializer.Serialize(answerList));
        }

        private List<Numbers> GetFromSession()
        {
            var sessionData = HttpContext.Session.GetString("AnswerList");
            return sessionData != null
                ? JsonSerializer.Deserialize<List<Numbers>>(sessionData)
                : new List<Numbers>();
        }
    }
}
