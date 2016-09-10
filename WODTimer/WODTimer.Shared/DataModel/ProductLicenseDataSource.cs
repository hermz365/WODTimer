using System;
using System.ComponentModel;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;

namespace HermzApp.WODTimer.Shared.DataModel
{
    public class ProductLicenseDataSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string Name = "WODTimerPro";
        private string _price;

        public ProductLicenseDataSource()
        {
#if DEV
            if (CurrentAppSimulator.LicenseInformation.ProductLicenses[Name].IsActive)
                IsLicensed = true;
            else
            {
                CurrentAppSimulator.LicenseInformation.LicenseChanged += OnLicenseChanged;
                GetListingInformationAsync();
            }
#else
            if (CurrentApp.LicenseInformation.ProductLicenses[Name].IsActive)
                IsLicensed = true;
            else
            {
                CurrentApp.LicenseInformation.LicenseChanged += OnLicenseChanged;
                GetListingInformationAsync();
            }
#endif
        }

        #region Properties
        public bool IsLicensed { get; private set; }

        public bool IsNotLicensed
        {
            get { return !IsLicensed; }
        }

        public string FormattedPrice
        {
            get
            {
                if (!String.IsNullOrEmpty(_price)) 
                    return "Premium Features for " + _price;

                return "Premium Features";
            }
        }
        #endregion

        #region Private Methods
        private async void GetListingInformationAsync()
        {
            _price = null;
            try
            {
#if DEV
                var listing = await CurrentAppSimulator.LoadListingInformationAsync();
#else
                var listing = await CurrentApp.LoadListingInformationAsync();
#endif
                _price = listing.ProductListings[Name].FormattedPrice;
            }
            catch (Exception)
            {
                _price = null;
            }
        }
        
        private async void OnLicenseChanged()
        {
#if DEV
            if (!CurrentAppSimulator.LicenseInformation.ProductLicenses[Name].IsActive) return;
            CurrentAppSimulator.LicenseInformation.LicenseChanged -= OnLicenseChanged;
#else
            if (!CurrentApp.LicenseInformation.ProductLicenses[Name].IsActive) return;
            CurrentApp.LicenseInformation.LicenseChanged -= OnLicenseChanged;
#endif
            IsLicensed = true;

            await ((App)Application.Current).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs("IsLicensed"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsNotLicensed"));
            });
        }
        #endregion
    }
}
