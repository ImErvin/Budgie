using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class BudgieMain : Page
    {
        private int balanceEditControlVaraible;

        public BudgieMain()
        {
            this.InitializeComponent();
            this.loadBalance();
            this.loadPage();
        }

        private void loadPage()
        {
            balanceEdit.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
        }

        private void loadBalance()
        {
            yourBalance.Text = "" + App.balance;
            if (App.balance > 0)
            {
                yourBalance.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                yourBalance.Foreground = new SolidColorBrush(Colors.Red);
            }
            
        }
        
        private void populateBalanceEdit(String editType)
        {
            dashboard.Visibility = Visibility.Collapsed;
            balanceEdit.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            if (editType == "Spend")
            {
                balanceEditLabel.Text = "Enter Expense:";
                balanceEditAction.Content = "Spend!";
                balanceEditAction.Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                balanceEditLabel.Text = "Enter Income:";
                balanceEditAction.Content = "Save!";
                balanceEditAction.Background = new SolidColorBrush(Colors.Green);
            }
        }

        private void newExpense_Click(object sender, RoutedEventArgs e)
        {
            populateBalanceEdit("Spend");
            balanceEditControlVaraible = 1;
        }

        private void newIncome_Click(object sender, RoutedEventArgs e)
        {
            populateBalanceEdit("Save");
            balanceEditControlVaraible = 2;
        }

        private void balanceEditAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (balanceEditControlVaraible == 1)
                {
                    App.balance = App.balance - (double.Parse(balanceEditTextBox.Text));
                    loadBalance();
                    balanceEditControlVaraible = 0;
                }
                else
                {
                    App.balance = App.balance + (double.Parse(balanceEditTextBox.Text));
                    loadBalance();
                    balanceEditControlVaraible = 0;
                }

                balanceEditTextBox.Text = "";
                dashboard.Visibility = Visibility.Visible;
                balanceEdit.Visibility = Visibility.Collapsed;
                backButton.Visibility = Visibility.Collapsed;
            }
            catch
            {
                errorMessage.Text = "Not a double value!";
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Visible;
            balanceEdit.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
        }
    }
}
