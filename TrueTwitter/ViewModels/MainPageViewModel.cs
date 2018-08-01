using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TrueTwitter.Models;
using TrueTwitter.Utils;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrueTwitter.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region properties
        private IEnumerable<IGrouping<string, Tweet>> _tweets;

        /// <summary>
        /// The tweets to display
        /// </summary>
        public IEnumerable<IGrouping<string, Tweet>> Tweets
        {
            get
            {
                return this._tweets;
            }
            set
            {
                this._tweets = value;
                this.OnPropertyChanged("Tweets");
            }
        }


        private bool _isFetching;
        /// <summary>
        /// Tells if the app is fetching tweets
        /// </summary>
        public bool IsFetching
        {
            get
            {
                return this._isFetching;
            }
            set
            {
                this._isFetching = value;
                this.OnPropertyChanged("IsFetching");
            }
        }

        private ICommand _goToSettingsCommand;
        /// <summary>
        /// Go to settings
        /// </summary>
        public ICommand GoToSettingsCommand
        {
            get
            {
                return this._goToSettingsCommand;
            }
            set
            {
                this._goToSettingsCommand = value;
            }
        }

        public ICommand _fetchTweetsCommand;
        /// <summary>
        /// Fetch tweets
        /// </summary>
        public ICommand FetchTweetsCommand
        {
            get
            {
                return this._fetchTweetsCommand;
            }
            set
            {
                this._fetchTweetsCommand = value;
            }
        }
        #endregion

        public MainPageViewModel()
        {
            this.GoToSettingsCommand = new RelayCommand(param => this.GoToSettingsPage());
            this.FetchTweetsCommand = new RelayCommand(param => this.FetchTweets(), () => !this.IsFetching);
        }

        #region methods
        /// <summary>
        /// Go to settings tweets
        /// </summary>
        public void GoToSettingsPage()
        {
            (Window.Current.Content as Frame).Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Fetch tweets
        /// </summary>
        public async void FetchTweets()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (!App.AppTwitterManager.IsAuth)
            {
                var dialog = new MessageDialog(loader.GetString("MainPage_Auth_Needed"));
                await dialog.ShowAsync();
                this.GoToSettingsPage();
                return;
            }

            this.IsFetching = true;

            var res = new List<Tweet>();
            var tmp = await App.AppTwitterManager.GetTweets(App.AppSettingsManager.Followings);

            //add a "All" category by copying all tweets
            var allTitle = loader.GetString("MainPage_AllCategory_Name");
            foreach (var t in tmp)
            {
                //Put user in all
                if (t.AssociatedFollowItem.Type == FollowType.USER)
                {
                    var twt = new Tweet(t);
                    twt.AssociatedID = allTitle;
                    res.Add(twt);
                }
                //put hashtag and search in a dedicated column
                else
                {
                    res.Add(t);
                }
            }

            res.Sort((x, y) =>
            {
                if (x.AssociatedID.Equals(y.AssociatedID))
                {
                    return y.Date.CompareTo(x.Date);
                }
                //all always first
                else if (x.AssociatedID.Equals(allTitle))
                {
                    return -1;
                }
                return y.Date.CompareTo(x.Date);
            });

            var groupedTweets = from item in res
                                group item by item.AssociatedID into g
                                select g;

            this.Tweets = groupedTweets;
            this.IsFetching = false;
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
