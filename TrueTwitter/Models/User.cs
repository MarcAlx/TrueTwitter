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
