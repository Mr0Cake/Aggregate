using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public class FeedItemStatus : BOBase
    {
        //public int StatusID { get; set; }
        public int FeedItemID { get; set; }
        public int UserID { get; set; }
        public bool? Read { get; set; }
        public bool? ReadLater { get; set; }
        public bool? Favourite { get; set; }
    }
}
