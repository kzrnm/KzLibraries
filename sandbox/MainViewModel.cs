using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sandbox
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _Double1 = 13232231.012;
        public double Double1
        {
            set => SetProperty(ref _Double1, value);
            get => _Double1;
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
