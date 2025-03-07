using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for an async execution relay command.
    /// </summary>
    public class RelayAsyncCommand : RelayCommand
    {
        #region - Fields -

        /// <summary>
        /// The is executing.
        /// </summary>
        private bool isExecuting = false;

        #endregion

        #region - Events -

        /// <summary>
        /// Occurs when [started].
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when [ended].
        /// </summary>
        public event EventHandler Ended;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is executing; otherwise, <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get { return this.isExecuting; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayAsyncCommand(Action execute, Func<Boolean> canExecute)
            : base(execute, canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayAsyncCommand(Action execute)
            : base(execute)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public override Boolean CanExecute(Object parameter)
        {
            return ((base.CanExecute(parameter)) && (!this.isExecuting));
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Execute(object parameter)
        {
            try
            {
                this.isExecuting = true;
                if (this.Started != null)
                {
                    this.Started(this, EventArgs.Empty);
                }

                Task task = Task.Factory.StartNew(() =>
                {
                    this.execute();
                });
                task.ContinueWith(t =>
                {
                    this.OnRunWorkerCompleted(EventArgs.Empty);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                this.OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:RunWorkerCompleted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void OnRunWorkerCompleted(EventArgs e)
        {
            this.isExecuting = false;
            if (this.Ended != null)
            {
                this.Ended(this, e);
            }
        }

        #endregion
    }
}
