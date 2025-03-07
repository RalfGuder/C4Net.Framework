using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table CREATOR_UPDATE_IDENTIFIC.
    /// </summary>
    [Serializable]
    public partial class CreatorUpdateIdentification : ICreatorUpdateIdentification, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly CreatorUpdateIdentificationExpression _ = new CreatorUpdateIdentificationExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column ent_id
        /// </summary>
        /// <value>
        /// Value of the column ent_id
        /// </value>
        public double EntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column creator_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column creator_attr_ix
        /// </value>
        public double CreatorAttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column update_attr_ix
        /// </summary>
        /// <value>
        /// Value of the column update_attr_ix
        /// </value>
        public double UpdateAttrIx { get; set; }
		
        #endregion
    }
}
