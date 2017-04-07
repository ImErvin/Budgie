using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Budgie.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.IO.IsolatedStorage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Budgie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void letStart_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BudgieInitial));
        }

        /*private void welcomeSaveBalance_Click(object sender, RoutedEventArgs e)
        {
            double balance = 0;
            try
            {
                balance = double.Parse(welcomeBalance.Text);
                yourBalance.Text = "" + balance;
                welcomePage.Visibility = Visibility.Collapsed;
                mainPage.Visibility = Visibility.Visible;
            }
            catch
            {
                errorMessage.Text = "Not a double value!";
            }
            
        }*/
    }
}
