using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Br.
    /// </summary>
    public class BusinessRuleExpression : BaseEntityExpression<BusinessRuleExpression>
    {
        #region - Properties -
		
        /// <summary>
        /// Expression for the column br_id.
        /// </summary>
        /// <value>
        /// The value of the column br_id.
        /// </value>
        public ColumnExpression BrId 
        { 
            get { return this.columns[0]; } 
        }
		
        /// <summary>
        /// Expression for the column cat_code.
        /// </summary>
        /// <value>
        /// The value of the column cat_code.
        /// </value>
        public ColumnExpression CatCode 
        { 
            get { return this.columns[1]; } 
        }
		
        /// <summary>
        /// Expression for the column section_xref_txt.
        /// </summary>
        /// <value>
        /// The value of the column section_xref_txt.
        /// </value>
        public ColumnExpression SectionXrefTxt 
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
        /// Expression for the column tab_xref_txt.
        /// </summary>
        /// <value>
        /// The value of the column tab_xref_txt.
        /// </value>
        public ColumnExpression TabXrefTxt 
        { 
            get { return this.columns[5]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleExpression"/> class.
        /// </summary>
        public BusinessRuleExpression()
            : base("[%LogicalName%%]", typeof(IBusinessRule))
        {
        }

        #endregion
    }
}
