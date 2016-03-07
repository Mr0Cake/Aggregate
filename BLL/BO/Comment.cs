using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public class Comment:BOBase
    {
        //public int CommentID { get; set; }
        public int UserID { get; set; }
        public int FeedItemID { get; set; }
        public string Value { get; set; }
        
    }
}
