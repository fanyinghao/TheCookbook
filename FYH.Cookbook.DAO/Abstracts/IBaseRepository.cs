using System.Collections.Generic;
using FYH.Cookbook.Model.Common;
using NHibernate.Criterion;

namespace FYH.Cookbook.DAO.Abstracts
{
    public interface IBaseRepository
    {
        /// <summary>
        /// Add Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddEntity(object entity);

        /// <summary>
        /// Add Or Update Entity
        /// </summary>
        /// <param name="entity"></param>
        void AddOrUpdateEntity(object entity);

        /// <summary>
        /// Execute SQL(Update or Delete)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteUpdate(string sql);

        /// <summary>
        /// Execute SQL(Update or Delete)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">允许为 null</param>
        /// <returns></returns>
        int ExecuteUpdate(string sql, IDictionary<string, object> parameters);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity"></param>
        void UpdateEntity(object entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity"></param>
        void DeleteEntity(object entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        void DeleteEntityById<T>(object id);

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntityById<T>(object id);

        /// <summary>
        /// Query Entity List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        IList<T> GetEntityList<T>(int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List By Criterions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        IList<T> GetEntityList<T>(IEnumerable<ICriterion> criterions, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);
        
        /// <summary>
        /// Query Entity List By Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orders">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        IList<T> GetEntityList<T>(IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List By Criterions And Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions">nullable</param>
        /// <param name="orders">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        IList<T> GetEntityList<T>(IEnumerable<ICriterion> criterions, IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        PagingResult<T> GetEntityPagingList<T>(int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List By Criterions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        PagingResult<T> GetEntityPagingList<T>(IEnumerable<ICriterion> criterions, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List By Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orders">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        PagingResult<T> GetEntityPagingList<T>(IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

        /// <summary>
        /// Query Entity List By Criterions And Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions">nullable</param>
        /// <param name="orders">nullable</param>
        /// <param name="firstIndex"></param>
        /// <param name="maxCount"></param>
        /// <param name="useSecondLevelCache">Is Use NHibernate Second Level Cache</param>
        /// <returns></returns>
        PagingResult<T> GetEntityPagingList<T>(IEnumerable<ICriterion> criterions, IEnumerable<Order> orders, int firstIndex = 0, int maxCount = int.MaxValue, bool useSecondLevelCache = true);

    }
}
