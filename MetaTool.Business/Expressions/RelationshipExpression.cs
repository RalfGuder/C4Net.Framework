using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;


namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Rel.
    /// </summary>
    public class RelationshipExpression : BaseEntityExpression<RelationshipExpression>
    {
        #region - Properties -
		
        /// <summary>
        /// Expression for the column pa_ent_id.
        /// </summary>
        /// <value>
        /// The value of the column pa_ent_id.
        /// </value>
        public ColumnExpression PaEntId 
        { 
            get { return this.columns[0]; } 
        }
		
        /// <summary>
        /// Expression for the column ch_ent_id.
        /// </summary>
        /// <value>
        /// The value of the column ch_ent_id.
        /// </value>
        public ColumnExpression ChEntId 
        { 
            get { return this.columns[1]; } 
        }
		
        /// <summary>
        /// Expression for the column rel_ix.
        /// </summary>
        /// <value>
        /// The value of the column rel_ix.
        /// </value>
        public ColumnExpression RelIx 
        { 
            get { return this.columns[2]; } 
        }
		
        /// <summary>
        /// Expression for the column type_code.
        /// </summary>
        /// <value>
        /// The value of the column type_code.
        /// </value>
        public ColumnExpression TypeCode 
        { 
            get { return this.columns[3]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipExpression"/> class.
        /// </summary>
        public RelationshipExpression()
            : base("[%LogicalName%%]", typeof(IRelationship))
        {
        }

        #endregion
    }
}
