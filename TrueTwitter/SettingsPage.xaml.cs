using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrueTwitter.Models;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace TrueTwitter
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.trySetCredentialsFromConfig();
            this.setStatus(App.AppTwitterManager.IsAuth);
            this.initFollowing();
            this.initInfoLabel();
        }

        public ObservableCollection<FollowItem> Following;

        /// <summary>
        /// Init following from settings
        /// </summary>
        private void initFollowing()
        {
            this.Following = new ObservableCollection<FollowItem>();
            foreach(FollowItem item in App.AppSettingsManager.Followings)
            {
                Following.Add(item);
            }
            this.Following.CollectionChanged += Following_CollectionChanged;
        }

        /// <summary>
        /// On reorder push edit to settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Following_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            App.AppSettingsManager.Followings = this.Following.ToList();
        }

        /// <summary>
        /// Try read credentials from config and fill input
        /// </summary>
        private bool trySetCredentialsFromConfig()
        {
            if (!String.IsNullOrEmpty(App.AppSettingsManager.ConsumerKey) && !String.IsNullOrEmpty(App.AppSettingsManager.ConsumerSecret))
            {
                this.consumerKeyInput.Text = App.AppSettingsManager.ConsumerKey;
                this.consumerSecretInput.Text = App.AppSettingsManager.ConsumerSecret;
                return true;
            }
            return false;
        }

        /// <summary>
        /// On home button click navigate to home
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Set status text according to auth status
        /// </summary>
        /// <param name="auth"></param>
        private void setStatus(bool auth)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (auth)
            {
                this.statusText.Text = loader.GetString("SettingPage_Auth_OK");
            }
            else
            {
                this.statusText.Text = loader.GetString("SettingPage_Auth_KO");
            }
        }

        /// <summary>
        /// On submit credential click, use twittermanager to auth and if it successes save in settings manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void submitCredentials_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            
            if (String.IsNullOrEmpty(this.consumerKeyInput.Text))
            {
                this.statusText.Text = loader.GetString("SettingPage_ConsumerKey_Needed");
                return;
            }

            if (String.IsNullOrEmpty(this.consumerSecretInput.Text))
            {
                this.statusText.Text = loader.GetString("SettingPage_ConsumerSecret_Needed");
                return;
            }

            var cKI = this.consumerKeyInput.Text.Trim();
            var cSI = this.consumerSecretInput.Text.Trim();
            var auth = await App.AppTwitterManager.SubmitCredentials(cKI, cSI);
            if (auth)
            {
                App.AppSettingsManager.ConsumerKey = cKI;
                App.AppSettingsManager.ConsumerSecret = cSI;
            }
            this.setStatus(auth);
        }

        /// <summary>
        /// On delete click remove from view and settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFollow_Click(object sender, RoutedEventArgs e)
        {
            FollowItem item = ((sender as Button).CommandParameter) as FollowItem;
            this.Following.Remove(item);
            App.AppSettingsManager.RemoveFollowing(item.Id);
        }

        /// <summary>
        /// On add click add to view and settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFollow_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.followInput.Text))
            {
                var tmp = this.followInput.Text.Trim();
                var item = FollowItem.FromString(tmp);
                if (!this.Following.Contains(item))
                {
                    this.Following.Add(FollowItem.FromString(tmp));
                    App.AppSettingsManager.AddFollow(tmp);
                }
            }
        }

        /// <summary>
        /// On keyup (enter only) -> Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void followInput_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.addFollow_Click(null, null);
                this.followInput.Text = "";
            }
        }

        /// <summary>
        /// init info label in ui
        /// </summary>
        private void initInfoLabel()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            String info = loader.GetString("SettingsPage_appInfo");
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            this.appInfoLabel.Text = String.Format(info, version.Major, version.Minor, version.Build, version.Revision);
        }

        /// <summary>
        /// On markdown link click open in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
