using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlCreateTool.Component.Model
{
    public class PageData
    {
        /// <summary>
        /// 目前页
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int index { get;set;}
        /// <summary>
        /// 该页数据量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object rows { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int total { get; set; }
    }
}
