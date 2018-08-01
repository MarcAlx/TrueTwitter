using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrueTwitter.Models;
using TrueTwitter.Utils;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrueTwitter.ViewModels
{
    public class SettingsPageViewModel : INotifyPropertyChanged
    {
        #region properties
        public string _statusText;
        /// <summary>
        /// Status text of the app
        /// </summary>
        public string StatusText
        {
            get
            {
                return this._statusText;
            }
            set
            {
                this._statusText = value;
                this.OnPropertyChanged("StatusText");
            }
        }

        public string _consumerKey;
        /// <summary>
        /// Consumer key
        /// </summary>
        public string ConsumerKey
        {
            get
            {
                return this._consumerKey;
            }
            set
            {
                this._consumerKey = value;
                this.OnPropertyChanged("ConsumerKey");
            }
        }

        public string _consumerSecret;
        /// <summary>
        /// Consumer secret
        /// </summary>
        public string ConsumerSecret
        {
            get
            {
                return this._consumerSecret;
            }
            set
            {
                this._consumerSecret = value;
                this.OnPropertyChanged("ConsumerSecret");
            }
        }

        public string _infoLabel;
        /// <summary>
        /// Info label about app (version number)
        /// </summary>
        public string InfoLabel
        {
            get
            {
                return this._infoLabel;
            }
            set
            {
                this._infoLabel = value;
                this.OnPropertyChanged("InfoLabel");
            }
        }

        public string _followInputText;
        /// <summary>
        /// Follow Input text
        /// </summary>
        public string FollowInputText
        {
            get
            {
                return this._followInputText;
            }
            set
            {
                this._followInputText = value;
                this.OnPropertyChanged("FollowInputText");
            }
        }

        private ObservableCollection<FollowItem> _following;
        public ObservableCollection<FollowItem> Following
        {
            get
            {
                return this._following;
            }
            set
            {
                this._following = value;
                this.OnPropertyChanged("Following");
            }
        }

        private ICommand _goToMainPageCommand;
        /// <summary>
        /// Go to settings
        /// </summary>
        public ICommand GoToMainPageCommand
        {
            get
            {
                return this._goToMainPageCommand;
            }
            set
            {
                this._goToMainPageCommand = value;
            }
        }

        private ICommand _addFollowCommand;
        /// <summary>
        /// Go to settings
        /// </summary>
        public ICommand AddFollowCommand
        {
            get
            {
                return this._addFollowCommand;
            }
            set
            {
                this._addFollowCommand = value;
            }
        }

        private ICommand _deleteFollowCommand;
        /// <summary>
        /// Go to settings
        /// </summary>
        public ICommand DeleteFollowCommand
        {
            get
            {
                return this._deleteFollowCommand;
            }
            set
            {
                this._deleteFollowCommand = value;
            }
        }

        public ICommand _submitCredentialsCommand;
        /// <summary>
        /// Submit credentials
        /// </summary>
        public ICommand SubmitCredentialsCommand
        {
            get
            {
                return this._submitCredentialsCommand;
            }
            set
            {
                this._submitCredentialsCommand = value;
            }
        }
        #endregion

        public SettingsPageViewModel()
        {
            this.trySetCredentialsFromConfig();
            this.setStatus(App.AppTwitterManager.IsAuth);
            this.initFollowing();
            this.initInfoLabel();

            this.GoToMainPageCommand= new RelayCommand(param => this.GoToMainPage());
            this.AddFollowCommand = new RelayCommand(param => this.SubmitFollow());
            this.DeleteFollowCommand = new RelayCommand(param => this.DeleteFollow(param as FollowItem));
            this.SubmitCredentialsCommand = new RelayCommand(param => this.SubmitCredentials());
        }

        #region methods
        /// <summary>
        /// Set status text according to auth status
        /// </summary>
        /// <param name="auth"></param>
        private void setStatus(bool auth)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (auth)
            {
                this.StatusText = loader.GetString("SettingPage_Auth_OK");
            }
            else
            {
                this.StatusText = loader.GetString("SettingPage_Auth_KO");
            }
        }

        /// <summary>
        /// Init following from settings
        /// </summary>
        private void initFollowing()
        {
            this.Following = new ObservableCollection<FollowItem>();
            foreach (FollowItem item in App.AppSettingsManager.Followings)
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
                this.ConsumerKey = App.AppSettingsManager.ConsumerKey;
                this.ConsumerSecret = App.AppSettingsManager.ConsumerSecret;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Go to main page
        /// </summary>
        public void GoToMainPage()
        {
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
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
            this.InfoLabel = String.Format(info, version.Major, version.Minor, version.Build, version.Revision);
        }

        public void SubmitFollow()
        {
            if (!String.IsNullOrEmpty(this.FollowInputText))
            {
                var tmp = this.FollowInputText.Trim();
                var item = FollowItem.FromString(tmp);
                if (!this.Following.Contains(item))
                {
                    this.Following.Add(FollowItem.FromString(tmp));
                    App.AppSettingsManager.AddFollow(tmp);
                }
                this.FollowInputText = "";
            }
        }

        /// <summary>
        /// Remove a follow item
        /// </summary>
        /// <param name="item"></param>
        public void DeleteFollow(FollowItem item)
        {
            this.Following.Remove(item);
            App.AppSettingsManager.RemoveFollowing(item.Id);
        }

        public async void SubmitCredentials()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            if (String.IsNullOrEmpty(this.ConsumerKey))
            {
                this.StatusText = loader.GetString("SettingPage_ConsumerKey_Needed");
                return;
            }

            if (String.IsNullOrEmpty(this.ConsumerKey))
            {
                this.StatusText = loader.GetString("SettingPage_ConsumerSecret_Needed");
                return;
            }

            var cKI = this.ConsumerKey.Trim();
            var cSI = this.ConsumerSecret.Trim();
            var auth = await App.AppTwitterManager.SubmitCredentials(cKI, cSI);
            if (auth)
            {
                App.AppSettingsManager.ConsumerKey = cKI;
                App.AppSettingsManager.ConsumerSecret = cSI;
            }
            this.setStatus(auth);
        }
        #endregion

        #region PropertyCHanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
