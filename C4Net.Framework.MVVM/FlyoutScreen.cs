using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Caliburn.Micro;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for a screen that is also a Flyout container.
    /// </summary>
    public abstract class FlyoutScreen : Screen, IDataErrorInfo
    {
        #region - Fields -

        /// <summary>
        /// The flyouts collection.
        /// </summary>
        protected IObservableCollection<FlyoutBaseViewModel> flyouts = new BindableCollection<FlyoutBaseViewModel>();

        /// <summary>
        /// The flyout dictionary that relates name of the flyout with the instance.
        /// </summary>
        private Dictionary<string, FlyoutBaseViewModel> flyoutDict = new Dictionary<string, FlyoutBaseViewModel>();

        /// <summary>
        /// The columns validations dictionary.
        /// </summary>
        protected Dictionary<string, string> columnsValidations = new Dictionary<string, string>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the flyout view models collection.
        /// </summary>
        /// <value>
        /// The flyout view models.
        /// </value>
        public IObservableCollection<FlyoutBaseViewModel> FlyoutViewModels
        {
            get { return this.flyouts; }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in columnsValidations.Values)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        builder.Append(str);
                        builder.Append("\n");
                    }
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string this[string columnName]
        {
            get
            {
                string result = this.ValidateColumn(columnName);
                if (this.columnsValidations.ContainsKey(columnName))
                {
                    this.columnsValidations[columnName] = result;
                }
                else
                {
                    this.columnsValidations.Add(columnName, result);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is validated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidated
        {
            get
            {
                foreach (string str in this.columnsValidations.Values)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the flyout.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FlyoutBaseViewModel GetFlyout(string name)
        {
            return this.flyoutDict[name];
        }

        /// <summary>
        /// Toggles the open state of a flyout by the index of the flyout in the collection.
        /// </summary>
        /// <param name="index">The index.</param>
        public void ToggleFlyoutByIndex(int index)
        {
            this.flyouts[index].Toggle();
        }

        /// <summary>
        /// Toggles the open state of a flyout by the name of the flyout.
        /// </summary>
        /// <param name="name">The name.</param>
        public void ToggleFlyout(string name)
        {
            this.flyoutDict[name].Toggle();
        }

        /// <summary>
        /// Adds one flyout.
        /// </summary>
        /// <param name="flyout">The flyout.</param>
        public void AddFlyout(FlyoutBaseViewModel flyout)
        {
            this.flyouts.Add(flyout);
            this.flyoutDict.Add(flyout.Name, flyout);
        }

        /// <summary>
        /// Abstract method to initialize the flyouts.
        /// </summary>
        protected abstract void InitializeFlyouts();

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.InitializeFlyouts();
            base.OnInitialize();
        }

        /// <summary>
        /// Class for a Notify Object, that is a PropertyChangedBase object able to set a field calling the
        /// Notify Property Changed.
        /// </summary>
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string callerName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                NotifyOfPropertyChange(callerName);
            }
        }

        /// <summary>
        /// Validates the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        protected virtual string ValidateColumn(string columnName)
        {
            return string.Empty;
        }

        #endregion

    }
}
