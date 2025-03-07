using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Interfaces
{
    /// <summary>
    /// Interface for the model class for the table FK_ATTR
    /// </summary>
    public interface IForeignKeyAttribute
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column host_ent_id
        /// </summary>
        /// <value>
        /// Value of the column host_ent_id
        /// </value>
        double HostEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column attr_ix
        /// </summary>
        /// <value>
        /// Value of the column attr_ix
        /// </value>
        double AttrIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column role_def_txt
        /// </summary>
        /// <value>
        /// Value of the column role_def_txt
        /// </value>
        string RoleDefTxt { get; set; }

        /// <summary>
        /// Gets or sets the value of the column rona_ind_code
        /// </summary>
        /// <value>
        /// Value of the column rona_ind_code
        /// </value>
        string RonaIndCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the column src_ent_id
        /// </summary>
        /// <value>
        /// Value of the column src_ent_id
        /// </value>
        double SrcEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column src_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column src_attr_ix
        /// </value>
        double SrcAttrIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column migr_rel_ix
        /// </summary>
        /// <value>
        /// Value of the column migr_rel_ix
        /// </value>
        double MigrRelIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column base_ent_id
        /// </summary>
        /// <value>
        /// Value of the column base_ent_id
        /// </value>
        double BaseEntId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column base_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column base_attr_ix
        /// </value>
        double BaseAttrIx { get; set; }

        /// <summary>
        /// Gets or sets the value of the column unif_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column unif_attr_ix
        /// </value>
        double UnifAttrIx { get; set; }

        #endregion
    }
}
