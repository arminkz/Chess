using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class ChessPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public ChessPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
