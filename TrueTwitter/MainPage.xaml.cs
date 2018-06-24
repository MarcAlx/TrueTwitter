using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrueTwitter.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrueTwitter
{

    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.searchProgressRing.IsActive = true;

            var res = new List<Tweet>();
            var tmp = await App.AppTwitterManager.GetTweets(App.AppSettingsManager.Followings);

            //add a "All" category by copying all tweets
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var allTitle = loader.GetString("MainPage_AllCategory_Name");
            res.AddRange(tmp.Select(t =>
            {
                var twt = new Tweet(t);
                twt.AssociatedID = allTitle;
                return twt;
            }));
            res.AddRange(tmp);

            var groupedTweets = from item in res
                      group item by item.AssociatedID into g
                      select g;
            this.cvs.Source = groupedTweets;
            this.searchProgressRing.IsActive = false;
        }

        private async void openButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri((sender as Button).CommandParameter.ToString()));
        }
    }
}
