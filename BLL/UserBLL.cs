using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data;
using BLL.BO;

namespace BLL
{
    public class UserBLL:BLLBase
    {

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.User user = u as BO.User;
            return new List<SqlParameter>
            {
            DAL.CommonMethods.parameter("p_UserName", user.UserName),
            DAL.CommonMethods.parameter("p_Mail", user.Mail)
            };
        }

        #region Update
        public override void Update(BO.BOBase us)
        {
            BO.User u = us as BO.User;
            int error = 0;
            List<SqlParameter> userparam = fillParams(u);
            userparam.Add(DAL.CommonMethods.parameter("w_UserID", u.ID));
            DAL.CommonMethods.UpdateDataTable("usp_update_AggregateUser", out error, userparam.ToArray());
        }
        #endregion

        #region Insert

        public override void Create(BO.BOBase us)
        {
            BO.User u = us as BO.User;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_AggregateUser",out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        #endregion

        #region Select

        public List<BO.User> GetAllUsers()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllUsers", fillUser);
        }

        public BO.User GetUserByID(int id)
        {
            return DAL.CommonMethods.GetDataTable("usp_select_User", fillUser, DAL.CommonMethods.parameter("p_UserID", id)).FirstOrDefault();
        }

        #endregion

        #region Delete
        public override void Delete(BO.BOBase u)
        {
            DAL.CommonMethods.UpdateDataTable("usp_delete_AggregateUser", DAL.CommonMethods.parameter("@p_ID", u.ID));
        }
        #endregion


        public BO.User fillUser(DataRow dr)
        {
            BO.User u = new BO.User();
            u.ID = Convert.ToInt32(dr["UserID"]);
            u.Mail = Convert.ToString(dr["Mail"]);
            u.UserName = Convert.ToString(dr["UserName"]);
            return u;
        }
        
    }
}
