using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity NkAttr.
    /// </summary>
    public class NonKeyAttributeExpression : BaseEntityExpression<NonKeyAttributeExpression>
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
        /// Expression for the column opt_ind_code.
        /// </summary>
        /// <value>
        /// The value of the column opt_ind_code.
        /// </value>
        public ColumnExpression OptIndCode 
        { 
            get { return this.columns[2]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="NonKeyAttributeExpression"/> class.
        /// </summary>
        public NonKeyAttributeExpression()
            : base("[%LogicalName%%]", typeof(INonKeyAttribute))
        {
        }

        #endregion
    }
}
