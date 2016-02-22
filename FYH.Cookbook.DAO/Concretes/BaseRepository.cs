using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FYH.Cookbook.Core.Attributes;
using FYH.Cookbook.Core.Caches;
using Spring.Data.NHibernate.Generic.Support;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.Common;
using FYH.Cookbook.Model.Enum;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;

namespace FYH.Cookbook.DAO.Concretes
{
    public class BaseRepository : HibernateDaoSupport, IBaseRepository
    {
        public int AddEntity(object entity)
        {
            return Convert.ToInt32(HibernateTemplate.Save(entity));
        }

        public void AddOrUpdateEntity(object entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
        }

        public int ExecuteUpdate(string sql)
        {
            return ExecuteUpdate(sql, null);
        }

        public int ExecuteUpdate(string sql, IDictionary<string, object> parameters)
        {
            return HibernateTemplate.Execute(session =>
            {
                // Create SQL Query
                var query = session.CreateSQLQuery(sql);

                // Parameters
                if (parameters != null && parameters.Count != 0)
                    foreach (var parameter in parameters)
                        query.SetParameter(parameter.Key, parameter.Value);

                HibernateTemplate.PrepareQuery(query);
                return query.ExecuteUpdate();
            });
        }

        public void UpdateEntity(object entity)
        {
            HibernateTemplate.Update(entity);
        }

        public void DeleteEntity(object entity)
        {
            HibernateTemplate.Delete(entity);
        }

        public void DeleteEntityById<T>(object id)
        {
            var entity = GetEntityById<T>(id);

            if (entity != null)
                DeleteEntity(entity);
        }

        public T GetEntityById<T>(object id)
        {
            return HibernateTemplate.Get<T>(id);
        }

        public IList<T> GetEntityList<T>(int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityList<T>(null, null, firstIndex, maxCount, useSecondLevelCache);
        }
        public IList<T> GetEntityList<T>(IEnumerable<ICriterion> criterions, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityList<T>(criterions, null, firstIndex, maxCount, useSecondLevelCache);
        }
        public IList<T> GetEntityList<T>(IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityList<T>(null, orders, firstIndex, maxCount, useSecondLevelCache);
        }
        public IList<T> GetEntityList<T>(IEnumerable<ICriterion> criterions, IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            if (!useSecondLevelCache)
                HibernateTemplate.CacheQueries = false;
            return HibernateTemplate.ExecuteFind(session =>
            {
                // Create Entity Criteria
                var criteria = session.CreateCriteria(typeof(T));

                // Set criterions
                if (criterions != null && criterions.Any())
                    foreach (var criterion in criterions)
                        criteria.Add(criterion);

                // Set orders
                if (orders != null && orders.Any())
                    foreach (var order in orders)
                        criteria.AddOrder(order);

                // Set range
                criteria.SetFirstResult(firstIndex);
                criteria.SetMaxResults(maxCount);

                HibernateTemplate.PrepareCriteria(criteria);

                return criteria.List<T>();
            });
        }

        public PagingResult<T> GetEntityPagingList<T>(int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityPagingList<T>(null, null, firstIndex, maxCount, useSecondLevelCache);
        }
        public PagingResult<T> GetEntityPagingList<T>(IEnumerable<ICriterion> criterions, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityPagingList<T>(criterions, null, firstIndex, maxCount, useSecondLevelCache);
        }
        public PagingResult<T> GetEntityPagingList<T>(IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            return GetEntityPagingList<T>(null, orders, firstIndex, maxCount, useSecondLevelCache);
        }
        public PagingResult<T> GetEntityPagingList<T>(IEnumerable<ICriterion> criterions, IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true)
        {
            if (!useSecondLevelCache)
                HibernateTemplate.CacheQueries = false;
            return HibernateTemplate.Execute(session =>
            {
                // Crete Entity Criteria
                var countCriteria = session.CreateCriteria(typeof(T));
                var contentCriteria = session.CreateCriteria(typeof(T));

                // Set criterions
                if (criterions != null && criterions.Count() != 0)
                    foreach (var criterion in criterions)
                    {
                        countCriteria.Add(criterion);
                        contentCriteria.Add(criterion);
                    }

                // Set Orders
                if (orders != null && orders.Count() != 0)
                    foreach (var order in orders)
                        contentCriteria.AddOrder(order);

                // Set count
                countCriteria.SetProjection(Projections.RowCount());

                // Set range
                contentCriteria.SetFirstResult(firstIndex);
                contentCriteria.SetMaxResults(maxCount);

                HibernateTemplate.PrepareCriteria(countCriteria);
                HibernateTemplate.PrepareCriteria(contentCriteria);

                var count = countCriteria.UniqueResult<int>();
                var data = contentCriteria.List<T>();
                return new PagingResult<T>
                {
                    Count = maxCount,
                    TotalCount = count,
                    Data = data
                };
            });
        }

