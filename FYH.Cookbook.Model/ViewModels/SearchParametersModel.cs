using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.Enum;

namespace FYH.Cookbook.Model.ViewModels
{
    public class SearchParametersModel
    {
        public string Keyword { get; set; }

        public int Page { get; set; }

        public int Rows { get; set; }

        public string SortBy { get; set; }

        public SqlSortedEnum? Order { get; set; }
    }
}
