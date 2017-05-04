using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;

namespace Spiewnik
{
    [Serializable]
    class Line
    {
        internal string FileName { get; set; } //not null only when image is changing
        internal byte[] SomeImageInBytes { get; set; } //BitmapImage stored in byte array
        internal string TextLine { get; set; } //RichTextBox textLine text
        internal bool TextChanged { get; set; } //true only when (this TextLine != RichTextBox textLine text)

        public Line(byte[] someImageInBytes, string defautText)
        {
            SomeImageInBytes = someImageInBytes;
            TextLine = defautText;
            TextChanged = false;
        }

        //Image/button click event
        internal void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFile.Filter = "Pliki obrazów (*.jpg; *.bmp; *.png)|*.jpg;*.bmp;*.png";
            if (openFile.ShowDialog() == true)
            {
                FileName = openFile.FileName;
            }
        }

        //RichTextBox textLine text changed event
        internal void textLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged = true;
        }
    }
}
