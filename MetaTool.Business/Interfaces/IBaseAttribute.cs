using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table BASE_ATTR
    /// </summary>
    public interface IBaseAttribute
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
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        string DefTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column data_type_code
        /// </summary>
        /// <value>
        /// Value of the column data_type_code
        /// </value>
        string DataTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column data_len_cnt
        /// </summary>
        /// <value>
        /// Value of the column data_len_cnt
        /// </value>
        double DataLenCnt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column data_dec_cnt
        /// </summary>
        /// <value>
        /// Value of the column data_dec_cnt
        /// </value>
        double DataDecCnt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column dom_id
        /// </summary>
        /// <value>
        /// Value of the column dom_id
        /// </value>
        double DomId { get; set; }

        #endregion
    }
}
