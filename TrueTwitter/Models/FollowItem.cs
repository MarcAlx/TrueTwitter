using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTwitter.Models
{
    public enum FollowType {
        USER,
        HASHTAG,
        SEARCH
    }

    public class FollowItem
    {
        public String Id { get; }

        public FollowType Type { get; }

        private FollowItem(String id,FollowType type)
        {
            this.Id = id;
            this.Type = type;
        }

        public FollowItem() { }

        /// <summary>
        /// Create a FollowItem from a string, assign type according to str
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FollowItem FromString(String str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                var tmp = str.Trim();
                if (tmp.StartsWith("@") && tmp.Split('@').Count() == 2)
                {
                    return new FollowItem(str,FollowType.USER);
                }
                else if (tmp.StartsWith("#") && tmp.Split('#').Count()==2)
                {
                    return new FollowItem(str, FollowType.HASHTAG);
                }
                else
                {
                    return new FollowItem(str, FollowType.SEARCH);
                }
            }
            return null;
        }

        public override bool Equals(object o)
        {
            if(o!=null && o is FollowItem)
            {
                return this.Id.Equals((o as FollowItem).Id);
            }
            return false;
        }

        public bool IsUser
        {
            get
            {
                return this.Type == FollowType.USER;
            }
        }

        public bool IsHashtag
        {
            get
            {
                return this.Type == FollowType.HASHTAG;
            }
        }

        public bool IsSearch {
            get
            {
                return this.Type == FollowType.SEARCH;
            }
        }
    }
}
