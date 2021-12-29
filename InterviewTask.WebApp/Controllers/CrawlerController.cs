using InterviewTask.CrawlerServices.Services;
using InterviewTask.DatabaseServices.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace InterviewTask.WebApp.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly WebsiteCrawler _crawler;
        private readonly LinkRequest _linkRequest;
        private readonly DatabaseOperation _databaseOperation;

        public CrawlerController(WebsiteCrawler crawler, LinkRequest linkRequest, DatabaseOperation databaseOperation)
        {
            _crawler = crawler;
            _linkRequest = linkRequest;
            _databaseOperation = databaseOperation;
        }

        [HttpGet]
        public IActionResult Input()
        {
            ViewData["completed"] = "wait";
            return View(model: "Exmaple: https:/example.com/");
        }

        [HttpPost]
        public IActionResult Input(Uri inputLink)
        {
            if (inputLink == null && ModelState.ErrorCount == 0)
            {
                ModelState.AddModelError("inputLink", "Input value equal null");
            }
            else if (ModelState.ErrorCount == 0)
            {
                try
                {
                    WebRequest.Create(inputLink).GetResponse().Close();
                }
                catch
                {
                    ModelState.AddModelError("inputLink", "Link dont work");
                }
            }

            if (ModelState.IsValid)
            {
                var listAllLinks = _crawler.Start(inputLink);
                var listAllLinksWithResponse = _linkRequest.GetListWithLinksResponseTime(listAllLinks);

                _databaseOperation.SaveToDatabase(inputLink, listAllLinks, listAllLinksWithResponse);

                ViewData["completed"] = "success";
                return View(model: "Parsing completed successfully");
            }

            ViewData["completed"] = "fail";
            return View(model: "Parsing failed");
        }
    }
}
