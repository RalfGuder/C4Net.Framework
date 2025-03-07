using System;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table SUBT_REL.
    /// </summary>
    [Serializable]
    public partial class SubtypeRelationship : ISubtypeRelationship, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly SubtypeRelationshipExpression _ = new SubtypeRelationshipExpression();

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
        /// Gets or sets the value of the column sub_ent_id
        /// </summary>
        /// <value>
        /// Value of the column sub_ent_id
        /// </value>
        public double SubEntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column rel_ix
        /// </summary>
        /// <value>
        /// Value of the column rel_ix
        /// </value>
        public double RelIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column cat_ix
        /// </summary>
        /// <value>
        /// Value of the column cat_ix
        /// </value>
        public double CatIx { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column dom_id
        /// </summary>
        /// <value>
        /// Value of the column dom_id
        /// </value>
        public double DomId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column dom_val_ix
        /// </summary>
        /// <value>
        /// Value of the column dom_val_ix
        /// </value>
        public double DomValIx { get; set; }

        public Entity Parent { get; set; }

        public Entity Son { get; set; }

        public Attribute CategoryAttribute { get; set; }

        public DomainValue DomainValue { get; set; }

        #endregion

        #region - Constructors -

        public SubtypeRelationship()
        {
            this.Parent = null;
            this.Son = null;
            this.CategoryAttribute = null;
            this.DomainValue = null;
        }

        #endregion

        #region - Methods -
        #endregion
    }
}
