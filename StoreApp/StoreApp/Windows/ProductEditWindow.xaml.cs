using System.Windows;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp
{
    public partial class ProductEditWindow : Window
    {
        private readonly ApplicationDbContext _context = new();
        private Product _product;
        public ProductEditWindow(Product product)
        {
            InitializeComponent();
            _product = product;
            if (_product != null) { NameBox.Text = _product.Name; PriceBox.Text = _product.Price.ToString(); }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameBox.Text)) { MessageBox.Show("Nome obbligatorio"); return; }
                if (!decimal.TryParse(PriceBox.Text, out decimal price)) { MessageBox.Show("Prezzo non valido"); return; }
                if (_product == null) { _product = new Product { Name = NameBox.Text, Price = price }; _context.Products.Add(_product); } else { _product.Name = NameBox.Text; _product.Price = price; _context.Products.Update(_product); }
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}