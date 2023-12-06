using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GenerateCaptch(CaptchaTb, CaptchaImage);
        }
        public int CountClick = 0;
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
        int _level = 5;// Значение для уровня капчи, с каждой неудачой оно будет увеличиваться и будет увеличивать количество букв
        string _captcha = null;
        public void GenerateCaptch(TextBlock TbCaptcha, Canvas ImageCaptcha)//Простой генератор капчи
        {
            CaptchaImage.Children.Clear();
            _captcha = null;
            string strings = "QWERTYUUIOPASDFGHJKLZXCVBNM123456789";
            Random random = new Random();
            for (int i = 1; i < _level + 1; i++)
            {
                GeneratorLine(ImageCaptcha);
                char a = strings.ToCharArray()[random.Next(0, strings.ToCharArray().Length)];
                _captcha += a;
                TextBlock text = new TextBlock();
                text.FontSize = random.Next(20, 30);
                text.FontWeight = FontWeights.Bold;
                text.RenderTransform = new RotateTransform(random.Next(-1, 1));
                text.SetValue(Canvas.LeftProperty, 1d * i * 30);
                text.SetValue(Canvas.TopProperty, 1d * random.Next(1, i * (int)(TbCaptcha.Height / 12)));
                text.Text = _captcha.ToCharArray()[i - 1].ToString();
                ImageCaptcha.Children.Add(text);
            }

            GeneratorNoise(TbCaptcha.Height, TbCaptcha.Width, ImageCaptcha);

        }
        public void GeneratorLine(Canvas ImageCaptcha)//Генератор линий уровень 1 создания шума
        {
            Random random = new Random();
            for (int j = 0; j < _level * 2; j++)
            {
                Line line = new Line();
                line.X1 = random.Next(0, 280);
                line.X2 = random.Next(0, 280);
                line.Y1 = random.Next(0, 60);
                line.Y2 = random.Next(0, 60);
                line.StrokeThickness = 1;
                line.Opacity = random.NextDouble();
                var brush = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
                line.Stroke = brush;
                ImageCaptcha.Children.Add(line);
            }
        }
        public void GeneratorNoise(double height, double width, Canvas ImageCaptcha)//Генерерирует шум ввиде очень много пискель на экране
        {
            Random random = new Random();
            for (int a = 0; a < height / 1.5; a++)
            {
                for (int b = 0; b < width / 2; b++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = rectangle.Width = 3;
                    rectangle.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
                    rectangle.Opacity = 0.3;
                    rectangle.SetValue(Canvas.LeftProperty, 2d * b);
                    rectangle.SetValue(Canvas.TopProperty, 2d * a);
                    ImageCaptcha.Children.Add(rectangle);
                }
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DuplicateCaptch_Click(object sender, RoutedEventArgs e)
        {

            GenerateCaptch(CaptchaTb, CaptchaImage);
        }

        int click_count=0;
        Entities db = new Entities();
        MessageWindow message;
        private void Autorization_Click(object sender, RoutedEventArgs e)
        {
            
                message = null;
                if(click_count>=0&click_count<=2)
                {
                    if (Login.Text.Length != 0 &&
                    PasswordClose.Password.Length != 0 || PasswordOpen.Text.Length != 0)
                    {
                        if (Login.Text.Contains("@") & 
                        Login.Text.Contains(".")&!Login.Text.Contains(" "))//Осуществляет проверку на правильность ввода пользователя
                        {
                            List<Employee> employees = new List<Employee>(db.Employee);
                            bool check1 = employees.Any(a => a.Login == Login.Text && (a.Password == PasswordOpen.Text || PasswordClose.Password == a.Password) && a.post_id == 1);//Проверка должности
                            bool check2 = employees.Any(a => a.Login == Login.Text && (a.Password == PasswordOpen.Text || PasswordClose.Password == a.Password) && a.post_id == 2);
                            bool check3 = employees.Any(a => a.Login == Login.Text && (a.Password == PasswordOpen.Text || PasswordClose.Password == a.Password) && a.post_id == 3);
                            if (db.Employee.Any(a => a.Login == Login.Text && (a.Password == PasswordOpen.Text || PasswordClose.Password == a.Password)))//Условия успешного входа
                            {
                                if (check1)//Первая должность и так далее
                                {
                                    message = new MessageWindow($"Успешный вход, роль: {db.Post.Find(1).Title},Время входа {DateTime.Now.Hour}:{DateTime.Now.Minute} ", "Ok");
                                AdministratorWindow administratorWindow = new AdministratorWindow();
                                administratorWindow.Show();
                                this.Close();
                            }
                                if (check2)
                                {
                                    message = new MessageWindow($"Успешный вход, роль: {db.Post.Find(2).Title},Время входа {DateTime.Now.Hour}:{DateTime.Now.Minute} ", "Ok");
                                ManagerWindow administratorWindow = new ManagerWindow();
                                administratorWindow.Show();
                                this.Close();
                            }
                                if (check3)
                                {
                                    message = new MessageWindow($"Успешный вход, роль: {db.Post.Find(3).Title},Время входа {DateTime.Now.Hour}:{DateTime.Now.Minute} ", "Ok");
                                ManagerWindow administratorWindow = new ManagerWindow();
                                administratorWindow.Show();
                                this.Close();

                            }
                                History_Employee history = new History_Employee();//Сохранение успешного входа
                                history.Date = DateTime.Now;
                                history.Id_employe = db.Employee.Where(a => a.Login == Login.Text && (a.Password == PasswordOpen.Text || PasswordClose.Password == a.Password)).ToList()[0].ID;
                                history.id_input = 2;//Присвоение успешного входа
                                db.History_Employee.Add(history);
                                db.SaveChanges();
                            }
                            else
                            {
                                click_count++;
                            message = new MessageWindow($"Данного пользоветеля несуществует, осталось {3 - click_count + 1} попытки", "Ok");
                            if (db.Employee.Any(a=>a.Login == Login.Text))//Реализация неуспешного входа при не верном пароли
                                {
                                History_Employee history = new History_Employee();
                                history.Date = DateTime.Now;
                                history.Id_employe = db.Employee.Where(a => a.Login == Login.Text).ToList()[0].ID;
                                history.id_input = 1;
                                db.History_Employee.Add(history);
                                db.SaveChanges();
                                message = new MessageWindow($"Пароль введен не корректно,осталось {3 - click_count + 1} попытки", "Ok");//Использование общего класса окна для реализации одного окна уведомление
                            }  
                            }
                        }
                        else
                        {
                            click_count++;
                            message = new MessageWindow($"Данные введены \n некорректно, осталось {3 - click_count + 1} попытки", "Ok");
                        }

                    }
                    else
                    {
                        click_count++;
                        message = new MessageWindow($"Данные введены \n некорректно, осталось {3 - click_count + 1} попытки", "Ok");
                    }
                }
                else
                {
                    EnterCaptcha.Clear();
                    EnterCaptcha.Foreground = Brushes.Black;
                    Autorization.IsEnabled = false;
                    GenerateCaptch(CaptchaTb, CaptchaImage);
                    this.Height = 500;
                   
                }
            
            if(message!=null)
            message.ShowDialog();
            message = null;
        }
        public int a = 0;
        private void PasswordOpen_Click(object sender, RoutedEventArgs e)
        {
            a++;
            if (a % 2 == 0)
            {
                ImageEye.Source = new BitmapImage(new Uri("Resource\\eyeOpen.png", UriKind.Relative));
                PasswordClose.Password = PasswordOpen.Text; // скопируем в PasswordBox из TextBox
                PasswordOpen.Visibility = Visibility.Hidden; // TextBox - скрыть
                PasswordClose.Visibility = Visibility.Visible; // PasswordBox - отобразить
            }
            else
            {
                ImageEye.Source = new BitmapImage(new Uri("Resource\\eyeClose.png", UriKind.Relative));
                PasswordOpen.Text = PasswordClose.Password; // скопируем в TextBox из PasswordBox
                PasswordOpen.Visibility = Visibility.Visible; // TextBox - отобразить
                PasswordClose.Visibility = Visibility.Hidden; // PasswordBox - скрыть
            }
        }
        int CheckCaptch;
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (EnterCaptcha.Text != _captcha)//Неуспешный вход
            {
                
                EnterCaptcha.Foreground = Brushes.Red;
                GenerateCaptch(CaptchaTb, CaptchaImage);
                CheckCaptch++;
                message = new MessageWindow($"Введено неверно,осталось попыток {3-CheckCaptch}❌","Ok");//Вызов сообщение об неуспеншом входе
                if (CheckCaptch==3)
                {
                    this.IsEnabled = false;
                    message = new MessageWindow("Блокировка окна на 10 секунд","Ok");
                    dispatcherTimer.Start();
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                }
                message.Show();
            }
            if (EnterCaptcha.Text == _captcha)//Успешный вход
            {
                EnterCaptcha.Foreground = Brushes.Green;
                this.Height = 280;
                Autorization.IsEnabled = true;
                message = new MessageWindow("Капча введена \n верно","Ok");//Вызов сообщение об успешном входе
                message.ShowDialog();
                click_count = 0;
            }
        }

       


        private void EnterCaptcha_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int BlockWindowTime;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            BlockWindowTime++;
            if(BlockWindowTime == 10)
            {
                this.IsEnabled = true;
                dispatcherTimer.Stop();
                BlockWindowTime = 0;
            }
        }
        private void EnterCaptcha_GotMouseCapture(object sender, MouseEventArgs e)
        {
            EnterCaptcha.Clear();
            EnterCaptcha.Foreground = Brushes.Black;
        }

       
    }
 

}
