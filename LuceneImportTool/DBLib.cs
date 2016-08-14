using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LuceneImportTool
{
    class DBLib
    {
        private string mConnectionString = "";
        private const int COMMANDTIMEOUT = 30;	//Command Time Out

        public DBLib(string sql)
        {
            this.mConnectionString = sql;
        }

        #region GetDataSetBySQL(3)
        /// <summary>
        /// 通过SQL语句得到DataSet
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <returns></returns>
        public DataSet GetDataSetBySQL(string Sql)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 通过SQL语句得到DataSet
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <param name="Conn">传入的数据库连接对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySQL(string Sql, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 通过SQL语句得到DataSet
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <param name="Conn">传入的数据库连接对象</param>
        /// <param name="Tran">传入的数据库事务对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySQL(string Sql, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, Conn);
                cmd.Transaction = Tran;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        #endregion

        #region ExecuteSQL(3)
        /// <summary>
        /// 执行没有返回值的SQL语句（自带事务控制）
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        public void ExecuteSQL(string Sql)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = this.CreateConnection();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                cmd.Transaction = tran;
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql;
                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行没有返回值的SQL语句
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <param name="Conn">传入的数据库连接对象</param>
        public void ExecuteSQL(string Sql, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 执行没有返回值的SQL语句
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <param name="Conn">传入的数据库连接对象</param>
        /// <param name="Tran">传入的数据库事务对象</param>
        public void ExecuteSQL(string Sql, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, Conn);
                cmd.Transaction = Tran;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        #endregion

        #region GetDataSetBySP(6)
        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP, SqlParameter[] Par)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <param name="Tran">数据库事务对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn, Tran);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <param name="Tran">数据库事务对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP, SqlParameter[] Par, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn, Tran);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        /// <summary>
        /// 通过存储过程得到DataSet
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <returns></returns>
        public DataSet GetDataSetBySP(string SP, SqlParameter[] Par, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        #endregion

        #region ExecuteSP(6)
        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <returns></returns>
        public void ExecuteSP(string SP)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <returns></returns>
        public void ExecuteSP(string SP, SqlParameter[] Par)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <param name="Tran">数据库事务对象</param>
        /// <returns></returns>
        public void ExecuteSP(string SP, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn, Tran);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <param name="Tran">数据库事务对象</param>
        /// <returns></returns>
        public void ExecuteSP(string SP, SqlParameter[] Par, SqlConnection Conn, SqlTransaction Tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn, Tran);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <returns></returns>
        public void ExecuteSP(string SP, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 执行没有返回值的存储过程
        /// </summary>
        /// <param name="SP">存储过程名</param>
        /// <param name="Par">参数数组</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <returns></returns>
        public void ExecuteSP(string SP, SqlParameter[] Par, SqlConnection Conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SP, Conn);
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        #endregion


        #region 数据库连接对象(2)
        /// <summary>
        /// 建立数据库连接对象
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateConnection()
        {
            try
            {
                SqlConnection cn = new SqlConnection(mConnectionString);
                cn.Open();
                return cn;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 释放数据库连接
        /// </summary>
        /// <param name="Conn">需要释放的数据库连接对象</param>
        public void Dispose(SqlConnection Conn)
        {
            try
            {
                if (Conn != null)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
                GC.Collect();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        #endregion

        #region 得到第一条记录第一列字段的值(3)
        /// <summary>
        /// 得到第一条记录第一列字段的值
        /// </summary>
        /// <param name="Sql">传入的SQL语句</param>
        /// <returns>没找到相应的值时返回空字符串</returns>
        public string GetFirstCellBySql(string Sql)
        {
            string ReturnValue = "";
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql;
                Object o = cmd.ExecuteScalar();
                if (o != null && o != System.DBNull.Value)
                {
                    ReturnValue = o.ToString();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ReturnValue;
        }

        /// <summary>
        /// 得到第一条记录第一列字段的值
        /// </summary>
        /// <param name="SP">存储过程名称</param>
        /// <returns>没找到相应的值时返回空字符串</returns>
        public string GetFirstCellBySP(string SP)
        {
            string ReturnValue = "";
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                Object o = cmd.ExecuteScalar();
                if (o != null && o != System.DBNull.Value)
                {
                    ReturnValue = o.ToString();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ReturnValue;
        }

        /// <summary>
        /// 得到第一条记录第一列字段的值
        /// </summary>
        /// <param name="SP">存储过程名称</param>
        /// <param name="Par">参数数组</param>
        /// <returns>没找到相应的值时返回空字符串</returns>
        public string GetFirstCellBySP(string SP, SqlParameter[] Par)
        {
            string ReturnValue = "";
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection cn = this.CreateConnection();
                cmd.Connection = cn;
                cmd.CommandTimeout = COMMANDTIMEOUT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SP;
                for (int i = 0; i <= Par.GetUpperBound(0); i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }
                Object o = cmd.ExecuteScalar();
                if (o != null && o != System.DBNull.Value)
                {
                    ReturnValue = o.ToString();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return ReturnValue;
        }

        #endregion

        #region ICommonUtil 成员

        public DataTable Select(int top, string tableName, string selectWhere, string orderField)
        {
            StringBuilder sql = new StringBuilder("SELECT");
            if (top > 0)
            {
                sql.AppendFormat(" TOP {0}", top);
            }
            sql.AppendFormat(" * FROM {0}", tableName);
            if (!string.IsNullOrEmpty(selectWhere))
            {
                sql.AppendFormat(" WHERE {0}", selectWhere);
            }
            if (!string.IsNullOrEmpty(orderField))
            {
                sql.AppendFormat(" ORDER BY {0}", orderField);
            }
            return GetDataSetBySQL(sql.ToString()).Tables[0];

        }

        #endregion
    }
}
