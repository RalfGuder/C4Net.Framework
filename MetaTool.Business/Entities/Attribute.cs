using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table ATTR.
    /// </summary>
    [Serializable]
    public partial class Attribute : IAttribute, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly AttributeExpression _ = new AttributeExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column ent_id
        /// </summary>
        /// <value>
        /// Value of the column ent_id
        /// </value>
        public double EntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column attr_ix
        /// </summary>
        /// <value>
        /// Value of the column attr_ix
        /// </value>
        public double AttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        public string NameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column col_name_txt
        /// </summary>
        /// <value>
        /// Value of the column col_name_txt
        /// </value>
        public string ColNameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column attr_seqnr_ord
        /// </summary>
        /// <value>
        /// Value of the column attr_seqnr_ord
        /// </value>
        public double AttrSeqnrOrd { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column pk_ind_code
        /// </summary>
        /// <value>
        /// Value of the column pk_ind_code
        /// </value>
        public string PkIndCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column fk_ind_code
        /// </summary>
        /// <value>
        /// Value of the column fk_ind_code
        /// </value>
        public string FkIndCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column stdn_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column stdn_lvl_code
        /// </value>
        public string StdnLvlCode { get; set; }

        public bool IsMandatory { get; set; }

        public bool IsPrimaryKey
        {
            get { return this.PkIndCode.Equals("PK"); }
        }

        public bool IsForeignKey
        {
            get { return this.FkIndCode.Equals("FK"); }
        }

        public string Definition { get; set; }

        public string DataType { get; set; }

        public int DataLength { get; set; }

        public int DataDecimals { get; set; }

        public int DomId { get; set; }

        public int SourceEntId { get; set; }

        public int SourceAttrIx { get; set; }

        public int BaseEntId { get; set; }

        public int BaseAttrIx { get; set; }

        public int RelIx { get; set; }

        public Attribute BaseAttribute { get; set; }

        public Attribute SourceAttribute { get; set; }

        public Entity Entity { get; set; }

        public bool IsCategoryAttribute { get; set; }

        #endregion

        #region - Constructors -

        public Attribute()
        {
            this.BaseAttribute = null;
            this.SourceAttribute = null;
            this.Entity = null;
            this.IsCategoryAttribute = false;
        }

        #endregion

        #region - Methods -

        public void Apply(NonKeyAttribute item)
        {
            this.IsMandatory = item.OptIndCode.Equals("MA");
        }

        public void Apply(BaseAttribute item)
        {
            this.Definition = item.DefTxt;
            this.DataType = item.DataTypeCode;
            this.DataLength = Convert.ToInt32(item.DataLenCnt);
            this.DataDecimals = Convert.ToInt32(item.DataDecCnt);
            this.DomId = Convert.ToInt32(item.DomId);
        }

        public void Apply(ForeignKeyAttribute item)
        {
            this.SourceEntId = Convert.ToInt32(item.SrcEntId);
            this.SourceAttrIx = Convert.ToInt32(item.SrcAttrIx);
            this.BaseEntId = Convert.ToInt32(item.BaseEntId);
            this.BaseAttrIx = Convert.ToInt32(item.BaseAttrIx);
            this.RelIx = Convert.ToInt32(item.MigrRelIx);
        }

        public void ApplyBase(Attribute item)
        {
            this.BaseAttribute = item;
            this.Definition = item.Definition;
            this.DataType = item.DataType;
            this.DataLength = item.DataLength;
            this.DataDecimals = item.DataDecimals;
            this.DomId = item.DomId;
        }

        public void ApplySource(Attribute item)
        {
            this.SourceAttribute = item;
        }

        #endregion
    }
}
