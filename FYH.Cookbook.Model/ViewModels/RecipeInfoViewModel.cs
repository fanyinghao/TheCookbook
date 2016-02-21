using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FYH.Cookbook.Model.ViewModels
{
    public class RecipeInfoViewModel
    {
        public RecipeInfoViewModel()
        {
            Images = new List<ImageInfo>();
            Tags = new List<TagInfo>();
            Ingredients = new List<IngredientInfo>();
        }

        public int? RecipeId { get; set; }

        [Description("Recipe Name")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Description("Description")]
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }

        [Description("Cooking Directions")]
        [Required(ErrorMessage = "Required")]
        public string Directions { get; set; }

        [Description("Images")]
        public List<ImageInfo> Images { get; set;}

        [Description("TagIds")]
        public int[] TagIds { get; set; }

        [Description("Tags")]
        public List<TagInfo> Tags { get; set; } 

        [Description("Ingredients")]
        public List<IngredientInfo> Ingredients { get; set; } 
    }
}
