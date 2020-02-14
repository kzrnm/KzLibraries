using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace sandbox
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _Text1 = "Text1";
        public string Text1
        {
            set => SetProperty(ref _Text1, value);
            get => _Text1;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }
        #endregion INotifyPropertyChanged
    }
}
