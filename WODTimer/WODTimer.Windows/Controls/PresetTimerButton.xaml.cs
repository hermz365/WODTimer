using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HermzApp.WODTimer.Windows81.Controls
{
    public sealed partial class PresetTimerButton : UserControl
    {
        public PresetTimerButton()
        {
            this.InitializeComponent();
        }

        public event EventHandler PresetTimerButtonTapped;

        public string TimeText
        {
            get { return this.TimeTextBlock.Text; }
            set { this.TimeTextBlock.Text = value; }
        }

        private void PresetTimerButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            if (PresetTimerButtonTapped != null)
            {
                PresetTimerButtonTapped(this, EventArgs.Empty);
            }
        }
    }
}
