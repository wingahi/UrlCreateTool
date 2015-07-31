using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace UrlCreateTool.Component.SqlHepler
{
    public class conn : IDisposable
    {
        public SqlTransaction tran;
        public SqlCommand sqlcmd;
        public SqlConnection connstr;
        public SqlDataReader dr;
        public string constr = "";
        public conn()
        {
            constr = System.Configuration.ConfigurationManager.ConnectionStrings["connstring"].ConnectionString.ToString();
            connstr = new SqlConnection(constr);
        }

        public conn(int i)
        {

            if (i == 2) constr = "";
            connstr = new SqlConnection(constr);
        }
        public conn(string sql)
        {
            constr = sql;
            connstr = new SqlConnection(constr);
        }

        public void OpenDB()
        {
            connstr = new SqlConnection(constr);
            if (connstr.State == ConnectionState.Closed)
                connstr.Open();
        }

        public void close()      //关闭数据库
        {
            if (dr != null)
                if (!dr.IsClosed)
                    dr.Close();
            connstr.Dispose();
            connstr.Close();
        }

        public SqlConnection getConn()
        {
            return connstr;
        }
        public void execsql(string sql)   //执行sql语句
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;

                cmd.ExecuteScalar();
                close();
            }
        }

        public bool exebscalar(string sql)   //执行sql语句 返回bool
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;

                if (cmd.ExecuteScalar() != null)
                {
                    close();
                    return true;
                }
                else
                {
                    close();
                    return false;
                }
            }
        }
        public string exesscalar(string sql)   //执行sql语句 返回string
        {
            string str = "";
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj.ToString() != "")
                    str = obj.ToString().Trim();
                close();
                return str;
            }
        }
        public int exeiscalar(string sql)   //执行sql语句 返回int
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            int i = 0;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;
                object obj=cmd.ExecuteScalar();
                if (obj != null && obj.ToString() != "")
                    i = Convert.ToInt32(obj);
                close();

                return i;
            }

        }


        public DataSet dataset(string sql)  //返回DataSet对象
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlDataAdapter rs = new SqlDataAdapter())
            {
                rs.SelectCommand = new SqlCommand();
                rs.SelectCommand.Connection = connstr;
                rs.SelectCommand.CommandText = "my";
                rs.SelectCommand.CommandType = CommandType.StoredProcedure;
                rs.SelectCommand.Parameters.Add("@sql", SqlDbType.NText).Value = sql;

                DataSet ds = new DataSet();
                rs.Fill(ds);
                close();
                return ds;
            }
        }
        
        public DataSet dataset(string TableNames, string PrimaryKey, string Fields, int PageSize, int PageIndex, int IsReCount, string strWhere, string Group, string Order)
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlDataAdapter rs = new SqlDataAdapter())
            {
                rs.SelectCommand = new SqlCommand();
                rs.SelectCommand.Connection = connstr;
                rs.SelectCommand.CommandText = "page";
                rs.SelectCommand.CommandType = CommandType.StoredProcedure;
                rs.SelectCommand.Parameters.Add("@TableNames", SqlDbType.NVarChar, 1000).Value = TableNames;
                rs.SelectCommand.Parameters.Add("@PrimaryKey", SqlDbType.NVarChar, 1000).Value = PrimaryKey;
                rs.SelectCommand.Parameters.Add("@Fields", SqlDbType.NVarChar, 1000).Value = Fields;
                rs.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                rs.SelectCommand.Parameters.Add("@CurrentPage", SqlDbType.Int).Value = PageIndex;
                rs.SelectCommand.Parameters.Add("@IsReCount", SqlDbType.Int).Value = IsReCount;
                rs.SelectCommand.Parameters.Add("@Filter", SqlDbType.NVarChar, 1000).Value = strWhere;
                rs.SelectCommand.Parameters.Add("@Group", SqlDbType.NVarChar, 1000).Value = Group;
                rs.SelectCommand.Parameters.Add("@Order", SqlDbType.NVarChar, 1000).Value = Order;
                DataSet ds = new DataSet();
                rs.Fill(ds);
                close();
                return ds;
            }
        }

        public DataView dataview(string sql) //返回DataView对象
        {
            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            DataSet ds = new DataSet();
            ds = dataset(sql);
            DataView dv = new DataView(ds.Tables[0]);
            return dv;
        }
        public SqlDataReader datareader(string sql) //返回DataReader对象
        {

            if (connstr.State == ConnectionState.Closed)
                OpenDB();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;
                dr = cmd.ExecuteReader();
                return dr;
            }
        }
        public static int getcount(string tablename)
        {
            using (conn con = new conn())
            {
                int i = con.exeiscalar("select count(classid) from " + tablename);
                return i;
            }
        }
        public static int getcount(string tablename, string tiaojian)
        {
            using (conn con = new conn())
            {
                int i = con.exeiscalar("select count(classid) from " + tablename + " where " + tiaojian);
                return i;
            }
        }
        public static int getcount(string tablename,string colName, string tiaojian)
        {
            using (conn con = new conn())
            {
                int i = con.exeiscalar("select count(" + colName + ") from " + tablename + " where " + tiaojian);
                return i;
            }
        }
        public static int maxid(string tablename)
        {
            using (conn con = new conn())
            {
                int i = con.exeiscalar("select max(id) from " + tablename);
                return i;
            }
        }
        public static int getivalue(string tablename, string getziduan, string tiaojian)
        {
            using (conn con = new conn())
            {
                int i = con.exeiscalar("select " + getziduan + " from " + tablename + " where " + tiaojian);
                return i;
            }
        }
        public static string getsvalue(string tablename, string getziduan, string tiaojian)
        {
            using (conn con = new conn())
            {
                string s = con.exesscalar("select " + getziduan + " from " + tablename + " where " + tiaojian);
                return s;
            }
        }
        public static string updatesvalue(string tablename, string ziduan, string tiaojian)
        {
            using (conn con = new conn())
            {
                string s = con.exesscalar("update " + tablename + " set " + ziduan + " where " + tiaojian);
                return s;
            }
        }

        public static void exesql(string sql)   //执行sql语句
        {
            using (conn con = new conn())
            {
                con.execsql(sql);
            }
        }
        public static int exeisql(string sql)   //执行sql语句
        {
            int i = 0;
            using (conn con = new conn())
            {
                i = con.exeiscalar(sql);
                return i;
            }
        }
        public static string exessql(string sql)
        {
            string s = "";
            using (conn con = new conn())
            {
                s = con.exesscalar(sql);
                return s;
            }
        }

        public static bool exebsql(string sql)
        {
            bool b = false;
            using (conn con = new conn())
            {
                b = con.exebscalar(sql);
                return b;
            }
        }

        public static DataTable getdatatable(string sql)
        {
            DataTable dt = new DataTable();
            using (conn con = new conn())
            {
                DataSet ds = con.dataset(sql);
                dt = ds.Tables[0];
                return dt;
            }
        }
        public static DateTime sgetCurrentDate()
        {
            string date = string.Empty;
            using (conn con = new conn())
            {
                date = con.exesscalar("select getdate()");
            }
            return DateTime.Parse(date);
        }
        public DateTime getCurrentDate()
        {
            string date = string.Empty;
            using (conn con = new conn())
            {
                date = con.exesscalar("select getdate()");
            }
            return DateTime.Parse(date);
        }
        //public int execsql(string cmdText,SqlParameter[] paras)   //执行sql语句
        //{
        //    int myop = 0;
        //    if (connstr.State == ConnectionState.Closed)
        //        OpenDB();
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connstr;
        //        cmd.CommandText = cmdText;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddRange(paras);
        //        myop = cmd.ExecuteNonQuery();
        //        close();
        //    }
        //    return myop;
        //}
        public static int execsql(string cmdText, SqlParameter[] paras)   //执行sql语句
        {
            int myop = 0;
            using (conn con = new conn())
            {
                if (con.connstr.State == ConnectionState.Closed)
                   con.OpenDB();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con.connstr;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    myop = cmd.ExecuteNonQuery();
                    con.close();
                }
            }
            return myop;
        }
        public static ArrayList execsql(string cmdText, SqlParameter[] paras, params string[] outparams)   //执行sql语句
        {
            int outid = 0;
            ArrayList array = new ArrayList();
            using (conn con = new conn())
            {
                if (con.connstr.State == ConnectionState.Closed)
                   con.OpenDB();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con.connstr;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(paras);
                    cmd.ExecuteNonQuery();
                    foreach (var item in outparams)
                    {
                        array.Add(cmd.Parameters[item].Value);
                    }
                    con.close();
                }
            }
            return array;
        }
        public static DataSet GetdatasetBySql(string sql)  //返回DataSet对象
        {
            using (conn con = new conn())
            {
                if (con.connstr.State == ConnectionState.Closed)
                    con.OpenDB();
                using (SqlDataAdapter rs = new SqlDataAdapter())
                {
                    rs.SelectCommand = new SqlCommand();
                    rs.SelectCommand.Connection = con.connstr;
                    rs.SelectCommand.CommandText = sql;
                    rs.SelectCommand.CommandType = CommandType.Text;
                    DataSet ds = new DataSet();
                    rs.Fill(ds);
                    con.close();
                    return ds;
                }
            }
        }
        public void OpenTransaction(conn con)
        {
            if (con.connstr.State == ConnectionState.Closed)
                con.OpenDB();
            this.tran = con.connstr.BeginTransaction();
            
        }
        public int ExcuteTran(string sql)   //执行sql语句 返回int
        {
            int i = 0;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Transaction = this.tran;
                cmd.Connection = connstr;
                cmd.CommandText = "my";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sql", SqlDbType.NText).Value = sql;
                var obj=cmd.ExecuteScalar();
                if (obj != null && obj.ToString() != "")
                    i = Convert.ToInt32(obj);
                return i;
            }

        }
        public void RollbackTran(conn con)
        {
            if (this.tran != null) this.tran.Rollback();
            if (con.connstr.State == ConnectionState.Open)
                con.close();
        }
        public void SumitTran(conn con)
        {
            if (this.tran != null) this.tran.Commit();
            if (con.connstr.State == ConnectionState.Open)
                con.close();
        }
        public void Dispose()
        {
            this.close();
        }
    }
}