using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTwitter.Models
{
    public class Tweet
    {
        /// <summary>
        /// URL template for retweet : https://developer.twitter.com/en/docs/twitter-for-websites/web-intents/overview
        /// </summary>
        private static string RETWEET_TEMPLATE_URL = "https://twitter.com/intent/retweet?tweet_id={0}";

        /// <summary>
        /// URL template for like : https://developer.twitter.com/en/docs/twitter-for-websites/web-intents/overview
        /// </summary>
        private static string LIKE_TEMPLATE_URL = "https://twitter.com/intent/like?tweet_id={0}";

        /// <summary>
        /// URL template for reply : https://developer.twitter.com/en/docs/twitter-for-websites/web-intents/overview
        /// </summary>
        private static string REPLY_TEMPLATE_URL = "https://twitter.com/intent/tweet?in_reply_to={0}";

        public Tweet()
        {

        }

        /// <summary>
        /// Constructor by copy
        /// </summary>
        /// <param name="t"></param>
        public Tweet(Tweet t)
        {
            this.Content = t.Content;
            this.Date = t.Date;
            this.URL = t.URL;
            this.InnerURL = t.InnerURL;
            this.Id = t.Id;
            this.AssociatedID = t.AssociatedID;
            this.MediaURI = new List<MediaItem>(t.MediaURI);
            this.User = new User(t.User);
            this.AssociatedFollowItem = t.AssociatedFollowItem;
        }

        /// <summary>
        /// Content of the Tweet
        /// </summary>
        public String Content { get; set; }

        /// <summary>
        /// Date of the Tweet
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// URI of the Tweet for direct access in Browser
        /// </summary>
        public String URL { get; set; }

        /// <summary>
        /// App search that lead to this tweet
        /// </summary>
        public String AssociatedID { get; set; }

        /// <summary>
        /// FollowItem that lead to this tweet
        /// </summary>
        public FollowItem AssociatedFollowItem { get; set; }

        /// <summary>
        /// List of associated media URI
        /// </summary>
        public List<MediaItem> MediaURI { get; set; }

        /// <summary>
        /// Id of the tweet
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User that post the Tweet
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Url that lead to intent retweet of this tweet
        /// </summary>
        public string RetweetURL {
            get
            {
                return String.Format(Tweet.RETWEET_TEMPLATE_URL,this.Id);
            }
        }

        /// <summary>
        /// Url that lead to intent like of this tweet
        /// </summary>
        public string LikeURL
        {
            get
            {
                return String.Format(Tweet.LIKE_TEMPLATE_URL, this.Id);
            }
        }

        /// <summary>
        /// Url that lead to intent reply of this tweet
        /// </summary>
        public string ReplyURL
        {
            get
            {
                return String.Format(Tweet.REPLY_TEMPLATE_URL, this.Id);
            }
        }

        /// <summary>
        /// Url mentionned inside tweet text
        /// 
        /// Could also be achieved by parsing : https://stackoverflow.com/questions/10576686/c-sharp-regex-pattern-to-extract-urls-from-given-string-not-full-html-urls-but
        /// </summary>
        public List<string> InnerURL
        {
            get; set;
        }
    }
}
