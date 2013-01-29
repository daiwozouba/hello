using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Data;
using System.Collections;

namespace DAL
{
    /// <summary>
    ///  create by hxl ,20120725
    /// </summary>
    public class DALBase : IDisposable
    {
        /// <summary>
        /// 保护变量，数据库连接
        /// </summary>
        protected SqlConnection Connection=null;

        /// <summary>
        /// 保护变量，数据库连接串
        /// </summary>
        protected string ConnectionString="";

        /// <summary>
        /// 构造函数
        /// </summary>
        public DALBase()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["testConStr"].ToString();
        }


        /// <summary>
        /// 构造函数
        /// <param name="pDatabaseConnectionString">数据库连接串</param>
        /// </summary>
        public DALBase(string _conString)
        {
            ConnectionString = _conString;
        }

        /// <summary>
        /// 析构函数，释放非托管资源
        /// </summary>
        ~DALBase()
        {
            try
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
            catch { }
            try
            {
                Dispose();
            }
            catch { }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        protected void Open()
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(ConnectionString);
            }
            if (Connection.State.Equals(ConnectionState.Closed))
            {
                Connection.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }

        /// <summary>
        /// 获取数据，返回一个SqlDataReader （调用后主意调用SqlDataReader.Close()）
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader GetDataReader(string SqlString)
        {
            Open();
            SqlCommand cmd = new SqlCommand(SqlString, Connection);
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 获取数据，返回一个DataSet
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sqlString)
        {
            Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, Connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            Close();
            return ds;
        }

        /// <summary>
        /// 获取数据，返回一个DataRow
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>DataRow</returns>
        public DataRow GetDataRow(string sqlString)
        {
            DataSet ds = GetDataSet(sqlString);
            ds.CaseSensitive = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 执行sql,返回影响的行数
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteSQL(string sqlString)
        {
            int count = -1;
            Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                count = cmd.ExecuteNonQuery();
            }
            catch
            {
                count = -1;
            }
            finally
            {
                Close();
            }
            return count;
        }

        /// <summary>
        /// 执行sql,一组sql 语句
        /// </summary>
        /// <param name="sqlString">一组sql 语句</param>
        /// <returns></returns>
        public bool ExecuteSQL(ArrayList sqlStrings)
        {
            bool success = true;
            Open();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction tran = Connection.BeginTransaction();
            cmd.Connection = Connection;
            cmd.Transaction = tran;
            try
            {
                foreach (string sql in sqlStrings)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
            catch
            {
                success = false;
                tran.Rollback();
            }
            finally
            {
                Close();
            }
            return success;
        }

        /// <summary>
        /// 更新一个数据表
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Cols">哈西表，键值为字段名，值为字段值</param>
        /// <param name="where">Where子句,如" where 1=1 "</param>
        /// <returns>更新是否成功</returns>
        public bool Update(string TableName, Hashtable Cols, string where)
        {
            int Count = 0;
            if (Cols.Count <= 0)
            {
                return true;
            }
            string Fields = "";
            foreach (DictionaryEntry item in Cols)
            {
                if (Count != 0)
                {
                    Fields += ",";
                }
                Fields += item.Key.ToString();
                Fields += "=";
                Fields += item.Value.ToString();
                Count++;
            }
            Fields += " ";
            string sqlStr = "update " + TableName + " set " + Fields + where;

            return Convert.ToBoolean(ExecuteSQL(sqlStr));
        }

        /// <summary>
        /// 插入数据到数据表
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Cols">哈西表，键值为字段名，值为字段值</param>
        /// <returns>插入是否成功</returns>
        public bool Insert(string TableName, Hashtable Cols)
        {
            int Count = 0;
            if (Cols.Count <= 0)
            {
                return true;
            }
            string Fields = "(";
            string Values = " values （";
            foreach (DictionaryEntry item in Cols)
            {
                if (Count != 0)
                {
                    Fields += ",";
                    Values += ",";
                }
                Fields += item.Key.ToString();
                Values += item.Value.ToString();
                Count++;
            }
            Fields += ") ";
            Values += ") ";
            string sqlStr = "insert into " + TableName + " " + Fields + Values;
            return Convert.ToBoolean(ExecuteSQL(sqlStr));
        }

        /// <summary>
        /// 取得分页数据,和总记录数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">where 条件,如" 1=1 and 2=2 "</param>
        /// <param name="orderBy">例如：ORDER BY 列名 DESC</param>
        /// <param name="fieldList">要查询的列</param>
        /// <param name="curpage">当前页，从0开始</param>
        /// <param name="pageRecord">每页的数据大小</param>
        /// <returns>Dataset,table[0]分页数据，table[1]是分页总记录数</returns>
        public DataSet GetPagingDataSet(string tableName, string where, string orderBy, string fieldList, int curpage, int pageRecord)
        {
            Open();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Connection;
            cmd.CommandText = "proc_XPaging";
            cmd.Parameters.AddWithValue("@tablename", tableName);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@orderby", orderBy);
            cmd.Parameters.AddWithValue("@fieldlist", fieldList);
            cmd.Parameters.AddWithValue("@curpage", curpage);
            cmd.Parameters.AddWithValue("@page_record", pageRecord);

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                Close();
            }
        }
    }
}
