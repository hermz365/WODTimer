using System;
using System.ComponentModel;
using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class FGBTimerViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "183893";
#endif
        private int _fgbRounds;
        private int _roundsFinished;
        private int _roundsLeft;
        private int _currInterval;
        #endregion

        #region Constructor
        public FGBTimerViewModel()
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
        public int FGBRounds
        {
            get { return _fgbRounds; }
            set
            {
                _fgbRounds = value;
                RoundsLeft = _fgbRounds;
                OnPropertyChanged();
            }
        }

        public int RoundsFinished
        {
            get { return _roundsFinished; }
            set
            {
                _roundsFinished = value;
                OnPropertyChanged();
            }
        }

        public int RoundsLeft
        {
            get { return _roundsLeft; }
            set
            {
                _roundsLeft = value;
                OnPropertyChanged();
            }
        }

        public int CurrInterval
        {
            get { return _currInterval; }
            set
            {
                _currInterval = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Initalization Functions
        private void InitalizeControlsState()
        {
            TitleText = "FGB-Style";
            IsFGBSecControlVisible = true;
            FGBRounds = App.WODSettingsValues.FGBRound;
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

            if (MainTimeLabelTimeSpan.Seconds == 0)
            {
                CurrInterval++;
                if (CurrInterval == 7)
                {
                    CurrInterval = 1;
                    RoundsFinished++;
                    RoundsLeft--;
                }

                if (!IsAllMuted && RoundsLeft != 0)
                {
                    SoundsData.PlaySound(SoundsData.ENDWOD);
                }
            }

            if (RoundsLeft == 0)
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
            MainTimeLabelTimeSpan = new TimeSpan(0, 0, 0);
            CurrInterval = 1;
            RoundsFinished = 0;
            RoundsLeft = FGBRounds;
            StopTimer();
        }
        #endregion
        #endregion

        #region Event Handlers
        protected override void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WODSettingsValues_PropertyChanged(sender, e);

            if (e.PropertyName.Equals("FGBRound"))
            {
                FGBRounds = RoundsLeft = App.WODSettingsValues.FGBRound;
            }
        }
        #endregion
    }
}
