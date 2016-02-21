using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;
using NHibernate;
using NHibernate.Criterion;

namespace FYH.Cookbook.Service.Concretes
{
    public class IngredientService : IIngredientService
    {
        private IBaseRepository BaseRepository { get; set; }

        public PagingResult<IngredientInfo> SerachIngredient(string keyword, int page, int rows)
        {
            var list = BaseRepository.GetEntityPagingList<Ingredient>(new List<ICriterion>
            {
                Restrictions.Or(
                    Restrictions.Gt(Projections.SqlFunction("LOCATE",
                    NHibernateUtil.Int32,
                    Projections.Constant(keyword),
                    Projections.Property(Ingredient.COL_NAME)), 0),
                Restrictions.Gt(Projections.SqlFunction("LOCATE",
                    NHibernateUtil.Int32,
                    Projections.Constant(keyword),
                    Projections.Property(Ingredient.COL_DESCRIPTION)), 0))
            }, (page - 1) * rows, rows);

            return new PagingResult<IngredientInfo>
            {
                Data = list.Data.Select(i => new IngredientInfo
                {
                    IngredientId = i.IngredientId.GetValueOrDefault(),
                    Name = i.Name
                }).ToList(),
                TotalCount = list.TotalCount,
                Count = list.Count
            };
        }
    }
}
