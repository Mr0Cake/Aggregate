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
    public class CommentBLL:BLLBase
    {
        public override void Create(BOBase u)
        {
            BO.Comment Comment = u as BO.Comment;
            int error = 0;
            int identity = 0;
            DAL.CommonMethods.UpdateDataTable("usp_insert_Comment", out error, out identity, fillParams(u).ToArray());
            u.ID = identity;
        }

        public override void Delete(BOBase u)
        {
            BO.Comment Comment = u as BO.Comment;
            DAL.CommonMethods.UpdateDataTable("usp_delete_Comment", DAL.CommonMethods.parameter("p_ID", u.ID));
        }

        public override List<SqlParameter> fillParams(BOBase u)
        {
            BO.Comment Comment = u as BO.Comment;
            return new List<SqlParameter>
            {
                DAL.CommonMethods.parameter("p_FeedItemID", Comment.FeedItemID),
                DAL.CommonMethods.parameter("p_Comment", Comment.Value),
                DAL.CommonMethods.parameter("p_UserID", Comment.UserID)
            };
        }

        public List<BO.Comment> GetAllComments()
        {
            return DAL.CommonMethods.GetDataTable("usp_select_AllComments", fillComment);
        }

        //public BO.Comment GetCommentByID(int id)
        //{
        //    return DAL.CommonMethods.GetDataTable("usp_select_Comment", fillComment, DAL.CommonMethods.parameter("p_ID", id)).FirstOrDefault();
        //}

        public BO.Comment fillComment(DataRow dr)
        {
            BO.Comment Comment = new BO.Comment();
            Comment.ID = Convert.ToInt32(dr["CommentID"]);
            Comment.UserID = Convert.ToInt32(dr["UserID"]);
            Comment.FeedItemID = Convert.ToInt32(dr["FeedItemID"]);
            Comment.Value = Convert.ToString(dr["Comment"]);
            return Comment;
        }


        public override void Update(BOBase u)
        {
            BO.Comment Comment = u as BO.Comment;
            List<SqlParameter> param = fillParams(u);
            param.Add(DAL.CommonMethods.parameter("w_CommentID", u.ID));
            int error = 0;
            DAL.CommonMethods.UpdateDataTable("usp_update_Comment", out error, param.ToArray());
        }
    }
}
