using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class CommonMethods
    {
        #region Parameters
        public static SqlParameter parameter(string name, object value)
        {
            SqlParameter sqlpara = new SqlParameter();
            sqlpara.ParameterName = name;
            if (value != null)
            {
                sqlpara.Value = value;
            }
            else
            {
                sqlpara.Value = DBNull.Value;
            }
            return sqlpara;
        }


        public static SqlParameter parameter(string name, SqlDbType type)
        {
            SqlParameter sqlpara = new SqlParameter();
            sqlpara.ParameterName = name;
            sqlpara.SqlDbType = type;
            sqlpara.Direction = ParameterDirection.Output;
            return sqlpara;
        }
        #endregion

        #region DataReader
        public static List<T> ConvertDataTable<T>(DataTable dt, Func<DataRow, T> fillFunc)
        {
            List<T> returnList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                returnList.Add(fillFunc(dr));
            }
            return returnList;
        }

        public static List<T> GetDataTable<T>(string sp, Func<DataRow, T> fillFunc, params SqlParameter[] sqlpara)
        {
            SqlCommand command = new SqlCommand(sp, CData.GetConnection);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            command.CommandType = CommandType.StoredProcedure;
            if (sqlpara.Length > 0)
                command.Parameters.AddRange(sqlpara);
            da.SelectCommand = command;
            da.Fill(dt);
            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();
            return ConvertDataTable(dt, fillFunc);
        }

        public static List<T> GetDataTable<T>(string sp, out int errorNr, Func<DataRow, T> fillFunc, params SqlParameter[] sqlpara)
        {
            errorNr = 1;
            SqlCommand command = new SqlCommand(sp, CData.GetConnection);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            command.CommandType = CommandType.StoredProcedure;
            if (sqlpara.Length > 0)
                command.Parameters.AddRange(sqlpara);
            command.Parameters.Add(DAL.CommonMethods.parameter("out_error_number", SqlDbType.Int));
            da.SelectCommand = command;
            da.Fill(dt);
            errorNr = Convert.ToInt32(command.Parameters["out_error_number"].Value);
            if(command.Connection.State == ConnectionState.Open)
                command.Connection.Close();
            return ConvertDataTable(dt, fillFunc);
        }
        #endregion

        #region DataWriter
        public static void UpdateDataTable(string sp, params SqlParameter[] sqlpara)
        {
            SqlCommand command = new SqlCommand(sp, CData.GetConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (sqlpara.Length > 0)
                command.Parameters.AddRange(sqlpara);
            command.Connection.Open();
            command.ExecuteNonQuery();
            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();
        }


        public static void UpdateDataTable(string sp, out int error, out int identity,params SqlParameter[] sqlpara)
        {
            error = 0;
            identity = 0;
            SqlCommand command = new SqlCommand(sp, CData.GetConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (sqlpara.Length > 0)
                command.Parameters.AddRange(sqlpara);
            command.Parameters.Add(DAL.CommonMethods.parameter("out_error_number", SqlDbType.Int));
            command.Parameters.Add(DAL.CommonMethods.parameter("out_identity", SqlDbType.Int));
            command.Connection.Open();
            command.ExecuteNonQuery();
            error = Convert.ToInt32(command.Parameters["out_error_number"].Value != DBNull.Value?command.Parameters["out_error_number"].Value:0);
            identity = Convert.ToInt32(command.Parameters["out_identity"].Value != DBNull.Value ? command.Parameters["out_identity"].Value : 0);
            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();
        }

        public static void UpdateDataTable(string sp, out int error, params SqlParameter[] sqlpara)
        {
            error = 0;
            SqlCommand command = new SqlCommand(sp, CData.GetConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (sqlpara.Length > 0)
                command.Parameters.AddRange(sqlpara);
            command.Parameters.Add(DAL.CommonMethods.parameter("out_error_number", SqlDbType.Int));
            command.Connection.Open();
            command.ExecuteNonQuery();
            error = Convert.ToInt32(command.Parameters["out_error_number"].Value);
            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();
        }
        #endregion
    }
}
