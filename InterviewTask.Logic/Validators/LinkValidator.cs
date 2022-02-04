using InterviewTask.CrawlerLogic.Services;
using InterviewTask.Logic.Exceptions;
using System;
using System.Threading.Tasks;

namespace InterviewTask.Logic.Validators
{
    public class LinkValidator
    {
        private readonly HttpService _httpService;

        public LinkValidator(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task CheckLinkAsync(Uri link)
        {
            if (link == null)
            {
                throw new InputLinkInvalidException();
            }

            if (!link.IsAbsoluteUri)
            {
                throw new InputLinkInvalidException();
            }

            await _httpService.GetResponseMessageAsync(link);
        }
    }
}
