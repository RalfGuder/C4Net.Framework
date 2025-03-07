using System;
using System.Collections.Generic;

namespace C4Net.Framework.Core.Accesor
{
    /// <summary>
    /// Factory for the Object Proxy accessor to objects.
    /// </summary>
    public static class ObjectProxyFactory
    {
        #region - Fields -

        /// <summary>
        /// Dictionary of proxies, relationship between the type and the proxy.
        /// </summary>
        private static Dictionary<Type, IObjectProxy> proxies = new Dictionary<Type, IObjectProxy>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Return the ObjectProxy for a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IObjectProxy GetByType(Type type)
        {
            if (proxies.ContainsKey(type))
            {
                return proxies[type];
            }
            else
            {
                ObjectProxy result = new ObjectProxy(type);
                proxies.Add(type, result);
                return result;
            }
        }

        /// <summary>
        /// Adds an object proxy given the type, instance type and collection type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="collectionType">Type of the collection.</param>
        public static void Add(Type type, Type objectType, Type collectionType)
        {
            if (!proxies.ContainsKey(type))
            {
                IObjectProxy proxy = new ObjectProxy(type, objectType, collectionType);
                proxies.Add(type, proxy);
                if (type != objectType)
                {
                    proxies.Add(objectType, proxy);
                }
            }
        }

        /// <summary>
        /// Adds an object proxy given the type and instance type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectType">Type of the object.</param>
        public static void Add(Type type, Type objectType)
        {
            if (!proxies.ContainsKey(type))
            {
                IObjectProxy proxy = new ObjectProxy(type, objectType);
                proxies.Add(type, proxy);
                if (type != objectType)
                {
                    proxies.Add(objectType, proxy);
                }
            }
        }

        /// <summary>
        /// Adds an object proxy given the type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void Add(Type type)
        {
            if (!proxies.ContainsKey(type))
            {
                proxies.Add(type, new ObjectProxy(type));
            }
        }

        /// <summary>
        /// Gets the object proxy for the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectProxy Get<T>()
        {
            return GetByType(typeof(T));
        }

        /// <summary>
        /// Gets the object proxy for the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IObjectProxy Get(object instance)
        {
            return GetByType(instance.GetType());
        }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateObject<T>()
        {
            return (T)Get<T>().CreateObject();
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> CreateList<T>()
        {
            return (IList<T>)Get<T>().CreateList();
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Type GetPropertyType<T>(string name)
        {
            return Get<T>().GetPropertyType(name);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object GetValue<T>(object instance, string name)
        {
            return Get<T>().GetValue(instance, name);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object GetValue(object instance, string name)
        {
            return Get(instance).GetValue(instance, name);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetValue<T>(object instance, string name, object value)
        {
            Get<T>().SetValue(instance, name, value);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(object instance, string name, object value)
        {
            Get(instance).SetValue(instance, name, value);
        }

        #endregion
    }
}
