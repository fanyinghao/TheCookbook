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
    public class TagService : ITagService
    {
        private IBaseRepository BaseRepository { get; set; }

        public PagingResult<TagInfo> SearchTag(string keyword, int page, int rows)
        {
            keyword = keyword ?? string.Empty;
            var list = BaseRepository.GetEntityPagingList<Tag>(
            string.IsNullOrEmpty(keyword) ? new List<ICriterion>() :
            new List<ICriterion>
            {
                Restrictions.Or(
                    Restrictions.Gt(Projections.SqlFunction("LOCATE",
                    NHibernateUtil.Int32,
                    Projections.Constant(keyword),
                    Projections.Property(Tag.COL_NAME)), 0),
                Restrictions.Gt(Projections.SqlFunction("LOCATE",
                    NHibernateUtil.Int32,
                    Projections.Constant(keyword),
                    Projections.Property(Tag.COL_DESCRIPTION)), 0))
            }, (page - 1) * rows, rows);

            return new PagingResult<TagInfo>
            {
                Data = list.Data.Select(i => new TagInfo
                {
                    TagId = i.TagId.GetValueOrDefault(),
                    Name = i.Name
                }).ToList(),
                TotalCount = list.TotalCount,
                Count = list.Count
            };
        }
    }
}
