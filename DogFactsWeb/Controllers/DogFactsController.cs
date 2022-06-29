using DogFactsWeb.Models;
using DogFactsWeb.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace DogFactsWeb.Controllers
{
    public class DogFactsController : Controller
    {
        private readonly DogFactsContract _dogFactsContract;

        public DogFactsController(DogFactsContract dogFactsContract)
        {
            _dogFactsContract = dogFactsContract;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DogsConsult(DogFactsVM dogFactsVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _dogFactsContract.GetDogFacts(dogFactsVM);

                if (response.Ok)
                {
                    ViewBag.Message = response.Message;
                    return View(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = response.Errors;
                    return View("Error");
                }
            }

            return View(nameof(Index), dogFactsVM);
        }
    }
}