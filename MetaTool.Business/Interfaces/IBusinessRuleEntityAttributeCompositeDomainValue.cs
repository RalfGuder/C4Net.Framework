using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table BR_ENT_ATTR_COMP_DOM_VAL
    /// </summary>
    public interface IBusinessRuleEntityAttributeCompositeDomainValue
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column br_id
        /// </summary>
        /// <value>
        /// Value of the column br_id
        /// </value>
        double BrId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column br_ent_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_ix
        /// </value>
        double BrEntIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column br_ent_attr_comp_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_attr_comp_ix
        /// </value>
        double BrEntAttrCompIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column br_ent_attr_comp_dom_val_ix
        /// </summary>
        /// <value>
        /// Value of the column br_ent_attr_comp_dom_val_ix
        /// </value>
        double BrEntAttrCompDomValIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column dom_id
        /// </summary>
        /// <value>
        /// Value of the column dom_id
        /// </value>
        double DomId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column dom_val_ix
        /// </summary>
        /// <value>
        /// Value of the column dom_val_ix
        /// </value>
        double DomValIx { get; set; }

        #endregion
    }
}
