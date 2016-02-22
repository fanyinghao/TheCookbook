using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.Enum;

namespace FYH.Cookbook.DAO.Concretes
{
    public class TagRepository : ITagRepository
    {
        private IBaseRepository BaseRepository { get; set; }

        public PagingResult<Tag> SearchTag(string keyword, IEnumerable<int?> ingredientIds, IEnumerable<int?> tagIds, IDictionary<string, SqlSortedEnum> orders, int page, int rows)
        {

            var sql = new StringBuilder(@"
select * from Tag t
where exists(
    select * from Recipe r, RecipeTagMapping rtm
    where r.IsDeleted = 0 and rtm.TagId = t.TagId and r.RecipeId = rtm.RecipeId 
");
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(@" and (charindex(:keyword, r.Name, 0) > 0 or 
		                            charindex(:keyword, r.[Description], 0) > 0 or
		                            exists(select * from 
				                            RecipeIngredientMapping rim, Ingredient i 
				                            where i.IngredientId = rim.IngredientId and 
					                              rim.RecipeId = r.RecipeId and 
					                              (charindex(:keyword, i.Name, 0) > 0 or 
					                              charindex(:keyword, i.[Description], 0) > 0)) or
		                            exists(select * from 
				                            RecipeTagMapping rtm, Tag t 
				                            where t.TagId = rtm.TagId and 
					                              rtm.RecipeId = r.RecipeId and 
					                              (charindex(:keyword, t.Name, 0) > 0 or 
					                              charindex(:keyword, t.[Description], 0) > 0))
                                )");
                parameters.Add("keyword", keyword);
            }

            if (ingredientIds != null)
            {
                var ingredientList = ingredientIds.Where(i => i > 0).ToList();
                if (ingredientList.Any())
                {
                    sql.AppendFormat(@" and (select count(1) from
				    RecipeIngredientMapping rim
				    where rim.IngredientId in ({0}) and rim.RecipeId = r.RecipeId) >= {1} ", string.Join(",", ingredientList), ingredientList.Count);
                }
            }

            if (tagIds != null)
            {
                var tagList = tagIds.Where(i => i > 0).ToList();
                if (tagList.Any())
                {
                    sql.AppendFormat(@" and (select count(1) from
				    RecipeTagMapping rtm
				    where rtm.TagId in ({0}) and rtm.RecipeId = r.RecipeId) >= {1} ", string.Join(",", tagList), tagList.Count);
                }
            }

            sql.Append(" )");

            return BaseRepository.ExecutePagingList<Tag>(sql.ToString(), orders, parameters, (page - 1) * rows, rows);
        }
    }
}
