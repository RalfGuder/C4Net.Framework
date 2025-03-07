using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table CARD_REL
    /// </summary>
    public interface ICardinalityRelationship
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
        /// Gets or sets the value of the column verb_name_txt
        /// </summary>
        /// <value>
        /// Value of the column verb_name_txt
        /// </value>
        string VerbNameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column inv_verb_name_txt
        /// </summary>
        /// <value>
        /// Value of the column inv_verb_name_txt
        /// </value>
        string InvVerbNameTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ident_ind_code
        /// </summary>
        /// <value>
        /// Value of the column ident_ind_code
        /// </value>
        string IdentIndCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column pa_card_code
        /// </summary>
        /// <value>
        /// Value of the column pa_card_code
        /// </value>
        string PaCardCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ch_card_code
        /// </summary>
        /// <value>
        /// Value of the column ch_card_code
        /// </value>
        string ChCardCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ch_mnm_card_cnt
        /// </summary>
        /// <value>
        /// Value of the column ch_mnm_card_cnt
        /// </value>
        double ChMnmCardCnt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column ch_max_card_cnt
        /// </summary>
        /// <value>
        /// Value of the column ch_max_card_cnt
        /// </value>
        double ChMaxCardCnt { get; set; }

        #endregion
    }
}
