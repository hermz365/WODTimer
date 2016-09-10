using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using HermzApp.WODTimer.Shared.ViewModels;
using HermzApp.WODTimer.Shared.Common;
using HermzApp.WODTimer.Shared.DataModel;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231
using HermzApp.WODTimer.Windows81.Views;

namespace HermzApp.WODTimer.Windows81
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this._navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this._defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this._navigationHelper = new NavigationHelper(this);
            this._navigationHelper.LoadState += navigationHelper_LoadState;
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
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await WODTimerDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((WODTimerDataItem)e.ClickedItem).UniqueId;
            TimerShellViewModelBase viewModel = null;
            Type pageTo = pageTo = typeof(TimerShellView);

            switch (((WODTimerDataItem)e.ClickedItem).UniqueId)
            {
                case "Standard-Timer":
                    TimerShellViewModelBase.CurrentTimerViewModel = new StandardTimerViewModel();
                    break;
                case "AMRAP-Timer":
                    TimerShellViewModelBase.CurrentTimerViewModel = new AMRAPTimerViewModel();
                    break;
                case "Tabata-Timer":
                    TimerShellViewModelBase.CurrentTimerViewModel = new TabataTimerViewModel();
                    break;
                case "FGB-Timer":
                    TimerShellViewModelBase.CurrentTimerViewModel = new FGBTimerViewModel();
                    break;
                case "EMOM-Timer":
                    TimerShellViewModelBase.CurrentTimerViewModel = new EMOMTimerViewModel();
                    break;
                case "Preset-Timers":
                    TimerShellViewModelBase.CurrentTimerViewModel = new PresetTimersViewModel();
                    break;
            }

            this.Frame.Navigate(pageTo, itemId);
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
        }

        #endregion

        private void SettingAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;
            var settingsFlyout = new WODTimersSettingsFlyout();
            settingsFlyout.ShowIndependent();
        }

        private void UpgradeOrAboutAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;
            var aboutFlyout = new AboutFlyout();
            aboutFlyout.ShowIndependent();
        }


    }
}