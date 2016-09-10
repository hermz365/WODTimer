using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;
using Windows.Storage;
using Windows.ApplicationModel;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A control that displays a countdown visualization
    /// and raises a CountdownComplete event when countdown is complete.
    /// Countdown duration is specified in Seconds.
    /// </summary>
    public sealed partial class CountdownControl
    {
        private bool _countingDown;
        /// <summary>
        /// Occurs when the countdown is complete.
        /// </summary>
        public event RoutedEventHandler CountdownComplete;

        #region Seconds
        /// <summary>
        /// The seconds property.
        /// </summary>
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register(
                "Seconds",
                typeof(int),
                typeof(CountdownControl),
                new PropertyMetadata(
                    0,
                    OnSecondsChanged));

        /// <summary>
        /// Gets or sets the seconds to countdown.
        /// </summary>
        /// <value>
        /// The seconds.
        /// </value>
        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        private static void OnSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CountdownControl)sender;
            var oldSeconds = (int)e.OldValue;
            var newSeconds = (int)e.NewValue;
            target.OnSecondsChanged(oldSeconds, newSeconds);
        }

        private void OnSecondsChanged(int oldSeconds, int newSeconds)
        {
            if (!_countingDown && newSeconds > 0)
            {
#pragma warning disable 4014
                StartCountdownAsync(newSeconds);
#pragma warning restore 4014
            }
        }
        #endregion

        
        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownControl" /> class.
        /// </summary>
        public CountdownControl()
        {
            InitializeComponent();
            InitiSounds();
        }

        #region Properties for scaling
        /// <summary>
        /// The height of the main canvas
        /// </summary>
        public double MainCanvasHeight
        {
            get { return this.MainCanvas.Height; }
            set { this.MainCanvas.Height = value; }
        }

        /// <summary>
        /// The width of the main canvas
        /// </summary>
        public double MainCanvasWidth
        {
            get { return this.MainCanvas.Width; }
            set { this.MainCanvas.Width = value; }
        }

        /// <summary>
        /// InnerRadius
        /// </summary>
        public double InnerRadius
        {
            get { return this.PART_RingSlice.InnerRadius; }
            set { this.PART_RingSlice.InnerRadius = value; }
        }

        /// <summary>
        /// Radius
        /// </summary>
        public double Radius
        {
            get { return this.PART_RingSlice.Radius; }
            set { this.PART_RingSlice.Radius = value; }
        }

        /// <summary>
        /// Main Canvas.Left 
        /// </summary>
        public int MainCanvasLeft
        {
            set { Canvas.SetLeft(this.PART_RingSlice, value); }
        }

        /// <summary>
        /// Main Canvas.Top 
        /// </summary>
        public int MainCanvasTop
        {
            set { Canvas.SetTop(this.PART_RingSlice, value); }
        }

        /// <summary>
        /// Number front size
        /// </summary>
        public double NumFrontSize
        {
            get { return this.PART_Label.FontSize; }
            set { this.PART_Label.FontSize = value; }
        }

        #endregion


        private async void InitiSounds()
        {
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync("Sounds");

            StorageFile file1 = await folder.GetFileAsync("1.wav");
            var stream1 = await file1.OpenAsync(FileAccessMode.Read);
            sound1.SetSource(stream1, file1.ContentType);
            
            StorageFile file2 = await folder.GetFileAsync("2.wav");
            var stream2 = await file2.OpenAsync(FileAccessMode.Read);
            sound2.SetSource(stream2, file2.ContentType);

            StorageFile file3 = await folder.GetFileAsync("3.wav");
            var stream3 = await file3.OpenAsync(FileAccessMode.Read);
            sound3.SetSource(stream3, file3.ContentType);

            StorageFile startWODFile = await folder.GetFileAsync("startWOD.wav");
            var startWODStream = await startWODFile.OpenAsync(FileAccessMode.Read);
            startSound.SetSource(startWODStream, startWODFile.ContentType);
        }

        /// <summary>
        /// Starts the countdown and completes when the countdown completes.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public async Task StartCountdownAsync(int seconds, bool isAllMuted = false)
        {
            _countingDown = true;

            this.Seconds = seconds;

            bool grow = true;

            while (this.Seconds > 0)
            {
                var sb = new Storyboard();

                if (grow)
                {
                    ////Debug.WriteLine(DateTime.Now.ToString("mm:ss.ffff") + "grow");

                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);
                    sb.RepeatBehavior = new RepeatBehavior(1);

                    Storyboard.SetTargetProperty(da, "EndAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }
                else
                {
                    ////Debug.WriteLine(DateTime.Now.ToString("mm:ss.ffff") + "shrink");

                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);
                    sb.RepeatBehavior = new RepeatBehavior(1);

                    Storyboard.SetTargetProperty(da, "StartAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }

                PART_Label.Text = this.Seconds.ToString();
                
                await sb.BeginAsync();

                if (grow)
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 359.999d;
                }
                else
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 0d;
                }

                grow = !grow;
                this.Seconds--;

                if(this.Seconds == 3 && !isAllMuted)
                {
                    sound3.Volume = 1;
                    sound3.Play();
                    sound3.Position = new TimeSpan(0, 0, 0);
                }

                if (this.Seconds == 2 && !isAllMuted)
                {
                    sound2.Volume = 1;
                    sound2.Play();
                    sound2.Position = new TimeSpan(0, 0, 0);
                }

                if (this.Seconds == 1 && !isAllMuted)
                {
                    sound1.Volume = 1;
                    sound1.Play();
                    sound1.Position = new TimeSpan(0, 0, 0);
                }

                if (this.Seconds == 0 && !isAllMuted)
                {
                    startSound.Volume = 1;
                    startSound.Play();
                    startSound.Position = new TimeSpan(0, 0, 0);
                }
            }

            PART_Label.Text = this.Seconds.ToString();

            if (this.CountdownComplete != null)
            {
                CountdownComplete(this, new RoutedEventArgs());
            }

            _countingDown = false;
        }
    }
}
