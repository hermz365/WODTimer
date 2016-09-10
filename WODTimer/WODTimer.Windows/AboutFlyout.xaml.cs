using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;

using Windows.ApplicationModel.Store;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace HermzApp.WODTimer.Windows81
{
    public sealed partial class AboutFlyout : SettingsFlyout
    {
        public AboutFlyout()
        {
            this.InitializeComponent();
        }

        private async void BuyNowAppBarButton_Click(object sender, RoutedEventArgs e)
        {
#if DEV
            var licenseInformation = CurrentAppSimulator.LicenseInformation;
#else
            var licenseInformation = CurrentApp.LicenseInformation;
#endif
            if (!licenseInformation.ProductLicenses["WODTimerPro"].IsActive)
            {
                try
                {
#if DEV
                    await CurrentAppSimulator.RequestProductPurchaseAsync("WODTimerPro");
#else
                    await CurrentApp.RequestProductPurchaseAsync("WODTimerPro");
#endif
                }
                catch (Exception)
                {
                    var messageDialog = new MessageDialog("Sorry! Unable to complete the purchase. Please try again later.");
                    messageDialog.Commands.Add(new UICommand("Close", null));
                    messageDialog.ShowAsync();
                }
            }
        }
    }
}
