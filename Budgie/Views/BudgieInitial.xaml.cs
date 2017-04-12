using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Popups;
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
    /// This is the secondary welcome view. The user is only going to navigate once their whole time on the app (unless they delete their 
    /// localstorage cache).
    /// 
    /// This page is used to start up the app by asking the user to enter their current balance, it will save that balance to a file and set it as the global balance.
    /// Once a balance is succesfully entered, the user is brought to the main (BudgieMain) view.
    /// </summary>
    public sealed partial class BudgieInitial : Page
    {
        public BudgieInitial()
        {
            this.InitializeComponent();
            // Load the balance on startup
            this.loadBalance();
        }

        // Function to laod the balance by using the getBalance method.
        private void loadBalance()
        {
            yourBalance.Text = "" + App.getBalance();
        }

        // Function to save the balance to a file
        private async void storeBalance()
        {
            try
            {
                var budgetFile = await App.localStorageFolder.CreateFileAsync("budgetFile.txt");
                await FileIO.WriteTextAsync(budgetFile, welcomeBalance.Text);
            }
            catch (Exception error)
            {
                await new MessageDialog(error.Message).ShowAsync();
            }
        }

        // welcomeSaveButton tap event to save the balance.
        private void welcomeSaveBalance_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                App.setBalance(double.Parse(welcomeBalance.Text));
                yourBalance.Text = "" + App.getBalance();

                storeBalance();

                // Navigate to the main frame if completed successfully.
                this.Frame.Navigate(typeof(BudgieMain));
            }
            catch
            {
                // Error handling.
                string error = string.Empty;

                if (welcomeBalance.Text == string.Empty)
                {
                    error = "Hey, try entering in a number!";
                }
                else
                {
                    error = "Sigh, '" + welcomeBalance.Text + "' is not a number.";
                }

                errorMessage.Text = error;
                textToSpeechMessage(error);
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
    }
}
