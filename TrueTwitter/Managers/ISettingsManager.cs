using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueTwitter.Models;

namespace TrueTwitter.Managers
{
    /// <summary>
    /// Define Settings Manager
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// ConsumerKey of the app
        /// </summary>
        String ConsumerKey { get; set; }

        /// <summary>
        /// ConsumerSecret of the app
        /// </summary>
        String ConsumerSecret { get; set; }

        /// <summary>
        /// List of following by user
        /// </summary>
        List<FollowItem> Followings { get; set;  }

        /// <summary>
        /// Add a follow to the app
        /// </summary>
        /// <param name="follow"></param>
        void AddFollow(String follow);

        /// <summary>
        /// Remove a follow from the app
        /// </summary>
        /// <param name="follow"></param>
        void RemoveFollowing(String follow);
    }
}
