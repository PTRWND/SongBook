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
using System.Windows.Shapes;

namespace Spiewnik
{
    public partial class NewSong : Window
    {
        public event EventHandler<NewSongLinesNumberEventArgs> RaiseLinesNumberEvent;

        public NewSong()
        {
            InitializeComponent();
            InitializeNumberOfLinesComboBox();
            InitializeHeightOfLinesComboBox();
        }

        private void createNewSong_Click(object sender, RoutedEventArgs e)
        {
            RaiseLinesNumberEvent(this, new NewSongLinesNumberEventArgs((int)numberOfLines.SelectedItem, (int)heightOfLines.SelectedItem));
            this.Close();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitializeNumberOfLinesComboBox()
        {
            for (int i = 1; i <= 20; i++)
                numberOfLines.Items.Add(i);
            numberOfLines.SelectedIndex = 0;
        }

        private void InitializeHeightOfLinesComboBox()
        {
            for (int i = 50; i <= 200; i += 10)
                heightOfLines.Items.Add(i);
            heightOfLines.SelectedIndex = 5;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = false;
        }
    }
}
