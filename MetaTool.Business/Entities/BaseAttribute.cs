using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table BASE_ATTR.
    /// </summary>
    [Serializable]
    public partial class BaseAttribute : IBaseAttribute, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly BaseAttributeExpression _ = new BaseAttributeExpression();

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
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        public string DefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column data_type_code
        /// </summary>
        /// <value>
        /// Value of the column data_type_code
        /// </value>
        public string DataTypeCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column data_len_cnt
        /// </summary>
        /// <value>
        /// Value of the column data_len_cnt
        /// </value>
        public double DataLenCnt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column data_dec_cnt
        /// </summary>
        /// <value>
        /// Value of the column data_dec_cnt
        /// </value>
        public double DataDecCnt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column dom_id
        /// </summary>
        /// <value>
        /// Value of the column dom_id
        /// </value>
        public double DomId { get; set; }
		
        #endregion
    }
}
