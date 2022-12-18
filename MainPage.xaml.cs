using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Game
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        Game _game;
        

        public TypedEventHandler<CoreWindow, KeyEventArgs> CoreWindow_KeyDown { get; }

        public MainPage()
        {
            this.InitializeComponent();
            _game = new Game(Playground, YouWinDialog, YouLoseDialog); // giving game the elements from xaml
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown1;
      
        }
        //calling keyboard actions from game
        private void CoreWindow_KeyDown1(CoreWindow sender, KeyEventArgs args)
        {
            _game.CoreWindow_KeyDown(sender, args);
        }
        //save button. calling save method from game
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _game.gameSave();
        }
        //start button. calling start method from game
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _game.gameStart();
        }
        //pause button. calling pause method from game
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            _game.gamePause();
        }
        //load button. calling load method from game
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            _game.gameLoad();
        }
        //exit button. closes game
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        //new game button in winning content dialog, calling new game method from game
        private void YouWinDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _game.NewGame();
        }
        //exit button in content dialog
        private void YouWinDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }
        //new game button in losing content dialog, calling new game method from game
        private void YouLoseDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _game.NewGame();
        }
        //exit button in content dialog
        private void YouLoseDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }
    }
}






