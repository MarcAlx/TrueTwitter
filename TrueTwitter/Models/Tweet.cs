using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTwitter.Models
{
    public class Tweet
    {
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
            this.AssociatedID = t.AssociatedID;
            this.MediaURI = new List<string>(t.MediaURI);
            this.User = new User(t.User);
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
        /// List of associated media URI
        /// </summary>
        public List<String> MediaURI { get; set; }

        /// <summary>
        /// User that post the Tweet
        /// </summary>
        public User User { get; set; }
    }
}
