using System;
using System.ComponentModel;

using HermzApp.WODTimer.Shared.DataModel;

namespace HermzApp.WODTimer.Shared.ViewModels
{
    public sealed class TabataTimerViewModel : TimerShellViewModelBase
    {
        #region Private variables
#if !DEV
        private const string PubCenterApplicationId = "41fd3111-37ac-443b-a127-d9fd6bd29da1";
        private const string PubCenteraAdUnitId = "183892";
#endif
        private int _tabataSet;
        private int _setFinished;
        private int _setLeft;
        private int _workRest;
        private int _work;
        private int _rest;
        private bool _isWorking = true;
        #endregion

        #region Constructor
        public TabataTimerViewModel()
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
        public int TabataSet
        {
            get { return _tabataSet; }
            set
            {
                _tabataSet = value;
                SetLeft = value;
                OnPropertyChanged();
            }
        }

        public int SetFinished
        {
            get { return _setFinished; }
            set
            {
                _setFinished = value;
                OnPropertyChanged();
            }
        }

        public int SetLeft
        {
            get { return _setLeft; }
            set
            {
                _setLeft = value;
                OnPropertyChanged();
            }
        }

        public int WorkRest
        {
            get { return _workRest; }
            set
            {
                _workRest = value;
                OnPropertyChanged();
            }
        }

        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Initalization Functions
        private void InitalizeControlsState()
        {
            TitleText = "TABATA";
            IsTabataSecControlVisible = true;
            TabataSet = App.WODSettingsValues.TabataSet;
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
            if (IsWorking)
            {
                _work--;
                if (_work == 3 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND3);

                if (_work == 2 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND2);

                if (_work == 1 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND1);

                if (_work == 0)
                {
                    if (!IsAllMuted) SoundsData.PlaySound(SoundsData.ENDWOD);
                    IsWorking = false;
                    WorkRest = _rest;
                    _work = 20;
                }
                else
                {
                    WorkRest = _work;
                }
            }
            else
            {
                _rest--;
                if (_rest == 3 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND3);

                if (_rest == 2 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND2);

                if (_rest == 1 && !IsAllMuted)
                    SoundsData.PlaySound(SoundsData.SOUND1);

                if (_rest == 0)
                {
                    if (!IsAllMuted && SetLeft != 1)
                    {
                        SoundsData.PlaySound(SoundsData.ENDWOD);
                    }
                    IsWorking = true;
                    WorkRest = _work;
                    _rest = 10;
                    SetFinished++;
                    SetLeft--;
                }
                else
                {
                    WorkRest = _rest;
                }
            }

            MainTimeLabelTimeSpan = MainTimeLabelTimeSpan.Add(TimeDeltaTimeSpan);

            if (SetLeft == 0)
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
            IsWorking = true;
            SetFinished = 0;
            SetLeft = TabataSet;
            _work = 20;
            _rest = 10;
            WorkRest = 20;
            StopTimer();
        }
        #endregion
        #endregion

        #region Event Handlers
        protected override void WODSettingsValues_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.WODSettingsValues_PropertyChanged(sender, e);

            if (e.PropertyName.Equals("TabataSet"))
            {
                TabataSet = App.WODSettingsValues.TabataSet;
            }
        }
        #endregion
    }
}
