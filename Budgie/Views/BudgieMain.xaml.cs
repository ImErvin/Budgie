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
            logs.Visibility = Visibility.Collapsed;
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
            showLogs();
        }

        private void showLogs()
        {
            //List<string> log = new List<string>();
            List<string> logList = new List<string>();

            foreach (var transaction in transactions)
            {
                logList.Add(transaction.ToString());
            }

            logList.Reverse();
            logs.DataContext = logList;

            /*
            foreach (string log in logs.Items)
            {
                if (log.Contains("Expense"))
                {
                    logs.Items[i].Background = new SolidColorBrush(GetSolidColorBrush("#FFe8465a").Color);
                }
                else
                {
                    result.Background = new SolidColorBrush(GetSolidColorBrush("#FF77ce6f").Color);
                }
            }*/
            
            //logs.DataContext = log;
        }
        
        private void addLog(string transactionType, string transactionName, string transactionDesc, double transactionAmount)
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
                transactionAmount = transactionAmount,
                transactionDate = DateTime.Now
            };
            transactions.Add(transaction);
        }

        private void populateBalanceEdit(string editType)
        {
            dashboard.Visibility = Visibility.Collapsed;
            balanceEdit.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            if (editType == "Spend")
            {
                balanceEditLabel.Text = "Expense Amount:";
                balanceEditName.Text = "Expense Name:";
                balanceEditDesc.Text = "Expense Description:";
                balanceEditAction.Content = "Spend!";
                balanceEditAction.Background = new SolidColorBrush(GetSolidColorBrush("#FFe8465a").Color);
            }
            else
            {
                balanceEditLabel.Text = "Income Amount:";
                balanceEditName.Text = "Income Name:";
                balanceEditDesc.Text = "Income Description:";
                balanceEditAction.Content = "Save!";
                balanceEditAction.Background = new SolidColorBrush(GetSolidColorBrush("#FF77ce6f").Color);
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
                    transactionType = "Expense";
                }
                else
                {
                    App.balance = App.balance + (double.Parse(balanceEditTextBox.Text));
                    transactionType = "Income";
                }

                addLog(transactionType, balanceEditNameBox.Text, balanceEditDescBox.Text, double.Parse(balanceEditTextBox.Text));
                balanceEditTextBox.Text = "";
                dashboard.Visibility = Visibility.Visible;
                balanceEdit.Visibility = Visibility.Collapsed;
                backButton.Visibility = Visibility.Collapsed;
                balanceEditControlVaraible = 0;
                loadBalance();
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
            logs.Visibility = Visibility.Collapsed;
        }


        private void openLogs_Click(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Visible;
            logs.Visibility = Visibility.Visible;
        }

        // Adapted from http://www.joeljoseph.net/converting-hex-to-color-in-universal-windows-platform-uwp/
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
