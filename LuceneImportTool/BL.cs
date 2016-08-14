using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LuceneImportTool
{
    public class BL
    {
        public DataTable GetProductTable()
        {
            string sql = @"SELECT [ID],[question],[answer] FROM [AQuestion]";

            return new DBLib(ConfigurationManager.ConnectionStrings["QAConnect"].ToString()).GetDataSetBySQL(sql).Tables[0];
        }
    }
}