using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using OpenAI_API;
using OpenAI_API.Completions;
using transiple.Models;

namespace transiple.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _openAIApiKey = "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";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(
                new TranslateVm()
                {
                    SourceLanguage = "tr-TR",
                    DestinationLanguage = "en-US,de-DE,fr-FR",
                    Text = "Bu uygulama chatgpt kullanılarak yapılmıştır"
                });
        }
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("translate")]
        public async Task<IActionResult> TranslateText(TranslateVm model)
        {
            var openAIClient = new OpenAIAPI(_openAIApiKey);

            var languageArray = model.DestinationLanguage.Split(",");
            var languages = string.Join(" and ", languageArray);
            var prompt = "Translate from " + model.SourceLanguage + " to " + languages + ": " + model.Text;
            CompletionRequest request = new CompletionRequest
            {
                Prompt = prompt + ". Insert <br> between translations.",
                MaxTokens = 4000,
                Model = OpenAI_API.Models.Model.DavinciText
            };

            var response = await openAIClient.Completions.CreateCompletionAsync(request);
            if (response != null)
            {
                model.Result = response.Completions.First().Text;
                return View("Index", model);
            }
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}