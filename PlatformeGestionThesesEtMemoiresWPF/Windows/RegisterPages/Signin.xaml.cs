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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLogicLayer;
using PlatformeGestionThesesEtMemoiresWPF.Windows.AdminPages;
using PlatformeGestionThesesEtMemoiresWPF.Windows.UserPages;

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages
{
    /// <summary>
    /// Interaction logic for Signin.xaml
    /// </summary>
    public partial class Signin : Page
    {


        public UserHomePage UserHomePage1;
        public AdminHomePage AdminHomePage1;

        private void pwd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox password && string.IsNullOrEmpty(password.Password))
            {
                password.Password = "password";
            }
        }

        private void pwd_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox password && password.Password == "password")
            {
                password.Password = "";
            }
        }

        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "username";
            }
        }
        private void email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "username")
            {
                textBox.Text = "";
            }
        }




        public Signin()
        {

            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserBLL u = UserConnectionBLL.UserLogin(emailTextBox.Text, passwordBox.Password);

            if (u.id != 0)
            {
                if(u.status == "accepted")
                {
                    if (u.isAdmin)
                    {
                        CurrentData.currentAdmin = new AdminBLL(u);
                        AdminHomePage1 = new AdminHomePage(CurrentData.currentAdmin);
                        NavigationService.Navigate(AdminHomePage1);
                    }

                    else
                    {
                        CurrentData.currentUser = u;
                        UserHomePage1 = new UserHomePage(CurrentData.currentUser);
                       
                        NavigationService.Navigate(UserHomePage1);
                    }
                }
                else if (u.status == "rejected")
                    MessageBox.Show("Your Account Creation demand has been rejected by the admin, Please try creating another account with valid information.");
                else
                    MessageBox.Show("Your Account Creation demand is still under review by the admin, Please try again later.");


            }
            else MessageBox.Show("invalide username or password");
           

        }
    }
}
