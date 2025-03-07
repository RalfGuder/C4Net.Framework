using System.ComponentModel;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Observable and editable key-value item.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class ObservableKeyValueItem<TKey, TValue> : NotifyObject, IEditableObject
    {
        #region - Fields -

        /// <summary>
        /// Saves the old key
        /// </summary>
        private TKey oldKey;

        /// <summary>
        /// Saves the old value
        /// </summary>
        private TValue oldValue;

        /// <summary>
        /// The key.
        /// </summary>
        private TKey key;

        /// <summary>
        /// The value
        /// </summary>
        private TValue value;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public TKey Key
        {
            get { return this.key; }
            set { this.SetProperty<TKey>(ref this.key, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TValue Value
        {
            get { return this.value; }
            set { this.SetProperty<TValue>(ref this.value, value); }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableKeyValueItem{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public ObservableKeyValueItem(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            this.oldKey = this.key;
            this.oldValue = this.value;
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit" /> call.
        /// </summary>
        public void CancelEdit()
        {
            this.Key = this.oldKey;
            this.Value = this.oldValue;
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit" /> or <see cref="M:System.ComponentModel.IBindingList.AddNew" /> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
        }

        #endregion
    }
}
