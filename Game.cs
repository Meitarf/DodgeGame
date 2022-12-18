using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;


namespace Game
{
    // Giving Class Game canvas, the creatures, timer, boolean checking whether the game is played, content dialog for win/lose, music
    class Game
    {
        //class members
        private Canvas _canvas;
        ImageBrush ib = new ImageBrush();
        private bool IsPlay;
        private bool IsPaused;
        Goodie _goodie;
        Baddie[] baddie = new Baddie[10];
        Random rnd = new Random();
        DispatcherTimer _timer;
        private Music _music = new Music();
        ContentDialog _winDialog = new ContentDialog();
        ContentDialog _loseDialog = new ContentDialog();


        //Game C'tor, setting timer to 150 milliseconds
        public Game(Canvas canvas, ContentDialog winDialog, ContentDialog loseDialog)
        {
            _canvas = canvas;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            _timer.Tick += logicTimer;
            _winDialog = winDialog;
            _loseDialog = loseDialog;
        }
        //The game is happenning in this method
        //runs every 150 milliseconds
        void logicTimer(object sender, object e)
        {
            ChaseGoodie();
            BaddieCollision();
            GoodieCollision();
            Winner();
            Loser();
        }
        //save method, creates an array with baddies top & left positions
        //in the arrays last index's saves goodie's top & position
        //creates a file with this array (string)
        public async void gameSave()
        {
            if (IsPlay == true && baddie.Length > 1)
            {
                _timer.Stop();
                string[] SaveFile = new string[(baddie.Length + 1) * 2];

                for (int i = 0; i < baddie.Length; i++)
                {
                    SaveFile[i * 2] = Canvas.GetLeft(baddie[i].Image).ToString();
                    SaveFile[i * 2 + 1] = Canvas.GetTop(baddie[i].Image).ToString();
                }
                SaveFile[SaveFile.Length - 1] = Canvas.GetLeft(_goodie.Image).ToString();
                SaveFile[SaveFile.Length - 2] = Canvas.GetTop(_goodie.Image).ToString();
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.CreateFileAsync("Save.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteLinesAsync(storageFile, SaveFile);
                _timer.Start();
            }
        }
        //load method, reads creatures positions from the saved file
        //creates baddies with their saved positions
        //creates goodie with its positions
        //uploads background, starts music and starts timer
        public async void gameLoad()
        {
            if (IsPlay) ClearGame();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync("Save.txt");
            IList<String> LoadFile = await FileIO.ReadLinesAsync(storageFile);
            int length = LoadFile.Count / 2 - 2;
            _goodie = new Goodie(_canvas, int.Parse(LoadFile[LoadFile.Count - 1]), int.Parse(LoadFile[LoadFile.Count - 2]), "ms-appx:///Assets/Crash.png", 50, 20);
            Baddie[] baddie1 = new Baddie[length];
            for (int i = 0; i < length; i++)
            {
                baddie1[i] = new Baddie(_canvas, int.Parse(LoadFile[i * 2]), int.Parse(LoadFile[i * 2 + 1]), "ms-appx:///Assets/cortex.png", 50, 5);
            }
            baddie = baddie1;
            ib.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/bg.jpg", UriKind.RelativeOrAbsolute));
            _canvas.Background = ib;
            _music.StopBgMusic();
            _timer.Stop();
            IsPlay = true;
            _timer.Start();
            await _music.PlayBackgroundMusic();
        }
        //pause method, if game is running - pause; if it's paused (not running) - play game
        public async void gamePause()
        {
            if (IsPlay)
            {
                IsPlay = false;
                IsPaused = true;
                _music.PauseBgMusic();
                _timer.Stop();
            }
            else if (!IsPlay)
            {
                IsPlay = true;
                IsPaused = false;
                await _music.PlayBackgroundMusic();
                _timer.Start();
            }
        }
        // Starting game
        public async void gameStart()
        {
            //first check if game is paused, if paused - start timer and continue
            if (IsPaused)
            {
                IsPaused = false;
                IsPlay = true;
                await _music.PlayBackgroundMusic();
                _timer.Start();
            }
            // if game is already played - start new game
            else if (IsPlay)
            {
                NewGame();
            }
            else SetGame();
        }
        //Goodie movement
        //if goodie's top position is smaller or equal to goodie's speed, don't move (keep him in the same position)
        //if not - move up       
        private void GoodieMoveUp(Goodie goodie)
        {
            if (Canvas.GetTop(goodie.Image) <= _goodie.Speed)
                Canvas.SetTop(goodie.Image, Canvas.GetTop(goodie.Image));
            else Canvas.SetTop(goodie.Image, Canvas.GetTop(goodie.Image) - _goodie.Speed);
        }
        //if goodie's top position + speed + goodie's height is bigger then the canvas height, don't move (keep him in the same position)
        //if not - move down  
        private void GoodieMoveDown(Goodie goodie)
        {
            if (Canvas.GetTop(goodie.Image) + _goodie.Speed + goodie.Image.ActualHeight > _canvas.ActualHeight)
                Canvas.SetTop(goodie.Image, Canvas.GetTop(goodie.Image));
            else Canvas.SetTop(goodie.Image, Canvas.GetTop(goodie.Image) + _goodie.Speed);
        }
        //if goodie's left position is smaller or equal to goodie's speed, don't move (keep him in the same position)
        //if not - move left
        private void GoodieMoveLeft(Goodie goodie)
        {
            if (Canvas.GetLeft(goodie.Image) <= _goodie.Speed)
                Canvas.SetLeft(goodie.Image, Canvas.GetLeft(goodie.Image));
            else Canvas.SetLeft(goodie.Image, Canvas.GetLeft(goodie.Image) - _goodie.Speed);
        }
        //if goodie's left position + speed + goodie's width is bigger then the canvas width, don't move (keep him in the same position)
        //if not - move right 
        private void GoodieMoveRight(Goodie goodie)
        {
            if (Canvas.GetLeft(goodie.Image) + _goodie.Speed + goodie.Image.ActualWidth > _canvas.ActualWidth)
                Canvas.SetLeft(goodie.Image, Canvas.GetLeft(goodie.Image));
            else Canvas.SetLeft(goodie.Image, Canvas.GetLeft(goodie.Image) + _goodie.Speed);
        }
        // sets goodie in a random position
        private void GoodieJump(Goodie goodie)
        {
            Canvas.SetLeft(goodie.Image, rnd.Next(800));
        }
        //checks which key was pressed and calls the matching method to each key (movement/jump)
        public void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    if (IsPlay)
                        GoodieMoveUp(_goodie);
                    break;
                case Windows.System.VirtualKey.Down:
                    if (IsPlay)
                        GoodieMoveDown(_goodie);
                    break;
                case Windows.System.VirtualKey.Left:
                    if (IsPlay)
                        GoodieMoveLeft(_goodie);
                    break;
                case Windows.System.VirtualKey.Right:
                    if (IsPlay)
                        GoodieMoveRight(_goodie);
                    break;
                case Windows.System.VirtualKey.Space:
                    if (IsPlay)
                        GoodieJump(_goodie);
                    break;
            }
        }
        //checks goodie position and checks if baddie's movement won't exceed the canvas
        //moves baddie towards goodie position
        private void ChaseGoodie()
        {
            if (IsPlay)
                for (int i = 0; i < baddie.Length; i++)
                {
                    if ((Canvas.GetLeft(baddie[i].Image) < Canvas.GetLeft(_goodie.Image)) && (Canvas.GetLeft(baddie[i].Image) + baddie[i].Speed + baddie[i].Image.ActualWidth <= _canvas.ActualWidth))
                        Canvas.SetLeft(baddie[i].Image, Canvas.GetLeft(baddie[i].Image) + baddie[i].Speed + rnd.Next(-2, 2));
                    else if (Canvas.GetLeft(_goodie.Image) > _goodie.Speed)
                        Canvas.SetLeft(baddie[i].Image, Canvas.GetLeft(baddie[i].Image) - baddie[i].Speed + rnd.Next(-2, 2));
                    if ((Canvas.GetTop(baddie[i].Image) < Canvas.GetTop(_goodie.Image)) && (Canvas.GetTop(baddie[i].Image) + baddie[i].Speed + baddie[i].Image.ActualHeight <= _canvas.ActualHeight))
                        Canvas.SetTop(baddie[i].Image, Canvas.GetTop(baddie[i].Image) + baddie[i].Speed + rnd.Next(-2, 2));
                    else if (Canvas.GetTop(_goodie.Image) > _goodie.Speed)
                        Canvas.SetTop(baddie[i].Image, Canvas.GetTop(baddie[i].Image) - baddie[i].Speed + rnd.Next(-2, 2));
                }
        }
        //differentiate one baddie from another
        private void BaddieCollision()
        {
            if (IsPlay)
            {
                for (int i = 0; i < baddie.Length; i++)
                {
                    for (int j = 0; j < baddie.Length; j++)
                    {
                        //check if the difference between one baddie's position is smaller than baddie's size
                        //if true - collision
                        if ((baddie[i].IsAlive == true) && (baddie[j].IsAlive==true))
                        {
                            if (i != j && Math.Abs(Canvas.GetTop(baddie[i].Image) - Canvas.GetTop(baddie[j].Image)) < baddie[i].Image.ActualHeight && Math.Abs(Canvas.GetLeft(baddie[i].Image) - Canvas.GetLeft(baddie[j].Image)) < baddie[i].Image.ActualWidth)
                            {
                                baddie[i].Kill(); //kill baddie
                                baddie[i].IsAlive = false; //mark him as not alive. this will help us know how many baddies alive are left     
                            }
                        }
                    }
                }
            }
        }
        //checks if the difference between baddie's and goodie's position is smaller than goodie's size
        //if true - collision
        private async void GoodieCollision()
        {
            if (IsPlay)
            {
                for (int i = 0; i < baddie.Length; i++)
                {
                    if
                       (Math.Abs(Canvas.GetTop(_goodie.Image) - Canvas.GetTop(baddie[i].Image)) < _goodie.Image.ActualHeight && Math.Abs(Canvas.GetLeft(_goodie.Image) - Canvas.GetLeft(baddie[i].Image)) < _goodie.Image.ActualWidth)
                    {
                        await _music.PlayWoahSound(); //collision sound
                        _goodie.Kill(); //kill goodie
                        _goodie.IsAlive = false; //mark him as not alive. (if this is true - you lost - call loser method)
                    }
                }
            }
        }

