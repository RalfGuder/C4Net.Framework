using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table AK_ATTR.
    /// </summary>
    [Serializable]
    public partial class AlternateKeyAttribute : IAlternateKeyAttribute, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly AlternateKeyAttributeExpression _ = new AlternateKeyAttributeExpression();

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
        /// Gets or sets the value of the column attr_ix
        /// </summary>
        /// <value>
        /// Value of the column attr_ix
        /// </value>
        public double AttrIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ak_ix
        /// </summary>
        /// <value>
        /// Value of the column ak_ix
        /// </value>
        public double AkIx { get; set; }
		
        #endregion
    }
}
