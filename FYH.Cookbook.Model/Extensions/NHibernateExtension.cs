using Spring.Context.Support;
using Spring.Data.NHibernate.Generic;

namespace FYH.Cookbook.Model.Extensions
{
    public static class NHibernateExtension
    {
        private static HibernateTemplate _hibernateTemplate;


        static NHibernateExtension()
        {
            var applicationContext = ContextRegistry.GetContext();
            var setName = "hibernatetemplate";

            if (applicationContext.ContainsObject(setName))
                _hibernateTemplate = applicationContext.GetObject(setName) as HibernateTemplate;
        }


        /// <summary>
        /// Get Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetEntityById<T>(object id) where T : class
        {
            return _hibernateTemplate == null || id == null ? null : _hibernateTemplate.Get<T>(id);
        }
    }
}
