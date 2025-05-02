using BusinessLogicLayer;
using PlatformeGestionThesesEtMemoiresWPF.Windows.AdminPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages
{
    /// <summary>
    /// Interaction logic for searshepage.xaml
    /// </summary>
    public partial class searshepage : Page
    {
        // declaration of the variables (canvasand canvas children)

        String title_searshe, keywords_searshe, author_searshe, faculte_searshe, university_searshe, domain_searshe, date_searshe;
        List<TheseBLL> thesesGlobal = new List<TheseBLL>();
        public searshepage()
        {
            InitializeComponent();
            // this is to get the txet on labels for shearse like the actual text in the title  label
            title_searshe = Title_searshe.Text;
            keywords_searshe = Keywords_searshe.Text;
            author_searshe = Author_searshe.Text;
            faculte_searshe = Faculte_searshe.Text;
            university_searshe = University_searshe.Text;
            domain_searshe = Domaine_searshe.Text;
            date_searshe = Date_searshe.Text;

            // Create a BitmapImage
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(CurrentData.logoPath), UriKind.Absolute);
            bitmap.EndInit();

            // Set it to the logo
            logo.Source = bitmap;

            // here we are checking if the user is logged in or not if not we set the login state to no login
            if (CurrentData.CurrentLoginState != CurrentData.LoginState.AdminLogin && CurrentData.CurrentLoginState != CurrentData.LoginState.UserLogin)
            {
                CurrentData.CurrentLoginState = CurrentData.LoginState.NoLogin;
                // we give the user without login a default user info
                CurrentData.currentUser = new UserBLL();
            }
                
            

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
                List<TheseBLL> thesesbyDmn = null;
                List<TheseBLL> thesesbyUni = null;
                List<TheseBLL> thesesbyFac = null;

                // You can add faculty, university, and date later too

                // Fetch data only for filled fields
                if ( Title_searshe.Text != "Title")
                    thesesbyTitle = CurrentData.currentUser.GetThesesByTitle(Title_searshe.Text);

                if (Author_searshe.Text!="Author")
                    thesesbyAuthor = CurrentData.currentUser.GetThesesByAuthor(Author_searshe.Text);

                if (Keywords_searshe.Text != "key words")
                    thesesbyKws = CurrentData.currentUser.GetThesesByKeywords(Keywords_searshe.Text);

                if (Domaine_searshe.Text != "Domain")
                    thesesbyDmn = CurrentData.currentUser.GetThesesByDomains(Domaine_searshe.Text);

                if (University_searshe.Text != "University")
                    thesesbyUni = CurrentData.currentUser.GetThesesByUniversity(University_searshe.Text);
                
                if (Faculte_searshe.Text != "Faculty")
                    thesesbyFac = CurrentData.currentUser.GetThesesByFaculty(Faculte_searshe.Text);
                
                //if (Date_searshe. != "Domain")
                //    thesesbyDmn = userBLLmain.GetThesesByDomains(Domaine_searshe.Text);


                // Start combining using AND logic
                List<TheseBLL> result = null;

                if (thesesbyTitle != null)
                    result = result == null ? thesesbyTitle : result.Intersect(thesesbyTitle, new TheseComparer()).ToList();
                result = thesesbyTitle;

                if (thesesbyAuthor != null)
                    result = result == null ? thesesbyAuthor : result.Intersect(thesesbyAuthor, new TheseComparer()).ToList();

                if (thesesbyKws != null)
                    result = result == null ? thesesbyKws : result.Intersect(thesesbyKws, new TheseComparer()).ToList();

                if (thesesbyDmn != null)
                    result = result == null ? thesesbyDmn : result.Intersect(thesesbyDmn, new TheseComparer()).ToList();
                
                if (thesesbyFac != null)
                    result = result == null ? thesesbyFac : result.Intersect(thesesbyFac, new TheseComparer()).ToList();

                if (thesesbyUni != null)
                    result = result == null ? thesesbyUni : result.Intersect(thesesbyUni, new TheseComparer()).ToList();

                //if (thesesbyDmn != null)
                //    result = result == null ? thesesbyDmn : result.Intersect(thesesbyDmn, new TheseComparer()).ToList();



                // If no field was filled, return empty
                if (result == null)
                    result = new List<TheseBLL>();

                else
                {
                    

                    for (int i = 0; i < result.Count ; i++)
                    {
                        stack_panel_these.Children.Add(CreateThesisCanvas(result[i].imagePath,
                            "Title : " +
                            result[i].titre,
                            result[i].description,
                            "Sub Title : " + result[i].sousTitre,
                            "Authore Name : " + result[i].author.FirstName + " " + result[i].author.LastName,
                            "University : " + result[i].university,
                            "Domaine : " + result[i].domain,
                            "Faculty : " + result[i].faculty,
                            "Date : " + result[i].datePub, result[i].pdfPath));
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


        // this is the creation of the canvas dunamiclly and the filling of the data in it
        public Canvas CreateThesisCanvas(string imagepath, string title, string descreptiontxt, string subtitle,
        string authname, string authuniversity, string authdomaine, string authfaculy,
        string authdate, string pdfpath)
        {
            // Create the main Canvas
            Canvas mainCanvas = new Canvas
            {
                Height = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(1, 2, 1, 2),
                Background = Brushes.White

            };

            // Create ScrollViewer
            ScrollViewer scrollViewer = new ScrollViewer
            {
                Height = 100,
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

            // Create Grid
            Grid grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(-0.2, 0, 0, 2)
            };
            // Set binding for Width on Grid
            Binding gridWidthBinding = new Binding("ActualWidth")
            {
                ElementName = "stack_panel_these",
                Mode = BindingMode.OneWay
            };
            grid.SetBinding(FrameworkElement.WidthProperty, gridWidthBinding);

            // Update the Image creation code to properly convert the string path to an ImageSource
            Image thesisImage = new Image
            {
                Margin = new Thickness(2, 1.6, 0, 0),
                Height = 95,
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransformOrigin = new Point(0.5, 0.5),
                Source = new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute))

            };
            grid.Children.Add(thesisImage);

            // Create and add Title Label
            Label titleLabel = new Label
            {
                Content = title ,
                Margin = new Thickness(100, 0, 35, 0),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(2, 1, 1, 1),
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Times New Roman"),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D"))
            };
            titleLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
            grid.Children.Add(titleLabel);

            // Create and add Description TextBlock
            TextBlock descriptionBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(100, 61, 60, 8),
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(Color.FromRgb(0xF1, 0xFF, 0xFC)),
                FontFamily = new FontFamily("Times New Roman"),
                Padding = new Thickness(5.8, 2, 0, 0),
                Text = descreptiontxt
            };
            grid.Children.Add(descriptionBlock);

            // Create and add Subtitle Label
            Label subtitleLabel = new Label
            {
                Content = subtitle,
                Margin = new Thickness(124, 18, 36, 0),
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(2, 1, 1, 1),
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontFamily = new FontFamily("Times New Roman")
            };
            subtitleLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
            grid.Children.Add(subtitleLabel);

            // Create StackPanel for author details
            StackPanel authorPanel = new StackPanel
            {
                Margin = new Thickness(100, 36, 35, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Height = 20,
                RenderTransformOrigin = new Point(0.512, 1.145)
            };

            // Add labels to StackPanel
            string[] labelContents = { authname, authuniversity, authdomaine, authfaculy, authdate };
            foreach (string content in labelContents)
            {
                Label authorLabel = new Label
                {
                    Content = content,
                    FontSize = 11,
                    Padding = new Thickness(1, 1, 1, 1),
                    Margin = new Thickness(2, 0, 2, 1),
                    Foreground = Brushes.Gray,
                    FontFamily = new FontFamily("Times New Roman"),
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0, 0, 1, 0),
                    BorderBrush= Brushes.Black
                };
                authorLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(0x4D, 0, 0, 0)); // Opacity 0.3
                authorPanel.Children.Add(authorLabel);
            }
            grid.Children.Add(authorPanel);

            // Create and add PDF Button with Border
            Border pdfBorder = new Border
            {
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 18, 80),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                CornerRadius = new CornerRadius(4),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 32
            };

            Button pdfButton = new Button
            {
                Content = "pdf",
                Background = null,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                BorderBrush = null,
                Foreground = new SolidColorBrush(Color.FromRgb(0xF7, 0xF7, 0xF7)),
                Tag = pdfpath,
            };
            pdfButton.Click += MyButtonClickHandler;
            pdfBorder.Child = pdfButton;
            grid.Children.Add(pdfBorder);

            // Add Grid to ScrollViewer
            scrollViewer.Content = grid;

            // Add ScrollViewer to Canvas
            mainCanvas.Children.Add(scrollViewer);

            return mainCanvas;
        }

        //click button creation event
        private void MyButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            OpenPdf(button.Tag as string);
        }
        // to open pdf file in these
        private void OpenPdf(string pdfPath)
        {
            
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            
        }



        private void reset(object sender, RoutedEventArgs e)
        {
            Title_searshe.Text = "Title";
            Author_searshe.Text = "Author";
            Faculte_searshe.Text = "Faculty";
            University_searshe.Text = "University";
            Keywords_searshe.Text = "key words";
            Domaine_searshe.Text = "Domain";
        }
        

        // Gestionnaires d'événements pour le focus

        // domain
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

        // author
        private void author_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Author")
            {
                textBox.Text = "";
            }
        }

        private void author_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Author";
            }
        }

        private void Date_searshe_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "YYYY")
            {
                textBox.Text = "";
            }
        }

        private void Date_searshe_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "YYYY";
            }
        }

        private void Date_searshe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
        }





        // sous titre
        private void sous_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "key words")
            {
                textBox.Text = "";
            }
        }

        private void sous_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "key words";
            }
        }


        //university
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

        // to go the Signin1page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1);

        }
        // to go the SignUp1page

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.SignUp1);
        }
    }
}
