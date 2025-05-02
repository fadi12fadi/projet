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

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.UserPages
{
    /// <summary>
    /// Logique d'interaction pour HistorquePage.xaml
    /// </summary>
    public partial class HistorquePage : Page
    {
       
        public HistorquePage()
        {
            InitializeComponent();
          DateTime dateTime = DateTime.Now;

            // Create a BitmapImage
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(CurrentData.logoPath), UriKind.Absolute);
            bitmap.EndInit();

            // Set it to the logo
            logo.Source = bitmap;

            List<TheseBLL> theses = CurrentData.currentUser.getHistory();
            foreach (TheseBLL t in theses)
            {
                string title = t.titre;
                string date = t.datePub.ToString();
                string pdfpath = t.pdfPath;
                Canvas thesisCanvas = CreateThesisCanvas(title, date, pdfpath,t.these_id);
                historique_these_panel.Children.Add(thesisCanvas);
            }

        }
        private Canvas CreateThesisCanvas
            ( string title,string Date, string pdfpath, int id)
        {
            // Create the main Canvas - INCREASED HEIGHT
            Canvas mainCanvas = new Canvas
            {
                Height = 50,  // Increased from 136
                Background = Brushes.White,
                Margin = new Thickness(2, 2, 2, 2)// Increased margin

            };

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
                ElementName = "historique_these_panel",
                Mode = BindingMode.OneWay
            };
            grid.SetBinding(FrameworkElement.WidthProperty, gridWidthBinding);

            // Create StackPanel for author details - REPOSITIONED & ENLARGED
            StackPanel authorPanel = new StackPanel
            {
                Margin = new Thickness(10, 10, 60, 0),  // Adjusted position (from 100, 36, 60, 0)
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Height = 30,  // Increased from 20
                RenderTransformOrigin = new Point(0.512, 1.145)
            };

            // Add labels to StackPanel - FONT SIZE INCREASED
            string[] labelContents = { title, Date};
            foreach (string content in labelContents)
            {
                Label authorLabel = new Label
                {
                    Content = content,
                    FontSize = 16,  // Increased from 11
                    Padding = new Thickness(2, 2, 2, 2),  // Increased padding
                    Margin = new Thickness(0, 0, 5, 1),  // Increased right margin for spacing
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
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
                Margin = new Thickness(0, 2, 30, 0),  // Adjusted margin (from 0, 0, 18, 0)
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                CornerRadius = new CornerRadius(6),  // Increased from 4
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 45,  // Increased from 32
                Height = 20,  // Increased from 26
                VerticalAlignment = VerticalAlignment.Top
            };

            Button pdfButton = new Button
            {
                Content = "pdf",
                Background = null,
                FontSize = 12,  // Increased from 16
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Times New Roman"),
                BorderBrush = null,
                Foreground = new SolidColorBrush(Color.FromRgb(0xF7, 0xF7, 0xF7)),
                Tag = pdfpath
            };
            pdfBorder.Child = pdfButton;
            grid.Children.Add(pdfBorder);
            pdfButton.Click += clickpdf;
            // Create FAV button - ENLARGED & REPOSITIONED


            Border refuseBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Height = 20,  // Increased from 24
                Margin = new Thickness(0, 22, 30, 0),  // Adjusted position (from 0, 26, 20, 0)
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
                FontSize = 12,  // Increased from 15
                Background = null,
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = null,

                Padding = new Thickness(0)
            };
            refuseBorder.Child = refuse;
            grid.Children.Add(refuseBorder);
            refuse.Click += (s, e) =>
            {
                CurrentData.currentUser.deleteTheseFromHistoric(id);
                historique_these_panel.Children.Remove(mainCanvas);
            };


            // Add grid to Canvas
            mainCanvas.Children.Add(grid);

            return mainCanvas;
        }

       

        private void clickpdf(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string hello = button.Tag as string;
         
            OpenPdf(hello);
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
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1.UserHomePage1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FavoritePage());

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1= new Signin();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(UserHomePage.Newtheseuser1);
        }
    }
}
