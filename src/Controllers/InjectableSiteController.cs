using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace dotnet_test_core_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InjectableSiteController : ControllerBase
    {
        private readonly ILogger<InjectableSiteController> _logger;

        public InjectableSiteController(ILogger<InjectableSiteController> logger)
        {
            _logger = logger;
        }

        private dynamic GetSubStringBetween(string content, string startStr, string endStr, int offset = 0)
        {
            var indexStart = content.IndexOf(startStr, offset) + startStr.Length;
            var indexEnd = content.IndexOf(endStr, indexStart);

            if (endStr == string.Empty)
            {
                indexEnd = content.Length;
            }

            if (indexStart == -1 || indexEnd == -1)
            {
                throw new Exception(string.Format("Can't find startStr or endStr, indexStart = {0}, indexEnd = {1}", indexStart, indexEnd));
            }

            var result = content.Substring(indexStart, indexEnd - indexStart);
            return new
            {
                result,
                indexEnd
            };
        }


        [HttpGet]
        public ContentResult Get(int? id)
        {
            var urls = new List<string>() {
                "https://1drv.ms/w/s!AvUFW-54mQzQjCztCq3VUMFPNs5p",
                "https://1drv.ms/w/s!AvUFW-54mQzQjBkZl-5L50sIYrTM",
                "https://1drv.ms/w/s!AvUFW-54mQzQjBjpS1xoVPH1sVz_"
            };

            if (id == null)
            {
                return Content(null, "text/html", Encoding.UTF8);
            }

            // read a url
            HttpClient client = new HttpClient();
            var response = client.GetAsync(urls[id.Value]).Result;
            var pageContents = response.Content.ReadAsStringAsync().Result;
            var url = GetSubStringBetween(pageContents, "content=\"0;url=", "\"");

            // read a page
            string wordPageUrl = System.Net.WebUtility.HtmlDecode(url.result);
            //wordPageUrl = wordPageUrl.Replace("view", "edit");


            response = client.GetAsync(wordPageUrl).Result;
            pageContents = response.Content.ReadAsStringAsync().Result;

            // url = GetSubStringBetween(pageContents, "content=\"0;url=", "\"");
            // wordPageUrl = System.Net.WebUtility.HtmlDecode(url.result);
            // response = client.GetAsync(wordPageUrl).Result;
            // pageContents = response.Content.ReadAsStringAsync().Result;

            return Content(pageContents, "text/html", Encoding.UTF8);
        }
    }
}
