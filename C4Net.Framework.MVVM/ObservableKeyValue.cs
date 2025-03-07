using System.Collections.ObjectModel;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for an observable collection of Key-Value items.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class ObservableKeyValue<TKey, TValue> : ObservableCollection<ObservableKeyValueItem<TKey, TValue>>
    {
        #region - Methods -

        /// <summary>
        /// Determines whether the collection contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            foreach (ObservableKeyValueItem<TKey, TValue> item in this)
            {
                if (item.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            this.Add(new ObservableKeyValueItem<TKey, TValue>(key, value));
        }

        #endregion
    }
}
