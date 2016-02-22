using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;
using NHibernate.Criterion;

namespace FYH.Cookbook.Web.Controllers
{
    public class CrawlerController : Controller
    {
        private IRecipeService RecipeService { get; set; }

        // GET: Crawler
        public ActionResult NYTimesCookingCrawler(string listRequestUrl)
        {
            var request = WebRequest.CreateHttp(listRequestUrl);
            request.Method = "GET";
            request.Accept = "*/*";
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36";
            request.Host = "cooking.nytimes.com";
            //request.Proxy = new WebProxy(new Uri("http://127.0.0.1:8787"));
            var response = request.GetResponse();

            var html = string.Empty;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                html = sr.ReadToEnd();
            }

            foreach (Match match in Regex.Matches(html, "data-url=\"/recipes/(\\S+)\""))
            {
                var pageid = match.Groups[1].ToString();

                request = WebRequest.CreateHttp("http://cooking.nytimes.com/recipes/" + pageid);
                request.Method = "GET";
                request.Accept = "*/*";
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36";
                request.Host = "cooking.nytimes.com";
                //request.Proxy = new WebProxy(new Uri("http://127.0.0.1:8787"));
                response = request.GetResponse();

                html = string.Empty;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    html = sr.ReadToEnd();

                    var name = Regex.Match(html, "<h1 class=\"recipe-title title name\" data-name=\"(\\S.+)\"").Groups[1].ToString();
                    var description = Regex.Match(html, "<meta name=\"description\" content=\"(\\S.+)\"/>").Groups[1].ToString();
                    var directions = Regex.Match(html.Replace("\n", "").Replace("\r", ""), "<ol class=\"recipe-steps\" itemprop=\"recipeInstructions\">(.*?)</ol>").Groups[0].ToString();
                    var imageUrl = Regex.Match(html, "src=\"(.*?)-articleLarge.jpg").Groups[1].ToString();
                    var images = new List<ImageInfo>();
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        images.Add(new ImageInfo
                        {
                            Url = imageUrl + "-articleLarge.jpg"
                        });
                    }
                    var ingredients = new List<IngredientInfo>();
                    foreach (Match m in Regex.Matches(html.Replace("\n", "").Replace("\r", ""),
                        "<span class=\"quantity\">(.*?)</span>(\\s+)<span class=\"ingredient-name\">(.*?)</li>"))
                    {
                        var match_name = Regex.Match(m.Groups[3].ToString().Trim(), "(.*?)<span>(.*?)</span>");
                        if (match_name.Groups.Count >= 3)
                            ingredients.Add(new IngredientInfo
                            {
                                Quantity = m.Groups[1].ToString().Trim(),
                                Unit = match_name.Groups[1].ToString().Trim(),
                                Name = match_name.Groups[2].ToString().Trim()
                            });
                    }
                    var tags = new List<TagInfo>();
                    foreach (Match m in Regex.Matches(html, "<a id=\"(.*?)\" href=/tag/(.*?)>(.*?)</a>"))
                    {
                        tags.Add(new TagInfo
                        {
                            Name = m.Groups[3].ToString().Trim()
                        });
                    }

                    RecipeService.AddRecipe(new RecipeInfoViewModel
                    {
                        Name = name,
                        Description = description,
                        Directions = directions,
                        Images = images,
                        Ingredients = ingredients,
                        Tags = tags
                    });
                }
            }

            return Content(html);
        }
    }
}