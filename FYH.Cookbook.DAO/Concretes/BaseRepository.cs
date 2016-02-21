using System;
using System.Collections.Generic;
using System.Linq;
using Spring.Data.NHibernate.Generic.Support;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.Common;
using NHibernate.Criterion;

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
    }
}
