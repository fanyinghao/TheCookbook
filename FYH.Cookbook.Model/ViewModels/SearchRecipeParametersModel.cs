using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Model.ViewModels
{
    public class SearchRecipeParametersModel : SearchParametersModel
    {
        public IEnumerable<int?> IngredientIds { get; set; }

        public IEnumerable<int?> TagIds { get; set; }
    }
}
