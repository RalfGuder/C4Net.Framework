using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;


namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity PkAttr.
    /// </summary>
    public class PrimaryKeyAttributeExpression : BaseEntityExpression<PrimaryKeyAttributeExpression>
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
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttributeExpression"/> class.
        /// </summary>
        public PrimaryKeyAttributeExpression()
            : base("[%LogicalName%%]", typeof(IPrimaryKeyAttribute))
        {
        }

        #endregion
    }
}
