using System.Linq;
using System.Web.Mvc;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;

namespace FYH.Cookbook.Web.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeService RecipeService { get; set; }

        // GET: Recipe Detail Page
        public ActionResult Index(int? recipeId)
        {
            if (!recipeId.HasValue) return Create();
            var viewModel = RecipeService.GetRecipeInfo(recipeId.Value);
            return View(viewModel);
        }

        // GET: Recipe Edit Page
        public ActionResult Edit(int? recipeId)
        {
            if (!recipeId.HasValue) return Create();
            var viewModel = RecipeService.GetRecipeInfo(recipeId.Value);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(RecipeInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
                RecipeService.UpdateRecipe(viewModel);
            return View(viewModel);
        }

        // GET: Recipe Create Page
        public ActionResult Create()
        {
            return View(new RecipeInfoViewModel());
        }

        [HttpPost]
        public ActionResult Create(RecipeInfoViewModel viewModel)
        {
            viewModel.Ingredients = viewModel.Ingredients.Where(i => i.IngredientId > 0).ToList();
            if (!ModelState.IsValid) return View(viewModel);
            RecipeService.AddRecipe(viewModel);
            return View("Index", viewModel);
        }
    }
}