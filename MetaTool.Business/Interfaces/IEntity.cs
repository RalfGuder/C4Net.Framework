using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table ENT
    /// </summary>
    public interface IEntity
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
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        string NameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column tab_name_txt
        /// </summary>
        /// <value>
        /// Value of the column tab_name_txt
        /// </value>
        string TabNameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        string DefTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column depen_code
        /// </summary>
        /// <value>
        /// Value of the column depen_code
        /// </value>
        string DepenCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column depth_cnt
        /// </summary>
        /// <value>
        /// Value of the column depth_cnt
        /// </value>
        double DepthCnt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column stg_type_code
        /// </summary>
        /// <value>
        /// Value of the column stg_type_code
        /// </value>
        string StgTypeCode { get; set; }

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

        #endregion
    }
}
