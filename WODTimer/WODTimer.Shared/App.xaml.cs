/* 
 * Filename: App.xaml.cs
 * Author: Hang Cheong Wong
 * Date:  May 2014
 * Company: BNTH LLC Copyright 2014
*/

using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using HermzApp.WODTimer.Shared.DataModel;

#if WINDOWS_APP 
// Using statements for Charm bar settings stuffs (e.g. Privacy Policy)
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using HermzApp.WODTimer.Windows81;
using Windows.Storage;
using Windows.ApplicationModel.Store;
#endif

#if WINDOWS_PHONE_APP
using HermzApp.WODTimer.WindowsPhone81;
#endif



// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace HermzApp.WODTimer.Shared
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection _transitions;
#endif
        private static WODSettingsValues _settings;
        private static SoundsData _soundData;
        private static DisplayRequest _display;
        private CoreDispatcher _dispatcher = null;
        private static int _numOfDisplayReqCalls;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {            
            this.InitializeComponent();
            this.RequestedTheme = WODSettingsValues.IsDarkTheme ? ApplicationTheme.Dark : ApplicationTheme.Light;
            this.Suspending += this.OnSuspending;
#if WINDOWS_APP 
            #if DEV
            LoadInAppPurchaseProxyFileAsync();
            #endif
#endif
        }

        private async void InitializeSounds()
        {
            _soundData = new SoundsData();

            await((App)Application.Current).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                _soundData.InitiSounds();
            });
        }

        public CoreDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }
        
        public static WODSettingsValues WODSettingsValues
        {
            get { return _settings ?? (_settings = new WODSettingsValues()); }
        }

        public static SoundsData SoundsData
        {
            get { return _soundData ?? (_soundData = new SoundsData()); }
        }

        private static DisplayRequest Display
        {
            get { return _display ?? (_display = new DisplayRequest()); }
        }

        public static void StayWake()
        {
            Display.RequestActive();
            _numOfDisplayReqCalls++;
        }

        public static bool CanSleepNow()
        {
            if (_numOfDisplayReqCalls > 0)
            {// when there is more than one or one RequstActive call made.
                Display.RequestRelease();
                _numOfDisplayReqCalls--;
            }

            return _numOfDisplayReqCalls != 0;
        }


        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);

#if WINDOWS_APP
            // Listening for this event lets the app initialize the settings commands and pause its UI until the user closes the pane.
            // To ensure your settings are available at all times in your app, place your CommandsRequested handler in the overridden
            // OnWindowCreated of App.xaml.cs
           SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
#endif
            _dispatcher = args.Window.Dispatcher;
        }

        internal async void OpenPrivacyStatementLink()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string Privacy_Statement_link = loader.GetString("PrivacyStatementLink");

            var uri = new Uri(Privacy_Statement_link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

#if WINDOWS_APP
        void onSettingsCommand(IUICommand command)
        {
            var settingsCommand = (SettingsCommand)command;
            if (settingsCommand.Id.Equals("PrivacyPolicy"))
            {
                OpenPrivacyStatementLink();
            }
        }

        void OnCommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            #region Privacy Statement Stuffs
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string privacyStatement = loader.GetString("PrivacyStatement");

            var handler = new UICommandInvokedHandler(onSettingsCommand);
            var privacyCommand = new SettingsCommand("PrivacyPolicy", privacyStatement, handler);
            eventArgs.Request.ApplicationCommands.Add(privacyCommand);
            #endregion

            #region Settings Stuffs
            // Disclaimar Stuffs
            var settingsSetting = new SettingsCommand("settingsSetting", "Settings", (handle) =>
            {
                var settingsFlayout = new WODTimersSettingsFlyout();
                settingsFlayout.Show();

            });

            eventArgs.Request.ApplicationCommands.Add(settingsSetting);
            #endregion

            #region About Stuffs
            // Disclaimar Stuffs
            var aboutSetting = new SettingsCommand("aboutFlyout", "About", (handleAbout) =>
            {
                var aboutFlayout = new AboutFlyout();
                aboutFlayout.Show();

            });

            eventArgs.Request.ApplicationCommands.Add(aboutSetting);
            #endregion
        }

        #if DEV
        private async void LoadInAppPurchaseProxyFileAsync()
        {
            StorageFolder proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("data");
            StorageFile proxyFile = await proxyDataFolder.GetFileAsync("in-app-purchase.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
        }
        #endif
#endif

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this._transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this._transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Initialize Sounds data here. If call this too early, the app is not ready. It will throw exception.
            this.InitializeSounds();
            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this._transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            while (CanSleepNow()) ;

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}