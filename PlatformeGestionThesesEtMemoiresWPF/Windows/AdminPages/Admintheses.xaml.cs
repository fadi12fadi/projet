using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour Admintheses.xaml
    /// </summary>
    public partial class Admintheses : Page
    {
        

       

        String title_searshe, keywords_searshe, author_searshe, faculte_searshe, university_searshe, domain_searshe, date_searshe;

        public Admintheses()
        {
            InitializeComponent();
           
            title_searshe = Title_searshe.Text;
            keywords_searshe = Keywords_searshe.Text;
            author_searshe = Author_searshe.Text;
            faculte_searshe = Faculte_searshe.Text;
            university_searshe = University_searshe.Text;
            domain_searshe = Domaine_searshe.Text;
            date_searshe = Date_searshe.Text;


        }

        public class TheseComparer : IEqualityComparer<TheseBLL>
        {
            public bool Equals(TheseBLL x, TheseBLL y)
            {
                return x?.these_id == y?.these_id; // Use whatever uniquely identifies a thesis
            }

            public int GetHashCode(TheseBLL obj)
            {
                return obj.these_id.GetHashCode();
            }
        }

        private void search(object sender, RoutedEventArgs e)
        {
            // check if user input is the same as before (no change)
            if (Title_searshe.Text == title_searshe
                && Keywords_searshe.Text == keywords_searshe
                && Author_searshe.Text == author_searshe
                && Faculte_searshe.Text == faculte_searshe
                && University_searshe.Text == university_searshe
                && Domaine_searshe.Text == domain_searshe
                && Date_searshe.Text == date_searshe)
            {
                // No changes in input → skip search
            }
            else
            {
                // Clear previous results
                if (stack_panel_these.Children.Count > 0)
                {
                    stack_panel_these.Children.Clear();
                }

                // Declare lists for each search field
                List<TheseBLL> thesesbyTitle = null;
                List<TheseBLL> thesesbyAuthor = null;
                List<TheseBLL> thesesbyKws = null;
                List<TheseBLL> thesesbyDms = null;
                List<TheseBLL> thesesbyUni = null;
                List<TheseBLL> thesesbyFac = null;
                List<TheseBLL> thesesbyYearGraduation = null;

                // You can add faculty, university, and date later too

                // Fetch data only for filled fields
                if (Title_searshe.Text != "Title")
                    thesesbyTitle = CurrentData.currentAdmin.GetThesesByTitle(Title_searshe.Text);

                if (Author_searshe.Text != "Author")
                    thesesbyAuthor = CurrentData.currentAdmin.GetThesesByAuthor(Author_searshe.Text);

                if (Keywords_searshe.Text != "key words")
                    thesesbyKws = CurrentData.currentAdmin.GetThesesByKeywords(Keywords_searshe.Text);

                if (Domaine_searshe.Text != "Domain")
                    thesesbyDms = CurrentData.currentAdmin.GetThesesByDomains(Domaine_searshe.Text);

                if (University_searshe.Text != "University")
                    thesesbyUni = CurrentData.currentUser.GetThesesByUniversity(University_searshe.Text);

                if (Faculte_searshe.Text != "Faculty")
                    thesesbyFac = CurrentData.currentUser.GetThesesByFaculty(Faculte_searshe.Text);

                if (Date_searshe.Text != "YYYY")
                    thesesbyYearGraduation = CurrentData.currentUser.GetThesesByGraduationYear(Date_searshe.Text);

                // Start combining using AND logic
                List<TheseBLL> result = null;

                if (thesesbyTitle != null)
                    result = result == null ? thesesbyTitle : result.Intersect(thesesbyTitle, new TheseComparer()).ToList();
                result = thesesbyTitle;

                if (thesesbyAuthor != null)
                    result = result == null ? thesesbyAuthor : result.Intersect(thesesbyAuthor, new TheseComparer()).ToList();

                if (thesesbyKws != null)
                    result = result == null ? thesesbyKws : result.Intersect(thesesbyKws, new TheseComparer()).ToList();

                if (thesesbyDms != null)
                    result = result == null ? thesesbyDms : result.Intersect(thesesbyDms, new TheseComparer()).ToList();

                if (thesesbyFac != null)
                    result = result == null ? thesesbyFac : result.Intersect(thesesbyFac, new TheseComparer()).ToList();

                if (thesesbyUni != null)
                    result = result == null ? thesesbyUni : result.Intersect(thesesbyUni, new TheseComparer()).ToList();

                if (thesesbyYearGraduation != null)
                    result = result == null ? thesesbyYearGraduation : result.Intersect(thesesbyYearGraduation, new TheseComparer()).ToList();
                
                // If no field was filled, return empty or maybe fetch all (your choice)
                if (result == null)
                    result = new List<TheseBLL>();

                else
                {


                    for (int i = 0; i < result.Count; i++)
                    {
                        stack_panel_these.Children.Add(CreateThesisCanvas(result[i].imagePath,
                            result[i].titre,
                            result[i].description,
                            result[i].sousTitre,
                            result[i].author.FirstName + " " + result[i].author.LastName,
                            result[i].university,
                            result[i].domain,
                            result[i].faculty,
                            result[i].datePub.ToString(), result[i].prof.nom, result[i].author.Email, result[i].pdfPath, result[i]));
                    }
                }

                // Save search inputs
                title_searshe = Title_searshe.Text;
                keywords_searshe = Keywords_searshe.Text;
                author_searshe = Author_searshe.Text;
                faculte_searshe = Faculte_searshe.Text;
                university_searshe = University_searshe.Text;
                domain_searshe = Domaine_searshe.Text;
                date_searshe = Date_searshe.Text;
            }
        }
        // Method to create the thesis canvas dynamically
        private Canvas CreateThesisCanvas(string imagepath, string title, string descreptiontxt, string subtitle,
            string authname, string authuniversity, string authdomaine, string authfaculy,
            string authdate, string profname, string authmail, string pdfpath, TheseBLL thesefav)
        {
            // Create the main Canvas - INCREASED HEIGHT
            //button color : #FF314E3D
            //button hover color: #1E232A
            Canvas mainCanvas = new Canvas
            {
                Height = 200,  // Increased from 136
                Background = Brushes.White,
                Margin = new Thickness(1, 2, 1, 2)// Increased margin

            };

            // Create ScrollViewer - INCREASED HEIGHT
            ScrollViewer scrollViewer = new ScrollViewer
            {
                Height = 200,  // Increased from 100
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top

            };
            // Set binding for Width
            Binding widthBinding = new Binding("ActualWidth")
            {
                ElementName = "stack_panel_these",
                Mode = BindingMode.OneWay
            };
            scrollViewer.SetBinding(FrameworkElement.WidthProperty, widthBinding);

            // Create Grid - ADDED HEIGHT and PADDING
            Grid grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 5, 5),  // Increased margin for more space
                Height = 400  // Added explicit height
            };
            // Set binding for Width on Grid
            Binding gridWidthBinding = new Binding("ActualWidth")
            {
                ElementName = "stack_panel_these",
                Mode = BindingMode.OneWay
            };
            grid.SetBinding(FrameworkElement.WidthProperty, gridWidthBinding);

            // Create and add Image - INCREASED SIZE
            Image thesisImage = new Image
            {
                Margin = new Thickness(10, 10, 0, 0),  // Increased margin
                Height = 140,  // Increased from 95
                Width = 120,   // Increased from 80
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransformOrigin = new Point(0.5, 0.5),
                Source = imagepath != null ? new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute)) : null,
            };
            grid.Children.Add(thesisImage);

            // Create and add Title Label - INCREASED SIZE AND MARGIN
            Label titleLabel = new Label
            {
                Content = "Title :" + title,
                Margin = new Thickness(150, 10, 35, 0),  // Adjusted to accommodate larger image
                FontSize = 20,  // Increased from 14
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(5, 3, 3, 3),  // Increased padding
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Times New Roman"),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D"))
            };
            titleLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
            grid.Children.Add(titleLabel);

            // Create and add Description TextBlock - REPOSITIONED & ENLARGED
            TextBlock descriptionBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(150, 120, 60, 0),  // Adjusted position (from 100, 69, 60, 0)
                FontSize = 14,  // Increased from 10
                Background = new SolidColorBrush(Color.FromRgb(0xF1, 0xFF, 0xFC)),
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(8, 5, 5, 5),  // Increased padding
                Text = "Descreption :" + descreptiontxt
            };
            grid.Children.Add(descriptionBlock);

            // Create and add Subtitle Label - REPOSITIONED & ENLARGED
            Label subtitleLabel = new Label
            {
                Content = "Subtitle : " + subtitle,
                Margin = new Thickness(174, 45, 50, 0),  // Adjusted position (from 124, 18, 50, 0)
                FontWeight = FontWeights.Bold,
                FontSize = 16,  // Added font size
                Padding = new Thickness(5, 3, 3, 3),  // Increased padding
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontFamily = new FontFamily("Times New Roman")
            };
            subtitleLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
            grid.Children.Add(subtitleLabel);

            // Create StackPanel for author details - REPOSITIONED & ENLARGED
            StackPanel authorPanel = new StackPanel
            {
                Margin = new Thickness(150, 75, 60, 0),  // Adjusted position (from 100, 36, 60, 0)
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Height = 30,  // Increased from 20
                RenderTransformOrigin = new Point(0.512, 1.145)
            };

            // Add labels to StackPanel - FONT SIZE INCREASED
            string[] labelContents = { "Authore name : " + authname, "University : " + authuniversity, "date : " + authdate };
            foreach (string content in labelContents)
            {
                Label authorLabel = new Label
                {
                    Content = content,
                    FontSize = 14,  // Increased from 11
                    Padding = new Thickness(2, 2, 2, 2),  // Increased padding
                    Margin = new Thickness(0, 0, 5, 1),  // Increased right margin for spacing
                    Foreground = Brushes.Gray,
                    FontFamily = new FontFamily("Times New Roman"),
                    FontWeight = FontWeights.Bold
                };
                authorLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
                authorPanel.Children.Add(authorLabel);
            }
            grid.Children.Add(authorPanel);

            // Create and add PDF Button with Border - ENLARGED & REPOSITIONED
            Border pdfBorder = new Border
            {
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 10, 30, 0),  // Adjusted margin (from 0, 0, 18, 0)
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                CornerRadius = new CornerRadius(6),  // Increased from 4
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 45,  // Increased from 32
                Height = 30,  // Increased from 26
                VerticalAlignment = VerticalAlignment.Top
            };

            Button pdfButton = new Button
            {
                Content = "pdf",
                Background = null,
                FontSize = 16,  // Increased from 16
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                BorderBrush = null,
                Foreground = new SolidColorBrush(Color.FromRgb(0xF7, 0xF7, 0xF7)),
                Tag = thesefav,  // Set the Tag property to the PDF path
            };
            pdfBorder.Child = pdfButton;
            grid.Children.Add(pdfBorder);
            pdfButton.Click += MyButtonClickHandler;

            // Create email StackPanel - REPOSITIONED & RESIZED
            StackPanel emailPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 25,  // Increased from 16
                Margin = new Thickness(150, 100, 0, 0),  // Adjusted position (from 100, 53, 0, 0)
                VerticalAlignment = VerticalAlignment.Top,
                // Increased from 428
                Orientation = Orientation.Horizontal
            };

            // Add email labels - ENLARGED


            Label domainLabel = new Label
            {
                Content = "Domain : " + authdomaine,
                FontSize = 14,  // Increased from 11
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(3, 2, 3, 2),  // Increased padding
                Foreground = Brushes.Gray,
                Margin = new Thickness(10, 0, 0, 0)  // Added left margin for spacing
            };
            emailPanel.Children.Add(domainLabel);


            Label facultyLabel = new Label
            {
                Content = "Faculty : " + authfaculy,
                FontSize = 14,  // Increased from 11
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(3, 2, 3, 2),  // Increased padding
                Foreground = Brushes.Gray,
                Margin = new Thickness(10, 0, 0, 0)  // Added left margin for spacing
            };
            emailPanel.Children.Add(facultyLabel);


            Label profLabel = new Label
            {
                Content = "prof : " + profname,
                FontSize = 14,  // Increased from 11
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(3, 2, 3, 2),  // Increased padding
                Foreground = Brushes.Gray,
                Margin = new Thickness(10, 0, 0, 0)  // Added left margin for spacing
            };
            emailPanel.Children.Add(profLabel);
            Label authorEmailLabel = new Label
            {
                Content = "AuthMail : " + authmail,
                FontSize = 14,  // Increased from 11
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(3, 2, 3, 2),  // Increased padding
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            emailPanel.Children.Add(authorEmailLabel);

            grid.Children.Add(emailPanel);

            // Create FAV button - ENLARGED & REPOSITIONED
            Border favBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Height = 30,  // Increased from 24
                Margin = new Thickness(0, 45, 30, 0),  // Adjusted position (from 0, 26, 20, 0)
                VerticalAlignment = VerticalAlignment.Top,
                Width = 45,  // Increased from 30
                CornerRadius = new CornerRadius(6),  // Increased from 4
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button favButton = new Button
            {
                Content = "FAV",
                FontFamily = new FontFamily("Times New Roman"),
                FontWeight = FontWeights.Bold,
                FontSize = 16,  // Increased from 15
                Background = null,
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = null,
                Padding = new Thickness(0),
                Tag = thesefav
            };
            favBorder.Child = favButton;
            grid.Children.Add(favBorder);
            favButton.Click += Favclick;
            // Add Grid to ScrollViewer
            scrollViewer.Content = grid;


            // Add ScrollViewer to Canvas
            mainCanvas.Children.Add(scrollViewer);

            return mainCanvas;
        }
        private void MyButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TheseBLL hello = button.Tag as TheseBLL;
            CurrentData.currentAdmin.putTheseInHistoric(hello.these_id);
            OpenPdf(hello.pdfPath);
        }
        private void Favclick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            TheseBLL heloo = button.Tag as TheseBLL;
            CurrentData.currentAdmin.setTheseAsFavorite(heloo.these_id);
            MessageBox.Show("Added to favorites");


        }
        //to open pdf file in these
        private void OpenPdf(string pdfPath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });

            }
            catch (Exception ex)
            {

            }
        }

        private void author_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Author_searshe.Text == "Author")
            {
                Author_searshe.Text = "";
            }
        }

        private void author_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Author_searshe.Text == "")
            {
                Author_searshe.Text = "Author";
            }
        }

        private void sous_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Keywords_searshe.Text == "key words")
            {
                Keywords_searshe.Text = "";
            }
        }

        private void sous_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Keywords_searshe.Text == "")
            {
                Keywords_searshe.Text = "key words";
            }

        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Domain")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Domain";
            }
        }

        //titre
        private void Titre_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Title")
            {
                textBox.Text = "";
            }
        }

        private void Titre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Title";
            }
        }

        // faculte
        private void faculte_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Faculty")
            {
                textBox.Text = "";
            }
        }

        private void faculte_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Faculty";
            }
        }
        private void university_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "University")
            {
                textBox.Text = "";
            }
        }

        private void university_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "University";
            }
        }


        private void reset(object sender, RoutedEventArgs e)
        {
            // search 
            //String[] data = { "ali", "belal", "ahmed" };

        }
        private void Button_Click0(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(MainWindow.Signin1.AdminHomePage1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.NewAcountsPage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.Theses1);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.NewTheseAdmine1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1 = new Signin();
            NavigationService.Navigate(MainWindow.searshepage1);
        }
    }
}
