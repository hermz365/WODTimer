using System;
using System.Collections.Generic;
using System.Text;

using Windows.Foundation;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HermzApp.WODTimer.Shared.DataModel
{
    public class SoundsData
    {
        private static Dictionary<string, MediaElement> _soundsCollection = new Dictionary<string,MediaElement>();

        public const string SOUND1 = "1.wav";
        public const string SOUND2 = "2.wav";
        public const string SOUND3 = "3.wav";
        public const string ENDWOD = "endWOD.wav";
        public const string ENDWODLONG = "endWODLong.wav";
        public const string STARTWOD = "startWOD.wav";

        public SoundsData()
        {
            _soundsCollection.Add(SOUND1, new MediaElement());
            _soundsCollection.Add(SOUND2, new MediaElement());
            _soundsCollection.Add(SOUND3, new MediaElement());
            _soundsCollection.Add(ENDWOD, new MediaElement());
            _soundsCollection.Add(ENDWODLONG, new MediaElement());
            _soundsCollection.Add(STARTWOD, new MediaElement());

            foreach(var pair in _soundsCollection)
            {
                pair.Value.AutoPlay = false;
                pair.Value.AreTransportControlsEnabled = false;
                pair.Value.IsHoldingEnabled = false;
                pair.Value.IsHitTestVisible = false;
                pair.Value.IsDoubleTapEnabled = false;
                pair.Value.IsRightTapEnabled = false;
                pair.Value.IsTapEnabled = false;
                pair.Value.AudioCategory = AudioCategory.ForegroundOnlyMedia;
            }
        }

        public async void InitiSounds()
        {
            StorageFile file;
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync("Sounds");

            foreach (var pair in _soundsCollection)
            {
                file = await folder.GetFileAsync(pair.Key);
                var stream = await file.OpenAsync(FileAccessMode.Read);
                pair.Value.SetSource(stream, file.ContentType);
            }
        }

        public static void PlaySound(string sound, int volume = 1)
        {
            if (_soundsCollection.ContainsKey(sound))
            {
                _soundsCollection[sound].Volume = volume;
                _soundsCollection[sound].Play();
                _soundsCollection[sound].Position = new TimeSpan(0, 0, 0);
            }
        }
    }
}
