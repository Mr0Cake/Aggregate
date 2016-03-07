using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.BO;

namespace BLL
{
    public class FeedBLL : BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.Feed feed = u as BO.Feed;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_Feed", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.Feed feed = u as BO.Feed;
            DAL.CommonMethods.UpdateDataTable("usp_delete_Feed", DAL.CommonMethods.parameter("p_ID",u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.Feed feed = u as BO.Feed;
            return new List<SqlParameter>
            {
                DAL.CommonMethods.parameter("p_LastPublishDate", feed.LastPublishDate),
                DAL.CommonMethods.parameter("p_HomePage", feed.HomePage),
                DAL.CommonMethods.parameter("p_FeedUrl", feed.FeedUrl)
            };
        }

        public List<BO.Feed> GetAllFeeds()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFeeds", fillFeed);
        }

        //public BO.Feed GetFeedByID(int id)
        //{
        //    return DAL.CommonMethods.GetDataTable("usp_select_Feed", fillFeed, DAL.CommonMethods.parameter("p_ID", id)).FirstOrDefault();
        //}

        public BO.Feed fillFeed(DataRow dr)
        {
            BO.Feed feed = new BO.Feed();
            feed.ID = Convert.ToInt32(dr["FeedID"]);
            feed.LastPublishDate = Convert.ToDateTime(dr["LastPublishDate"]);
            feed.FeedUrl = Convert.ToString(dr["FeedUrl"]);
            feed.HomePage = Convert.ToString(dr["HomePage"]);
            return feed;
        }


        public override void Update(BOBase u)
        {
            BO.Feed feed = u as BO.Feed;
            List<SqlParameter> userparam = fillParams(u);
            userparam.Add(DAL.CommonMethods.parameter("w_FeedID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_Feed", out error, DAL.CommonMethods.parameter("p_ID", u.ID));
        }
    }
}
