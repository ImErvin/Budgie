﻿using System;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgieInitial : Page
    {
        public BudgieInitial()
        {
            this.InitializeComponent();
            this.loadBalance();
        }

        private void loadBalance()
        {
            yourBalance.Text = "" + App.balance;
        }

        private async void welcomeSaveBalance_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                App.balance = double.Parse(welcomeBalance.Text);
                yourBalance.Text = "" + App.balance;

                try
                {
                    var budgetFile = await App.localStorageFolder.CreateFileAsync("budget.txt");
                    await FileIO.WriteTextAsync(budgetFile, welcomeBalance.Text);
                }
                catch (Exception error)
                {
                    await new MessageDialog(error.Message).ShowAsync();
                }

                this.Frame.Navigate(typeof(BudgieMain));
            }
            catch
            {
                MediaElement mediaElement = new MediaElement();
                var synth = new SpeechSynthesizer();
                string error = "Quak.. '" + welcomeBalance.Text + "' is not a number.";

                errorMessage.Text = error;

                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(error);
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            }
        }
    }
}
