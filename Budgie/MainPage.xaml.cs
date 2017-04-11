﻿using System;
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
            

            try
            {
                var budgetFile = await App.localStorageFolder.GetFileAsync("budget.txt");

                if (budgetFile.FileType == ".txt")
                {
                    MediaElement mediaElement = new MediaElement();
                    var synth = new SpeechSynthesizer();


                    List<string> welcomes = new List<string>();
                    welcomes.Add("Polly wants a penny!");
                    welcomes.Add("Quak! Oh it's you again..");
                    welcomes.Add("You're spending too much money!");
                    welcomes.Add("Welcome baa.. *Sigh* Just click the damn button..");
                    welcomes.Add("Quaakk! I hate my job!");

                    letStart.Visibility = Visibility.Collapsed;
                    continueButton.Visibility = Visibility.Visible;

                    App.balance = double.Parse(await FileIO.ReadTextAsync(budgetFile));

                    Random random = new Random();
                    int randomNumber = random.Next(0, 3);

                    welcomeMessage.Text = welcomes[randomNumber];
                    SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(welcomes[randomNumber]);
                    mediaElement.SetSource(stream, stream.ContentType);
                    mediaElement.Play();

                    try
                    {
                        var transactionsFile = await App.localStorageFolder.GetFileAsync("transactions.txt");
                        var buffer = await FileIO.ReadLinesAsync(transactionsFile);
                        Transaction transaction = new Transaction();
                        int lineNo = 0;

                        for (int i = 0; i <= buffer.Count - 1; i++)
                        {

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
                                BudgieMain.transactions.Add(transaction);
                                transaction = new Transaction();
                                lineNo = 0;
                            }
                        }

                    }
                    catch (Exception E)
                    {

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

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BudgieMain));
        }
    }
}
