using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Game_Project
{
    internal class Beállítások
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static string directions;
        public Beállítások()
        {
            Width = 16;
            Height = 16;
            directions = "left";
        }
    }
}