        //if there's only 1 baddie left and goodie is alive, you won
        //call content dialog for win
        private async void Winner()
        {
            int baddiesLeft = 10; // counter to help us indicate how many baddies alive are left
            if (IsPlay)
            {
                for (int i = 0; i < baddie.Length; i++)
                {
                    if (!baddie[i].IsAlive)
                    {
                        baddiesLeft--; //if baddie is dead, decremenet baddies left counter
                    }
                }
                //if there's only 1 baddie left and goodie is alive, you won
                if ((baddiesLeft == 1) && (_goodie.IsAlive == true))
                {
                    _timer.Stop();
                    _music.StopBgMusic();
                    await _music.YouWinMusic();
                    YouWinMessage();
                }
            }
        }

        //if goodie is dead, you lost
        //call content dialog for lose
        private async void Loser()
        {
            if (IsPlay)
                if (_goodie.IsAlive == false)
                {
                    _timer.Stop();
                    _music.StopBgMusic();
                    await _music.YouLoseMusic();
                    YouLoseMessage();
                }
        }
        //new game method is called when pressing "New Game" button on the content dialog when the game is over
        //you can either start a new game or exit
        // Clear game and start a new game
        public void NewGame()
        {
            ClearGame();
            gameStart();
        }
        //Set game - create background image, creating a goodie, creates an array of baddies, starting timer, start music
        public async void SetGame()
        {
            ib.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/bg.jpg", UriKind.RelativeOrAbsolute));
            _canvas.Background = ib;
            _music.StopBgMusic();
            _timer.Stop();
            IsPlay = true;
            _goodie = new Goodie(_canvas, 500, 500, "ms-appx:///Assets/Crash.png", 50, 20);
            for (int i = 0; i < baddie.Length; i++)
            {
                baddie[i] = new Baddie(_canvas, i * 200, rnd.Next(100, 500), "ms-appx:///Assets/cortex.png", 50, 5);
            }
            _timer.Start();
            await _music.PlayBackgroundMusic();
        }
        //kill all baddies and goodie
        public void ClearGame()
        {
            IsPlay = false;
            _goodie.Kill();
            for (int i = 0; i < baddie.Length; i++)
            {
                baddie[i].Kill();
                baddie[i] = null;
            }
            _music.StopYouWinSound();
            _music.StopYouLoseSound();
        }
        //shows you win content dialog
        private async void YouWinMessage()
        {
            await _winDialog.ShowAsync();
        }
        //shows you lose content dialog
        private async void YouLoseMessage()
        {
            await _loseDialog.ShowAsync();
        }
    }
}
    










