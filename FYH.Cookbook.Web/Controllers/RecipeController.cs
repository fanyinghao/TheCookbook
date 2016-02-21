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

        // GET: Recipe Create Page
        public ActionResult Create()
        {
            return View("Edit", new RecipeInfoViewModel());
        }

        // GET: Recipe Edit Page
        public ActionResult Edit(int? recipeId)
        {
            if (!recipeId.HasValue) return Create();
            var viewModel = RecipeService.GetRecipeInfo(recipeId.Value);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(RecipeInfoViewModel viewModel)
        {
            viewModel.Ingredients = viewModel.Ingredients.Where(i => i.IngredientId > 0).ToList();
            if (!ModelState.IsValid) return View(viewModel);
            if (viewModel.RecipeId > 0)
                RecipeService.UpdateRecipe(viewModel);
            else
                RecipeService.AddRecipe(viewModel);
            return Redirect("/Recipe?RecipeId=" + viewModel.RecipeId);
        }

        public ActionResult Delete(int recipeId)
        {
            RecipeService.DeleteRecipe(recipeId);
            return Redirect("/Home");
        }
    }
}