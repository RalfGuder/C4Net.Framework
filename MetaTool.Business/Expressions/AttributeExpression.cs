using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Attr.
    /// </summary>
    public class AttributeExpression : BaseEntityExpression<AttributeExpression>
    {
        #region - Properties -

        /// <summary>
        /// Expression for the column ent_id.
        /// </summary>
        /// <value>
        /// The value of the column ent_id.
        /// </value>
        public ColumnExpression EntId
        {
            get { return this.columns[0]; }
        }

        /// <summary>
        /// Expression for the column attr_ix.
        /// </summary>
        /// <value>
        /// The value of the column attr_ix.
        /// </value>
        public ColumnExpression AttrIx
        {
            get { return this.columns[1]; }
        }

        /// <summary>
        /// Expression for the column name_txt.
        /// </summary>
        /// <value>
        /// The value of the column name_txt.
        /// </value>
        public ColumnExpression NameTxt
        {
            get { return this.columns[2]; }
        }

        /// <summary>
        /// Expression for the column col_name_txt.
        /// </summary>
        /// <value>
        /// The value of the column col_name_txt.
        /// </value>
        public ColumnExpression ColNameTxt
        {
            get { return this.columns[3]; }
        }

        /// <summary>
        /// Expression for the column attr_seqnr_ord.
        /// </summary>
        /// <value>
        /// The value of the column attr_seqnr_ord.
        /// </value>
        public ColumnExpression AttrSeqnrOrd
        {
            get { return this.columns[4]; }
        }

        /// <summary>
        /// Expression for the column pk_ind_code.
        /// </summary>
        /// <value>
        /// The value of the column pk_ind_code.
        /// </value>
        public ColumnExpression PkIndCode
        {
            get { return this.columns[5]; }
        }

        /// <summary>
        /// Expression for the column fk_ind_code.
        /// </summary>
        /// <value>
        /// The value of the column fk_ind_code.
        /// </value>
        public ColumnExpression FkIndCode
        {
            get { return this.columns[6]; }
        }

        /// <summary>
        /// Expression for the column stdn_lvl_code.
        /// </summary>
        /// <value>
        /// The value of the column stdn_lvl_code.
        /// </value>
        public ColumnExpression StdnLvlCode
        {
            get { return this.columns[7]; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeExpression"/> class.
        /// </summary>
        public AttributeExpression()
            : base("[%LogicalName%%]", typeof(IAttribute))
        {
        }

        #endregion
    }
}
