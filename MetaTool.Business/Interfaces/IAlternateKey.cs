using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table ALT_KEY
    /// </summary>
    public interface IAlternateKey
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column ent_id
        /// </summary>
        /// <value>
        /// Value of the column ent_id
        /// </value>
        double EntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ak_ix
        /// </summary>
        /// <value>
        /// Value of the column ak_ix
        /// </value>
        double AkIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ak_no_qty
        /// </summary>
        /// <value>
        /// Value of the column ak_no_qty
        /// </value>
        double AkNoQty { get; set; }

        /// <summary>
        /// Gets or sets the value of the column uniq_ind_code
        /// </summary>
        /// <value>
        /// Value of the column uniq_ind_code
        /// </value>
        string UniqIndCode { get; set; }

        #endregion
    }
}
