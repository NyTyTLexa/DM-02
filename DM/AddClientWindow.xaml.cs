using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        Entities db = new Entities();
        
        public AddClientWindow()
        {
            InitializeComponent();
            CreateComboboxCity();
            CreateComboboxStreet();
            CreateGrid();
            CreateGridStreet();
        }
        public TextBox textBox = new TextBox();
        public Button button = new Button();
        public Grid GridCity = new Grid();
        public void CreateComboboxStreet()
        {
            Street.Items.Clear();
            for (int i = 0; i <= db.Street.Select(a => a.Title).ToList().Count; i++)
            {
                if (i == db.Street.Select(a => a.Title).ToList().Count)
                {

                    Street.Items.Add(GridStreet);
                }
                else
                {
                    Street.Items.Add(db.Street.Select(a => a.Title).ToList()[i]);
                }
            }
        }

        public void CreateComboboxCity()
        {
            City.Items.Clear();
            for (int i = 0; i <= db.City.Select(a => a.Title).ToList().Count; i++)
            {
                if (i == db.City.Select(a => a.Title).ToList().Count)
                {
                    
                    City.Items.Add(GridCity);
                }
                else
                {
                    City.Items.Add(db.City.Select(a => a.Title).ToList()[i]);
                }
            }
        }
        public void CreateGrid()
        {
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.FontSize = City.FontSize;
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Content = "➕";
            button.Background = Brushes.Green;
            button.BorderBrush = Brushes.Transparent;
            button.Click += AddClick;
            GridCity.Children.Add(button);
            GridCity.Children.Add(textBox);
            GridCity.Width = 200;
            textBox.Width = 150;
            
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (db.City.Any(a => a.Title == "г. " + textBox.Text))
            {
                var r = db.City.Where(a => a.Title == textBox.Text).ToList();
                textBox.Text = "Город существует";
            }
            else
            {
                City city = new City();
                city.Title = "г. " + textBox.Text;
                db.City.Add(city);
                db.SaveChanges();
                db = new Entities();
                CreateComboboxCity();
            }
        }
        public TextBox tbstreet = new TextBox();
        public Button btstreet = new Button();
        public Grid GridStreet = new Grid();
        public void CreateGridStreet()
        {
            tbstreet.HorizontalAlignment = HorizontalAlignment.Left;
            tbstreet.FontSize = City.FontSize;
            btstreet.HorizontalAlignment = HorizontalAlignment.Right;
            btstreet.Content = "➕";
            btstreet.Background = Brushes.Green;
            btstreet.BorderBrush = Brushes.Transparent;
            btstreet.Click += AddClickStreet;
            GridStreet.Children.Add(btstreet);
            GridStreet.Children.Add(tbstreet);
            GridStreet.Width = 200;
            tbstreet.Width = 150;

        }
        private void AddClickStreet(object sender, RoutedEventArgs e)
        {
            if (db.Street.Any(a => a.Title == "ул. " + tbstreet.Text))
            {
                var r = db.City.Where(a => a.Title == tbstreet.Text).ToList();
                tbstreet.Text = "Улица существует";
            }
            else
            {
                Street city = new Street();
                city.Title = "ул. " + tbstreet.Text;
                db.Street.Add(city);
                db.SaveChanges();
                db = new Entities();
                CreateComboboxStreet();
            }
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
        MessageWindow message;
        Client client;
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if (Sname.Text.Length != 0 && Name.Text.Length != 0 && Fname.Text.Length != 0 && SeriosPassport.Text.Length != 0 &&
                NumberPassport.Text.Length != 0 && City.Text.Length!=0&&Street.Text.Length!=0&&House.Text.Length!=0&&Flat.Text.Length !=0&&DateOfBirth.Text.Length!=0&&
                Email.Text.Length!=0)//Проверка на пустоту
            {
               if(Convert.ToDateTime(DateOfBirth.Text)<DateTime.Now&&
                    Convert.ToDateTime(DateOfBirth.Text).Year>DateTime.Now.Year-100)//Проверка возраста
                {
                    if (Email.Text.Contains('@') & Email.Text.Contains('.'))//Проверка на email
                    {
                        if(SeriosPassport.Text.Length==4&&NumberPassport.Text.Length==6)//Проверка паспортных данных
                        {
                            if (City.SelectedIndex != db.City.Select(a => a.Title).ToList().Count)
                            {
                                if (Street.SelectedIndex != db.Street.Select(a => a.Title).ToList().Count)
                                {
                                   
                                    client = new Client();
                                    client.ID = db.Client.ToList().Last().ID + 1;
                                    client.Password =$"cl{(db.Client.ToList().Count+1)}";
                                    client.Sname = Sname.Text;
                                    client.Name = Name.Text;
                                    client.Fname = Fname.Text;
                                    client.SeriosPassport = Convert.ToInt32(SeriosPassport.Text);
                                    client.NumberPassport = Convert.ToInt32(NumberPassport.Text);
                                    client.Email = Email.Text;
                                    client.City_id = db.City.Where(a => a.Title == City.SelectedItem).ToList()[0].ID;
                                    client.Street_id = db.Street.Where(a => a.Title == Street.SelectedItem).ToList()[0].ID;
                                    client.House = Convert.ToInt32(House.Text);
                                    client.Index = Convert.ToInt32(Index.Text);
                                    client.Flat = Convert.ToInt32(Flat.Text);
                                    client.DateOfBirth = Convert.ToDateTime(DateOfBirth.Text);
                                    db.Client.Add(client);
                                    db.SaveChanges();
                                    message = new MessageWindow("Успешно создан новый клиент","Ok");
                                    this.Close();


                                }
                                else
                                {
                                    message = new MessageWindow("Выбран улица которого не существует", "Ok");
                                }
                            }
                            else
                            {
                                message = new MessageWindow("Выбрана город которого не существует", "Ok");
                             }

                        }
                        else
                        {
                            message = new MessageWindow("Введите корректно паспортные данные", "Ok");
                        }
                    }
                    else
                    {
                        message = new MessageWindow("Введите корректно email", "Ok");
                    }
                }
               else
                {
                    message = new MessageWindow("Введите корректно возраст","Ok");
                }
            } 
            else
            {
                message = new MessageWindow("Заполните все поля","Ok");
            }
            message.ShowDialog();
        }

        private void SeriosPassport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)&& e.Text !=" ")
            {
                e.Handled = true;
            }
        }

        private void NumberPassport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)&&e.Text != " ")
            {
                e.Handled = true;
            }
        }

        private void Sname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           if (Char.IsDigit(e.Text, 0)&&e.Text != " ")
            {
                e.Handled = true;
            }
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Street_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
