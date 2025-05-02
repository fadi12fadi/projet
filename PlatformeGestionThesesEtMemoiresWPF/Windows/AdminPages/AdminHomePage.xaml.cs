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
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    
    public partial class AdminHomePage : Page
    {
        UserBLL userBLL1;
        public static NewAcountsPage NewAcountsPage1;
        public static NewTheseAdmine NewTheseAdmine1;
        public static Theses Theses1 = new Theses() ;
        public static Admintheses Admintheses1;
        public AdminHomePage(UserBLL userBLL1)
        {
            this.userBLL1 = userBLL1;
            InitializeComponent();
            Admintheses1 = new Admintheses();
            NewAcountsPage1 = new NewAcountsPage();
            NewTheseAdmine1 = new NewTheseAdmine();
            Theses1 = new Theses();

        }
        public class UserComparer : IEqualityComparer<UserBLL>
        {
            public bool Equals(UserBLL x, UserBLL y)
            {
                return x?.id == y?.id; // Using whatever uniquely identifies a user
            }

            public int GetHashCode(UserBLL obj)
            {
                return obj.id.GetHashCode();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Clear previous results
            if (stack_panel_user.Children.Count > 0)
            {
                stack_panel_user.Children.Clear();
            }

            UserBLL userByUsername = null;
            UserBLL userByMatricule = null;
            List<UserBLL> usersByFirstName = null;
            List<UserBLL> usersByLastName = null;

            List<UserBLL> usersResult = new List<UserBLL>();
            if (tbsearch_Username.Text != "" && tbsearch_Username.Text != "Username")
                userByUsername = CurrentData.currentAdmin.getUserByUserName(tbsearch_Username.Text);

            if (tbsearch_Matricule.Text != "" && tbsearch_Matricule.Text != "Matricule")
                userByMatricule = CurrentData.currentAdmin.getUserByMatricule(tbsearch_Matricule.Text);

            if (tbsearch_FName.Text != "" && tbsearch_FName.Text != "First Name")
                usersByFirstName = CurrentData.currentAdmin.getUsersByfName(tbsearch_FName.Text);

            if (tbsearch_LName.Text != "" && tbsearch_LName.Text != "Last Name")
                usersByLastName = CurrentData.currentAdmin.getUserBylName(tbsearch_LName.Text);

            List<UserBLL> userResult = null;

            if (userByUsername != null)
                if (userResult == null)
                    userResult = new List<UserBLL> { userByUsername };
                else
                    userResult = userResult.Intersect(new List<UserBLL> { userByUsername }, new UserComparer()).ToList();

            if (userByMatricule != null)
                if (userResult == null)
                    userResult = new List<UserBLL> { userByMatricule };
                else
                    userResult = userResult.Intersect(new List<UserBLL> { userByMatricule }, new UserComparer()).ToList();

            if (usersByFirstName != null)
                if (userResult == null)
                    userResult = usersByFirstName;
                else
                    userResult = userResult.Intersect(usersByFirstName, new UserComparer()).ToList();

            if (usersByLastName != null)
                if (userResult == null)
                    userResult = usersByLastName;
                else
                    userResult = userResult.Intersect(usersByLastName, new UserComparer()).ToList();

            if (userResult != null/* && !String.IsNullOrWhiteSpace(userResult[0].firstName)*/)
            {
                for (int i = 0; i < userResult.Count ; i++) {
                 Canvas h = CreateUserCard(userResult[i].matricule, userResult[i].lastName + ' ' + userResult[i].firstName, userResult[i].username, userResult[i].email, "jjj");
                stack_panel_user.Children.Add(h); }
            }

        }
        private Canvas CreateUserCard(string matricule, string fullName, string username, string email, string imagePath)
        {
            // 1) Outer Canvas wrapper
            Canvas userCanvas = new Canvas
            {
                Background = Brushes.White,
                Margin = new Thickness(2,2,2,0),
                Height = 80
            };

            // 2) We’ll use a Grid (called containerPanel) instead of a DockPanel
            Grid containerPanel = new Grid
            {
                Margin = new Thickness(5, 0,0,0),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 80
            };

            // Three columns: image (Auto), info (Star), buttons (Auto)
            containerPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            containerPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            containerPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // 3) User image
            Image userImage = new Image
            {
                Width = 73,
                Height = 78,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 10, 0)
            };
            if (!string.IsNullOrEmpty(imagePath))
            {
                try { userImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)); }
                catch { /* ignore */ }
            }
            Grid.SetColumn(userImage, 0);
            containerPanel.Children.Add(userImage);

            // 4) Build the StackPanel of text fields
            StackPanel matriculeStack = new StackPanel
            {
                Margin = new Thickness(5, 0, 5, 0),
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinWidth = 300,

            };

            TextBlock matriculeText = new TextBlock
            {
                Text = "Matricule: " + matricule,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            TextBlock username1 = new TextBlock
            {
                Text = "UserName: " + username,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            TextBlock fulname1 = new TextBlock
            {
                Text = "FullName: " + fullName,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            TextBlock email1 = new TextBlock
            {
                Text = "Email: " + email,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };

            matriculeStack.Children.Add(matriculeText);
            matriculeStack.Children.Add(username1);
            matriculeStack.Children.Add(fulname1);
            matriculeStack.Children.Add(email1);

            // 5) Wrap it in your ScrollViewer named “sisi”
            ScrollViewer sisi = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = matriculeStack
            };
            Grid.SetColumn(sisi, 1);
            containerPanel.Children.Add(sisi);

            // 6) Button stack
            StackPanel buttonStack = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 70
            };

            Button addadminbtn = new Button
            {
                Content = "Add Admin",
                Margin = new Thickness(0, 0, 0, 4),
                BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF207729")),
                Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF4EFF00")),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = Brushes.White,
                Tag=username
            };

            addadminbtn.Click += (s, e) => {

                UserBLL u = CurrentData.currentAdmin.getUserByUserName(username);
                if (u.isAdmin)
                    MessageBox.Show($"This user is already an admin.");
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to make {username} an Admin?\nThis will give them the ability to delete users, delete theses, and control the platform.",
                        "Set User as Admin",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        if (CurrentData.currentAdmin.makeUserAnAdmin(username))
                        {
                            MessageBox.Show($"Done! the user {username} is now an admin.");
                        }
                        else
                            MessageBox.Show($"an error occured while making the user an admin.");

                    }
                    //else
                    //{
                    //    // User clicked No
                    //    // Do nothing or show a message
                    //}
                }


            };
            Button rejectButton = new Button
            {
                Content = "Delete",
                Margin = new Thickness(0, 0, 0, 4),
                Background = Brushes.Red,
                BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF920000")),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = Brushes.White,
                Tag = username
            };
            rejectButton.Click += (s, e) => {
                if (username == CurrentData.currentAdmin.username)
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"You can't delete your account as an admin !\nIf you want to delete your account tell another admin to delete your admin account",
                        "Delete User",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else
                {
                    UserBLL u = CurrentData.currentAdmin.getUserByUserName(username);
                    if (u.isAdmin)
                    {
                        MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to delete the Admin :{username} ?",
                        "Delete ADMINISTRATOR",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (CurrentData.currentAdmin.deleteUserAccount(username))
                                MessageBox.Show($"Done! the user {username} is deleted seccussfully.");
                            else
                                MessageBox.Show($"an error occured while deleting the user.");
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to delete the User :{username} ?",
                        "Delete User",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (CurrentData.currentAdmin.deleteUserAccount(username))
                                MessageBox.Show($"Done! the user {username} is deleted seccussfully.");
                            else
                                MessageBox.Show($"an error occured while deleting the user.");
                        }
                    }

                }
            };

            buttonStack.Children.Add(addadminbtn);
            buttonStack.Children.Add(rejectButton);
            
            Grid.SetColumn(buttonStack, 2);
            containerPanel.Children.Add(buttonStack);
           
            userCanvas.Children.Add(containerPanel);
            Canvas.SetLeft(containerPanel, 0);
            Canvas.SetTop(containerPanel, 0);

            return userCanvas;
        }

       

        private void reset(object sender, RoutedEventArgs e)
        {
            tbsearch_FName.Text = "First Name";
            tbsearch_LName.Text = "Last Name";
            tbsearch_Matricule.Text = "Matricule";
            tbsearch_Username.Text = "Username";


        }
       

        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "domain")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "domain";
            }
        }

        //titre
        private void matricule_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Matricule")
            {
                textBox.Text = "";
            }
        }

        private void matricule_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Matricule";
            }
        }

        // faculte
        private void first_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "First Name")
            {
                textBox.Text = "";
            }
        }

        private void first_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "First Name";
            }
        }

        // author
        private void last_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Last Name")
            {
                textBox.Text = "";
            }
        }

        private void last_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Last Name";
            }
        }

        // sous titre
        private void username_gotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Username")
            {
                textBox.Text = "";
            }
        }

        private void username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Username";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NewAcountsPage1 = new NewAcountsPage();
            NavigationService.Navigate(NewAcountsPage1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(Theses1);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(NewTheseAdmine1);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Signin1 = new Signin();
            NavigationService.Navigate(MainWindow.searshepage1);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Admintheses1);
        }
    }
}
