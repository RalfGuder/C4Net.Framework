using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity CardRel.
    /// </summary>
    public class CardinalityRelationshipExpression : BaseEntityExpression<CardinalityRelationshipExpression>
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
        /// Expression for the column verb_name_txt.
        /// </summary>
        /// <value>
        /// The value of the column verb_name_txt.
        /// </value>
        public ColumnExpression VerbNameTxt 
        { 
            get { return this.columns[3]; } 
        }
		
        /// <summary>
        /// Expression for the column inv_verb_name_txt.
        /// </summary>
        /// <value>
        /// The value of the column inv_verb_name_txt.
        /// </value>
        public ColumnExpression InvVerbNameTxt 
        { 
            get { return this.columns[4]; } 
        }
		
        /// <summary>
        /// Expression for the column ident_ind_code.
        /// </summary>
        /// <value>
        /// The value of the column ident_ind_code.
        /// </value>
        public ColumnExpression IdentIndCode 
        { 
            get { return this.columns[5]; } 
        }
		
        /// <summary>
        /// Expression for the column pa_card_code.
        /// </summary>
        /// <value>
        /// The value of the column pa_card_code.
        /// </value>
        public ColumnExpression PaCardCode 
        { 
            get { return this.columns[6]; } 
        }
		
        /// <summary>
        /// Expression for the column ch_card_code.
        /// </summary>
        /// <value>
        /// The value of the column ch_card_code.
        /// </value>
        public ColumnExpression ChCardCode 
        { 
            get { return this.columns[7]; } 
        }
		
        /// <summary>
        /// Expression for the column ch_mnm_card_cnt.
        /// </summary>
        /// <value>
        /// The value of the column ch_mnm_card_cnt.
        /// </value>
        public ColumnExpression ChMnmCardCnt 
        { 
            get { return this.columns[8]; } 
        }
		
        /// <summary>
        /// Expression for the column ch_max_card_cnt.
        /// </summary>
        /// <value>
        /// The value of the column ch_max_card_cnt.
        /// </value>
        public ColumnExpression ChMaxCardCnt 
        { 
            get { return this.columns[9]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="CardinalityRelationshipExpression"/> class.
        /// </summary>
        public CardinalityRelationshipExpression()
            : base("[%LogicalName%%]", typeof(ICardinalityRelationship))
        {
        }

        #endregion
    }
}
