using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;

namespace Spiewnik
{
    public partial class MainWindow : Window
    {
        NewSong newSong;

        private Song song;

        private string mainFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private string currentPath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void plikNowy_Click(object sender, RoutedEventArgs e)
        {
            if (newSong == null || !newSong.IsEnabled)
            {
                newSong = new NewSong();
                newSong.RaiseLinesNumberEvent += new EventHandler<NewSongLinesNumberEventArgs>(newSong_RaiseLinesNumberEvent);
                newSong.Show();
            }
            newSong.createNewSong.Click += createNewSong_Click;
        }

        private void plikOtwórz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = mainFolder;
            openFile.Filter = "Pliki piosenek (*.song)|*.song";
            if (openFile.ShowDialog() == true)
            {
                this.Title = "Śpiewnik";
                song = new Song();
                song.Open(openFile.FileName);
                display.Children.Clear();
                RefreshLines();
                currentPath = openFile.FileName;
                folderPathLabel.Content = currentPath;
            }
        }

        private void plikZapisz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = mainFolder;
            saveFile.Filter = "Pliki piosenek (*.song)|*.song";
            saveFile.FileName = currentPath;
            if (saveFile.ShowDialog() == true)
            {
                this.Title = "Śpiewnik";
                song.Save(saveFile.FileName);
                currentPath = saveFile.FileName;
                folderPathLabel.Content = currentPath;
            } 
        }

        private void folder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog directory = new System.Windows.Forms.FolderBrowserDialog();
            directory.RootFolder = Environment.SpecialFolder.Desktop;
            System.Windows.Forms.DialogResult result = directory.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                mainFolder = directory.SelectedPath;
            }
        }

        void createNewSong_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Śpiewnik    *** (nie zapisany) ***";
            currentPath = "Nowy plik";
            folderPathLabel.Content = currentPath;
            display.Children.Clear();
            display.RowDefinitions.Clear();
            AddLines(song.NumberOfLines);
        }

        private void newSong_RaiseLinesNumberEvent(object sender, NewSongLinesNumberEventArgs e)
        {
            song = new Song(e.NumberOfLines, e.HeightOfLines, "tu wprowadź swój tekst");
        }

        public void AddLines(int numberOfLines)
        {
            //Setd efault textLine
            string text = string.Empty;

            //Set default image
            BitmapImage defaultImage = new BitmapImage();
            defaultImage.BeginInit();
            defaultImage.UriSource = new Uri(@"pack://application:,,,/Resources/nowaPięciolinia.jpg");
            defaultImage.EndInit();

            //Create specified number of Line class instances
            display.RowDefinitions.Clear();
            for (int i = 0; i < numberOfLines; i++)
                song.Lines.Add(new Line(ImageToByteArray(defaultImage), "tu wprowadź swój tekst"));
            RefreshLines();
        }

        private void RefreshLines()
        {
            if (song != null)
            {
                plikZapisz.IsEnabled = true;
                display.Children.Clear();
                display.RowDefinitions.Clear();
                for (int i = 0; i < (song.Lines.Count * 2); i += 2)
                {
                    Button button;
                    Image image;
                    RichTextBox textLine;

                    //Adding button
                    button = new Button();
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Width = display.Width;
                    button.Height = song.HeightOfLines;
                    button.Click += song.Lines[i / 2].button_Click;
                    button.Click += button_Click2;

                    //Adding image (in button)
                    image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.VerticalAlignment = VerticalAlignment.Top;
                    image.Width = display.Width;
                    image.Height = button.Height;
                    image.Source = ByteArrayToImage(song.Lines[i / 2].SomeImageInBytes);

                    //Adding RichTextBox (text line under the button/image)
                    textLine = new RichTextBox();
                    textLine.AcceptsReturn = false;
                    textLine.HorizontalAlignment = HorizontalAlignment.Left;
                    textLine.VerticalAlignment = VerticalAlignment.Top;
                    textLine.Width = display.Width;
                    textLine.Height = 25;
                    textLine.TextChanged -= song.Lines[i / 2].textLine_TextChanged;
                    textLine.TextChanged -= textLine_TextChanged;
                    textLine.Document.Blocks.Clear();
                    textLine.AppendText(song.Lines[i / 2].TextLine);
                    textLine.TextChanged += song.Lines[i / 2].textLine_TextChanged;
                    textLine.TextChanged += textLine_TextChanged;

                    //Set grid row and add button/image
                    display.RowDefinitions.Insert(i, new RowDefinition() { Height = new GridLength(image.Height) });
                    Grid.SetRow(button, i);
                    display.Children.Add(button);
                    Grid imageInButton = new Grid();
                    imageInButton.RowDefinitions.Insert(0, new RowDefinition() { Height = new GridLength(image.Height) });
                    Grid.SetRow(image, 0);
                    imageInButton.Children.Add(image);
                    button.Content = imageInButton;

                    //Set grid row and add text line
                    display.RowDefinitions.Insert((i + 1), new RowDefinition() { Height = new GridLength(textLine.Height) });
                    Grid.SetRow(textLine, (i + 1));
                    display.Children.Add(textLine);
                }
                text1.TextChanged -= text1_TextChanged;
                text1.Document.Blocks.Clear();
                text1.AppendText(song.Text1);
                text1.TextChanged += text1_TextChanged;

                text2.TextChanged -= text2_TextChanged;
                text2.Document.Blocks.Clear();
                text2.AppendText(song.Text2);
                text2.TextChanged += text2_TextChanged;

                text3.TextChanged -= text3_TextChanged;
                text3.Document.Blocks.Clear();
                text3.AppendText(song.Text3);
                text3.TextChanged += text3_TextChanged;

                text4.TextChanged -= text4_TextChanged;
                text4.Document.Blocks.Clear();
                text4.AppendText(song.Text4);
                text4.TextChanged += text4_TextChanged;
            }
            else
                plikZapisz.IsEnabled = false;
        }

        private void button_Click2(object sender, RoutedEventArgs e)
        {
            foreach (Line line in song.Lines)
            {
                if (line.FileName != null)
                {
                    this.Title = "Śpiewnik    *** (nie zapisany) ***";
                    BitmapImage someImage = new BitmapImage();
                    someImage.BeginInit();
                    someImage.UriSource = new Uri(line.FileName);
                    someImage.EndInit();
                    byte[] someImageInBytes = ImageToByteArray(someImage);
                    line.SomeImageInBytes = someImageInBytes;
                    line.FileName = null;
                }    
            }
            RefreshLines();
        }

        private byte[] ImageToByteArray(BitmapImage someImage)
        {
            byte[] imageInByte;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(someImage));
            using(MemoryStream memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                imageInByte = memoryStream.ToArray();
            }
            return imageInByte;
        }

        private BitmapImage ByteArrayToImage(byte[] imageInBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageInBytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
                return image;
            }
        }

        private string RichTextBoxToString(RichTextBox richTextBox)
        {
            string text;
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            text = textRange.Text;
            return text;
        }

        private void textLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (Line line in song.Lines)
            {
                if (line.TextChanged)
                {
                    this.Title = "Śpiewnik    *** (nie zapisany) ***";
                    if (line.TextLine == "tu wprowadź swój tekst")
                    {
                        var rtb = sender as RichTextBox;
                        rtb.Document.Blocks.Clear();
                    }
                    line.TextLine = RichTextBoxToString((RichTextBox)sender);
                }
                line.TextChanged = false;
            }
        }

        private void text1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Title = "Śpiewnik    *** (nie zapisany) ***";
            if (song.Text1 == "tu wprowadź swój tekst")
                text1.Document.Blocks.Clear();
            song.Text1 = RichTextBoxToString(text1);
        }

        private void text2_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Title = "Śpiewnik    *** (nie zapisany) ***";
            if (song.Text2 == "tu wprowadź swój tekst")
                text2.Document.Blocks.Clear();
            song.Text2 = RichTextBoxToString(text2);
        }

        private void text3_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Title = "Śpiewnik    *** (nie zapisany) ***";
            if (song.Text3 == "tu wprowadź swój tekst")
                text3.Document.Blocks.Clear();
            song.Text3 = RichTextBoxToString(text3);
        }

        private void text4_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Title = "Śpiewnik    *** (nie zapisany) ***";
            if (song.Text4 == "tu wprowadź swój tekst")
                text4.Document.Blocks.Clear();
            song.Text4 = RichTextBoxToString(text4);
        }

        private void aboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Program do zapisu piosenek wraz z nutami.\nUmożliwia zapis pięciolinii w formie obrazów\nwraz z tekstem.\n\nTwórca:\nPiotr Wiatr\nptrwnd@gmail.com\n\nwersja: 1.0", "O programie...",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void demoSong_Click(object sender, RoutedEventArgs e)
        {
            this.Title = "Śpiewnik";
            song = new Song();
            song.Open(Spiewnik.Properties.Resources.MazurekDabrowskiego);
            display.Children.Clear();
            RefreshLines();
            currentPath = "(Demo -- Mazurek Dąbrowskiego)";
            folderPathLabel.Content = currentPath;
        }
    }
}
