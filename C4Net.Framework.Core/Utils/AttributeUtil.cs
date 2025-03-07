using System;
using System.Linq.Expressions;
using System.Reflection;

namespace C4Net.Framework.Core.Utils
{
    /// <summary>
    /// Util for working with attributes.
    /// </summary>
    public static class AttributeUtil
    {
        #region - Methods -

        /// <summary>
        /// From a lambda expresion of a property, return the property info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The lambda expression of the property.</param>
        /// <returns>Property info of the property</returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyLambda)
        {
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' doesn't refers to a property.", propertyLambda.ToString()));
            }
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));
            }
            return propInfo;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public static object[] GetCustomAttributes<T>(Expression<Func<T>> propertyLambda, Type attributeType)
        {
            PropertyInfo info = GetPropertyInfo(propertyLambda);
            return attributeType == null ? info.GetCustomAttributes(true) : info.GetCustomAttributes(attributeType, true);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public static object GetCustomAttribute<T>(Expression<Func<T>> propertyLambda, Type attributeType)
        {
            PropertyInfo info = GetPropertyInfo(propertyLambda);
            return info.GetCustomAttribute(attributeType, true);
        }

        /// <summary>
        /// From the lambda expression of a property, return the value of a custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the custom attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object Get<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
        {
            object attribute = GetCustomAttribute(propertyLambda, attributeType);
            PropertyInfo info = attributeType.GetProperty(propertyName);
            return info.GetValue(attribute);
        }

        #endregion
    }
}
