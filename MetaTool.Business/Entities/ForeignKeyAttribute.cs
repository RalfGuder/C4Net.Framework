using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table FK_ATTR.
    /// </summary>
    [Serializable]
    public partial class ForeignKeyAttribute : IForeignKeyAttribute, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly ForeignKeyAttributeExpression _ = new ForeignKeyAttributeExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column host_ent_id
        /// </summary>
        /// <value>
        /// Value of the column host_ent_id
        /// </value>
        public double HostEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column attr_ix
        /// </summary>
        /// <value>
        /// Value of the column attr_ix
        /// </value>
        public double AttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column role_def_txt
        /// </summary>
        /// <value>
        /// Value of the column role_def_txt
        /// </value>
        public string RoleDefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column rona_ind_code
        /// </summary>
        /// <value>
        /// Value of the column rona_ind_code
        /// </value>
        public string RonaIndCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column src_ent_id
        /// </summary>
        /// <value>
        /// Value of the column src_ent_id
        /// </value>
        public double SrcEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column src_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column src_attr_ix
        /// </value>
        public double SrcAttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column migr_rel_ix
        /// </summary>
        /// <value>
        /// Value of the column migr_rel_ix
        /// </value>
        public double MigrRelIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column base_ent_id
        /// </summary>
        /// <value>
        /// Value of the column base_ent_id
        /// </value>
        public double BaseEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column base_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column base_attr_ix
        /// </value>
        public double BaseAttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column unif_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column unif_attr_ix
        /// </value>
        public double UnifAttrIx { get; set; }
		
        #endregion
    }
}
