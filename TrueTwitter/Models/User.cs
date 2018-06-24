using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTwitter.Models
{
    /// <summary>
    /// Represent a tweeter account
    /// </summary>
    public class User
    {
        public User()
        {

        }

        /// <summary>
        /// Constructor by copy
        /// </summary>
        /// <param name="user"></param>
        public User(User user)
        {
            this.Name = user.Name;
            this.ImageURI = user.ImageURI;
        }

        /// <summary>
        /// The name of the account
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The image URI of the profile
        /// </summary>
        public String ImageURI { get; set; }
    }
}
