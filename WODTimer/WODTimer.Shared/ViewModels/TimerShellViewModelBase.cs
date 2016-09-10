using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using HermzApp.WODTimer.Shared.Common;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public abstract class TimerShellViewModelBase : BaseViewModel
    {
        #region Private variables
        #region Common UI controls states private variables
        private string _titleText;
        private bool _isRepCounterVisible;
        private bool _isStartButtonVisible = true;
        private bool _isStopButtonVisible = true;
        private bool _isResetButtonVisible = true;
        private bool _isRepsCounterToggleVisible = true;
        private bool _isMainTimeLabelVisible = true;

        private bool _isAMRAPTopSecControlVisible = false;
        private bool _isEMOMTopSecControlVisible = false;
        private bool _isTabataSecControlVisible = false;
        private bool _isFGBSecControlVisible = false;
        private bool _isPresetTimersGridVisible = false;
        #endregion

        private bool _isAllMuted;
        private int _secondsToCountDown;
        private int _repsCount;

        #region Command private variables
        private RelayCommand _startTimerCommand;
        private RelayCommand _stopTimerCommand;
        private RelayCommand _resetTimerCommand;
        #endregion

        #region Timer related private variables
        private TimeSpan _mainTimeLabelTimeSpan;
        private bool _isTimerInProgress;
        #endregion

        #region Protected variables 
        protected DispatcherTimer Timer;
        protected TimeSpan EndTimeSpan;
        protected TimeSpan TimeDeltaTimeSpan;

        protected bool IsResetButtonEnabled;
        protected bool IsWODEndedItself;
        #endregion

        #region PubCenter Ads private variables
        // These two are Test Mode PubCenter AdContorl values. The ViewModel child will overwrite them in production build
        private string _applicationId = "d25517cb-12d4-4699-8bdc-52040c712cab";
        private string _adUnitId = "10042998";
        #endregion
        #endregion

        #region Static Member
        public static TimerShellViewModelBase CurrentTimerViewModel;
        #endregion

        #region Constructor

        protected TimerShellViewModelBase()
        {
            App.WODSettingsValues.PropertyChanged += WODSettingsValues_PropertyChanged;
            _isRepCounterVisible = App.WODSettingsValues.IsShowRepCounter;
            _isAllMuted = App.WODSettingsValues.IsMuteAllTimerSounds;
            _secondsToCountDown = App.WODSettingsValues.SecondsToCountDown;
            TimeDeltaTimeSpan = new TimeSpan(0, 0, 1);
        }
        #endregion

        #region Abstract and Virtual Methods

        protected virtual void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsShowRepCounter"))
            {
                IsRepCounterVisible = App.WODSettingsValues.IsShowRepCounter;
            }

            if (e.PropertyName.Equals("IsMuteAllTimerSounds"))
            {
                IsAllMuted = App.WODSettingsValues.IsMuteAllTimerSounds;
            }

            if (e.PropertyName.Equals("SecondsToCountDown"))
            {
                SecondsToCountDown = App.WODSettingsValues.SecondsToCountDown;
            }
        }

        #region Start, Stop, Reset Commands
        protected virtual void InitalizeICommands()
        {
            StartTimerCommand = new RelayCommand(StartTimer, IsTimerStopped);
            StopTimerCommand = new RelayCommand(StopTimer, IsTimerRunning);
            ResetTimerCommand = new RelayCommand(ResetTimer, IsResetEnabled);
        }

        protected virtual void StartTimer()
        {
            if (!Timer.IsEnabled)
            {
                if (IsWODEndedItself)
                    ResetTimer();
                App.StayWake();
                IsTimerInProgress = true;
                IsResetButtonEnabled = false;
                Timer.Start();
            }
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
            ResetTimerCommand.RaiseCanExecuteChanged();
        }

        protected virtual void StopTimer()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
                IsResetButtonEnabled = true;
                IsTimerInProgress = false;
                App.CanSleepNow();
            }
            StopTimerCommand.RaiseCanExecuteChanged();
            StartTimerCommand.RaiseCanExecuteChanged();
            ResetTimerCommand.RaiseCanExecuteChanged();
        }

        protected virtual void ResetTimer()
        {
            IsResetButtonEnabled = true;
            IsWODEndedItself = false;
        }

        protected virtual bool IsTimerStopped()
        {
            return !IsTimerInProgress;
        }

        protected virtual bool IsTimerRunning()
        {
            return IsTimerInProgress;
        }

        protected virtual bool IsResetEnabled()
        {
            return IsResetButtonEnabled;
        }
        #endregion

        #region Timer
        public virtual void InitalizeTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeDeltaTimeSpan;
        }

        public abstract void Timer_Tick(object sender, object e);
        #endregion
        #endregion

        #region Properties
        public string TitleText
        {
            get { return  _titleText; }
            set
            {
                _titleText = value;
                OnPropertyChanged();
            }
        }

        public bool IsRepCounterVisible
        {
            get { return _isRepCounterVisible; }
            set
            {
                _isRepCounterVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStartButtonVisible
        {
            get { return _isStartButtonVisible; }
            set
            {
                _isStartButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStopButtonVisible
        {
            get { return _isStopButtonVisible; }
            set
            {
                _isStopButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsResetButtonVisible
        {
            get { return _isResetButtonVisible; }
            set
            {
                _isResetButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsRepsCounterToggleVisible
        {
            get { return _isRepsCounterToggleVisible; }
            set
            {
                _isRepsCounterToggleVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsMainTimeLabelVisible
        {
            get { return _isMainTimeLabelVisible; }
            set
            {
                _isMainTimeLabelVisible = value;
                OnPropertyChanged();
            }
        }

        public int RepsCount
        {
            get { return _repsCount; }
            set
            {
                if (!((ProductLicenseDataSource) Application.Current.Resources["License"]).IsLicensed) return;
                _repsCount = value;
                OnPropertyChanged();
            }
        }

        public bool IsTimerInProgress
        {
            get { return _isTimerInProgress; }
            set
            {
                _isTimerInProgress = value;
                OnPropertyChanged("IsTimerNotInProgress");
                OnPropertyChanged();
            }
        }

        public bool IsTimerNotInProgress
        {
            get { return !_isTimerInProgress; }
        }
        
        public TimeSpan MainTimeLabelTimeSpan
        {
            get { return _mainTimeLabelTimeSpan; }
            set
            {
                _mainTimeLabelTimeSpan = value;
                OnPropertyChanged();
            }
        }

        public string ApplicationId
        {
            get { return _applicationId; }
            set
            {
                _applicationId = value;
                OnPropertyChanged();
            }
        }

        public string AdUnitId
        {
            get { return _adUnitId; }
            set
            {
                _adUnitId = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand StartTimerCommand
        {
            get { return _startTimerCommand; }
            set
            {
                _startTimerCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand StopTimerCommand
        {
            get { return _stopTimerCommand; }
            set
            {
                _stopTimerCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ResetTimerCommand
        {
            get { return _resetTimerCommand; }
            set
            {
                _resetTimerCommand = value;
                OnPropertyChanged();
            }
        }

        public bool IsAllMuted
        {
            get { return _isAllMuted; }
            set
            {
                _isAllMuted = value;
                OnPropertyChanged();
            }
        }

        public int SecondsToCountDown
        {
            get { return _secondsToCountDown; }
            set
            {
                _secondsToCountDown = value;
                OnPropertyChanged();
            }
        }

        public bool IsAMRAPTopSecControlVisible
        {
            get { return _isAMRAPTopSecControlVisible; }
            set
            {
                _isAMRAPTopSecControlVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEMOMTopSecControlVisible
        {
            get { return _isEMOMTopSecControlVisible; }
            set
            {
                _isEMOMTopSecControlVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabataSecControlVisible
        {
            get { return _isTabataSecControlVisible; }
            set
            {
                _isTabataSecControlVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsFGBSecControlVisible
        {
            get { return _isFGBSecControlVisible; }
            set
            {
                _isFGBSecControlVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsPresetTimersGridVisible
        {
            get { return _isPresetTimersGridVisible; }
            set
            {
                _isPresetTimersGridVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion;
    }
}
