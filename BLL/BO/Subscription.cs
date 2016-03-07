using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BO
{
    public class Subscription : BOBase
    {
        //public int SubscriptionID { get; set; }
        public int UserID { get; set; }
        public int FeedID { get; set; }

    }
}
