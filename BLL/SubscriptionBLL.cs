using BLL.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SubscriptionBLL:BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.Subscription Subscription = u as BO.Subscription;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_Subscription", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.Subscription Subscription = u as BO.Subscription;
            DAL.CommonMethods.UpdateDataTable("usp_delete_Subscription", DAL.CommonMethods.parameter("p_ID", u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.Subscription Subscription = u as BO.Subscription;
            return new List<SqlParameter>
            {
                DAL.CommonMethods.parameter("p_FeedID", Subscription.FeedID),
                DAL.CommonMethods.parameter("p_UserID", Subscription.UserID)
            };
        }

        public List<BO.Subscription> GetAllSubscriptions()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllSubscriptions", fillSubscription);
        }

        public List<BO.Subscription> GetAllSubscriptions(int id)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllSubscriptions_ByUserID", fillSubscription, DAL.CommonMethods.parameter("p_UserID", id));
        }
        

        public BO.Subscription fillSubscription(System.Data.DataRow dr)
        {
            BO.Subscription Subscription = new BO.Subscription();
            Subscription.ID = Convert.ToInt32(dr["SubscriptionID"]);
            Subscription.UserID = Convert.ToInt32(dr["UserID"]);
            Subscription.FeedID = Convert.ToInt32(dr["FeedID"]);
            return Subscription;
        }


        public override void Update(BOBase u)
        {
            BO.Subscription Subscription = u as BO.Subscription;
            List<SqlParameter> param = fillParams(u);
            param.Add(DAL.CommonMethods.parameter("w_SubscriptionID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_Subscription", out error, param.ToArray());
        }
    }
}
