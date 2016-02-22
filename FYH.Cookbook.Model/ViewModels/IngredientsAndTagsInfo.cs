using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Model.ViewModels
{
    public class IngredientsAndTagsInfo
    {
        public List<TagInfo> Tags { get; set; }
        
        public List<IngredientInfo> Ingredients { get; set; }  
    }
}
