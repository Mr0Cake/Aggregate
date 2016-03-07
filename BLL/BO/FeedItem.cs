using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public class FeedItem : BOBase
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Summary { get; set; }
        public string ArticleLink { get; set; }
        public int FeedID { get; set; }
        public string Authors { get; set; }
        public string Contributors { get; set; }
        public string Links { get; set; }
        public string SourceFeed { get; set; }
        public string CopyRight { get; set; }
        public string Content { get; set; }
        public string ImageLink { get; set; }
        //public int FeedItemID { get; set; }

        public override string ToString()
        {
            return Title;
        }

    }
}
