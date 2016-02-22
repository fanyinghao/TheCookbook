using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Core.Utilities
{
    public class Singleton<T> where T : new()
    {
        private Singleton() { }

        public static T Instance
        {
            get { return SingletonCreator.Instance; }
        }

        private class SingletonCreator
        {
            internal static readonly T Instance = new T();
        }
    }

    public class Singleton
    {
        private Singleton() { }

        public static object Instance(Type type)
        {
            return Singleton<SingletonCreator>.Instance[type];
        }

        private class SingletonCreator : Dictionary<Type, object>
        {
            internal new object this[Type key]
            {
                get
                {
                    if (!ContainsKey(key))
                    {
                        lock (this)
                        {
                            if (!ContainsKey(key))
                                Add(key, key.Assembly.CreateInstance(key.FullName));
                        }
                    }

                    return base[key];
                }

                set
                {
                    Add(key, value);
                }
            }
        }
    }
}
