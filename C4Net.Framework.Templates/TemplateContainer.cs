using System;
using System.Collections.Generic;
using System.Linq;
using C4Net.Framework.Core.Accesor;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Templates
{
    /// <summary>
    /// Class for a tree structure containing variables for the template, and iterators.
    /// One container contains attributes (values), sons (instances of other objects) and array values (indicates
    /// that the container is an array and can be iterated). Also contains one object where the properties can be 
    /// inspected and used like attributes.
    /// </summary>
    public class TemplateContainer
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public TemplateContainer Parent { get; private set; }

        /// <summary>
        /// Gets or sets the inner object.
        /// </summary>
        /// <value>
        /// The inner object.
        /// </value>
        public object InnerObject { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public Dictionary<String, string> Attributes { get; private set; }

        /// <summary>
        /// Gets the childs.
        /// </summary>
        /// <value>
        /// The childs.
        /// </value>
        public Dictionary<string, TemplateContainer> Childs { get; private set; }

        /// <summary>
        /// Gets the array values.
        /// </summary>
        /// <value>
        /// The array values.
        /// </value>
        public List<TemplateContainer> ArrayValues { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is array.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is array; otherwise, <c>false</c>.
        /// </value>
        public bool IsArray
        {
            get { return this.ArrayValues.Count > 0; }
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public TemplateContainer Root
        {
            get
            {
                TemplateContainer container = this;
                while (container.Parent != null)
                {
                    container = container.Parent;
                }
                return container;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContainer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="innerObject">The inner object.</param>
        public TemplateContainer(TemplateContainer parent, object innerObject)
        {
            this.Parent = parent;
            this.InnerObject = innerObject;
            this.Attributes = new Dictionary<String, string>();
            this.Childs = new Dictionary<string, TemplateContainer>();
            this.ArrayValues = new List<TemplateContainer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContainer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public TemplateContainer(TemplateContainer parent)
            : this(parent, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContainer"/> class.
        /// </summary>
        public TemplateContainer()
            : this(null, null)
        {
        }

        #endregion

        #region - Method - 

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddAttribute(string name, string value)
        {
            this.Attributes[name] = value;
        }

        public void AddAttribute(string name, double value)
        {
            this.Attributes[name] = value.ToString();
        }

        public void AddAttribute(string name, bool value)
        {
            this.Attributes[name] = value.ToString();
        }
        

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveAttribute(string name)
        {
            this.Attributes.Remove(name);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object GetAttribute(string name)
        {
            if (this.Attributes.ContainsKey(name))
            {
                return this.Attributes[name];
            }
            return null;
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="innerObject">The inner object.</param>
        /// <returns></returns>
        public TemplateContainer AddChild(string name, object innerObject = null)
        {
            TemplateContainer result;
            if (this.Childs.ContainsKey(name))
            {
                result = this.Childs[name];
                result.InnerObject = innerObject;
            }
            else
            {
                result = new TemplateContainer(this, innerObject);
                this.Childs.Add(name, result);
            }
            return result;
        }

        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveChild(string name)
        {
            this.Childs.Remove(name);
        }

        /// <summary>
        /// Gets the child.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TemplateContainer GetChild(string name)
        {
            if (this.Childs.ContainsKey(name))
            {
                return this.Childs[name];
            }
            return null;
        }

        /// <summary>
        /// Adds the array value.
        /// </summary>
        /// <param name="innerObject">The inner object.</param>
        /// <returns></returns>
        public TemplateContainer AddArrayValue(object innerObject = null)
        {
            TemplateContainer result = new TemplateContainer(this, innerObject);
            this.ArrayValues.Add(result);
            return result;
        }

        /// <summary>
        /// Removes the array value.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveArrayValue(int index)
        {
            this.ArrayValues.RemoveAt(index);
        }

        /// <summary>
        /// Gets the array value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public TemplateContainer GetArrayValue(int index)
        {
            return this.ArrayValues[index];
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Attributes.Clear();
            this.Childs.Clear();
            this.ArrayValues.Clear();
        }

        /// <summary>
        /// Gets one object specified by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public Object GetByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return this;
            }
            if (path.StartsWith("/"))
            {
                return this.Root.GetByPath(path.Remove(0, 1));
            }
            if (path.StartsWith("../"))
            {
                if (this.Parent == null)
                {
                    return null;
                }
                return this.Parent.GetByPath(path.Remove(0, 3));
            }
            string[] paths = path.Split('.');
            string currentPath = paths[0];
            string arrayPos = StringUtil.PopBetween(ref currentPath, "[", "]");
            if (string.IsNullOrEmpty(arrayPos))
            {
                if (paths.Length == 1)
                {
                    if (this.Attributes.ContainsKey(currentPath))
                    {
                        return this.Attributes[currentPath];
                    }
                    if (this.InnerObject != null)
                    {
                        IObjectProxy proxy = ObjectProxyFactory.Get(this.InnerObject);
                        if (proxy.PropertyNames.Contains(currentPath))
                        {
                            return proxy.GetValue(this.InnerObject, currentPath);
                        }
                    }
                }
                if (this.Childs.ContainsKey(currentPath))
                {
                    TemplateContainer child = this.Childs[currentPath];
                    if (paths.Length == 1)
                    {
                        return child;
                    }
                    return child.GetByPath(path.Remove(0, currentPath.Length + 1));
                }
            }
            else
            {
                if (this.Childs.ContainsKey(currentPath))
                {
                    TemplateContainer child = this.Childs[currentPath];
                    TemplateContainer arrayContainer = child.ArrayValues[int.Parse(arrayPos)];
                    if (paths.Length == 1)
                    {
                        return arrayContainer;
                    }
                    return arrayContainer.GetByPath(path.Remove(0, currentPath.Length + 1));
                }
            }
            if (this.Parent != null)
            {
                return this.Root.GetByPath(path);
            }
            return null;
        }

        #endregion
    }
}
