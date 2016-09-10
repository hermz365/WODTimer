using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using HermzApp.WODTimer.Shared;
using Windows.UI.Popups;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace HermzApp.WODTimer.Windows81
{
    public sealed partial class WODTimersSettingsFlyout : SettingsFlyout
    {
        public WODTimersSettingsFlyout()
        {
            this.InitializeComponent();
            this.ShowRepsCounterToggleSwitch.IsOn = App.WODSettingsValues.IsShowRepCounter;
            this.DarkThemeToggleSwitch.IsOn = App.WODSettingsValues.IsDarkTheme;
            this.SecToStartUpDown.Value = App.WODSettingsValues.SecondsToCountDown;
            this.AMRAPMinToStartUpDown.Value = App.WODSettingsValues.AMRAPMinute;
            this.AMRAPSecToStartUpDown.Value = App.WODSettingsValues.AMRAPSecond;
            this.EMOMMinToStartUpDown.Value = App.WODSettingsValues.EMOMMinute;
            this.TabataSetUpDown.Value = App.WODSettingsValues.TabataSet;
            this.FGBStyleRdUpDown.Value = App.WODSettingsValues.FGBRound;
            this.SoundsToggleSwitch.IsOn = App.WODSettingsValues.IsMuteAllTimerSounds;
        }

        private void ResetDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowRepsCounterToggleSwitch.IsOn = App.WODSettingsValues.IsShowRepCounter = App.WODSettingsValues.IsShowRepCounterDefault;
            this.DarkThemeToggleSwitch.IsOn = App.WODSettingsValues.IsDarkTheme = App.WODSettingsValues.IsDarkThemeDefault;
            this.SecToStartUpDown.Value = App.WODSettingsValues.SecondsToCountDown = App.WODSettingsValues.SecondsToCountDownDefault;
            this.AMRAPMinToStartUpDown.Value = App.WODSettingsValues.AMRAPMinute = App.WODSettingsValues.AMRAPMinuteDefault;
            this.AMRAPSecToStartUpDown.Value = App.WODSettingsValues.AMRAPSecond = App.WODSettingsValues.AMRAPSecondDefault;
            this.EMOMMinToStartUpDown.Value = App.WODSettingsValues.EMOMMinute = App.WODSettingsValues.EMOMMinuteDefault;
            this.TabataSetUpDown.Value = App.WODSettingsValues.TabataSet = App.WODSettingsValues.TabataSetDefault;
            this.FGBStyleRdUpDown.Value = App.WODSettingsValues.FGBRound = App.WODSettingsValues.FGBRoundDefault;
            this.SoundsToggleSwitch.IsOn = App.WODSettingsValues.IsMuteAllTimerSounds = App.WODSettingsValues.IsMuteAllTimerSoundsDefault;
        }

        private void ShowRepsCounterToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            App.WODSettingsValues.IsShowRepCounter = this.ShowRepsCounterToggleSwitch.IsOn;
        }

        private void DarkThemeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var oldValue = App.WODSettingsValues.IsDarkTheme;
            App.WODSettingsValues.IsDarkTheme = this.DarkThemeToggleSwitch.IsOn;
            if (oldValue != this.DarkThemeToggleSwitch.IsOn)
            {
                var dialog = new MessageDialog("New theme will take effect after restarting WOD Timer application.");
                dialog.ShowAsync();
            }
        }

        private void SecToStartUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.SecondsToCountDown = (int)this.SecToStartUpDown.Value;
        }        

        private void AMRAPMinToStartUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.AMRAPMinute = (int)this.AMRAPMinToStartUpDown.Value;
        }

        private void AMRAPSecToStartUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.AMRAPSecond = (int)this.AMRAPSecToStartUpDown.Value;
        }

        private void EMOMMinToStartUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.EMOMMinute = (int)this.EMOMMinToStartUpDown.Value; 
        }

        private void TabataSetUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.TabataSet = (int)this.TabataSetUpDown.Value;
        }

        private void FGBStyleRdUpDown_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            App.WODSettingsValues.FGBRound = (int)this.FGBStyleRdUpDown.Value;
        }

        private void SoundsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            App.WODSettingsValues.IsMuteAllTimerSounds = this.SoundsToggleSwitch.IsOn;
        }
    }
}
