using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table REL
    /// </summary>
    public interface IRelationship
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column pa_ent_id
        /// </summary>
        /// <value>
        /// Value of the column pa_ent_id
        /// </value>
        double PaEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ch_ent_id
        /// </summary>
        /// <value>
        /// Value of the column ch_ent_id
        /// </value>
        double ChEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column rel_ix
        /// </summary>
        /// <value>
        /// Value of the column rel_ix
        /// </value>
        double RelIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column type_code
        /// </summary>
        /// <value>
        /// Value of the column type_code
        /// </value>
        string TypeCode { get; set; }

        #endregion
    }
}
