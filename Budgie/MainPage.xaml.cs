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
using Windows.Storage;
using Windows.UI.Popups;

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
            this.checkLocalStorage();
        }

        private void letStart_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BudgieInitial));
        }
        
        private async void checkLocalStorage()
        {
            StorageFolder localStorageFolder = ApplicationData.Current.LocalFolder;
            StorageFile budgetFile;

            try
            {
                budgetFile = await localStorageFolder.GetFileAsync("budget.txt");
                
                this.Frame.Navigate(typeof(BudgieMain));
            }
            catch
            {
                await new MessageDialog("Not Found").ShowAsync();
            }
        }
    }
}
