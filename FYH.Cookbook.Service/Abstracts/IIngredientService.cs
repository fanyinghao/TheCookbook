using FYH.Cookbook.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.Common;

namespace FYH.Cookbook.Service.Abstracts
{
    public interface IIngredientService
    {
        PagingResult<IngredientInfo> SerachIngredient(string keyword, int page, int rows);
    }
}
