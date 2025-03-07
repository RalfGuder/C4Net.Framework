using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table CAT.
    /// </summary>
    [Serializable]
    public partial class CategoryRelationship : ICategoryRelationship, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly CategoryRelationshipExpression _ = new CategoryRelationshipExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column sup_ent_id
        /// </summary>
        /// <value>
        /// Value of the column sup_ent_id
        /// </value>
        public double SupEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column cat_ix
        /// </summary>
        /// <value>
        /// Value of the column cat_ix
        /// </value>
        public double CatIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        public string DefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column discr_ix
        /// </summary>
        /// <value>
        /// Value of the column discr_ix
        /// </value>
        public double DiscrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column compl_ind_code
        /// </summary>
        /// <value>
        /// Value of the column compl_ind_code
        /// </value>
        public string ComplIndCode { get; set; }
		
        #endregion
    }
}
