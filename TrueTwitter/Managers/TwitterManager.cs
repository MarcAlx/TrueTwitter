﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueTwitter.Models;
using Tweetinvi;
using Tweetinvi.Models;

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

        public async Task<List<Models.Tweet>> GetTweets(List<FollowItem> items)
        {
            List<Models.Tweet> res = new List<Models.Tweet>();

            //1. launch tasks
            List<Task<IEnumerable<ITweet>>> toWait = new List<Task<IEnumerable<ITweet>>>();
            Dictionary<Task, FollowItem> taskToItem = new Dictionary<Task, FollowItem>();
            foreach (var item in items)
            {
                if(item.Type==FollowType.HASHTAG || item.Type == FollowType.SEARCH)
                {
                    var tmpT = SearchAsync.SearchTweets(item.Id);
                    toWait.Add(tmpT);
                    taskToItem.Add(tmpT, item);
                }
                else if (item.Type == FollowType.USER)
                {
                    var tmpT = TimelineAsync.GetUserTimeline(item.Id);
                    toWait.Add(tmpT);
                    taskToItem.Add(tmpT, item);
                }
                else
                {
                    //TODO
                }
            }

            //2. wait for completion
            await Task.WhenAll(toWait);

            //3. Produce results from TweetInvi models
            foreach(var task in toWait)
            {
                if(task.IsCompleted && !task.IsFaulted && !task.IsCanceled && task.Result!=null)
                {
                    foreach(var tweet in task.Result)
                    {
                        if (String.IsNullOrEmpty(tweet.InReplyToScreenName)){
                            res.Add(new Models.Tweet()
                            {
                                InnerURL = tweet.Urls.Select(x=>x.ExpandedURL).ToList(),
                                Content = tweet.Text,
                                Date = tweet.CreatedAt,
                                URL = tweet.Url,
                                Id = tweet.IdStr,
                                AssociatedID = taskToItem[task].Id,
                                AssociatedFollowItem = taskToItem[task],
                                MediaURI = tweet.Media.Select(item => new MediaItem() { URI = item.MediaURL }).ToList(),
                                User = new Models.User()
                                {
                                    Name = tweet.CreatedBy.Name,
                                    ImageURI = tweet.CreatedBy.ProfileImageUrl400x400
                                }
                            });
                        }
                    }
                }
            }

            //4. order results
            res = res.OrderBy(x => new DateTimeOffset(x.Date)).Reverse().ToList();
            
            return res;
        }
    }
}
