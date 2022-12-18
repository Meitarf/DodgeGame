using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Game
{

    public class Music
    {
        //class members
        private MediaElement _background = new MediaElement();
        private MediaElement _youWinSound = new MediaElement();
        private MediaElement _youLoseSound = new MediaElement();
        private MediaElement _woahSound = new MediaElement();

        public async Task<MediaElement> PlayBackgroundMusic()
        {
            var BgMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("CB3.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BgMusicElement.SetSource(stream, "");
            BgMusicElement.Play();         
            _background = BgMusicElement;
            return BgMusicElement;
        }
        public async Task<MediaElement> PlayWoahSound()
        {
            var CollisionSoundElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("woah.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            CollisionSoundElement.SetSource(stream, "");
            CollisionSoundElement.Play();
            return CollisionSoundElement;
        }
        public async Task<MediaElement> YouWinMusic()
        {
            var YouWinElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Winner.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            YouWinElement.SetSource(stream, "");
            YouWinElement.Play();
            _youWinSound = YouWinElement;
            return YouWinElement;
        }
        public async Task<MediaElement> YouLoseMusic()
        {
            var YouLoseElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Loser.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            YouLoseElement.SetSource(stream, "");
            YouLoseElement.Play();
            _youLoseSound = YouLoseElement;
            return YouLoseElement;
        }
        //Stops the background music
        public void StopBgMusic()
        {
            _background.Stop();
        }
        //Pauses the background music
        public void PauseBgMusic()
        {
            _background.Pause();
        }
        //Stops Winner music
        public void StopYouWinSound()
        {
            _youWinSound.Stop();
        }
        //Stops Loser music
        public void StopYouLoseSound()
        {
            _youLoseSound.Stop();
        }
    }
}
