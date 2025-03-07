using System;
using System.Collections.Generic;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table BR.
    /// </summary>
    [Serializable]
    public partial class BusinessRule : IBusinessRule, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly BusinessRuleExpression _ = new BusinessRuleExpression();

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
        /// Gets or sets the value of the column cat_code
        /// </summary>
        /// <value>
        /// Value of the column cat_code
        /// </value>
        public string CatCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column section_xref_txt
        /// </summary>
        /// <value>
        /// Value of the column section_xref_txt
        /// </value>
        public string SectionXrefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        public string NameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        public string DefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column tab_xref_txt
        /// </summary>
        /// <value>
        /// Value of the column tab_xref_txt
        /// </value>
        public string TabXrefTxt { get; set; }

        /// <summary>
        /// Gets or sets the ent of interest id.
        /// </summary>
        /// <value>
        /// The ent of interest id.
        /// </value>
        public double EntOfInterestId { get; set; }

        /// <summary>
        /// Gets or sets the ent of interest.
        /// </summary>
        /// <value>
        /// The ent of interest.
        /// </value>
        public Entity EntOfInterest { get; set; }

        public List<Attribute> Attributes { get; set; }

        public List<ValidCombination> ValidCombination { get; set; }

        public List<BusinessRuleEntity> EntityRules { get; set; }

        public bool IsSelected { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRule"/> class.
        /// </summary>
        public BusinessRule()
        {
            this.EntOfInterest = null;
            this.EntOfInterestId = 0;
            this.Attributes = new List<Attribute>();
            this.EntityRules = new List<BusinessRuleEntity>();
            this.IsSelected = false;
        }

        #endregion

        #region - Methods -

        public BusinessRuleEntity GetEntityRule(long brEntIx)
        {
            foreach (BusinessRuleEntity item in this.EntityRules)
            {
                if (item.BrEntIx == brEntIx)
                {
                    return item;
                }
            }
            return null;
        }

        public int AttributePosition(double attrIx)
        {
            for (int i = 0; i < this.Attributes.Count; i++)
            {
                Attribute attribute = this.Attributes[i];
                if (attribute.AttrIx == attrIx)
                {
                    return i;
                }
            }
            return -1;
        }

        public void BuildValidCombinations(Dictionary<string, Domain> domains)
        {
            if (this.EntityRules.Count == 0)
            {
                return;
            }
            // 1. Get the attributes
            BusinessRuleEntity currentRuleEntity = this.EntityRules[0];
            foreach (BusinessRuleEntityAttributeComposite composite in currentRuleEntity.CompositeRules)
            {
                this.Attributes.Add(this.EntOfInterest.GetAttributeByIx(Convert.ToInt32(composite.AttrIx)));
            }
            // 2. Get the valid combinations
            foreach (BusinessRuleEntity item in this.EntityRules)
            {
                // each of this is a new rule.
                ValidCombination combination = new ValidCombination();
                combination.Prepare(this.Attributes.Count);
                foreach (BusinessRuleEntityAttributeComposite composite in item.CompositeRules)
                {
                    int attributePosition = this.AttributePosition(composite.AttrIx);
                    combination.AllowNull[attributePosition] = composite.NullIndCode.Equals("YES");
                    foreach (BusinessRuleEntityAttributeCompositeDomainValue value in composite.DomainValues)
                    {
                        Domain domain = domains[value.DomId.ToString()];
                        DomainValue domVal = domain.GetValue(Convert.ToInt32(value.DomValIx));
                        combination.Values[attributePosition].Add(domVal);
                    }
                }
            }
        }

        #endregion
    }
}
