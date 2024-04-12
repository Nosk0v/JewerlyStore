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
        private int startPosition;
        


        public Authorization()
        {

            InitializeComponent();
            random = new Random(Environment.TickCount);
            entities = new Database.TradeEntities();
            

        }

        private void OnSignClick(object sender, RoutedEventArgs e)
        {
            if (isRequireCaptcha && captchaCode != tbCaptcha.Text.Trim().ToLower())
            {
                MessageBox.Show("fdsfsfs");
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
                GenerateCaptcha();
                return;
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
            label.FontSize = random.Next(24, 32);
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-12, 12));
            

            canvas.Children.Add(label);

            int startPosition = (int)((canvas.ActualWidth / 2) - ((letterWidth * 3) / 2));

            Canvas.SetLeft(label, startPosition + (index * letterWidth));
            Canvas.SetTop(label, random.Next(-10, 10));
        }

        private void GenerateNoise()
        {
            for (int i = 1; i < 100; i++)
            {
   
                double x = random.NextDouble() * canvas.ActualWidth;
                double y = random.NextDouble() * canvas.ActualHeight;

                int radius = random.Next(4, 10);
                Ellipse ellipse = new Ellipse
                {
                    Width = radius,
                    Height = radius,
                    Fill = GetRandomBrush((byte)random.Next(100, 180)),
                    Stroke = Brushes.Transparent
                };

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                canvas.Children.Add(ellipse);
            }
            int lineCount = random.Next(2, 6);
            for (int i = 0; i < lineCount; i++)
            {
                Line line = new Line();


                line.X1 = random.Next(100, 120);
                line.Y1 = random.Next(10, 54);
                line.X2 = random.Next(260, 280);
                line.Y2 = random.Next(10, 54);
                line.Stroke = GetRandomBrush();
                line.StrokeThickness = random.Next(2, 4);

                canvas.Children.Add(line);
            }
        }
        private SolidColorBrush GetRandomBrush(byte alpha = 255)
        {
            return new SolidColorBrush(Color.FromArgb(alpha, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
        }
    }
}
