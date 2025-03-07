using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table BR
    /// </summary>
    public interface IBusinessRule
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
        /// Gets or sets the value of the column cat_code
        /// </summary>
        /// <value>
        /// Value of the column cat_code
        /// </value>
        string CatCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column section_xref_txt
        /// </summary>
        /// <value>
        /// Value of the column section_xref_txt
        /// </value>
        string SectionXrefTxt { get; set; }

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
        /// Gets or sets the value of the column tab_xref_txt
        /// </summary>
        /// <value>
        /// Value of the column tab_xref_txt
        /// </value>
        string TabXrefTxt { get; set; }

        #endregion
    }
}
