using System.Windows;
using StoreApp.Data;
using StoreApp.Models;
using System.Linq;
using System.Collections.Generic;

namespace StoreApp
{
    public partial class OrderEditWindow : Window
    {
        private readonly ApplicationDbContext _context = new();
        private Order _order;
        public OrderEditWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            UserCombo.ItemsSource = _context.Users.ToList();
            ProductsList.ItemsSource = _context.Products.ToList();
            if (_order != null)
            {
                UserCombo.SelectedItem = _order.User;
                foreach (var op in _order.OrderProducts) ProductsList.SelectedItems.Add(op.Product);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserCombo.SelectedItem is not ApplicationUser user) { MessageBox.Show("Seleziona un utente"); return; }
                if (ProductsList.SelectedItems.Count == 0) { MessageBox.Show("Seleziona almeno un prodotto"); return; }
                if (_order == null) { _order = new Order { DateCreated = System.DateTime.Now, UserId = user.Id, OrderProducts = new List<OrderProduct>() }; _context.Orders.Add(_order); } else { _order.UserId = user.Id; _order.OrderProducts.Clear(); }
                foreach (Product p in ProductsList.SelectedItems) { _order.OrderProducts.Add(new OrderProduct { Order = _order, ProductId = p.Id }); }
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
