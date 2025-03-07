using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Cat.
    /// </summary>
    public class CategoryRelationshipExpression : BaseEntityExpression<CategoryRelationshipExpression>
    {
        #region - Properties -
		
        /// <summary>
        /// Expression for the column sup_ent_id.
        /// </summary>
        /// <value>
        /// The value of the column sup_ent_id.
        /// </value>
        public ColumnExpression SupEntId 
        { 
            get { return this.columns[0]; } 
        }
		
        /// <summary>
        /// Expression for the column cat_ix.
        /// </summary>
        /// <value>
        /// The value of the column cat_ix.
        /// </value>
        public ColumnExpression CatIx 
        { 
            get { return this.columns[1]; } 
        }
		
        /// <summary>
        /// Expression for the column def_txt.
        /// </summary>
        /// <value>
        /// The value of the column def_txt.
        /// </value>
        public ColumnExpression DefTxt 
        { 
            get { return this.columns[2]; } 
        }
		
        /// <summary>
        /// Expression for the column discr_ix.
        /// </summary>
        /// <value>
        /// The value of the column discr_ix.
        /// </value>
        public ColumnExpression DiscrIx 
        { 
            get { return this.columns[3]; } 
        }
		
        /// <summary>
        /// Expression for the column compl_ind_code.
        /// </summary>
        /// <value>
        /// The value of the column compl_ind_code.
        /// </value>
        public ColumnExpression ComplIndCode 
        { 
            get { return this.columns[4]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRelationshipExpression"/> class.
        /// </summary>
        public CategoryRelationshipExpression()
            : base("[%LogicalName%%]", typeof(ICategoryRelationship))
        {
        }

        #endregion
    }
}
