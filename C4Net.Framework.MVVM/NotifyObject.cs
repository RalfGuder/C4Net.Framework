using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace C4Net.Framework.MVVM
{
    /// <summary>
    /// Class for a Notify Object, that is a PropertyChangedBase object able to set a field calling the
    /// Notify Property Changed.
    /// </summary>
    public class NotifyObject : PropertyChangedBase
    {
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string callerName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                NotifyOfPropertyChange(callerName);
            }
        }
    }
}
