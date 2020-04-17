using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XFCovid19.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation;
        public BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        public static bool InternetConnectivity()
        {
            var connectivity = Connectivity.NetworkAccess;
            if (connectivity == NetworkAccess.Internet)
                return true;

            return false;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetProperty(ref _isBusy, value);
            }
        }

        private bool _isBusyCountry;
        public bool IsBusyCountry
        {
            get { return _isBusyCountry; }
            set
            {
                SetProperty(ref _isBusyCountry, value);
            }
        }

        private bool _isTouched;
        public bool IsTouched
        {
            get { return _isTouched; }
            set
            {
                SetProperty(ref _isTouched, value);
            }
        }

        public virtual void OnDisappearing() { }
    }
}

