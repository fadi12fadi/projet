using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        
        public SignUp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String result = null;
            if (passwordBox.Password == confirmPasswordBox.Password &&
                !string.IsNullOrWhiteSpace(firstNameTextBox.Text) &&
                !string.IsNullOrWhiteSpace(lastNameTextBox.Text) &&
                !string.IsNullOrWhiteSpace(codeNINTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(usernameTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(passwordBox.Password) &&
                    !string.IsNullOrWhiteSpace(matriculeTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(emailTextBox.Text) )
            {
                //string firstName, string lastName, string codeNIN, string username, string password, string matricule, string email                
                AccountCreationBLL accountCreation = new AccountCreationBLL();
                // use accountCreation object
                result = AccountCreationBLL.CreateUserAccount(firstNameTextBox.Text, lastNameTextBox.Text, codeNINTextBox.Text,
                   usernameTextBox.Text, passwordBox.Password, matriculeTextBox.Text, emailTextBox.Text);
                MessageBox.Show(result);
                if (result == "Account created successfully")
                {
                    MainWindow.SignUp1 = new SignUp();
                    NavigationService.Navigate(MainWindow.SignUp1);
                }

            }
            
            
            else if (
                string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(codeNINTextBox.Text) ||
                   string.IsNullOrWhiteSpace(usernameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(passwordBox.Password) ||
                    string.IsNullOrWhiteSpace(matriculeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(emailTextBox.Text))
            { result =  "Please fill all the fields!";
                MessageBox.Show(result);
            
            }
            else
            {
                result =  "The password entered to confirm password is not matching!";
                MessageBox.Show(result);
          
            }


        }

       

        private void codeNINTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
        }
    }
}
