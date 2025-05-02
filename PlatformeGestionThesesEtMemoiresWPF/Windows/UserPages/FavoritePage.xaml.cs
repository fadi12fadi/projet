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

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.UserPages
{
    /// <summary>
    /// Logique d'interaction pour FavoritePage.xaml
    /// </summary>
    public partial class FavoritePage : Page
    {
        
        public FavoritePage()
        {
            
            InitializeComponent();
            List<TheseBLL> thesesfav = null;
            // Create a BitmapImage
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(CurrentData.logoPath), UriKind.Absolute);
            bitmap.EndInit();

            // Set it to the logo
            logo.Source = bitmap;
            try
            {

                thesesfav = CurrentData.currentUser.getFavorites();
                
                for (int i = 0 ; i < thesesfav.Count; i++)
                {
                    favorite_these_panel.Children.Add(
                    CreateThesisCanvas(thesesfav[i].imagePath, thesesfav[i].titre, thesesfav[i].description, thesesfav[i].sousTitre,
                        thesesfav[i].author.FirstName + thesesfav[i].author.LastName, thesesfav[i].university, thesesfav[i].domain, thesesfav[i].faculty,
                        thesesfav[i].datePub.ToString(), thesesfav[i].prof.nom, thesesfav[i].author.Email, thesesfav[i].pdfPath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading theses: "  , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }
        private Canvas CreateThesisCanvas(string imagepath, string title, string descreptiontxt, string subtitle,
            string authname, string authuniversity, string authdomaine, string authfaculy,
            string authdate, string profname, string authmail, string pdfpath)
        {
            // Create the main Canvas - INCREASED HEIGHT
            //button color : #FF314E3D
            //button hover color: #1E232A
            Canvas mainCanvas = new Canvas
            {
                Height = 200,  // Increased from 136
                Background = Brushes.White,
                Margin = new Thickness(2, 2, 2, 2)// Increased margin

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
                ElementName = "favorite_these_panel",
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
                ElementName = "favorite_these_panel",
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
                Height = 35,  // Increased from 26
                VerticalAlignment = VerticalAlignment.Top
            };

            Button pdfButton = new Button
            {
                Content = "pdf",
                Background = null,
                FontSize = 18,  // Increased from 16
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                BorderBrush = null,
                Foreground = new SolidColorBrush(Color.FromRgb(0xF7, 0xF7, 0xF7)),
                Tag = pdfpath,  // Set the Tag property to the PDF path
            };
            pdfBorder.Child = pdfButton;
            grid.Children.Add(pdfBorder);
            //pdfButton.Click += MyButtonClickHandler;

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

            // Create FAV button - ENLARGED & REPOSITIONED


            Border refuseBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Height = 35,  // Increased from 24
                Margin = new Thickness(0, 55, 30, 0),  // Adjusted position (from 0, 26, 20, 0)
                VerticalAlignment = VerticalAlignment.Top,
                Width = 45,  // Increased from 30
                CornerRadius = new CornerRadius(6),  // Increased from 4
                Background = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button refuse = new Button
            {
                Content = "delete",
                FontFamily = new FontFamily("Times New Roman"),
                FontWeight = FontWeights.Bold,
                FontSize = 14,  // Increased from 15
                Background = null,
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = null,

                Padding = new Thickness(0)
            };
            refuseBorder.Child = refuse;
            grid.Children.Add(refuseBorder);

            // Add Grid to ScrollViewer
            scrollViewer.Content = grid;

            // Add ScrollViewer to Canvas
            mainCanvas.Children.Add(scrollViewer);

            return mainCanvas;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1 = new Signin();
            MainWindow.SignUp1 = new SignUp();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1.UserHomePage1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistorquePage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(UserHomePage.Newtheseuser1);
        }
    }
}
