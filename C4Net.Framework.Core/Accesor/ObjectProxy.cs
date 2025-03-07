using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace C4Net.Framework.Core.Accesor
{
    /// <summary>
    /// Object proxy for a type.
    /// </summary>
    public class ObjectProxy : IObjectProxy
    {
        #region - Fields -

        /// <summary>
        /// The type of the object.
        /// </summary>
        private Type type;

        /// <summary>
        /// The type for creating instances.
        /// </summary>
        private Type objectType;

        /// <summary>
        /// The type for creating collections.
        /// </summary>
        private Type collectionType;

        /// <summary>
        /// Dictionary to access the PropertyInfo of the type by the property name.
        /// </summary>
        protected Dictionary<string, PropertyInfo> propInfo = new Dictionary<string, PropertyInfo>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <value>
        /// The property names.
        /// </value>
        public string[] PropertyNames
        {
            get
            {
                return propInfo.Keys.ToArray();
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="collectionType">Type of the collection.</param>
        public ObjectProxy(Type type, Type objectType, Type collectionType)
        {
            this.type = type;
            this.objectType = objectType;
            this.collectionType = collectionType;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectType">Type of the object.</param>
        public ObjectProxy(Type type, Type objectType)
            : this(type, objectType, typeof(List<>).MakeGenericType(objectType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ObjectProxy(Type type)
            : this(type, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public ObjectProxy(object instance)
            : this(instance.GetType())
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected void Initialize()
        {
            foreach (PropertyInfo info in this.type.GetProperties())
            {
                propInfo.Add(info.Name, info);
            }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns></returns>
        public object CreateObject()
        {
            return Activator.CreateInstance(this.objectType);
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <returns></returns>
        public IList CreateList()
        {
            return (IList)Activator.CreateInstance(this.collectionType);
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        public Type GetPropertyType(string name)
        {
            return propInfo[name].PropertyType;
        }

        /// <summary>
        /// Gets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        public object GetValue(object instance, string name)
        {
            return propInfo[name].GetValue(instance);
        }

        /// <summary>
        /// Sets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object instance, string name, object value)
        {
            propInfo[name].SetValue(instance, value);
        }

        #endregion
    }
}
