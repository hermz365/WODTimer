using System.Collections.Generic;
using System.ComponentModel;

// Using statement for access the user data for settings
using Windows.Storage;

namespace HermzApp.WODTimer.Shared.DataModel
{
    public class WODSettingsValues : INotifyPropertyChanged
    {
        private static ApplicationDataContainer _roamingSettings = null;

        #region All Default Settings Values
        public readonly bool IsShowRepCounterDefault = true;
        public readonly bool IsDarkThemeDefault = false;
        public readonly int SecondsToCountDownDefault = 5;
        public readonly bool IsMuteAllTimerSoundsDefault = false;
        public readonly int AMRAPMinuteDefault = 15;
        public readonly int AMRAPSecondDefault = 0;
        public readonly int EMOMMinuteDefault = 10;
        public readonly int TabataSetDefault = 8;
        public readonly int FGBRoundDefault = 3;
        #endregion

        public WODSettingsValues()
        {
            _roamingSettings = ApplicationData.Current.RoamingSettings;
            this.InitAllSettingsValues();
        }

        private void InitAllSettingsValues()
        {
            #region Boolean - Show Rep Counter (Across all Timers)
            if (_roamingSettings.Values.ContainsKey("ShowRepCounter"))
            {
                IsShowRepCounter = (bool)_roamingSettings.Values["ShowRepCounter"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["ShowRepCounter"] = IsShowRepCounterDefault;
                _isShowRepCounter = IsShowRepCounterDefault;
            }
            #endregion

            #region Boolean - Dark Theme
            if (_roamingSettings.Values.ContainsKey("IsDarkTheme"))
            {
                IsDarkTheme = (bool)_roamingSettings.Values["IsDarkTheme"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["IsDarkTheme"] = IsDarkThemeDefault;
                _isDarkTheme = IsDarkThemeDefault;
            }
            #endregion

            #region Int - Seconds to Count Down (Across all Timers)
            if (_roamingSettings.Values.ContainsKey("SecondsToCountDown"))
            {
                SecondsToCountDown = (int)_roamingSettings.Values["SecondsToCountDown"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["SecondsToCountDown"] = SecondsToCountDownDefault;
                _secondsToCountDown = SecondsToCountDownDefault;
            }
            #endregion

            #region Mute All Timer Sounds (Across all Timers)
            if (_roamingSettings.Values.ContainsKey("IsMuteAllTimerSounds"))
            {
                IsMuteAllTimerSounds = (bool)_roamingSettings.Values["IsMuteAllTimerSounds"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["IsMuteAllTimerSounds"] = IsMuteAllTimerSoundsDefault;
                _IsMuteAllTimerSounds = IsMuteAllTimerSoundsDefault;
            }
            #endregion

            #region Int - AMRAP Minute
            if (_roamingSettings.Values.ContainsKey("AMRAPMinute"))
            {
                AMRAPMinute = (int)_roamingSettings.Values["AMRAPMinute"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["AMRAPMinute"] = AMRAPMinuteDefault;
                _AMRAPMinute = AMRAPMinuteDefault;
            }
            #endregion

            #region Int - AMRAP Second
            if (_roamingSettings.Values.ContainsKey("AMRAPSecond"))
            {
                AMRAPSecond = (int)_roamingSettings.Values["AMRAPSecond"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["AMRAPSecond"] = AMRAPSecondDefault;
                _AMRAPSecond = AMRAPSecondDefault;
            }
            #endregion

            #region Int - EMOM Minute
            if (_roamingSettings.Values.ContainsKey("EMOMMinute"))
            {
                EMOMMinute = (int)_roamingSettings.Values["EMOMMinute"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["EMOMMinute"] = EMOMMinuteDefault;
                _EMOMMinute = EMOMMinuteDefault;
            }
            #endregion

            #region Int - Tabata Set
            if (_roamingSettings.Values.ContainsKey("TabataSet"))
            {
                TabataSet = (int)_roamingSettings.Values["TabataSet"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["TabataSet"] = TabataSetDefault;
                _TabataSet = TabataSetDefault;
            }
            #endregion

            #region Int - FGB Round
            if (_roamingSettings.Values.ContainsKey("FGBRound"))
            {
                FGBRound = (int)_roamingSettings.Values["FGBRound"];
            }
            else
            { // Very first time boot
                _roamingSettings.Values["FGBRound"] = FGBRoundDefault;
                _FGBRound = FGBRoundDefault;
            }
            #endregion
        }

        #region Boolean - Show Rep Counter (Across all Timers)
        private bool _isShowRepCounter;
        public bool IsShowRepCounter
        {
            get { return _isShowRepCounter; }
            set { SetField(ref _isShowRepCounter, value, "IsShowRepCounter", "ShowRepCounter"); }
        }
        #endregion

        #region Boolean - Dark Theme
        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set { SetField(ref _isDarkTheme, value, "IsDarkTheme", "IsDarkTheme"); }
        }
        #endregion

        #region Int - Seconds to Count Down (Across all Timers)
        private int _secondsToCountDown;
        public int SecondsToCountDown
        {
            get { return _secondsToCountDown; }
            set { SetField(ref _secondsToCountDown, value, "SecondsToCountDown", "SecondsToCountDown"); }
        }
        #endregion

        #region Boolean - Mute All Timer Sounds (Across all Timers)
        private bool _IsMuteAllTimerSounds;
        public bool IsMuteAllTimerSounds
        {
            get { return _IsMuteAllTimerSounds; }
            set { SetField(ref _IsMuteAllTimerSounds, value, "IsMuteAllTimerSounds", "IsMuteAllTimerSounds"); }
        }
        #endregion

        #region Int - AMRAP Minute 
        private int _AMRAPMinute;
        public int AMRAPMinute
        {
            get { return _AMRAPMinute; }
            set { SetField(ref _AMRAPMinute, value, "AMRAPMinute", "AMRAPMinute"); }
        }
        #endregion

        #region Int - AMRAP Second
        private int _AMRAPSecond;
        public int AMRAPSecond
        {
            get { return _AMRAPSecond; }
            set { SetField(ref _AMRAPSecond, value, "AMRAPSecond", "AMRAPSecond"); }
        }
        #endregion

        #region Int - EMOM Minute
        private int _EMOMMinute;
        public int EMOMMinute
        {
            get { return _EMOMMinute; }
            set { SetField(ref _EMOMMinute, value, "EMOMMinute", "EMOMMinute"); }
        }
        #endregion

        #region Int - Tabata Set
        private int _TabataSet;
        public int TabataSet
        {
            get { return _TabataSet; }
            set { SetField(ref _TabataSet, value, "TabataSet", "TabataSet"); }
        }
        #endregion

        #region Int - FGB Round
        private int _FGBRound;
        public int FGBRound
        {
            get { return _FGBRound; }
            set { SetField(ref _FGBRound, value, "FGBRound", "FGBRound"); }
        }
        #endregion

        #region boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName, string winStoreName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            else
            {
                field = value;
                _roamingSettings.Values[winStoreName] = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
        #endregion
    }
}
