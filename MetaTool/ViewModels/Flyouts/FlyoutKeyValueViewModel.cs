using C4Net.Framework.MVVM;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace MetaTool.ViewModels.Flyouts
{
    /// <summary>
    /// ViewModel for the New/Edit KeyValue flyout.
    /// </summary>
    public class FlyoutKeyValueViewModel : FlyoutBaseViewModel
    {
        #region - Fields -

        /// <summary>
        /// The key
        /// </summary>
        private string key;

        /// <summary>
        /// The value
        /// </summary>
        private string value;

        /// <summary>
        /// The original parameter
        /// </summary>
        private ObservableKeyValueItem<string, string> originalParameter;

        /// <summary>
        /// Indicates if is edit.
        /// </summary>
        private bool isEdit;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key
        {
            get { return this.key; }
            set { this.SetProperty<string>(ref this.key, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get { return this.value; }
            set { this.SetProperty<string>(ref this.value, value); }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutKeyValueViewModel"/> class.
        /// </summary>
        public FlyoutKeyValueViewModel()
            : base("Parameter", "Parameter", Position.Bottom)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a new parameter.
        /// </summary>
        public void Add()
        {
            this.originalParameter = null;
            this.isEdit = false;
            this.Header = "New parameter";
            this.Key = "";
            this.Value = "";
            this.IsOpen = true;
        }

        /// <summary>
        /// Edits the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Edit(ObservableKeyValueItem<string, string> parameter)
        {
            this.originalParameter = parameter;
            this.isEdit = true;
            this.Header = "Edit parameter";
            this.Key = this.originalParameter.Key;
            this.Value = this.originalParameter.Value;
            this.IsOpen = true;
        }

        /// <summary>
        /// Saves the parameter.
        /// </summary>
        public void Save()
        {
            if (this.isEdit)
            {
                this.originalParameter.Key = this.Key;
                this.originalParameter.Value = this.Value;
            }
            else
            {
                IoC.Get<IShell>().Parameters.Add(this.Key, this.Value);
            }
            this.IsOpen = false;
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            this.IsOpen = false;
        }

        /// <summary>
        /// Validates the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        protected override string ValidateColumn(string columnName)
        {
            if (columnName.Equals("Key"))
            {
                if (string.IsNullOrEmpty(this.Key))
                {
                    return "The key cannot be null";
                }
                if ((!this.isEdit) || ((this.isEdit) && (!this.Key.Equals(this.originalParameter.Key))))
                {
                    if (IoC.Get<IShell>().Parameters.ContainsKey(this.Key))
                    {
                        return "There is another parameter with this key";
                    }
                }
            }
            return base.ValidateColumn(columnName);
        }

        #endregion
    }
}
