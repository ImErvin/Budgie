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
using Budgie.Model;
using Windows.Devices.Geolocation;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Budgie
{
    /// <summary>
    /// This is the landing page. All users are going to see this page everytime they open the app.
    /// This page is used to load in localstorage and to populate relevant material from localstorage.
    /// 
    /// The content of this page differs depending if the user is a "first time user" or "reaccuring user"
    /// The first time user will see a welcome page and will be led to the BudgieInitial view, which acts as 
    /// a secondary welcome state.
    /// 
    /// The reaccuring user will be greeted with a witty message that differs each time they log in and will be
    /// Led to the BudgieMain view that acts as their main user interface.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Function to check if there is a local storage file.
            this.checkLocalStorage();
        }
        
        // Function to check if there is a local storage file.
        // Used to determine if the user should be presented with the welcome view
        // or to go straight to their user interface. Promotes a good User Experience.
        private async void checkLocalStorage()
        {
            try
            {   
                // Will try to connect to the file named "budgetFile.txt" in the LocalStorage folder
                var budgetFile = await App.localStorageFolder.GetFileAsync("budgetFile.txt");

                // If statement will activate if the file was succesffully parsed.
                if (budgetFile.IsAvailable)
                {
                    // Collapse the first-time user view and display the welcome back view.
                    letStart.Visibility = Visibility.Collapsed;
                    continueButton.Visibility = Visibility.Visible;

                    // Use the budgetFile.txt to retrieve and set the global balance.
                    App.setBalance(double.Parse(await FileIO.ReadTextAsync(budgetFile)));

                    // A welcome message to be displayed and voiced. Promotes a good user experience.
                    welcomeMessage.Text = listsOfPhrases();
                    textToSpeechMessage(welcomeMessage.Text);

                    try
                    {
                        // Function to populate the transactions list from a separate storagefile.
                        populateTransactionsList();
                    }
                    catch (Exception E)
                    {
                        await new MessageDialog(E.Message).ShowAsync();
                    }

                    return;
                }
                else
                {
                    return;
                }
            }
            catch(Exception E)
            {
                await new MessageDialog(E.Message).ShowAsync();
            }

        }

        // Function that will return a random string.
        // This string will be used to populate the display and voice message. Promotes UI & UX.
        private string listsOfPhrases()
        {
            List<string> welcomePhrases = new List<string>();
            welcomePhrases.Add("Polly wants a penny!");
            welcomePhrases.Add("Quak! Oh it's you again..");
            welcomePhrases.Add("You're spending too much money!");
            welcomePhrases.Add("Well, well, well, if it isn't the big spender.");

            Random random = new Random();
            int randomNumber = random.Next(0, 3);

            return welcomePhrases[randomNumber];
        }

        // Function to parse the "transactionsFile.txt" and populate a List of Objects.
        private async void populateTransactionsList()
        {
            // Try to retrieve the transactionsFile.txt
            var transactionsFile = await App.localStorageFolder.GetFileAsync("transactionsFile.txt");
            // Use of a buffer to read the file line by line.
            var buffer = await FileIO.ReadLinesAsync(transactionsFile);
            // Create a new instance of Transaction object.
            Transaction transaction = new Transaction();
            // Control variables for the loops.
            int lineNo = 0, i = 0;

            // Will loop over each line in the file.
            for (i = 0; i <= buffer.Count - 1; i++)
            {
                // Use of lineNo integer to determine which variable is being passed in from the .txt file.
                // Taking in 5 different variables, in the order of Type, Amount, Name, Desc, Date.
                // This if statement will populate each one of those variables and once it hits the 6th line
                // , it will restart. (This was my hardcoded way of populating the transactions list. There may
                // be better methods out there, but I wasn't fortunate enough to find a simpler way with using
                // localstorage files.)
                if (lineNo == 0)
                {
                    transaction.transactionType = "" + buffer[i];

                    lineNo++;
                }
                else if (lineNo == 1)
                {
                    transaction.transactionAmount = double.Parse(buffer[i]);

                    lineNo++;
                }
                else if (lineNo == 2)
                {
                    transaction.transactionName = "" + buffer[i];

                    lineNo++;
                }
                else if (lineNo == 3)
                {
                    transaction.transactionDesc = "" + buffer[i];

                    lineNo++;
                }
                else if (lineNo == 4)
                {
                    transaction.transactionDate = DateTime.Parse(buffer[i]);

                    lineNo++;
                }
                else if (lineNo == 5)
                {
                    // Adds the transaction object to the List in BudgieMain view.
                    BudgieMain.transactions.Add(transaction);
                    // Resets variables for the next item.
                    transaction = new Transaction();
                    lineNo = 0;
                }
            }
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

        // Button to navigate to the main user interface.
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BudgieMain));
        }

        // Button to navigate to the second welcome view.
        private void letStart_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BudgieInitial));
        }
    }
}
