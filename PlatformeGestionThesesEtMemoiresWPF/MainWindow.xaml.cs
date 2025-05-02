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

// accessing the classes and functions that are implemented in the business logic layer 
using BusinessLogicLayer;

using PlatformeGestionThesesEtMemoiresWPF.Windows.RegisterPages;

/*  
 * This Layer is responsible for the User Interface (UI).  
 * It should only handle user interactions and display data.  
 * It communicates with the Business Logic Layer (BLL) to get or update data.  
 * It should NOT contain business logic or database queries.  
 * Example: A WPF window displaying user details retrieved from BLL.  
 */


namespace PlatformeGestionThesesEtMemoiresWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public static searshepage searshepage1 = new searshepage();
        public static Signin Signin1 = new Signin();
        public static SignUp SignUp1 = new SignUp();
        public MainWindow()
        {
            

            InitializeComponent();
            MainFrame.Navigate(searshepage1);
            
        }
    }
}
