using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages;

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.UserPages
{
    /// <summary>
    /// Logique d'interaction pour Newtheseuser.xaml
    /// </summary>
    public partial class Newtheseuser : Page
    {
        // Gemini API key - this is your Google Gemini API key
        private const string GeminiApiKey = "AIzaSyBQRsb28aEAjnaItXtzkOWFQTQyy7Y9wuo";
        private GeminiService _geminiService;
        
        public Newtheseuser()
        {
            InitializeComponent();
            // Create a BitmapImage
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(CurrentData.logoPath), UriKind.Absolute);
            bitmap.EndInit();

            // Set it to the logo
            logo.Source = bitmap;
            
            // Initialize the Gemini service
            _geminiService = new GeminiService(GeminiApiKey);
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
        // ADDING PDF
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
        //ADDING IMAGE
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           MainWindow.Signin1 = new Signin();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistorquePage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FavoritePage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1.UserHomePage1);
        }
        // THE FOLLOWING EVENT TO ADD TEHSE


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DateTime x;
            if (these_date.SelectedDate == null)
                x = default;
            else
                x = these_date.SelectedDate.Value;

            AuthorBLL these_author1 = null;
            if (these_authorename.Text != "" && these_authorelastname.Text != "")
                these_author1 = new AuthorBLL(these_authorename.Text, these_authorelastname.Text, these_authormail.Text);
            
            ProfessorBLL these_professor1 = null;
            if (these_profname.Text != "" && these_proflastname.Text != "")
                these_professor1 = new ProfessorBLL(these_profname.Text, these_proflastname.Text);

            List<AuthorBLL> L = null;
            if (these_author1 != null)
                L = new List<AuthorBLL> { these_author1 };

            string result = CurrentData.currentUser.proposeTheseToAdmin(
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
                UserHomePage.Newtheseuser1 = new Newtheseuser();
                NavigationService.Navigate(UserHomePage.Newtheseuser1);
            }
        }

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        // Auto-Fill button click event handler
        private async void Button_Click_AutoFill(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a PDF has been selected
                if (string.IsNullOrEmpty(PdfPathTextBox.Text))
                {
                    MessageBox.Show("Please select a PDF file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Show loading indicator or disable controls
                this.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;
                
                try
                {
                    // Extract text from the first page of the PDF
                    string pdfText = _geminiService.ExtractTextFromFirstPage(PdfPathTextBox.Text);
                    
                    if (string.IsNullOrWhiteSpace(pdfText))
                    {
                        MessageBox.Show("No text could be extracted from the first page of the PDF.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Use Gemini to analyze the text and extract thesis information
                    ThesisInfo thesisInfo = await _geminiService.AnalyzeTextWithGeminiAsync(pdfText);
                    
                    if (thesisInfo != null)
                    {
                        // Fill the form fields with the extracted information
                        these_title.Text = thesisInfo.Title ?? "";
                        these_subtitle.Text = thesisInfo.Subtitle ?? "";
                        these_domain.Text = thesisInfo.Domain ?? "";
                        these_university.Text = thesisInfo.University ?? "";
                        these_faculty.Text = thesisInfo.Faculty ?? "";
                        
                        // Try to parse the date if available
                        if (!string.IsNullOrEmpty(thesisInfo.PublicationDate))
                        {
                            if (DateTime.TryParse(thesisInfo.PublicationDate, out DateTime date))
                            {
                                these_date.SelectedDate = date;
                            }
                        }
                        
                        these_authorename.Text = thesisInfo.AuthorFirstName ?? "";
                        these_authorelastname.Text = thesisInfo.AuthorLastName ?? "";
                        these_profname.Text = thesisInfo.ProfessorFirstName ?? "";
                        these_proflastname.Text = thesisInfo.ProfessorLastName ?? "";
                        these_keyword.Text = thesisInfo.Keywords ?? "";
                        these_authormail.Text = thesisInfo.AuthorEmail ?? "";
                        these_description.Text = thesisInfo.Description ?? "";
                        
                        MessageBox.Show("Successfully extracted information from the thesis PDF.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Could not extract thesis information from the PDF.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Restore UI state
                    this.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
