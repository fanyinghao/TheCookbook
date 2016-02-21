using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.ViewModels;

namespace FYH.Cookbook.Service.Abstracts
{
    public interface IRecipeService
    {
        RecipeInfoViewModel GetRecipeInfo(int recipeId);

        void AddRecipe(RecipeInfoViewModel viewModel);

        void UpdateRecipe(RecipeInfoViewModel viewModel);

        void DeleteRecipe(int recipeId);
    }
}
