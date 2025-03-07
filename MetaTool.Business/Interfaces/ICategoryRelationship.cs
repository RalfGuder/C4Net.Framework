using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table CAT
    /// </summary>
    public interface ICategoryRelationship
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
        /// Gets or sets the value of the column cat_ix
        /// </summary>
        /// <value>
        /// Value of the column cat_ix
        /// </value>
        double CatIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        string DefTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column discr_ix
        /// </summary>
        /// <value>
        /// Value of the column discr_ix
        /// </value>
        double DiscrIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column compl_ind_code
        /// </summary>
        /// <value>
        /// Value of the column compl_ind_code
        /// </value>
        string ComplIndCode { get; set; }

        #endregion
    }
}
