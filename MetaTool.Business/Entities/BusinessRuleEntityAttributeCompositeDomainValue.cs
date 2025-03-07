using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table BR_ENT_ATTR_COMP_DOM_VAL.
    /// </summary>
    [Serializable]
    public partial class BusinessRuleEntityAttributeCompositeDomainValue : IBusinessRuleEntityAttributeCompositeDomainValue, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly BusinessRuleEntityAttributeCompositeDomainValueExpression _ = new BusinessRuleEntityAttributeCompositeDomainValueExpression();

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
        /// Gets or sets the value of the column br_ent_attr_comp_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_attr_comp_ix
        /// </value>
        public double BrEntAttrCompIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column br_ent_attr_comp_dom_val_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_attr_comp_dom_val_ix
        /// </value>
        public double BrEntAttrCompDomValIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column dom_id
        /// </summary>
        /// <value>
        /// Value of the column dom_id
        /// </value>
        public double DomId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column dom_val_ix
        /// </summary>
        /// <value>
        /// Value of the column dom_val_ix
        /// </value>
        public double DomValIx { get; set; }
		
        #endregion
    }
}
