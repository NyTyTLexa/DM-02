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

namespace DM
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string text, string messageButton)
        {
            InitializeComponent();
            if(messageButton == "Ok")
            {
                Ok.Visibility = Visibility.Visible;
            }
            if(messageButton == "YesOrNo")
            {
                YES.Visibility = Visibility.Visible;
                NO.Visibility = Visibility.Visible;
            }
            Text.Text = text;
        }

        private void YES_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NO_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
