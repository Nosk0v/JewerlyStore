using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private readonly Database.TradeEntities entities;
        public ObservableCollection<Database.Product> Products { get; set; }
        public ProductWindow(Database.TradeEntities entities, Database.User user)
        {
            InitializeComponent();
            this.entities = entities;
            Products = new ObservableCollection<Database.Product>(entities.Products);
            DataContext = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }
    }
}
