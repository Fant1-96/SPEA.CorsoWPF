using System.Windows;
using StoreApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StoreApp.Models;

namespace StoreApp
{
    public partial class MainWindow : Window
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public MainWindow()
        {
            InitializeComponent();
            _context.Database.EnsureCreated();
            LoadUsers();
            LoadProducts();
            LoadOrders();
            LoadReturns();
        }
        private void LoadUsers() { UsersGrid.ItemsSource = _context.Users.AsNoTracking().ToList(); }
        private void LoadProducts() { ProductsGrid.ItemsSource = _context.Products.AsNoTracking().ToList(); }
        private void LoadOrders() { OrdersGrid.ItemsSource = _context.Orders.Include(o => o.User).Include(o => o.OrderProducts).ThenInclude(op => op.Product).AsNoTracking().ToList(); }
        private void LoadReturns() { ReturnsGrid.ItemsSource = _context.Returns.Include(o => o.User).Include(r => r.ReturnedProduct).ThenInclude(op => op.Product).AsNoTracking().ToList(); }
        private void RefreshUsers_Click(object sender, RoutedEventArgs e) { LoadUsers(); }
        private void RefreshProducts_Click(object sender, RoutedEventArgs e) { LoadProducts(); }
        private void RefreshOrders_Click(object sender, RoutedEventArgs e) { LoadOrders(); }
        private void RefreshReturns_Click(object sender, RoutedEventArgs e) { LoadReturns(); }
        private void NewUser_Click(object sender, RoutedEventArgs e) { var win = new UserEditWindow(null); if (win.ShowDialog() == true) { LoadUsers(); } }
        private void EditUser_Click(object sender, RoutedEventArgs e) { var user = (ApplicationUser)UsersGrid.SelectedItem; var win = new UserEditWindow(user); if (win.ShowDialog() == true) { LoadUsers(); } }
        private void DeleteUser_Click(object sender, RoutedEventArgs e) { var user = (ApplicationUser)UsersGrid.SelectedItem; if (user == null) return; if (MessageBox.Show("Confermi cancellazione?", "Attenzione", MessageBoxButton.YesNo) == MessageBoxResult.Yes) { _context.Users.Remove(user); _context.SaveChanges(); LoadUsers(); LoadOrders(); } }
        private void NewProduct_Click(object sender, RoutedEventArgs e) { var win = new ProductEditWindow(null); if (win.ShowDialog() == true) { LoadProducts(); } }
        private void EditProduct_Click(object sender, RoutedEventArgs e) { var product = (Product)ProductsGrid.SelectedItem; var win = new ProductEditWindow(product); if (win.ShowDialog() == true) { LoadProducts(); } }
        private void DeleteProduct_Click(object sender, RoutedEventArgs e) { var product = (Product)ProductsGrid.SelectedItem; if (product == null) return; if (MessageBox.Show("Confermi cancellazione?", "Attenzione", MessageBoxButton.YesNo) == MessageBoxResult.Yes) { _context.Products.Remove(product); _context.SaveChanges(); LoadProducts(); LoadOrders(); } }
        private void NewOrder_Click(object sender, RoutedEventArgs e) { var win = new OrderEditWindow(null); if (win.ShowDialog() == true) { LoadOrders(); } }
        private void EditOrder_Click(object sender, RoutedEventArgs e) { var order = (Order)OrdersGrid.SelectedItem; var win = new OrderEditWindow(order); if (win.ShowDialog() == true) { LoadOrders(); } }
        private void DeleteOrder_Click(object sender, RoutedEventArgs e) { var order = (Order)OrdersGrid.SelectedItem; if (order == null) return; if (MessageBox.Show("Confermi cancellazione?", "Attenzione", MessageBoxButton.YesNo) == MessageBoxResult.Yes) { _context.Orders.Remove(order); _context.SaveChanges(); LoadOrders(); } }

        private void EditReturn_Click(object sender, RoutedEventArgs e)
        {
            var rReturn = (Return)ReturnsGrid.SelectedItem; var win = new ReturnEditWindow(rReturn); if (win.ShowDialog() == true) { LoadOrders(); }
        }

        private void DeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            var rReturn = (Return)ReturnsGrid.SelectedItem;
            if (rReturn == null) return;
            if (MessageBox.Show("Confermi cancellazione?", "Attenzione", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Returns.Remove(rReturn);
                _context.SaveChanges();                
                LoadOrders();
                LoadReturns();
            }
        }

        private void NewReturn_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReturnEditWindow(null);
            if (win.ShowDialog() == true)
            {
                LoadOrders();
                LoadReturns();
            }
        }
    }
}
