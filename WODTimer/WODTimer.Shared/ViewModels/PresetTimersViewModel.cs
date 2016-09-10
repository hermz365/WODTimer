using System;

using HermzApp.WODTimer.Shared.Common;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class PresetTimersViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "188149";
#endif
        #endregion

        #region Constructor
        public PresetTimersViewModel(): base()
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
            TitleText = "PRESET";
            IsStartButtonVisible = false;
            IsResetButtonVisible = false;
            IsPresetTimersGridVisible = true;
            IsMainTimeLabelVisible = false;
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
            StopTimerCommand = new RelayCommand(StopTimer, IsTimerRunning);
        }
        #endregion

        #region Timer Logics
        public override void Timer_Tick(object sender, object e)
        {
            MainTimeLabelTimeSpan = MainTimeLabelTimeSpan.Subtract(TimeDeltaTimeSpan);

            if (MainTimeLabelTimeSpan.Equals(EndTimeSpan))
            {
                if (!IsAllMuted) SoundsData.PlaySound(SoundsData.ENDWOD);
                StopTimer();
            }
        }

        #region ICommend Execute and CanExecute Methods
        public void StartTimer(int sec)
        {
            if (sec <= 0) return;

            App.StayWake();
            MainTimeLabelTimeSpan = new TimeSpan(0, 0, sec);
            IsTimerInProgress = true;
            IsMainTimeLabelVisible = true;
            IsPresetTimersGridVisible = false;
            Timer.Start();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        protected override void StopTimer()
        {
            Timer.Stop();
            App.CanSleepNow();
            IsTimerInProgress = false;
            IsPresetTimersGridVisible = true;
            IsMainTimeLabelVisible = false;
            StopTimerCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion

        #region Event Handlers
        #endregion
    }
}
