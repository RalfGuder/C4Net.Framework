using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity DomVal.
    /// </summary>
    public class DomainValueExpression : BaseEntityExpression<DomainValueExpression>
    {
        #region - Properties -
		
        /// <summary>
        /// Expression for the column dom_id.
        /// </summary>
        /// <value>
        /// The value of the column dom_id.
        /// </value>
        public ColumnExpression DomId 
        { 
            get { return this.columns[0]; } 
        }
		
        /// <summary>
        /// Expression for the column dom_val_ix.
        /// </summary>
        /// <value>
        /// The value of the column dom_val_ix.
        /// </value>
        public ColumnExpression DomValIx 
        { 
            get { return this.columns[1]; } 
        }
		
        /// <summary>
        /// Expression for the column descr_txt.
        /// </summary>
        /// <value>
        /// The value of the column descr_txt.
        /// </value>
        public ColumnExpression DescrTxt 
        { 
            get { return this.columns[2]; } 
        }
		
        /// <summary>
        /// Expression for the column name_txt.
        /// </summary>
        /// <value>
        /// The value of the column name_txt.
        /// </value>
        public ColumnExpression NameTxt 
        { 
            get { return this.columns[3]; } 
        }
		
        /// <summary>
        /// Expression for the column def_txt.
        /// </summary>
        /// <value>
        /// The value of the column def_txt.
        /// </value>
        public ColumnExpression DefTxt 
        { 
            get { return this.columns[4]; } 
        }
		
        /// <summary>
        /// Expression for the column type_code.
        /// </summary>
        /// <value>
        /// The value of the column type_code.
        /// </value>
        public ColumnExpression TypeCode 
        { 
            get { return this.columns[5]; } 
        }
		
        /// <summary>
        /// Expression for the column stdn_lvl_code.
        /// </summary>
        /// <value>
        /// The value of the column stdn_lvl_code.
        /// </value>
        public ColumnExpression StdnLvlCode 
        { 
            get { return this.columns[6]; } 
        }
		
        /// <summary>
        /// Expression for the column src_txt.
        /// </summary>
        /// <value>
        /// The value of the column src_txt.
        /// </value>
        public ColumnExpression SrcTxt 
        { 
            get { return this.columns[7]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainValueExpression"/> class.
        /// </summary>
        public DomainValueExpression()
            : base("[%LogicalName%%]", typeof(IDomainValue))
        {
        }

        #endregion
    }
}
