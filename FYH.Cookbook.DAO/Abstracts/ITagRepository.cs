using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.Enum;
using FYH.Cookbook.Model.DBEntity;

namespace FYH.Cookbook.DAO.Abstracts
{
    public interface ITagRepository
    {
        /// <summary>
        /// Get tags which has mapping with recipe by search result
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ingredientIds"></param>
        /// <param name="tagIds"></param>
        /// <param name="orders"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        PagingResult<Tag> SearchTag(string keyword, IEnumerable<int?> ingredientIds, IEnumerable<int?> tagIds, IDictionary<string, SqlSortedEnum> orders, int page, int rows);
    }
}
