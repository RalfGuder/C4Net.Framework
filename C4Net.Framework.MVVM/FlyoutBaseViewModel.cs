using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MahApps.Metro.Controls;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// View Model for a Flyout.
    /// </summary>
    public class FlyoutBaseViewModel : NotifyObject, IDataErrorInfo
    {
        #region - Fields -

        /// <summary>
        /// The header.
        /// </summary>
        private string header;

        /// <summary>
        /// Indicates if the flyout is opened.
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The position.
        /// </summary>
        private Position position;

        /// <summary>
        /// The columns validations dictionary.
        /// </summary>
        protected Dictionary<string, string> columnsValidations = new Dictionary<string, string>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header
        {
            get { return this.header; }
            set { this.SetProperty<string>(ref this.header, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get { return this.isOpen; }
            set { this.SetProperty<bool>(ref this.isOpen, value); }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Position Position
        {
            get { return this.position; }
            set { this.SetProperty<Position>(ref this.position, value); }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        /// <exception cref="System.NotImplementedException"></exception>
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

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutBaseViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="header">The header.</param>
        /// <param name="position">The position.</param>
        public FlyoutBaseViewModel(string name, string header, Position position)
        {
            this.Name = name;
            this.Header = header;
            this.Position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutBaseViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FlyoutBaseViewModel(string name)
            : this(name, name, Position.Left)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Toggle the isOpen state.
        /// </summary>
        public void Toggle()
        {
            this.IsOpen = !this.IsOpen;
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
