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
    /// Logique d'interaction pour Page2.xaml
    /// </summary>
    public partial class NewAcountsPage : Page
    {

        public NewAcountsPage( )
        {
            InitializeComponent();
            List<UserBLL> users = AdminBLL.getWaitingAccounts();
            if (users_demand_panel.Children.Count < users.Count) // this means the ui is not updated
            {
                users_demand_panel.Children.Clear();

                // the counter i starts from the number of children in the panel so that we don't clear and add the same users again
                for (int i = users_demand_panel.Children.Count; i < AdminBLL.getWaitingAccounts().Count; i++)
                {
                    users_demand_panel.Children.Add(CreateUserCard(users[i].matricule, users[i].firstName + users[i].lastName, users[i].username,
                        users[i].email, "C:\\Users\\tassili\\Downloads\\blank-profile-picture-973460_1280.webp"));
                }
            }

        }
        
        private Canvas CreateUserCard(string matricule, string fullName, string username, string email, string imagePath)
        {
            // Main canvas - using Auto for width to allow resizing
            Canvas userCanvas = new Canvas
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1E8D6")),
                Margin = new Thickness(2, 2, 2, 0),
                Height = 80
            };

            // Main horizontal stack panel - setting HorizontalAlignment to Stretch
            StackPanel mainStack = new StackPanel
            {
                Height = 80,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Using DockPanel for better layout control and responsiveness
            DockPanel containerPanel = new DockPanel
            {
                LastChildFill = true,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // User image with fixed size (avatars are usually fixed size)
            Image userImage = new Image
            {
                Width = 73,
                Height = 78,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(5, 0, 10, 0)
            };
            DockPanel.SetDock(userImage, Dock.Left);

            // Set image source if provided
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                    userImage.Source = bitmap;
                }
                catch
                {
                    // Silently handle image loading errors
                }
            }

            // Create Grid to hold the user information
            Grid infoGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            DockPanel.SetDock(infoGrid, Dock.Left);

            // Define grid columns with Star sizing for responsiveness
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Matricule stack
            StackPanel matriculeStack = new StackPanel
            {
                Margin = new Thickness(5, 0, 5, 0)
            };
            Label matriculeLabel = new Label
            {
                Content = "*Matricule :",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontSize = 14,
                Height = 40
            };

            Label matriculeValue = new Label
            {
                Content = matricule,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gray,
                Height = 40
            };

            matriculeStack.Children.Add(matriculeLabel);
            matriculeStack.Children.Add(matriculeValue);
            Grid.SetColumn(matriculeStack, 0);

            // Name stack
            StackPanel nameStack = new StackPanel
            {
                Margin = new Thickness(5, 0, 5, 0)
            };
            Label nameLabel = new Label
            {
                Content = "*FName LName :",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontSize = 14,
                Height = 40
            };

            Label nameValue = new Label
            {
                Content = fullName,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gray,
                Height = 40
            };

            nameStack.Children.Add(nameLabel);
            nameStack.Children.Add(nameValue);
            Grid.SetColumn(nameStack, 1);

            // Username stack
            StackPanel usernameStack = new StackPanel
            {
                Margin = new Thickness(5, 0, 5, 0)
            };
            Label usernameLabel = new Label
            {
                Content = "*Username :",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontSize = 14,
                Height = 40
            };

            Label usernameValue = new Label
            {
                Content = username,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gray,
                Height = 40
            };

            usernameStack.Children.Add(usernameLabel);
            usernameStack.Children.Add(usernameValue);
            Grid.SetColumn(usernameStack, 2);

            // Email stack
            StackPanel emailStack = new StackPanel
            {
                Margin = new Thickness(5, 0, 5, 0)
            };
            Label emailLabel = new Label
            {
                Content = "*mail :",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF314E3D")),
                FontSize = 14,
                Height = 40
            };

            Label emailValue = new Label
            {
                Content = email,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gray,
                Height = 40
            };

            emailStack.Children.Add(emailLabel);
            emailStack.Children.Add(emailValue);
            Grid.SetColumn(emailStack, 3);

            // Add information stacks to grid
            infoGrid.Children.Add(matriculeStack);
            infoGrid.Children.Add(nameStack);
            infoGrid.Children.Add(usernameStack);
            infoGrid.Children.Add(emailStack);

            // Button stack - fixed width for buttons
            StackPanel buttonStack = new StackPanel
            {
                Width = 70,
                Margin = new Thickness(10, 0, 5, 0)
            };
            DockPanel.SetDock(buttonStack, Dock.Right);

            Button acceptButton = new Button
            {
                Content = "accepte",
                Height = 36,
                Margin = new Thickness(0, 0, 0, 4),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF207729")),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4EFF00")),
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = Brushes.White
            };

            Button rejectButton = new Button
            {
                Content = "refuse",
                Height = 36,
                Background = Brushes.Red,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF920000")),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };

            // Add click handlers for buttons
            acceptButton.Click += (s, e) => {
                MessageBox.Show(CurrentData.currentAdmin.acceptUserDemand(username));

                users_demand_panel.Children.Remove(userCanvas);
            };

            rejectButton.Click += (s, e) => {
                MessageBox.Show(CurrentData.currentAdmin.refuseUserDemand(username));

                users_demand_panel.Children.Remove(userCanvas);
            };

            buttonStack.Children.Add(acceptButton);
            buttonStack.Children.Add(rejectButton);

            // Add elements to DockPanel (order matters)
            containerPanel.Children.Add(buttonStack);  // Right
            containerPanel.Children.Add(userImage);    // Left
            containerPanel.Children.Add(infoGrid);     // Fill remaining space

            // Add DockPanel to Canvas (Canvas is needed for compatibility with your existing code)
            userCanvas.Children.Add(containerPanel);

            // Make sure the container panel stretches to fill the canvas
            containerPanel.Width = double.NaN; // Auto width

            // Handle responsiveness through SizeChanged event
            userCanvas.SizeChanged += (s, e) => {
                // Update container panel width to match canvas
                containerPanel.Width = e.NewSize.Width;
            };

            return userCanvas;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(MainWindow.Signin1.AdminHomePage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Theses theses1 = new Theses();
            NavigationService.Navigate(theses1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.NewTheseAdmine1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1 = new Signin();
            MainWindow.SignUp1 = new SignUp();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(AdminHomePage.Admintheses1);
        }
    }
}
