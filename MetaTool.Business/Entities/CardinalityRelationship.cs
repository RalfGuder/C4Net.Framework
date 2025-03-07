using System;
using System.Collections.Generic;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table CARD_REL.
    /// </summary>
    [Serializable]
    public partial class CardinalityRelationship : ICardinalityRelationship, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly CardinalityRelationshipExpression _ = new CardinalityRelationshipExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column pa_ent_id
        /// </summary>
        /// <value>
        /// Value of the column pa_ent_id
        /// </value>
        public double PaEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ch_ent_id
        /// </summary>
        /// <value>
        /// Value of the column ch_ent_id
        /// </value>
        public double ChEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column rel_ix
        /// </summary>
        /// <value>
        /// Value of the column rel_ix
        /// </value>
        public double RelIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column verb_name_txt
        /// </summary>
        /// <value>
        /// Value of the column verb_name_txt
        /// </value>
        public string VerbNameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column inv_verb_name_txt
        /// </summary>
        /// <value>
        /// Value of the column inv_verb_name_txt
        /// </value>
        public string InvVerbNameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ident_ind_code
        /// </summary>
        /// <value>
        /// Value of the column ident_ind_code
        /// </value>
        public string IdentIndCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column pa_card_code
        /// </summary>
        /// <value>
        /// Value of the column pa_card_code
        /// </value>
        public string PaCardCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ch_card_code
        /// </summary>
        /// <value>
        /// Value of the column ch_card_code
        /// </value>
        public string ChCardCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ch_mnm_card_cnt
        /// </summary>
        /// <value>
        /// Value of the column ch_mnm_card_cnt
        /// </value>
        public double ChMnmCardCnt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ch_max_card_cnt
        /// </summary>
        /// <value>
        /// Value of the column ch_max_card_cnt
        /// </value>
        public double ChMaxCardCnt { get; set; }

        public Entity Parent { get; set; }

        public Entity Son { get; set; }

        public CardinalityTypeEnum CardinalityType { get; set; }

        public List<Attribute> ParentAttributes { get; set; }

        public List<Attribute> SonAttributes { get; set; }

        public bool IsSelected { get; set; }

        public string Verb { get; set; }

        public string InverseVerb { get; set; }

        #endregion

        #region - Constructors -

        public CardinalityRelationship()
        {
            this.Parent = null;
            this.Son = null;
            this.ParentAttributes = new List<Attribute>();
            this.SonAttributes = new List<Attribute>();
            this.IsSelected = false;
        }

        #endregion

        #region - Methods -

        private CardinalityTypeEnum GetCardinalityType()
        {
            if (this.PaCardCode.Equals("MA"))
            {
                if (this.ChCardCode.Equals("ZM"))
                {
                    return CardinalityTypeEnum.OneToMany;
                }
                else if (this.ChCardCode.Equals("ZO"))
                {
                    return CardinalityTypeEnum.OneToZeroOrOne;
                }
                else
                {
                    return CardinalityTypeEnum.OneToOneOrMore;
                }
            }
            else
            {
                if (this.ChCardCode.Equals("ZM"))
                {
                    return CardinalityTypeEnum.ZeroToMany;
                }
                else if (this.ChCardCode.Equals("ZO"))
                {
                    return CardinalityTypeEnum.ZeroToZeroOrOne;
                }
                else
                {
                    return CardinalityTypeEnum.ZeroToOneOrMore;
                }
            }
        }

        public void SetCardinality()
        {
            this.CardinalityType = this.GetCardinalityType();
        }

        public void AddAttributes(Attribute parentAttribute, Attribute sonAttribute)
        {
            this.ParentAttributes.Add(parentAttribute);
            this.SonAttributes.Add(sonAttribute);
        }

        #endregion
    }
}
