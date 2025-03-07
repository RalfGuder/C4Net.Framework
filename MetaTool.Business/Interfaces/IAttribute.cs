using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table ATTR
    /// </summary>
    public interface IAttribute
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
        /// Gets or sets the value of the column attr_ix
        /// </summary>
        /// <value>
        /// Value of the column attr_ix
        /// </value>
        double AttrIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        string NameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column col_name_txt
        /// </summary>
        /// <value>
        /// Value of the column col_name_txt
        /// </value>
        string ColNameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column attr_seqnr_ord
        /// </summary>
        /// <value>
        /// Value of the column attr_seqnr_ord
        /// </value>
        double AttrSeqnrOrd { get; set; }

        /// <summary>
        /// Gets or sets the value of the column pk_ind_code
        /// </summary>
        /// <value>
        /// Value of the column pk_ind_code
        /// </value>
        string PkIndCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column fk_ind_code
        /// </summary>
        /// <value>
        /// Value of the column fk_ind_code
        /// </value>
        string FkIndCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column stdn_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column stdn_lvl_code
        /// </value>
        string StdnLvlCode { get; set; }

        #endregion
    }
}
