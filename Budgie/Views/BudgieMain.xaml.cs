using Budgie.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Budgie.Views
{
    /// <summary>
    /// The BudgieMain view is the "main" view. This is where the user interface is.
    /// This view acts as 3 views. The implementation of Visibility mimicks 3 different views.
    /// 
    /// The user has 3 different options, to subtract from their balance, to add to their balance, or see a list
    /// of all previous transactions. Tapping on the add or subtract button will populate a dynamic view depending
    /// on which button they press. Tapping on the transactions button will populate a listview of all the
    /// previous transactions they have made.
    /// </summary>
    public sealed partial class BudgieMain : Page
    {
        // Global control int for the balanceEdit view.
        private int balanceEditControlVaraible;
        // A list of transactions that can be seen in different classes. Used to populate the transactions listview.
        public static List<Transaction> transactions = new List<Transaction>();
        // A control for a voice message.
        private int noOfLoadedTimes = 0;

        public BudgieMain()
        {
            this.InitializeComponent();
            // Loads the page once navigated to this view.
            this.loadPage();
        }

        private void loadPage()
        {
            // Loads the balance to be displayed on top of the page.
            loadBalance();
            // Shows the main view.
            dashboard.Visibility = Visibility.Visible;
            // Hides the virtual views and buttons.
            balanceEdit.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
            logs.Visibility = Visibility.Collapsed;

            return;
        }

        private void loadBalance()
        {
            

            // Sets the text for yourBalance to the value returned from the global getter for balance.
            yourBalance.Text = "" + App.getBalance();

            //If/else statement to change the color of the balance.
            if (App.getBalance() >= 0)
            {
                yourBalance.Foreground = new SolidColorBrush(GetSolidColorBrush("#FF77ce6f").Color);
            }
            else
            {
                // A warning to be played to the user if their balance is negative. Will only play once per app run.
                if (noOfLoadedTimes == 0)
                {
                    textToSpeechMessage("Looks like you're in a deficite. I would recommend not spending any more money..");
                }
                yourBalance.Foreground = new SolidColorBrush(GetSolidColorBrush("#FFe8465a").Color);
            }


            noOfLoadedTimes++;
            return;
        }

        // Function to populate a list of strings and display them in a listview.
        private void showLogs()
        {
            List<string> logList = new List<string>();

            // Loop over the transactions list and add onto the List of strings
            foreach (var transaction in transactions)
            {
                logList.Add(transaction.ToString());
            }

            // Reverse the list before displaying it
            logList.Reverse();
            // Set listview logs datacontext to be the list of strings.
            logs.DataContext = logList;

            return;
        }

        // Function to append a log onto the transactionsFile
        private async void storeLog(Transaction log)
        {
            try
            {
                var transactionsFile = await App.localStorageFolder.GetFileAsync("transactionsFile.txt");

                await FileIO.AppendTextAsync(transactionsFile, log.ToFile());
            }
            catch
            {
                var transactionsFile = await App.localStorageFolder.CreateFileAsync("transactionsFile.txt");

                await FileIO.AppendTextAsync(transactionsFile, log.ToFile());
            }
        }
        
        // Function to add a log to the transactions list.
        private void addLog(string transactionType, string transactionName, string transactionDesc, double transactionAmount)
        { 
            // If the name/desc is empty, it will set the name/desc to "not set"
            // Promotes user experience.
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
                transactionName = transactionName,
                transactionDesc = transactionDesc,
                transactionAmount = transactionAmount,
                transactionDate = DateTime.Now
            };
            // Add the transaction to the list
            transactions.Add(transaction);

            // Appends the transaction to the transactionFile.txt
            storeLog(transaction);
        }

        // Function to dynamically populate the balanceEdit view
        private void populateBalanceEdit(string editType)
        {
            // Hide the dashboard view and display the balanceEdit view and the back button.
            dashboard.Visibility = Visibility.Collapsed;
            balanceEdit.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;

            // If they clicked expense or income, populate accordingly
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

        // Function to save current balance to localstorage.
        private async void storeBalance()
        {
            try
            {
                var budgetFile = await App.localStorageFolder.GetFileAsync("budgetFile.txt");
                await FileIO.WriteTextAsync(budgetFile, "" + App.getBalance());
            }
            catch (Exception E)
            {
                await new MessageDialog(E.Message).ShowAsync();
            }
        }

        // Function to reset the input fields, so that when the user is to open it again, they won't be presented with what they entered before.
        private void resetInputBoxes()
        {
            errorMessage.Text = "";
            balanceEditTextBox.Text = "";
            balanceEditNameBox.Text = "";
            balanceEditDescBox.Text = "";
        }

        // Tap event for expense button
        private void newExpense_Tapped(object sender, RoutedEventArgs e)
        {
            populateBalanceEdit("Spend");
            balanceEditControlVaraible = 1;
        }

        // Tap event for expense button
        private void newIncome_Tapped(object sender, RoutedEventArgs e)
        {
            populateBalanceEdit("Save");
            balanceEditControlVaraible = 2;
        }

        // Tap event for the balanceEditButton
        private void balanceEditAction_Tapped(object sender, RoutedEventArgs e)
        {
            string transactionType;

            try
            {
                // If the control variable is 1, then it's an expense.
                if (balanceEditControlVaraible == 1)
                {
                    // Subtract from balance
                    App.setBalance(App.getBalance() - (double.Parse(balanceEditTextBox.Text)));
                    transactionType = "Expense";

                    // Message for the user.
                    textToSpeechMessage(listsOfPhrases(transactionType));
                }
                else
                {
                    // Add to the balance
                    App.setBalance(App.getBalance() + (double.Parse(balanceEditTextBox.Text)));
                    transactionType = "Income";

                    // Message for the user.
                    textToSpeechMessage(listsOfPhrases(transactionType));
                }

                // Save the balance to local storage
                storeBalance();
                // Add a transaction log with transaction details
                addLog(transactionType, balanceEditNameBox.Text, balanceEditDescBox.Text, double.Parse(balanceEditTextBox.Text));
                // Reset the input boxes
                resetInputBoxes();
                // Reset the page to default view
                loadPage();
                // Reset the control variable for use again
                balanceEditControlVaraible = 0;
            }
            catch
            {
                // Error handling on input boxes
                string error = string.Empty;

                if (balanceEditTextBox.Text == string.Empty)
                {
                    error = "Hey, try entering in a number!";
                }
                else
                {
                    error = "Sigh, '" + balanceEditTextBox.Text + "' is not a number.";
                }

                errorMessage.Text = error;
                textToSpeechMessage(error);
            }
        }

        // Back button tap event, will reset the page to default view
        private void backButton_Tapped(object sender, RoutedEventArgs e)
        {
            loadPage();
        }

        // openLogs button tap event
        private void openLogs_Tapped(object sender, RoutedEventArgs e)
        {
            // Loads the logs listview item
            showLogs();
            // Hides unesseary content and show necessary content.
            dashboard.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Visible;
            logs.Visibility = Visibility.Visible;
        }

        // Adapted from http://www.joeljoseph.net/converting-hex-to-color-in-universal-windows-platform-uwp/
        // Used to pass HEX code colors and create a solidcolor.
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

        // Function to pass in a string and use SpeechSynthesisStream to read out the string as a sound.
        // Used for calling out error messages, welcome messages, or sarcastic comments to the user. Promotes User Experience.
        private async void textToSpeechMessage(string message)
        {
            MediaElement mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();

            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(message);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
        
        // List of phrases to be used by the textToSpeechMessage.
        // Returns a random string depending on the context being passed in.
        private string listsOfPhrases(string typeOfPhrase)
        {
            List<string> expensePhrases = new List<string>();
            expensePhrases.Add("Did you really have to buy that?");
            expensePhrases.Add("There are wants, and there are needs.... This was definitely a want.");
            expensePhrases.Add("I'm a parrot and even I know you didn't need that.");
            expensePhrases.Add("I hope you bought some seeds, I'm getting hungry.");
            expensePhrases.Add("Spend, spend, spend.. When will you ever learn?");

            List<string> incomePhrases = new List<string>();
            incomePhrases.Add("Quak! It's payday.");
            incomePhrases.Add("Don't spend it all in one place..");
            incomePhrases.Add("Have yus got a euro for the luas?");
            incomePhrases.Add("Wow, maybe now you can hire a real accountant.");
            incomePhrases.Add("Don't worry, I'll keep this transaction under the table.");

            // Choose a random phrase and return it.
            if (typeOfPhrase == "Expense")
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 4);

                return expensePhrases[randomNumber];
            }
            else if (typeOfPhrase == "Income")
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 4);

                return incomePhrases[randomNumber];
            }
            else
            {
                return "";
            }

        }
    }
}
