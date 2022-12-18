using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Game
{
    public class Goodie : Creature
    {
        //Goodie c'tor - inherits from class Creature
        public Goodie(Canvas canvas, int cordx, int cordy, string source, int size, int speed) : base(canvas, cordx, cordy, source, size, speed)
        {

        }
    }
}
