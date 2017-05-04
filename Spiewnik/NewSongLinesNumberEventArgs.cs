using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiewnik
{
    public class NewSongLinesNumberEventArgs : EventArgs
    {
        public int NumberOfLines { get; private set; }
        public int HeightOfLines { get; private set; }
        public NewSongLinesNumberEventArgs(int numberOfLines, int heightOfLines)
        {
            NumberOfLines = numberOfLines;
            HeightOfLines = heightOfLines;
        }
    }
}
