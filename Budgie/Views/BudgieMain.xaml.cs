using Budgie.Model;
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
        private List<Transaction> transactions = new List<Transaction>();

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
        
        private void addLog(String transactionType, String transactionName, String transactionDesc, double transactionAmount)
        { 
            if(transactionName == "" && transactionDesc == "")
            {
                transactionName = "Not Set";
                transactionDesc = "Not Set";
            }
            else if(transactionName == "")
            {
                transactionName = "Not Set";
            }else if(transactionDesc == "")
            {
                transactionDesc = "Not Set";
            }

            Transaction transaction = new Transaction()
            {
                transactionType = transactionType,
                trasnactionName = transactionName,
                transactionDesc = transactionDesc,
                transactionAmount = transactionAmount
            };
            transactions.Add(transaction);

            test.Text = transactions[transactions.Count - 1].ToString();
        }

        private void populateBalanceEdit(String editType)
        {
            dashboard.Visibility = Visibility.Collapsed;
            balanceEdit.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            if (editType == "Spend")
            {
                balanceEditLabel.Text = "Transaction Expense Amount:";
                balanceEditAction.Content = "Spend!";
                balanceEditAction.Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                balanceEditLabel.Text = "Transaction Income Amount:";
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
            String transactionType;
            try
            {
                if (balanceEditControlVaraible == 1)
                {
                    App.balance = App.balance - (double.Parse(balanceEditTextBox.Text));
                    loadBalance();
                    transactionType = "Expense";
                }
                else
                {
                    App.balance = App.balance + (double.Parse(balanceEditTextBox.Text));
                    loadBalance();
                    transactionType = "Income";
                }

                addLog(transactionType, balanceEditNameBox.Text, balanceEditDescBox.Text, double.Parse(balanceEditTextBox.Text));
                balanceEditTextBox.Text = "";
                dashboard.Visibility = Visibility.Visible;
                balanceEdit.Visibility = Visibility.Collapsed;
                backButton.Visibility = Visibility.Collapsed;
                balanceEditControlVaraible = 0;
            }
            catch
            {
                errorMessage.Text = "Please enter a number value";
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
