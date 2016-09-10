using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
