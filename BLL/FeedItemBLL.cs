using BLL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FeedItemBLL:BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.FeedItem FeedItem = u as BO.FeedItem;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_FeedItem", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.FeedItem FeedItem = u as BO.FeedItem;
            DAL.CommonMethods.UpdateDataTable("usp_delete_FeedItem", DAL.CommonMethods.parameter("p_ID", u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.FeedItem FeedItem = u as BO.FeedItem;
            return new List<SqlParameter>
            {
               DAL.CommonMethods.parameter("p_Title", FeedItem.Title)
              ,DAL.CommonMethods.parameter("p_PublishDate", FeedItem.PublishDate)
              ,DAL.CommonMethods.parameter("p_Summary", FeedItem.Summary)
              ,DAL.CommonMethods.parameter("p_ArticleLink", FeedItem.ArticleLink)
              ,DAL.CommonMethods.parameter("p_FeedID", FeedItem.FeedID)
              ,DAL.CommonMethods.parameter("p_Authors", FeedItem.Authors)
              ,DAL.CommonMethods.parameter("p_Contributors", FeedItem.Contributors)
              ,DAL.CommonMethods.parameter("p_Links", FeedItem.Links)
              ,DAL.CommonMethods.parameter("p_SourceFeed", FeedItem.SourceFeed)
              ,DAL.CommonMethods.parameter("p_Copyright", FeedItem.CopyRight)
              ,DAL.CommonMethods.parameter("p_Content", FeedItem.Content)
            };
        }

        public List<BO.FeedItem> Top3_ByFeedID(int ID)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_Top3_ByFeedID", fillFeedItem, DAL.CommonMethods.parameter("p_FeedID", ID));
        }

        public BO.FeedItem GetFeedItemByID(int ID)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_FeedItem_ByID", fillFeedItem, DAL.CommonMethods.parameter("p_ID", ID)).FirstOrDefault();
        }

        public List<BO.FeedItem> GetAllFeedItems()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFeedItems", fillFeedItem);
        }

        public List<BO.FeedItem> GetUserFavourites()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFeedItems_ByUserID_ReadLater", fillFeedItem);
        }

        public List<BO.FeedItem> GetUserReadLater()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFeedItems", fillFeedItem);
        }

        public List<BO.FeedItem> GetUserUnread()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllUnreadFeedItems", fillFeedItem);
        }

        //public BO.FeedItem GetFeedItemByID(int id)
        //{
        //    return DAL.CommonMethods.GetDataTable("usp_select_FeedItem", fillFeedItem, DAL.CommonMethods.parameter("p_ID", id)).FirstOrDefault();
        //}

        public BO.FeedItem fillFeedItem(DataRow dr)
        {
            BO.FeedItem FeedItem = new BO.FeedItem();
            FeedItem.Title = Convert.ToString(dr["Title"]);
            try {
                FeedItem.PublishDate = ((DateTimeOffset)dr["PublishDate"]).DateTime;
            }
            catch (Exception)
            {
                FeedItem.PublishDate = DateTime.Now;
            }
            FeedItem.Summary = Convert.ToString(dr["Summary"]);
            FeedItem.ArticleLink = Convert.ToString(dr["ArticleLink"]);
            FeedItem.FeedID = Convert.ToInt32(dr["FeedID"]);
            FeedItem.Authors = IsDbNull<string>(dr["Authors"]);
            FeedItem.Contributors = IsDbNull<string>(dr["Contributors"]);
            FeedItem.Links = IsDbNull<string>(dr["Links"]);
            FeedItem.SourceFeed = IsDbNull<string>(dr["SourceFeed"]);
            FeedItem.CopyRight = IsDbNull<string>(dr["CopyRight"]);
            FeedItem.Content = IsDbNull<string>(dr["Content"]);
            FeedItem.ID = IsDbNull<int>(dr["FeedItemID"]);
            FeedItem.ImageLink = IsDbNull<string>(dr["ImageLink"]);
            return FeedItem;
        }


        public override void Update(BOBase u)
        {
            BO.FeedItem FeedItem = u as BO.FeedItem;
            List<SqlParameter> param = fillParams(u);
            param.Add(DAL.CommonMethods.parameter("w_FeedItemID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_FeedItem", out error, param.ToArray());
        }
    }
}
