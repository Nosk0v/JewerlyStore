using JewerlyStore.Database;
using JewerlyStore.Middleware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
   
    public partial class ProductWindow : Window
    {
        private readonly Database.TradeEntities entities;
        private readonly Database.Manufacture allManufacturer;
        public Database.Manufacture SelectedManufacturer { get; set; }
        public ObservableCollection<Database.Product> Products { get; set; }
        public ObservableCollection<Database.Manufacture> Manufactures { get; set; }
        public ObservableCollection<SortItem> SortItems { get; set; }
        public SortItem SelectedSort { get; set; }
        public ProductWindow(Database.TradeEntities entities1, Database.User user)
        {
            InitializeComponent();
            entities = entities1;
            allManufacturer = new Manufacture() { ID = 0, Name = "Все производители" };
            Products = new ObservableCollection<Database.Product>(entities.Products);
            Manufactures =  new ObservableCollection<Database.Manufacture>(entities.Manufactures);
            Manufactures.Insert(0, allManufacturer);
            SortItems = new ObservableCollection<SortItem>()
            {

                new SortItem()
                { 
                    Text = "Сортировать по возрастастанию цены",
                  Description = new SortDescription() 
                  { 
                      PropertyName = "ProductCost", 
                      Direction = ListSortDirection.Ascending 
                  } 
                },
                new SortItem()
                { 
                    Text = "Сортировать по убыванию цены",
                  Description = new SortDescription() 
                  { 
                      PropertyName = "ProductCost",
                      Direction = ListSortDirection.Descending 
                  } 
                }
            };

      
            DataContext = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }

       
        private void ApplyFilter()
        {
     
            var searchString = tbSearch.Text.Trim().ToLower();
            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);
            if (view == null) return;
            var searchStringLower = searchString.ToLower();

            view.Filter = (object o) =>
            {
                var product = o as Database.Product;
                if (product == null) return false;

                


                if (searchString.Length > 0)
                {
                    
                   if (!(product.ProductName.ToLower().Contains(searchString) ||
                       product.ProductDescription.ToLower().Contains(searchString) ||
                       product.ProductCategory1.Name.ToLower().Contains(searchString) ||
                       product.Manufacture.Name.ToLower().Contains(searchString) ||
                       product.Provider.Name.ToLower().Contains(searchString)))
                    {
                        return false;
                    }
                }
                if (SelectedManufacturer != null && SelectedManufacturer != allManufacturer) 
                {
                    if (product.Manufacture != SelectedManufacturer)
                    {
                        return false;
                    }
                }
                return true;
            };
        }

        private void ApplySort()
        {
            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);
            if (view == null) return;

            view.SortDescriptions.Clear();
            if (SelectedSort != null)
            {
                view.SortDescriptions.Add(SelectedSort.Description);
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
            if (sender is ComboBox comboBox && comboBox.SelectedItem is SortItem selectedSortItem && selectedSortItem.Description == null)
            {
                comboBox.SelectedItem = null;
            }
        }
    }
}
