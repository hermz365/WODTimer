/* 
 * Filename: MainPage.xaml.cs
 * Author: Hang Cheong Wong
 * Date:  May 2014
 * Company: BNTH LLC Copyright 2014
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using HermzApp.WODTimer.Shared;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace HermzApp.WODTimer.WindowsPhone81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void PrivacyStatement_Click(object sender, RoutedEventArgs e)
        {
            ((App)(Application.Current)).OpenPrivacyStatementLink();
        }



        private void AboutTexts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(e != null) e.Handled = true;

            var textBlock = sender as TextBlock;
            
            if(textBlock == null) 
                return;

            NavigateToPage(textBlock.Text); 
        }

        private void AppBarButtonClicked(object sender, RoutedEventArgs e)
        {
            var appBarButton = sender as AppBarButton;
            if (appBarButton == null) return;

            NavigateToPage(appBarButton.Label);           
        }

        private void NavigateToPage(string text)
        {
            if (this.Frame != null)
            {
                Type page = null;
                switch (text)
                {
                    case "About":
                        page = typeof(AboutPage);
                        break;
                    case "Settings":
                        page = typeof(SettingsPage);
                        break;
                    case "Upgrade":
                        page = typeof(UpgradePage);
                        break;
                }
                this.Frame.Navigate(page);
            }
        }
    }
}
