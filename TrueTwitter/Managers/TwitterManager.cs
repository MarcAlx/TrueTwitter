using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace TrueTwitter.Managers
{
    public class TwitterManager : ITwitterManager
    {
        #region singleton and init
        private static volatile TwitterManager instance;
        private static object syncRoot = new Object();

        public static TwitterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TwitterManager();
                    }
                }

                return instance;
            }
        }

        private TwitterManager()
        {
        }
        #endregion

        #region private properties
        private string clientId = null;
        private string clientSecret = null;
        #endregion

        public bool IsAuth {
            get
            {
                return Auth.ApplicationCredentials != null;
            }
        }

        public async Task<bool> SubmitCredentials(String consumerKey, String consumerSecret)
        {
            try
            {
                // If you do not already have a BearerToken you can set it up after the credentials object has been created
                var appCreds = Auth.SetApplicationOnlyCredentials(consumerKey, consumerSecret);
                
                // This method execute the required webrequest to set the bearer Token
                Auth.InitializeApplicationOnlyCredentials(appCreds);

                if (String.IsNullOrEmpty(appCreds.ApplicationOnlyBearerToken))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
