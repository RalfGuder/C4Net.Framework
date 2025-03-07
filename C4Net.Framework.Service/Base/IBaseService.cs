using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C4Net.Framework.Business.DTO;

namespace C4Net.Framework.Service.Base
{
    public interface IBaseService<T> where T : IDTO
    {
        DtoItemList<T> GetList();

        T GetItem(string key);

        DtoItem<T> CreateItem(T dto);

        T UpdateItem(T dto);

        void DeleteItem(string key);
    }
}
