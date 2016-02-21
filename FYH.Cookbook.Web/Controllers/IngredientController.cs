using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYH.Cookbook.Service.Abstracts;

namespace FYH.Cookbook.Web.Controllers
{
    public class IngredientController : Controller
    {
        private IIngredientService IngredientService { get; set; }
        // GET: Ingredient
        public ActionResult SearchIngredient(string keyword, int page, int rows)
        {
            return Json(IngredientService.SerachIngredient(keyword, page, rows));
        }
    }
}