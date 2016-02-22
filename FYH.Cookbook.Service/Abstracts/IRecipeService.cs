using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.Enum;
using FYH.Cookbook.Model.ViewModels;

namespace FYH.Cookbook.Service.Abstracts
{
    public interface IRecipeService
    {
        RecipeInfoViewModel GetRecipeInfo(int recipeId);

        void AddRecipe(RecipeInfoViewModel viewModel);

        void UpdateRecipe(RecipeInfoViewModel viewModel);

        void DeleteRecipe(int recipeId);

        /// <summary>
        /// search recipe
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ingredientIds"></param>
        /// <param name="tagIds"></param>
        /// <param name="sortBy"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        PagingResult<RecipeListItemModel> SearchRecipe(string keyword, IEnumerable<int?> ingredientIds, IEnumerable<int?> tagIds, string sortBy, SqlSortedEnum order, int page, int rows);

        /// <summary>
        /// get ingredient and tag which has mapping with recipe by search
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ingredientIds"></param>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        IngredientsAndTagsInfo SearchIngredientsAndTags(string keyword, IEnumerable<int?> ingredientIds, IEnumerable<int?> tagIds);
    }
}
