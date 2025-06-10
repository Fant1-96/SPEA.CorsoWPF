using System.Linq;
using System.Windows;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp
{
    public partial class ReturnEditWindow : Window
    {
        private readonly ApplicationDbContext _context = new();
        private Return _return;

        public ReturnEditWindow(Return pReturn)
        {
            InitializeComponent();
            _return = pReturn;

            UserCombo.ItemsSource = _context.Users.ToList();
            OrderCombo.ItemsSource = _context.Orders.ToList();

            if (_return != null)
            {
                ReasonBox.Text = _return.Reason;
                UserCombo.SelectedItem = _return.Order.User;
                OrderCombo.SelectedItem = _return.OrderId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserCombo.SelectedItem is not ApplicationUser user) { MessageBox.Show("Seleziona un utente"); return; }
                if (OrderCombo.SelectedItem is not Order order) { MessageBox.Show("Seleziona un ordine"); return; }
                if (string.IsNullOrWhiteSpace(ReasonBox.Text)) { MessageBox.Show("Motivo obbligatorio"); return; }
                if (!decimal.TryParse(RefundBox.Text, out decimal refund)) { MessageBox.Show("Rimborso non valido"); return; }
                if (_return == null)
                {
                    _return = new Return
                    {
                        RefundAmount = refund,
                        DateCreated = System.DateTime.Now,
                        OrderId = order.Id,
                        //Order = order,
                        Reason = ReasonBox.Text
                    };
                    _context.Returns.Add(_return);
                }
                else
                {
                    _return.Reason = ReasonBox.Text;
                    _return.OrderId= order.Id;
                    //_return.Order = order;
                    _context.Returns.Update(_return);
                }
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UserCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var user = UserCombo.SelectedItem as ApplicationUser;
            if (user != null)
            {
                OrderCombo.ItemsSource = _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .ToList();
            }
            else
                OrderCombo.ItemsSource = null;
        }
    }
}