using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using Microsoft.Advertising.WinRT.UI;

using HermzApp.WODTimer.Shared;
using HermzApp.WODTimer.Shared.Common;
using HermzApp.WODTimer.Shared.ViewModels;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Windows81.Views
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class TimerShellView : Page
    {
        private readonly NavigationHelper _navigationHelper;
        private readonly TimerShellViewModelBase _timerShellViewModelBase;

        public TimerShellView()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;

            DataContext = _timerShellViewModelBase = TimerShellViewModelBase.CurrentTimerViewModel;
            InitalizeAdControl();
        }

        #region Initalization 
        /// <summary>
        /// WPF doesn't allow data binding for the ApplicationId and AdUnitId on XAML.
        /// This initalization is to assgin the AdUnitId and ApplicationId value of BannarAds from ViewModel.
        /// </summary>
        private void InitalizeAdControl()
        {
            this.BannerAds.AdUnitId = MicrosoftAdsUnitsData.ADUNITID; 
            this.BannerAds.ApplicationId = MicrosoftAdsUnitsData.APPLICATIONID; 
        }
        #endregion

        #region Navigation Helper
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this._navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //object navigationParameter;
            //if (e.PageState != null && e.PageState.ContainsKey("SelectedItem"))
            //{
            //    navigationParameter = e.PageState["SelectedItem"];
            //}
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
            if (_timerShellViewModelBase.IsTimerInProgress)
                App.CanSleepNow();
        }

        #endregion
        #endregion

        #region Event Handlers
        private void UpgradeAppBarButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            var aboutFlyout = new AboutFlyout();
            aboutFlyout.ShowIndependent();
        }

        private void TimerShellView_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            _timerShellViewModelBase.RepsCount++;
        }

        private void ResetRepButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            _timerShellViewModelBase.RepsCount = 0;
        }
               
        /// <summary>
        /// This method only exists to mark the TappedRoutedEvent handled, so it doesn't pass it to page to count as rep.
        /// All the logic is handled by the ICommend in the ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartStopResetButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            var b = sender as Button;

            if (b != null && b.Content != null &&
                String.Compare(b.Content.ToString(), "Start", StringComparison.Ordinal) == 0)
            {
                _timerShellViewModelBase.IsTimerInProgress = true;

                this.TotalTime.Visibility = Visibility.Collapsed;
                MyCountdownControl.Visibility = Visibility.Visible;
                await MyCountdownControl.StartCountdownAsync(_timerShellViewModelBase.SecondsToCountDown, _timerShellViewModelBase.IsAllMuted);
                MyCountdownControl.Visibility = Visibility.Collapsed;
                this.TotalTime.Visibility = Visibility.Visible;
                
                _timerShellViewModelBase.StartTimerCommand.Execute(null);
            }
        }

        private void TimerButtonsOnTapped(object sender, EventArgs e)
        {
            var license = (ProductLicenseDataSource)App.Current.Resources["License"];

            var button = sender as Controls.PresetTimerButton;
            if (button == null) return;

            var sec = int.Parse(button.Name.Substring(3));

            if (sec == 10 || sec == 20 || sec == 240 || license.IsLicensed)
            {
                var viewModel = _timerShellViewModelBase as PresetTimersViewModel;
                if (viewModel != null)
                {
                    viewModel.StartTimer(sec);
                }
            }
            else
            {
                var aboutFlyout = new AboutFlyout();
                aboutFlyout.ShowIndependent();
            }
        }


        /// <summary>
        /// On Ads Error. Don't display the ads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdsError(object sender, AdErrorEventArgs e)
        {
            this.BannerAds.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
