using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Spiewnik
{
    [Serializable]
    class Song
    {
        internal int NumberOfLines { get; private set; }
        internal int HeightOfLines { get; private set; }
        private List<Line> lines = new List<Line>();
        internal List<Line> Lines { get { return lines; } set { lines = value; } }
        internal string Text1 { get; set; }
        internal string Text2 { get; set; }
        internal string Text3 { get; set; }
        internal string Text4 { get; set; }

        public Song() { }

        public Song(int numberOfLines, int heightOfLines, string defaultText)
        {
            NumberOfLines = numberOfLines;
            HeightOfLines = heightOfLines;
            Text1 = defaultText;
            Text2 = defaultText;
            Text3 = defaultText;
            Text4 = defaultText;
        }

        //serialize and save song
        public void Save(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileInfo file = new FileInfo(fileName);
            using (Stream output = file.OpenWrite())
            {
                formatter.Serialize(output, this);
            }
        }

        //restore serialized song
        public void Open(string songPath)
        {
            Song tempSong;
            BinaryFormatter formatter = new BinaryFormatter();
            FileInfo file = new FileInfo(songPath);
            using (Stream input = file.OpenRead())
            {
                tempSong = (Song)formatter.Deserialize(input);
            }
            NumberOfLines = tempSong.NumberOfLines;
            HeightOfLines = tempSong.HeightOfLines;
            lines = tempSong.lines;
            Text1 = tempSong.Text1;
            Text2 = tempSong.Text2;
            Text3 = tempSong.Text3;
            Text4 = tempSong.Text4;
        }

        public void Open(byte[] songFile)
        {
            Song tempSong;
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream input = new MemoryStream(songFile))
            {
                tempSong = (Song)formatter.Deserialize(input);
            }
            NumberOfLines = tempSong.NumberOfLines;
            HeightOfLines = tempSong.HeightOfLines;
            lines = tempSong.lines;
            Text1 = tempSong.Text1;
            Text2 = tempSong.Text2;
            Text3 = tempSong.Text3;
            Text4 = tempSong.Text4;
        }
    }
}
