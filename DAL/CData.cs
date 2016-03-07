using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class CData
    {
        private const string CATALOG = @"aggregate";
        private const string DATASOURCE = @"CAKE-PC\SQLEXPRESS";

        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder sqlb = new SqlConnectionStringBuilder();
            sqlb.IntegratedSecurity = true;
            sqlb.PersistSecurityInfo = false;
            sqlb.InitialCatalog = CATALOG;
            sqlb.DataSource = DATASOURCE;
            return sqlb.ConnectionString;
        }

        private static SqlConnection _Connection;

        public static SqlConnection GetConnection
        {
            get { return (_Connection = _Connection ?? new SqlConnection(GetConnectionString())); }
        }



        //public static SqlConnection GetConnection()
        //{
        //    return new SqlConnection(GetConnectionString());
        //}
    }
}
