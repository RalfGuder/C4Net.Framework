using System;
using System.Runtime.Serialization;
using C4Net.Framework.Business.DTO;

namespace C4Net.Framework.Service.Base
{
    [DataContract(Name = "DtoItem", Namespace = "")]
    public class DtoItem<T> where T : IDTO
    {
        #region - Properties -

        [DataMember]
        public Uri ItemLink { get; set; }

        [DataMember]
        public T Item { get; set; }

        #endregion
    }
}
