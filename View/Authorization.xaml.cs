using System;
using System.Linq;
using System.Windows;
using JewerlyStore.Middleware;

namespace JewerlyStore.View
{

    public partial class Authorization : Window
    {
        private readonly Database.TradeEntities entities;
        private Database.User user;
        private bool isRequireCaptcha;
        private string captchaCode;
        private CaptchaLogicGenerator captchaLogicGenerator;



        public Authorization()
        {

            InitializeComponent();
            captchaLogicGenerator = new CaptchaLogicGenerator();
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
                captchaLogicGenerator.GenerateCaptcha(canvas);
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

