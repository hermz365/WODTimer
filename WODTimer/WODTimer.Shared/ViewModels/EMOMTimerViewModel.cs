using System;
using System.ComponentModel;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class EMOMTimerViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "183891";
#endif
        private double _emomMin;
        #endregion

        #region Constructor
        public EMOMTimerViewModel()
            : base()
        {
            this.InitalizeControlsState();
            this.InitalizeTimer();
            this.InitalizeAdControlInfo();
            this.InitalizeICommands();
            this.ResetTimer();
        }
        #endregion

        #region Properties
        public double EMOMMin
        {
            get { return _emomMin; }
            set
            {
                _emomMin = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Initalization Functions
        private void InitalizeControlsState()
        {
            TitleText = "EMOM";
            EMOMMin = App.WODSettingsValues.EMOMMinute;

            IsEMOMTopSecControlVisible = true;
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
        }
        #endregion

        #region Timer Logics
        public override void Timer_Tick(object sender, object e)
        {
            MainTimeLabelTimeSpan = MainTimeLabelTimeSpan.Add(TimeDeltaTimeSpan);
            int currSec = MainTimeLabelTimeSpan.Seconds;

            if (currSec == 57 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND3);

            if (currSec == 58 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND2);

            if (currSec == 59 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND1);

            if (MainTimeLabelTimeSpan.Equals(EndTimeSpan))
            {
                if (!IsAllMuted) SoundsData.PlaySound(SoundsData.ENDWODLONG);
                StopTimer();
                IsWODEndedItself = true;
            }
            else if (currSec == 60)
            {
                if (!IsAllMuted) SoundsData.PlaySound(SoundsData.ENDWOD);
            }
        }

        #region ICommend Execute and CanExecute Methods
        protected override void StartTimer()
        {
            EndTimeSpan = new TimeSpan(0, (int)EMOMMin, 0);
            base.StartTimer();
        }

        protected override void ResetTimer()
        {
            base.ResetTimer();
            MainTimeLabelTimeSpan = new TimeSpan(0 ,0, 0);
        }
        #endregion
        #endregion

        #region Event Handlers
        protected override void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WODSettingsValues_PropertyChanged(sender, e);

            if (e.PropertyName.Equals("EMOMMinute"))
            {
                EMOMMin = App.WODSettingsValues.EMOMMinute;
            }
        }
        #endregion
    }
}
