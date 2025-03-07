using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table DOM
    /// </summary>
    public interface IDomain
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
        /// Gets or sets the value of the column class_name_txt
        /// </summary>
        /// <value>
        /// Value of the column class_name_txt
        /// </value>
        string ClassNameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column restr_type_code
        /// </summary>
        /// <value>
        /// Value of the column restr_type_code
        /// </value>
        string RestrTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column meas_unit_descr_txt
        /// </summary>
        /// <value>
        /// Value of the column meas_unit_descr_txt
        /// </value>
        string MeasUnitDescrTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column pa_dom_id
        /// </summary>
        /// <value>
        /// Value of the column pa_dom_id
        /// </value>
        double PaDomId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column stdn_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column stdn_lvl_code
        /// </value>
        string StdnLvlCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column mod_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column mod_lvl_code
        /// </value>
        string ModLvlCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column def_src_txt
        /// </summary>
        /// <value>
        /// Value of the column def_src_txt
        /// </value>
        string DefSrcTxt { get; set; }

        #endregion
    }
}
