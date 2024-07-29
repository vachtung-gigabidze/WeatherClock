using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherClock
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        private void GifPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            GifPlayer.Play();
        }

        private void GifPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            GifPlayer.Position = TimeSpan.Zero;
            GifPlayer.Play();
        }
    }
}
