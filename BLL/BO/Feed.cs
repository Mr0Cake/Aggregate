using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public class Feed:BOBase
    {
        //public int FeedID { get; set; }
        public DateTime LastPublishDate { get; set; }
        public string HomePage { get; set; }
        public string FeedUrl { get; set; }
        private List<FeedItem> _Top3;

        public List<FeedItem> Top3
        {
            get { return (_Top3 = _Top3?? (new FeedItemBLL()).Top3_ByFeedID(ID)); }
            set { _Top3 = value; }
        }

    }
}
