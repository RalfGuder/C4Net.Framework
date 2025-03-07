using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity BrEntAttrCompDomVal.
    /// </summary>
    public class BusinessRuleEntityAttributeCompositeDomainValueExpression : BaseEntityExpression<BusinessRuleEntityAttributeCompositeDomainValueExpression>
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
        /// Expression for the column br_ent_ix.
        /// </summary>
        /// <value>
        /// The value of the column br_ent_ix.
        /// </value>
        public ColumnExpression BrEntIx
        {
            get { return this.columns[1]; }
        }

        /// <summary>
        /// Expression for the column br_ent_attr_comp_ix.
        /// </summary>
        /// <value>
        /// The value of the column br_ent_attr_comp_ix.
        /// </value>
        public ColumnExpression BrEntAttrCompIx
        {
            get { return this.columns[2]; }
        }

        /// <summary>
        /// Expression for the column br_ent_attr_comp_dom_val_ix.
        /// </summary>
        /// <value>
        /// The value of the column br_ent_attr_comp_dom_val_ix.
        /// </value>
        public ColumnExpression BrEntAttrCompDomValIx
        {
            get { return this.columns[3]; }
        }

        /// <summary>
        /// Expression for the column dom_id.
        /// </summary>
        /// <value>
        /// The value of the column dom_id.
        /// </value>
        public ColumnExpression DomId
        {
            get { return this.columns[4]; }
        }

        /// <summary>
        /// Expression for the column dom_val_ix.
        /// </summary>
        /// <value>
        /// The value of the column dom_val_ix.
        /// </value>
        public ColumnExpression DomValIx
        {
            get { return this.columns[5]; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleEntityAttributeCompositeDomainValueExpression"/> class.
        /// </summary>
        public BusinessRuleEntityAttributeCompositeDomainValueExpression()
            : base("[%LogicalName%%]", typeof(IBusinessRuleEntityAttributeCompositeDomainValue))
        {
        }

        #endregion
    }
}
