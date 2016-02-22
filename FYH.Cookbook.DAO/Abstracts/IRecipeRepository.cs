using System.Collections.Generic;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.Enum;
using FYH.Cookbook.Model.ViewModels;

namespace FYH.Cookbook.DAO.Abstracts
{
    public interface IRecipeRepository
    {
        PagingResult<RecipeListItemModel> SearchRecipe(string keyword, IEnumerable<int?> ingredientIds, IEnumerable<int?> tagIds, IDictionary<string, SqlSortedEnum> orders, int page, int rows);
    }
}
