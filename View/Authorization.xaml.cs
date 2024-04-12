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
        private readonly Database.TradeEntities entities;
        private Database.User user;

        public Authorization()
        {
            InitializeComponent();
            entities = new Database.TradeEntities();
        }

        private void OnSignClick(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string password = tbPassword.Password.Trim();
            if (login.Length < 1 || password.Length < 1 )
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            user = entities.Users.Where(u => u.UserLogin ==  login && u.UserPassword == password).FirstOrDefault();
            if (user == null) 
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
