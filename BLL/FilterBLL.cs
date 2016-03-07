using BLL.BO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FilterBLL:BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.Filter Filter = u as BO.Filter;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_Filter", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.Filter Filter = u as BO.Filter;
            DAL.CommonMethods.UpdateDataTable("usp_delete_Filter", DAL.CommonMethods.parameter("p_ID", u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.Filter Filter = u as BO.Filter;
            return new List<SqlParameter>
            {
                DAL.CommonMethods.parameter("p_FeedID", Filter.FeedID),
                DAL.CommonMethods.parameter("p_FilterString", Filter.FilterString),
                DAL.CommonMethods.parameter("p_UserID", Filter.UserID)
            };
        }

        public List<BO.Filter> GetAllFilters()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFilters", fillFilter);
        }

        public List<BO.Filter> GetAllFilters(int id)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllFilters_ByUserID", fillFilter, DAL.CommonMethods.parameter("p_UserID", id));
        }


        public BO.Filter fillFilter(System.Data.DataRow dr)
        {
            BO.Filter Filter = new BO.Filter();
            Filter.ID = Convert.ToInt32(dr["FilterID"]);
            Filter.UserID = Convert.ToInt32(dr["UserID"]);
            Filter.FeedID = Convert.ToInt32(dr["FeedID"]);
            Filter.FilterString = Convert.ToString(dr["FilterString"]);
            return Filter;
        }


        public override void Update(BOBase u)
        {
            BO.Filter Filter = u as BO.Filter;
            List<SqlParameter> param = fillParams(u);
            param.Add(DAL.CommonMethods.parameter("w_FilterID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_Filter", out error, param.ToArray());
        }
    }
}
