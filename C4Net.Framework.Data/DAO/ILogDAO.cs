using C4Net.Framework.Data.Attributes;

namespace C4Net.Framework.Data.DAO
{
    public interface ILogDAO : IDAO
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the value of the column creator_id.
        ///
        /// A value assigned to the row to identify the organisation which created that row. This is referenced by an application level business rule to an OBJ_ITEM entry with a cat_code = OR and to a corresponding ORG subtype entry.
        /// </summary>
        /// <value>
        /// Value of the column creator_id.
        /// </value>
        [DataLength(20)]
        decimal CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the value of the column update_seqnr.
        ///
        /// An absolute sequence number, assigned to represent the validity (in terms of seniority) of a certain data item.
        /// </summary>
        /// <value>
        /// Value of the column update_seqnr.
        /// </value>
        [DataLength(15)]
        long UpdateSeqnr { get; set; }

        #endregion
    }
}
