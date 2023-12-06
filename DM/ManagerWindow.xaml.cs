using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        Entities db = new Entities();
        public ManagerWindow()
        {
            InitializeComponent();
            List.ItemsSource = db.ClientManager.ToList();
            ListService.ItemsSource = db.Service.ToList();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<ClientManager> clientView = db.ClientManager.ToList();
            if (SearchClient.Text.Length == 0)
            { 
                List.ItemsSource = db.ClientManager.ToList();
            }
            else
            {
                string searchText = SearchClient.Text; 
                var filteredRecords = clientView.Where(r => r.Sname.ToString().ToLower().StartsWith(searchText.ToLower())
                | r.Name.ToString().ToLower().StartsWith(searchText.ToLower())
                | r.Fname.ToString().ToLower().StartsWith(searchText.ToLower())|
                r.DateOfBirth.ToString().ToLower().StartsWith(searchText.ToLower()) |
                r.SeriosPassport.ToString().ToLower().StartsWith(searchText.ToLower()) |
                r.NumberPassport.ToString().ToLower().StartsWith(searchText.ToLower()) 
                | r.City.ToString().ToLower().StartsWith(searchText.ToLower()) |
                r.Street.ToString().ToLower().StartsWith(searchText.ToLower()) |
                r.Email.ToLower().StartsWith(searchText.ToLower())) ;
                List.ItemsSource = filteredRecords; 
            }
        }

        private void SearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Service> clientView = db.Service.ToList();
            if (SearchClient.Text.Length == 0)
            {
                List.ItemsSource = db.Service.ToList();
            }
            else
            {
                string searchText = SearchProduct.Text;
                var filteredRecords = clientView.Where(r => r.name.ToString().ToLower().StartsWith(searchText.ToLower()) ||
                r.cost.ToString().ToLower().StartsWith(searchText.ToLower()) ||
                r.code_service.ToString().ToLower().StartsWith(searchText.ToLower()));
                List.ItemsSource = filteredRecords;
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow add = new AddClientWindow();
            add.ShowDialog();
            db = new Entities();
            List.ItemsSource = db.ClientManager.ToList();
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
          if(TitleService.Text.Length!=0&&CostService.Text.Length!=0)
            {
                
            }
        }


        private void CostService_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) && e.Text != " ")
            {
                e.Handled = true;
            }
        }

        private void TitleService_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != " ")
            {
                e.Handled = true;
            }
        }
    }
}
