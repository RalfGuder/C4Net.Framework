using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table SUBT_REL
    /// </summary>
    public interface ISubtypeRelationship
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column sup_ent_id
        /// </summary>
        /// <value>
        /// Value of the column sup_ent_id
        /// </value>
        double SupEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column sub_ent_id
        /// </summary>
        /// <value>
        /// Value of the column sub_ent_id
        /// </value>
        double SubEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column rel_ix
        /// </summary>
        /// <value>
        /// Value of the column rel_ix
        /// </value>
        double RelIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column cat_ix
        /// </summary>
        /// <value>
        /// Value of the column cat_ix
        /// </value>
        double CatIx { get; set; }

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
