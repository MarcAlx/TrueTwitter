using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTwitter.Managers
{
    /// <summary>
    /// Define Twitter Manager
    /// </summary>
    public interface ITwitterManager
    {
        /// <summary>
        /// Submit new credentials to the app then test against twitter validity
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="cliendSecret"></param>
        /// <returns></returns>
        Task<bool> SubmitCredentials(String consumerKey, String consumerSecret);

        /// <summary>
        /// Tells if auth has been set
        /// </summary>
        bool IsAuth { get; }
    }
}
