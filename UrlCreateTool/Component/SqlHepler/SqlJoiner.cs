using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UrlCreateTool.Component.Model;
using UrlCreateTool.Common;

namespace UrlCreateTool.Component.SqlHepler
{
    public class SqlJoiner
    {
        public NewBaseParam param;
        /// <summary>
        /// 参数NewBaseParam，自动装配CurPage、PageSize、sort、order
        /// </summary>
        public NewBaseParam Param
        {
            get { return param; }
            set {
                param = value;
                this.CurPage = param.page;
                if (this.CurPage < 1) this.CurPage = 1;
                this.PageSize = param.rows;
                if (this.PageSize < 1) this.PageSize = 10;
            }
        } 
        public int PageSize { get; set; }
        public int CurPage { get; set; }
        public string Tables { get; set; }
        public string Fields { get; set; }
        public string Condition { get; set; }
        public string Orderby { get; set; }
        public string Groupby { get; set; }
        /// <summary>
        /// 是否返回总数SQL
        /// </summary>
        public bool isTotalCount { get; set; }
        /// <summary>
        /// id所属表
        /// </summary>
        public string strid { get; set; }
        /// <summary>
        /// 得到分页sql语句
        /// </summary>
        public string StrSql
        {
            get
            {
                Condition = Condition._IsNullOrEmpty()?"":Condition;
                Condition = Condition.Trim();
                //去除条件语句前后的and
                if (Condition.ToUpper().Equals("AND"))
                {
                    Condition = "";
                }
                else
                {
                    if (Condition.ToUpper().StartsWith("AND "))
                    {
                        Condition = Condition.Substring(4);
                    }
                    if (Condition.ToUpper().EndsWith(" AND"))
                    {
                        Condition = Condition.Substring(0, Condition.Length - 4);
                    }
                }
                this.Orderby = "";
                if (!param.sort._IsNullOrEmpty())
                {
                    this.Orderby = strid + param.sort + " " + param.order;
                }
                string sql = function.pagesql(PageSize, CurPage, Fields, Tables, Condition, Orderby, strid);
                if (isTotalCount)
                {
                    sql += ";select count(" + strid + "id) from " + Tables + (Condition._IsNullOrEmpty()?"":" where " + Condition);
                }
                return sql;
            }
        }
        /// <summary>
        /// 得到分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回分页数据对象PageData</returns>
        public PageData GetData<T>()
            where T:new()
        {
            PageData data = new PageData();
            using (conn con = new conn())
            {
                DataSet ds = con.dataset(this.StrSql);
                if (ds != null && ds.Tables.Count == 2)
                {
                    DataTable dtcount = ds.Tables[1];
                    if (dtcount != null && dtcount.Rows.Count > 0)
                    {
                        data.total = dtcount.Rows[0][0].ToStr().ToInt32();
                    }
                    DataTable dtdata = ds.Tables[0];
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        data.rows = ModelHandler<T>.FillModel(dtdata);
                        data.index = param.rows;
                        data.count = dtdata.Rows.Count;
                        data.currentPage = param.page;
                    }
                }
                else if (!isTotalCount)
                {
                    DataTable dtdata = ds.Tables[0];
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        data.rows = ModelHandler<T>.FillModel(dtdata);
                        data.index = param.rows;
                        data.count = dtdata.Rows.Count;
                        data.currentPage = param.page;
                    }
                }
            }
            if (data.rows == null) data.rows = new T();
            return data;
        }
    }
}
