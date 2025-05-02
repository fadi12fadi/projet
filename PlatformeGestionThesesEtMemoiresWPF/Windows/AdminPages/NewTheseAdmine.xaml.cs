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
using PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages;

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.AdminPages
{
    /// <summary>  
    /// Logique d'interaction pour NewTheseAdmine.xaml  
    /// </summary>  
    public partial class NewTheseAdmine : Page
    {

        public NewTheseAdmine()
        {
            
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1.AdminHomePage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.NewAcountsPage1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.Theses1);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Word1/word2/word3/....")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Word1/word2/word3/....";
            }
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Path(../../)")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Path(../../)";
            }
        }
        
        
        

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1 = new Signin();
            MainWindow.SignUp1 = new SignUp();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Select a PDF File";
            dialog.Filter = "PDF files (*.pdf)|*.pdf";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                PdfPathTextBox.Text = dialog.FileName;
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Select an Image File";
            dialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files (*.*)|*.*";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                ImagePathTextBox.Text = dialog.FileName;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DateTime x;
            if (these_date.SelectedDate == null)
            {
                x = default;
                
            }
            else
            {
                x = these_date.SelectedDate.Value;
                
            }

            AuthorBLL these_author1 = new AuthorBLL(these_authorename.Text, these_authorelastname.Text, these_authormail.Text);
            ProfessorBLL these_professor1 = new ProfessorBLL(these_profname.Text,these_proflastname.Text);
            
            List<AuthorBLL> L = new List<AuthorBLL> { these_author1 };
          string result = CurrentData.currentAdmin.addTheseManually(
                these_title.Text,
                these_subtitle.Text,
                these_faculty.Text,
                PdfPathTextBox.Text,
                these_description.Text,
                these_keyword.Text,
                these_domain.Text,
                these_university.Text,               
                ImagePathTextBox.Text,
                x,
                L,
                these_professor1);

            MessageBox.Show(result);
           
            if (result == "Thesis added successfully!")
            {
                AdminHomePage.NewTheseAdmine1 = new NewTheseAdmine();
                NavigationService.Navigate(AdminHomePage.NewTheseAdmine1);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.Admintheses1);
        }
    }
}
