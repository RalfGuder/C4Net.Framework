using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaTool.Business.Entities
{
    public class ValidCombination
    {
        #region - Properties -

        public List<List<DomainValue>> Values { get; set; }

        public List<bool> AllowNull { get; set; }

        #endregion

        #region - Constructros -

        public ValidCombination()
        {
            this.Values = new List<List<DomainValue>>();
            this.AllowNull = new List<bool>();
        }

        #endregion

        #region - Methods -

        public void Prepare(int attributeCount)
        {
            for (int i = 0; i < attributeCount; i++)
            {
                this.Values.Add(new List<DomainValue>());
                this.AllowNull.Add(false);
            }
        }

        #endregion
    }
}
