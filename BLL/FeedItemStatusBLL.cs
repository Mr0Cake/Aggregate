using BLL.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FeedItemStatusBLL:BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.FeedItemStatus FeedItemStatus = u as BO.FeedItemStatus;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_FeedItemStatus", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.FeedItemStatus FeedItemStatus = u as BO.FeedItemStatus;
            DAL.CommonMethods.UpdateDataTable("usp_delete_FeedItemStatus", DAL.CommonMethods.parameter("p_ID", u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.FeedItemStatus FeedItemStatus = u as BO.FeedItemStatus;
            return new List<SqlParameter>
            {
                //convert to bit
                DAL.CommonMethods.parameter("p_FeedItemID", FeedItemStatus.FeedItemID),
                DAL.CommonMethods.parameter("p_UserID", FeedItemStatus.UserID),
                DAL.CommonMethods.parameter("p_Read", FeedItemStatus.Read),
                DAL.CommonMethods.parameter("p_ReadLater", FeedItemStatus.ReadLater),
                DAL.CommonMethods.parameter("p_Favourite", FeedItemStatus.Favourite)
            };
        }

        public List<BO.FeedItemStatus> GetAllFeedItemStatus()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFeedItemStatus", fillFeedItemStatus);
        }

        public BO.FeedItemStatus FeedItemStatus(int id)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_FeedItemStatus_ByID", fillFeedItemStatus, DAL.CommonMethods.parameter("p_ID", id)).FirstOrDefault();
        }

        


        public BO.FeedItemStatus fillFeedItemStatus(System.Data.DataRow dr)
        {
            BO.FeedItemStatus FeedItemStatus = new BO.FeedItemStatus();
            FeedItemStatus.ID = Convert.ToInt32(dr["FeedItemStatusID"]);
            FeedItemStatus.UserID = Convert.ToInt32(dr["UserID"]);
            FeedItemStatus.FeedItemID = Convert.ToInt32(dr["FeedItemID"]);
            FeedItemStatus.Read = Convert.ToBoolean(dr["Read"]);
            FeedItemStatus.ReadLater = Convert.ToBoolean(dr["ReadLater"]);
            FeedItemStatus.Favourite = Convert.ToBoolean(dr["Favourite"]);
            return FeedItemStatus;
        }


        public override void Update(BOBase u)
        {
            BO.FeedItemStatus FeedItemStatus = u as BO.FeedItemStatus;
            List<SqlParameter> param = fillParams(u);
            param.Add(DAL.CommonMethods.parameter("w_FeedItemStatusID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_FeedItemStatus", out error, param.ToArray());
        }
    }
}
