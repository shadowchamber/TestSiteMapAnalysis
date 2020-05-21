using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestSiteMapAnalysis.Models;
using TestSiteMapAnalysis.Storage;

namespace TestSiteMapAnalysis.Controllers
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

        [HttpPost]
        public async Task<IActionResult> Analyze([FromForm] string url)
        {
            try
            {
                string sitemapUrl = url;

                if (!sitemapUrl.EndsWith("sitemap.xml"))
                {
                    sitemapUrl += "/sitemap.xml";
                }

                DataContext db = new DataContext();

                DbInitializer.Initialize(db);

                SitemapResult sitemapResult = new SitemapResult()
                {
                    Url = url,
                    AnalysisDate = DateTime.Now
                };

                db.SitemapResults.Add(sitemapResult);

                string sitemapXml;

                sitemapXml = await this.GetSitemap(sitemapUrl);

                XmlDocument sitemap = new XmlDocument();

                sitemap.LoadXml(sitemapXml);

                foreach (XmlNode urlnode in sitemap.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode locnode in urlnode.ChildNodes)
                    {
                        if (locnode.Name != "loc")
                        {
                            continue;
                        }

                        string checkingUrl = locnode.InnerText.Trim();
                        double responseTime = await this.CheckUrlAsync(checkingUrl);

                        SitemapItemResult itemResult = new SitemapItemResult()
                        {
                            ItemUrl = checkingUrl,
                            RespponseTimeMS = responseTime,
                            SitemapResult = sitemapResult
                        };

                        db.SitemapItemResults.Add(itemResult);
                    }
                }

                await db.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                ViewBag.Exception = exception;

                return View("AnalizeError");
            }

            return View();
        }

        public async Task<string> StreamToStringAsync(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> GetSitemap(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string res = await this.StreamToStringAsync(response.GetResponseStream()).ConfigureAwait(false);

            response.Close();

            return res;
        }

        public async Task<double> CheckUrlAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            System.Diagnostics.Stopwatch timer = new Stopwatch();

            timer.Start();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            return timeTaken.TotalMilliseconds;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
