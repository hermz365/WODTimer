using System;
using System.ComponentModel;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class AMRAPTimerViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "183890";
#endif
        private double _amrapMin;
        private double _amrapSec;
        #endregion

        #region Constructor
        public AMRAPTimerViewModel()
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
        public double AMRAPMin
        {
            get { return _amrapMin; }
            set
            {
                _amrapMin = value;
                MainTimeLabelTimeSpan = new TimeSpan(0, (int)_amrapMin, (int)AMRAPSec);
                OnPropertyChanged();
            }
        }

        public double AMRAPSec
        {
            get { return _amrapSec; }
            set
            {
                _amrapSec = value;
                MainTimeLabelTimeSpan = new TimeSpan(0, (int)AMRAPMin, (int)_amrapSec);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Initalization Functions
        private void InitalizeControlsState()
        {
            TitleText = "AMRAP";
            AMRAPMin = App.WODSettingsValues.AMRAPMinute;
            AMRAPSec = App.WODSettingsValues.AMRAPSecond;

            IsAMRAPTopSecControlVisible = true;
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
            MainTimeLabelTimeSpan = new TimeSpan(0, (int)AMRAPMin, (int)AMRAPSec); // get from setting
            EndTimeSpan = new TimeSpan(0, 0, 0);
        }
        #endregion

        #region Timer Logics
        public override void Timer_Tick(object sender, object e)
        {
            MainTimeLabelTimeSpan = MainTimeLabelTimeSpan.Subtract(TimeDeltaTimeSpan);
            int currSec = MainTimeLabelTimeSpan.Seconds;

            if (currSec == 3 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND3);

            if (currSec == 2 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND2);

            if (currSec == 1 && !IsAllMuted)
                SoundsData.PlaySound(SoundsData.SOUND1);

            if (MainTimeLabelTimeSpan.Equals(EndTimeSpan))
            {
                if (!IsAllMuted) SoundsData.PlaySound(SoundsData.ENDWODLONG);
                StopTimer();
                IsWODEndedItself = true;
            }
        }

        #region ICommend Execute and CanExecute Methods
        protected override void ResetTimer()
        {
            base.ResetTimer();
            MainTimeLabelTimeSpan = new TimeSpan(0 , (int)AMRAPMin, (int)AMRAPSec);
        }
        #endregion
        #endregion

        #region Event Handlers
        protected override void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WODSettingsValues_PropertyChanged(sender, e);

            if (e.PropertyName.Equals("AMRAPMinute"))
            {
                AMRAPMin = App.WODSettingsValues.AMRAPMinute;
            }
            
            if (e.PropertyName.Equals("AMRAPSecond"))
            {
               AMRAPSec = App.WODSettingsValues.AMRAPSecond;
            }
        }
        #endregion
    }
}
