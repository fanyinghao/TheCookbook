using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYH.Cookbook.Service.Abstracts;

namespace FYH.Cookbook.Web.Controllers
{
    public class TagController : Controller
    {
        private ITagService TagService { get; set; }
        // GET: Tag
        public ActionResult SearchTag(string keyword, int page, int rows)
        {
            return Json(TagService.SerachTag(keyword, page, rows));
        }
    }
}