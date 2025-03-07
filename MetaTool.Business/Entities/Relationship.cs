using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table REL.
    /// </summary>
    [Serializable]
    public partial class Relationship : IRelationship, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly RelationshipExpression _ = new RelationshipExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column pa_ent_id
        /// </summary>
        /// <value>
        /// Value of the column pa_ent_id
        /// </value>
        public double PaEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column ch_ent_id
        /// </summary>
        /// <value>
        /// Value of the column ch_ent_id
        /// </value>
        public double ChEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column rel_ix
        /// </summary>
        /// <value>
        /// Value of the column rel_ix
        /// </value>
        public double RelIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column type_code
        /// </summary>
        /// <value>
        /// Value of the column type_code
        /// </value>
        public string TypeCode { get; set; }
		
        #endregion
    }
}
