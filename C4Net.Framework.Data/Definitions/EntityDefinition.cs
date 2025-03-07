using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using C4Net.Framework.Core.Types;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Definitions
{
    /// <summary>
    /// Class for the definition of an Entity.
    /// </summary>
    public class EntityDefinition
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public int EntId { get; set; }

        /// <summary>
        /// Gets or sets the logical name of the entity.
        /// </summary>
        /// <value>
        /// The logical name of the entity.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the physical name of the entity.
        /// </summary>
        /// <value>
        /// The physical name of the entity.
        /// </value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the dependency.
        /// </summary>
        /// <value>
        /// The dependency.
        /// </value>
        public EntityDependencyEnum Dependency { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loggable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loggable; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggable { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the interface.
        /// </summary>
        /// <value>
        /// The name of the interface.
        /// </value>
        public string InterfaceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the type of the interface.
        /// </summary>
        /// <value>
        /// The type of the interface.
        /// </value>
        public Type InterfaceType { get; set; }

        /// <summary>
        /// Gets or sets the type of the class.
        /// </summary>
        /// <value>
        /// The type of the class.
        /// </value>
        public Type ClassType { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IList<AttributeDefinition> Attributes { get; private set; }

        /// <summary>
        /// Gets the primary keys.
        /// </summary>
        /// <value>
        /// The primary keys.
        /// </value>
        public IList<AttributeDefinition> PrimaryKeys { get; private set; }

        /// <summary>
        /// Gets the attribute dictionary.
        /// </summary>
        /// <value>
        /// The attribute dictionary.
        /// </value>
        public Dictionary<string, AttributeDefinition> AttributeDict { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDefinition"/> class.
        /// </summary>
        public EntityDefinition()
        {
            this.Attributes = new List<AttributeDefinition>();
            this.PrimaryKeys = new List<AttributeDefinition>();
            this.AttributeDict = new Dictionary<string, AttributeDefinition>();
            this.Clear();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.EntId = 0;
            this.Name = string.Empty;
            this.TableName = string.Empty;
            this.Dependency = EntityDependencyEnum.Independent;
            this.Depth = 0;
            this.IsLoggable = false;
            this.ModuleName = string.Empty;
            this.InterfaceName = string.Empty;
            this.ClassName = string.Empty;
            this.InterfaceType = null;
            this.ClassType = null;
            this.AttributeDict.Clear();
            this.Attributes.Clear();
            this.PrimaryKeys.Clear();
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="node">The node.</param>
        public void LoadFromXml(XmlNode node)
        {
            NodeAttributes nodeAttributes = new NodeAttributes(node);
            this.EntId = nodeAttributes.AsInteger("EntId");
            this.Name = nodeAttributes.AsString("Name");
            this.TableName = nodeAttributes.AsString("TableName");
            int depend = nodeAttributes.AsInteger("Dependency");
            switch (depend)
            {
                case 0: 
                    this.Dependency = EntityDependencyEnum.Independent;
                    break;
                case 1: 
                    this.Dependency = EntityDependencyEnum.Dependent;
                    break;
                case 2: 
                    this.Dependency = EntityDependencyEnum.Subtype;
                    break;
            }
            this.Depth = nodeAttributes.AsInteger("Depth");
            this.IsLoggable = nodeAttributes.AsBool("IsLoggable");
            this.ModuleName = nodeAttributes.AsString("Module");
            foreach (XmlNode childNode in node.SelectNodes("Attributes/Attribute"))
            {
                AttributeDefinition definition = new AttributeDefinition(this);
                definition.LoadFromXml(childNode);
                this.Attributes.Add(definition);
            }
        }

        /// <summary>
        /// Recalculates this instance.
        /// </summary>
        public void Recalculate(string interfacesAssembly, string classesAssembly)
        {
            this.ClassName = StringUtil.Pascalize(this.Name);
            this.InterfaceName = "I" + this.ClassName;
            if (!string.IsNullOrEmpty(interfacesAssembly))
            {
                this.InterfaceType = TypesManager.ResolveType(interfacesAssembly+"."+this.InterfaceName);
            }
            else
            {
                this.InterfaceType = TypesManager.ResolveType(this.InterfaceName);
            }
            if (!string.IsNullOrEmpty(classesAssembly))
            {
                this.ClassType = TypesManager.ResolveType(classesAssembly + "." + this.ClassName);
            }
            else
            {
                this.ClassType = TypesManager.ResolveType(this.ClassName);
            }
        }

        /// <summary>
        /// Fluent define the depth.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        public EntityDefinition WithDepth(int depth)
        {
            this.Depth = depth;
            return this;
        }

        /// <summary>
        /// Fluent define the entity id.
        /// </summary>
        /// <param name="entId">The ent id.</param>
        /// <returns></returns>
        public EntityDefinition WithEntId(int entId)
        {
            this.EntId = entId;
            return this;
        }

        /// <summary>
        /// Fluent define the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public EntityDefinition WithName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Fluent define the table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public EntityDefinition WithTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }

        /// <summary>
        /// Fluent define the dependency.
        /// </summary>
        /// <param name="dependency">The dependency.</param>
        /// <returns></returns>
        public EntityDefinition WithDependency(EntityDependencyEnum dependency)
        {
            this.Dependency = dependency;
            return this;
        }

        /// <summary>
        /// Fluent define the entity as dependent.
        /// </summary>
        /// <returns></returns>
        public EntityDefinition SetIsDependent()
        {
            return this.WithDependency(EntityDependencyEnum.Dependent);
        }

        /// <summary>
        /// Fluent define the entity as independent.
        /// </summary>
        /// <returns></returns>
        public EntityDefinition SetIsIndependent()
        {
            return this.WithDependency(EntityDependencyEnum.Independent);
        }

        /// <summary>
        /// Fluent define the entity as subtype.
        /// </summary>
        /// <returns></returns>
        public EntityDefinition SetIsSubtype()
        {
            return this.WithDependency(EntityDependencyEnum.Subtype);
        }

        /// <summary>
        /// Fluent define if is loggable (creator id + update sequence number)
        /// </summary>
        /// <param name="isLoggable">if set to <c>true</c> [is loggable].</param>
        /// <returns></returns>
        public EntityDefinition SetIsLoggable(bool isLoggable = true)
        {
            this.IsLoggable = isLoggable;
            return this;
        }

        /// <summary>
        /// Fluent define that is not loggable.
        /// </summary>
        /// <returns></returns>
        public EntityDefinition SetIsNotLoggable()
        {
            return this.SetIsLoggable(false);
        }

        /// <summary>
        /// Fluent define the module name.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        public EntityDefinition WithModule(string moduleName)
        {
            this.ModuleName = moduleName;
            return this;
        }

        /// <summary>
        /// Fluent define the interface type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        public EntityDefinition WithInterfaceType(Type interfaceType)
        {
            if (interfaceType != null)
            {
                this.InterfaceName = interfaceType.Name;
            }
            this.InterfaceType = interfaceType;
            return this;
        }

        /// <summary>
        /// Gets one attribute definition by the property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public AttributeDefinition GetAttributeByPropertyName(string propertyName)
        {
            foreach (AttributeDefinition attribute in this.Attributes)
            {
                if (attribute.Name.Equals(propertyName))
                {
                    return attribute;
                }
            }
            return null;
        }

        /// <summary>
        /// Determines whether the specified property name has attribute.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public AttributeDefinition HasAttribute(string propertyName)
        {
            AttributeDefinition result = this.GetAttributeByPropertyName(propertyName);
            if (result == null)
            {
                result = new AttributeDefinition(this);
                this.Attributes.Add(result);
                result.Name = propertyName;
            }
            return result;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public void Build()
        {
            this.Attributes = this.Attributes.OrderBy(x => x.SequenceNumber).ToList();
            this.PrimaryKeys.Clear();
            this.AttributeDict.Clear();
            foreach (AttributeDefinition attribute in this.Attributes)
            {
                attribute.Recalculate();
                if (attribute.IsPrimaryKey)
                {
                    this.PrimaryKeys.Add(attribute);
                }
                this.AttributeDict.Add(attribute.Name, attribute);
            }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AttributeDefinition GetAttribute(string name)
        {
            if (this.AttributeDict.ContainsKey(name)) 
            {
                return this.AttributeDict[name];
            }
            return null;
        }

        
        #endregion
    }
}
