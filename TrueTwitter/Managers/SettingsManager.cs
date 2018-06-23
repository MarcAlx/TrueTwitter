using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueTwitter.Models;
using Windows.Storage;

namespace TrueTwitter.Managers
{
    public class SettingsManager : ISettingsManager
    {
        private static volatile SettingsManager instance;
        private static object syncRoot = new Object();

        public static ISettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SettingsManager();
                    }
                }

                return instance;
            }
        }

        private SettingsManager()
        {
        }

        public String ConsumerKey
        {
            get
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("ConsumerKey"))
                {
                    return Windows.Storage.ApplicationData.Current.LocalSettings.Values["ConsumerKey"].ToString();
                }
                return null;
            }
            set
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["ConsumerKey"] = value;
            }
        }

        public String ConsumerSecret
        {
            get
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("ConsumerSecret"))
                {
                    return Windows.Storage.ApplicationData.Current.LocalSettings.Values["ConsumerSecret"].ToString();
                }
                return null;
            }
            set
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["ConsumerSecret"] = value;
            }
        }

        public List<FollowItem> Followings
        {
            get
            {
                var res = new List<FollowItem>();
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("Following"))
                {
                    var following = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] as ApplicationDataCompositeValue;
                    foreach(var key in following.Keys)
                    {
                        res.Add(FollowItem.FromString(key));
                    }

                }
                return res;
            }
            set
            {
                if (value != null)
                {
                    var following = new ApplicationDataCompositeValue();
                    foreach (var item in value)
                    {
                        following[item.Id] = "follow";
                    }
                    //Save edits
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] = following;
                }
            }
        }

        
        public void AddFollow(String follow)
        {
            if (!String.IsNullOrEmpty(follow))
            {
                ApplicationDataCompositeValue following = null;
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("Following"))
                {
                    following = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] as ApplicationDataCompositeValue;
                }
                else
                {
                    following = new ApplicationDataCompositeValue();
                }

                if (!following.ContainsKey(follow))
                {
                    following[follow] = "follow";
                }
                //Save edits
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] = following;
            }
        }

        public void RemoveFollowing(String follow)
        {
            if (!String.IsNullOrEmpty(follow))
            {
                ApplicationDataCompositeValue following = null;
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("Following"))
                {
                    following = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] as ApplicationDataCompositeValue;
                }
                else
                {
                    following = new ApplicationDataCompositeValue();
                }

                if (following.ContainsKey(follow))
                {
                    following.Remove(follow);
                }
                //Save edits
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Following"] = following;
            }
        }
    }
}
