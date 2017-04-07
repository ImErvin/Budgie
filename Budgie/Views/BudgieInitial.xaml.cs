using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Budgie.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgieInitial : Page
    {
        public BudgieInitial()
        {
            this.InitializeComponent();
        }

        private void welcomeSaveBalance_Click(object sender, RoutedEventArgs e)
        {
            double balance = 0;
            try
            {
                balance = double.Parse(welcomeBalance.Text);
                yourBalance.Text = "" + balance;
            }
            catch
            {
                errorMessage.Text = "Not a double value!";
            }
        }
    }
}
