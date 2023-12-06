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
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        Entities db = new Entities();
        public AdministratorWindow()
        {
            InitializeComponent();
            List.ItemsSource = db.AdminWindow.ToList();
            Table.ItemsSource = db.HistoryAdminWindow.ToList();
            Filter.ItemsSource = db.Employee.ToList().Select(a => a.Login);
        }

     private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void Filter_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                Table.ItemsSource = db.HistoryAdminWindow.Where(w => w.Login == Filter.SelectedItem).ToList();
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            Table.ItemsSource = db.HistoryAdminWindow.ToList();
        }
    }
}
