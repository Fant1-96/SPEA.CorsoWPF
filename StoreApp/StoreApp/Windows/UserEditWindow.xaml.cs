using System.Windows;
using StoreApp.Data;
using StoreApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace StoreApp
{
    public partial class UserEditWindow : Window
    {
        private readonly ApplicationDbContext _context = new();
        private ApplicationUser _user;
        public UserEditWindow(ApplicationUser user)
        {
            InitializeComponent();
            _user = user;
            if (_user != null) { UserNameBox.Text = _user.UserName; PasswordBox.Password = "********"; }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserNameBox.Text)) { MessageBox.Show("UserName obbligatorio"); return; }
                var hasher = new PasswordHasher<ApplicationUser>();
                if (_user == null) { _user = new ApplicationUser { UserName = UserNameBox.Text }; _user.PasswordHash = hasher.HashPassword(_user, PasswordBox.Password); _context.Users.Add(_user); } else { _user.UserName = UserNameBox.Text; if (!string.IsNullOrWhiteSpace(PasswordBox.Password) && PasswordBox.Password != "********") _user.PasswordHash = hasher.HashPassword(_user, PasswordBox.Password); _context.Users.Update(_user); }
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
