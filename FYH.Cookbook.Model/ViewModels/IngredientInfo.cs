using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Model.ViewModels
{
    public class IngredientInfo
    {
        public int IngredientId { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public string Quantity { get; set; }
    }
}
