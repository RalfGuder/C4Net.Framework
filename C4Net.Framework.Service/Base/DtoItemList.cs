using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using C4Net.Framework.Business.DTO;

namespace C4Net.Framework.Service.Base
{
    [DataContract(Name = "DtoItemList", Namespace = "")]
    public class DtoItemList<T> where T : IDTO
    {
        #region - Properties -

        [DataMember]
        public List<DtoItem<T>> Items { get; private set; }

        [DataMember]
        public Uri NextPage { get; private set; }

        [DataMember]
        public Uri PreviousPage { get; private set; }

        #endregion

        #region - Constructors -

        public DtoItemList(List<DtoItem<T>> items, Uri nextPage, Uri previousPage)
        {
            this.Items = items;
            this.NextPage = nextPage;
            this.PreviousPage = previousPage;
        }

        #endregion
    }
}
