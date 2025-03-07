using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table DOM_VAL
    /// </summary>
    public interface IDomainValue
    {
        #region - Properties -

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

        /// <summary>
        /// Gets or sets the value of the column descr_txt
        /// </summary>
        /// <value>
        /// Value of the column descr_txt
        /// </value>
        string DescrTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        string NameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        string DefTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column type_code
        /// </summary>
        /// <value>
        /// Value of the column type_code
        /// </value>
        string TypeCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column stdn_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column stdn_lvl_code
        /// </value>
        string StdnLvlCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column src_txt
        /// </summary>
        /// <value>
        /// Value of the column src_txt
        /// </value>
        string SrcTxt { get; set; }

        #endregion
    }
}
