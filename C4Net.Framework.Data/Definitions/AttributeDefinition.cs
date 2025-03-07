using System;
using System.Data;
using System.Diagnostics;
using System.Xml;
using C4Net.Framework.Core.Types;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Definitions
{
    /// <summary>
    /// Class for an Attribute Definition
    /// </summary>
    [Serializable]
    public class AttributeDefinition 
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public EntityDefinition Entity { get; set; }

        /// <summary>
        /// Gets or sets the ent id.
        /// </summary>
        /// <value>
        /// The ent id.
        /// </value>
        public int EntId { get; set; }

        /// <summary>
        /// Gets or sets the attribute index.
        /// </summary>
        /// <value>
        /// The attribute index.
        /// </value>
        public int AttrIx { get; set; }

        /// <summary>
        /// Gets or sets the logical name.
        /// </summary>
        /// <value>
        /// The logical name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the physical name.
        /// </summary>
        /// <value>
        /// The physical name.
        /// </value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        /// <value>
        /// The sequence number.
        /// </value>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mandatory.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is mandatory; otherwise, <c>false</c>.
        /// </value>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is foreign key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
        /// </value>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the net data.
        /// </summary>
        /// <value>
        /// The type of the net data.
        /// </value>
        public string NetDataTypeStr { get; set; }

        /// <summary>
        /// Gets or sets the length of the data.
        /// </summary>
        /// <value>
        /// The length of the data.
        /// </value>
        public int DataLength { get; set; }

        /// <summary>
        /// Gets or sets the data decimals.
        /// </summary>
        /// <value>
        /// The data decimals.
        /// </value>
        public int DataDecimals { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public DbType DataType { get; set; }

        #endregion

        #region - Constructors -

        public AttributeDefinition()
        {
            this.EntId = 0;
            this.Entity = null;
            this.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeDefinition"/> class.
        /// </summary>
        public AttributeDefinition(EntityDefinition entity)
        {
            this.EntId = entity.EntId;
            this.Entity = entity;
            this.Clear();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.AttrIx = 0;
            this.Name = string.Empty;
            this.ColumnName = string.Empty;
            this.SequenceNumber = 0;
            this.IsPrimaryKey = false;
            this.IsMandatory = false;
            this.IsForeignKey = false;
            this.DataLength = 0;
            this.DataDecimals = 0;
            this.DataType = DbType.String;
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="node">The node.</param>
        public void LoadFromXml(XmlNode node)
        {
            this.Clear();
            NodeAttributes nodeAttributes = new NodeAttributes(node);
            this.AttrIx = nodeAttributes.AsInteger("AttrIx");
            this.Name = nodeAttributes.AsString("Name");
            this.ColumnName = nodeAttributes.AsString("ColumnName");
            this.SequenceNumber = nodeAttributes.AsInteger("SequenceNumber");
            this.IsPrimaryKey = nodeAttributes.AsBool("IsPrimaryKey");
            this.IsMandatory = this.IsPrimaryKey ? true : nodeAttributes.AsBool("IsMandatory");
            this.IsForeignKey = nodeAttributes.AsBool("IsForeignKey");
            this.NetDataTypeStr = nodeAttributes.AsString("NetDataType");
            this.DataLength = nodeAttributes.AsInteger("DataLength");
            this.DataDecimals = nodeAttributes.AsInteger("DataDecimals");
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="node">The node.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        private void AddAttribute(XmlDocument doc, XmlNode node, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = doc.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            node.Attributes.Append(attribute);
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="node">The node.</param>
        public void SaveToXml(XmlDocument doc, XmlNode node)
        {
            this.AddAttribute(doc, node, "EntId", XmlConvert.ToString(this.EntId));
            this.AddAttribute(doc, node, "AttrIx", XmlConvert.ToString(this.AttrIx));
            this.AddAttribute(doc, node, "Name", this.Name);
            this.AddAttribute(doc, node, "ColumnName", this.ColumnName);
            this.AddAttribute(doc, node, "SequenceNumber", XmlConvert.ToString(this.SequenceNumber));
            if (this.IsPrimaryKey)
            {
                this.AddAttribute(doc, node, "IsPrimaryKey", XmlConvert.ToString(this.IsPrimaryKey));
            }
            if ((this.IsMandatory) && (!this.IsPrimaryKey))
            {
                this.AddAttribute(doc, node, "IsMandatory", XmlConvert.ToString(this.IsMandatory));
            }
            if (this.IsForeignKey)
            {
                this.AddAttribute(doc, node, "IsForeignKey", XmlConvert.ToString(this.IsForeignKey));
            }
            if (!string.IsNullOrEmpty(this.NetDataTypeStr))
            {
                this.AddAttribute(doc, node, "NetDataType", this.NetDataTypeStr);
            }
            this.AddAttribute(doc, node, "DataLength", XmlConvert.ToString(this.DataLength));
            if (this.DataDecimals > 0)
            {
                this.AddAttribute(doc, node, "DataDecimals", XmlConvert.ToString(this.DataDecimals));
            }
        }

        /// <summary>
        /// Recalculates this instance.
        /// </summary>
        public void Recalculate()
        {
            this.DataType = TypesManager.GetDbType(this.NetDataTypeStr);
            //public IEntityDefinition Entity { get; set; }
            //public IAttributeDefinition SourceAttribute { get; set; }
            //public IAttributeDefinition BaseAttribute { get; set; }
            //public string PropertyName { get; set; }
            //public DbType DataType { get; set; }
        }

        /// <summary>
        /// Fluent define AttrIx
        /// </summary>
        /// <param name="attrIx">The attr ix.</param>
        /// <returns></returns>
        public AttributeDefinition WithAttrIx(int attrIx)
        {
            this.AttrIx = attrIx;
            return this;
        }

        /// <summary>
        /// Fluent define name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AttributeDefinition WithName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Fluent define column name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public AttributeDefinition WithColumnName(string columnName)
        {
            this.ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// Fluent define sequence number.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <returns></returns>
        public AttributeDefinition WithSequenceNumber(int sequenceNumber)
        {
            this.SequenceNumber = sequenceNumber;
            return this;
        }

        /// <summary>
        /// Fluent define if is primary key.
        /// </summary>
        /// <param name="isPrimaryKey">if set to <c>true</c> [is primary key].</param>
        /// <returns></returns>
        public AttributeDefinition SetIsPrimaryKey(bool isPrimaryKey = true)
        {
            this.IsPrimaryKey = isPrimaryKey;
            if (this.IsPrimaryKey)
            {
                this.IsMandatory = true;
            }
            return this;
        }

        /// <summary>
        /// Fluent define if is mandatory.
        /// </summary>
        /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
        /// <returns></returns>
        public AttributeDefinition SetIsMandatory(bool isMandatory = true)
        {
            this.IsMandatory = IsMandatory;
            if (!this.IsMandatory)
            {
                this.IsPrimaryKey = false;
            }
            return this;
        }

        /// <summary>
        /// Fluent define if is foreign key.
        /// </summary>
        /// <param name="isForeignKey">if set to <c>true</c> [is foreign key].</param>
        /// <returns></returns>
        public AttributeDefinition SetIsForeignKey(bool isForeignKey = true)
        {
            this.IsForeignKey = isForeignKey;
            return this;
        }

        /// <summary>
        /// Fluent define .NET data type string.
        /// </summary>
        /// <param name="netDataTypeStr">The net data type STR.</param>
        /// <returns></returns>
        public AttributeDefinition WithNetDataTypeStr(string netDataTypeStr)
        {
            this.NetDataTypeStr = netDataTypeStr;
            return this;
        }

        /// <summary>
        /// Fluent define data length.
        /// </summary>
        /// <param name="dataLength">Length of the data.</param>
        /// <returns></returns>
        public AttributeDefinition WithDataLength(int dataLength)
        {
            this.DataLength = dataLength;
            return this;
        }

        /// <summary>
        /// Fluent define data decimals.
        /// </summary>
        /// <param name="dataDecimals">The data decimals.</param>
        /// <returns></returns>
        public AttributeDefinition WithDataDecimals(int dataDecimals)
        {
            this.DataDecimals = dataDecimals;
            return this;
        }

        /// <summary>
        /// Fluent define the database type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public AttributeDefinition WithDataType(DbType type)
        {
            this.DataType = type;
            return this;
        }

        /// <summary>
        /// Fluent define another attribute in the same entity.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public AttributeDefinition HasAttribute(string propertyName)
        {
            return (this.Entity as EntityDefinition).HasAttribute(propertyName);
        }

        #endregion
    }
}