        /// <summary>
        /// Convert Scalar
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IDictionary<string, IType> ConvertTypeScalar(Type type)
        {
            IDictionary<string, IType> scalars = null;
            var cacheKey = string.Format("{0}{1}", "TYPESCALARS_", type.FullName);
            if (CacheManager.MemoryCacheManager.Contains(cacheKey))
                scalars = CacheManager.MemoryCacheManager.Get(cacheKey) as IDictionary<string, IType>;
            else
            {
                scalars = new Dictionary<string, IType>();
                if (!type.IsValueType && type != typeof(string))
                {
                    type.GetProperties()
                        .ToList()
                        .ForEach(p =>
                        {
                            if (p.GetCustomAttribute<IgnoreConvertTypeScalarAttribute>() == null)
                                scalars.Add(p.Name, NHibernateUtil.GuessType(p.PropertyType));
                        });
                }
                CacheManager.MemoryCacheManager.Set(cacheKey, scalars, CacheExpirationTypeEnum.None, null);
            }

            return scalars;
        }

        public PagingResult<T> ExecutePagingList<T>(string sql, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            var scalars = ConvertTypeScalar(typeof(T));
            return ExecutePagingList<T>(sql, null, scalars, null, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, SqlSortedEnum> orders, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            var scalars = ConvertTypeScalar(typeof(T));
            return ExecutePagingList<T>(sql, orders, scalars, null, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, IType> scalars, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            return ExecutePagingList<T>(sql, null, scalars, null, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, object> parameters, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            var scalars = ConvertTypeScalar(typeof(T));
            return ExecutePagingList<T>(sql, null, scalars, parameters, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, SqlSortedEnum> orders, IDictionary<string, IType> scalars, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            return ExecutePagingList<T>(sql, orders, scalars, null, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, SqlSortedEnum> orders, IDictionary<string, object> parameters, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            var scalars = ConvertTypeScalar(typeof(T));
            return ExecutePagingList<T>(sql, orders, scalars, parameters, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, IType> scalars, IDictionary<string, object> parameters, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            return ExecutePagingList<T>(sql, null, scalars, parameters, firstIndex, maxCount);
        }
        public PagingResult<T> ExecutePagingList<T>(string sql, IDictionary<string, SqlSortedEnum> orders, IDictionary<string, IType> scalars, IDictionary<string, object> parameters, int firstIndex = 0, int maxCount = int.MaxValue)
        {
            return HibernateTemplate.Execute(session =>
            {
                var countQuery = session.CreateSQLQuery(string.Format("SELECT count (*) FROM ({0}) pagingcount", sql));
                if (orders != null && orders.Count != 0)
                {
                    sql = string.Format(
                        "{0} ORDER BY {1}",
                        sql,
                        string.Join(", ", orders.Select(o => string.Format("{0} {1}", o.Key, o.Value.ToString())))
                    );
                }
                var contentQuery = session.CreateSQLQuery(sql);

                if (scalars != null && scalars.Count != 0)
                    foreach (var scalar in scalars)
                        contentQuery.AddScalar(scalar.Key, scalar.Value);

                if (parameters != null && parameters.Count != 0)
                    foreach (var parameter in parameters)
                    {
                        countQuery.SetParameter(parameter.Key, parameter.Value);
                        contentQuery.SetParameter(parameter.Key, parameter.Value);
                    }

                contentQuery.SetFirstResult(firstIndex);
                contentQuery.SetMaxResults(maxCount);

                HibernateTemplate.PrepareQuery(countQuery);
                if (scalars != null && scalars.Count != 0)
                    contentQuery.SetResultTransformer(new AliasToBeanResultTransformer(typeof(T)));
                HibernateTemplate.PrepareQuery(contentQuery);

                var count = countQuery.UniqueResult<int>();
                var data = contentQuery.List<T>();
                return new PagingResult<T>
                {
                    Count = maxCount,
                    TotalCount = count,
                    Data = data
                };
            });
        }
    }
}
