using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace Game
{
    //Giving Class Creature canvas, size, source (image), speed, image, and a boolean checking whether creature is alive
    public class Creature
    {    
        //class members
        private Canvas _canvas;
        private int _size;
        private string _source;
        private int _speed;
        private Image _image;
        private bool isAlive;
      //Creature c'tor
      //each creature (goodie and baddie) must have these properties
        public Creature (Canvas canvas, int cordx, int cordy, string source, int size, int speed)
        {
            isAlive = true;
            _canvas = canvas;
            _source = source;
            _size = size;
            _speed = speed;
            Image _image = new Image();
            _image.Source = new BitmapImage(new Uri(_source));
            _image.Width = _size;
            _image.Height = _size;
            Canvas.SetLeft(_image, cordx);
            Canvas.SetTop(_image, cordy);
            Image = _image;
            _canvas.Children.Add(_image);
        }
        //class properties
        public Image Image
        {
            get { return _image; }
            set { _image = value; }          
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
            public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
       //kill method - removes creature from canvas
        public void Kill()
        {                       
            _canvas.Children.Remove(Image);
        }
    }
}
