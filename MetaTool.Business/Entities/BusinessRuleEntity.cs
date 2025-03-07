using System;
using System.Collections.Generic;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table BR_ENT.
    /// </summary>
    [Serializable]
    public partial class BusinessRuleEntity : IBusinessRuleEntity, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly BusinessRuleEntityExpression _ = new BusinessRuleEntityExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column br_id
        /// </summary>
        /// <value>
        /// Value of the column br_id
        /// </value>
        public double BrId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column br_ent_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_ix
        /// </value>
        public double BrEntIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ent_of_interest_id
        /// </summary>
        /// <value>
        /// Value of the column ent_of_interest_id
        /// </value>
        public double EntOfInterestId { get; set; }

        public List<BusinessRuleEntityAttributeComposite> CompositeRules { get; set; }

        #endregion

        #region - Constructors -

        public BusinessRuleEntity()
        {
            this.CompositeRules = new List<BusinessRuleEntityAttributeComposite>();
        }

        #endregion

        #region - Methods -

        public BusinessRuleEntityAttributeComposite GetCompositeRule(long brEntAttrCompIx)
        {
            foreach (BusinessRuleEntityAttributeComposite item in this.CompositeRules)
            {
                if (item.BrEntAttrCompIx == brEntAttrCompIx)
                {
                    return item;
                }
            }
            return null;
        }

        #endregion
    }
}
