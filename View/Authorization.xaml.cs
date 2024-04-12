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

namespace JewerlyStore.View
{

    public partial class Authorization : Window
    {
        private readonly double letterWidth = 40;
        private readonly Random random;
        private readonly string captchaSymbols = "QWERTYUIOPASDFGHJKLZXCVBN1234567890";
        private readonly Database.TradeEntities entities;
        private Database.User user;
        private bool isRequireCaptcha;
        private string captchaCode;
        private double startPosition;
        


        public Authorization()
        {

            InitializeComponent();
            random = new Random(Environment.TickCount);
            entities = new Database.TradeEntities();
            

        }

        private void OnSignClick(object sender, RoutedEventArgs e)
        {
            if (isRequireCaptcha)
            {
                MessageBox.Show("Введите Captcha");
                return;
            }
            string login = tbLogin.Text.Trim();
            string password = tbPassword.Password.Trim();
            if (login.Length < 1 || password.Length < 1)
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            user = entities.Users.Where(u => u.UserLogin == login && u.UserPassword == password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                isRequireCaptcha = true;
                spCaptcha.Visibility = Visibility.Visible;
                GenerateCaptcha();
                return;
            }
            if (isRequireCaptcha)
            {
                isRequireCaptcha = false;
            }
            
            switch (user.UserRole)
            {
                case 1: // Администратор
                    break;
                case 2: // Менеджер
                case 3: // Клиент

                    ProductWindow productWindow = new ProductWindow(entities, user);
                    productWindow.Owner = this;
                    productWindow.Show();
                    Hide();
                    break;
            }
        }
      
        private void GenerateCaptcha()
        {
            canvas.Children.Clear();
            captchaCode = GetNewCaptchaCode();
            for (int i = 0; i < captchaCode.Length; i++)
            {
                AddCharToCanvas(i, captchaCode[i]);
            }
            GenerateNoise();
        }
        private string GetNewCaptchaCode()
        {
            string code = "";
            for (int i = 0; i < 4; i++)
            {
                code += captchaSymbols[random.Next(captchaSymbols.Length)];
            }
            return code;
        }

       private void AddCharToCanvas(int index, char ch)
        {
            Label label = new Label();
            label.Content = ch.ToString();
            label.Width = letterWidth;
            label.Height = 60;
            label.Foreground = GetRandomBrush();
            label.FontWeight = FontWeights.Black;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.FontSize = random.Next(24, 36);
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-20, 20));
            label.FontFamily = new FontFamily("Comic Sans MS");


            canvas.Children.Add(label);

            startPosition = (canvas.ActualWidth / 2) - (letterWidth / 2);

            Canvas.SetLeft(label, startPosition + (index * letterWidth));
            Canvas.SetTop(label, random.Next(0, 10));
        }

        private void GenerateNoise()
        {
            int ellipseCount = random.Next(50, 150);
            for (int i = 1; i < ellipseCount; i++)
            {
                double x = random.NextDouble() * 400;
                double y = random.NextDouble() * 50;

                int radius = random.Next(2, 6);
                Ellipse ellipse = new Ellipse
                {
                    Width = radius,
                    Height = radius,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    Stroke = Brushes.Transparent,
                };
                canvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
            }

            int lineCount = random.Next(2, 6);
            for (int i = 0; i < lineCount; i++)
            {
             


                Line line = new Line
                {
                    X1 = random.Next(80, 150),
                    Y1 = random.Next(10, 70),
                    X2 = random.Next(240, 280),
                    Y2 = random.Next(10, 70),
                    Stroke = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    StrokeThickness = 1,
                };

                canvas.Children.Add(line);
            }
        }
        private SolidColorBrush GetRandomBrush(byte alpha = 255)
        {
            return new SolidColorBrush(Color.FromArgb(alpha, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
        }

        private void OnCaptcha(object sender, RoutedEventArgs e)
        {
            if (isRequireCaptcha && captchaCode.ToLower() == tbCaptcha.Text.Trim().ToLower())
            {
                MessageBox.Show("Все правильно");
                isRequireCaptcha = false;
                spCaptcha.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Неверно");
                return;
            }
        }
        
    }
    }

