using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;

namespace FYH.Cookbook.Web.Controllers
{
    public class IngredientController : Controller
    {
        private IIngredientService IngredientService { get; set; }
        // GET: Ingredient
        public ActionResult SearchIngredient(SearchParametersModel parameters)
        {
            return Json(IngredientService.SearchIngredient(parameters.Keyword, parameters.Page, parameters.Rows));
        }
    }
}