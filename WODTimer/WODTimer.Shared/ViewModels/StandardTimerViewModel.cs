using System;
using System.ComponentModel;
using HermzApp.WODTimer.Shared.Common;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class StandardTimerViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "175532";
#endif
        #endregion

        #region Constructor
        public StandardTimerViewModel() : base()
        {
            
            this.InitalizeControlsState();
            this.InitalizeTimer();
            this.InitalizeAdControlInfo();
            this.InitalizeICommands();
        }
        #endregion

        #region Initalization Functions
        private void InitalizeControlsState()
        {
            TitleText = "STANDARD";
        }

        private void InitalizeAdControlInfo()
        {
#if !DEV
            ApplicationId = PubCenterApplicationId;
            AdUnitId = PubCenteraAdUnitId;
#endif
        }

        public override void InitalizeTimer()
        {
            base.InitalizeTimer();
            MainTimeLabelTimeSpan = new TimeSpan(0, 0, 0);
        }

        protected override void InitalizeICommands()
        {
            base.InitalizeICommands();
            ResetTimerCommand = new RelayCommand(ResetTimer); 
        }
        #endregion

        #region Timer Logics
        public override void Timer_Tick(object sender, object e)
        {
            MainTimeLabelTimeSpan = MainTimeLabelTimeSpan.Add(TimeDeltaTimeSpan);
        }

        #region ICommend Execute and CanExecute Methods
        protected override void StartTimer()
        {
            App.StayWake();
            IsTimerInProgress = true;
            Timer.Start();
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        protected override void StopTimer()
        {
            Timer.Stop();
            App.CanSleepNow();
            IsTimerInProgress = false;
            StopTimerCommand.RaiseCanExecuteChanged();
            StartTimerCommand.RaiseCanExecuteChanged();
        }

        protected override void ResetTimer()
        {
            MainTimeLabelTimeSpan = TimeSpan.Zero;
        }
        #endregion
        #endregion

        #region Event Handlers
        protected override void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WODSettingsValues_PropertyChanged(sender, e);
        }
        #endregion
    }
}
