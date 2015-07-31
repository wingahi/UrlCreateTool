using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlCreateTool.LiteDB.Model
{
    /// <summary>
    /// 生成记录实体
    /// </summary>
    public class Record
    {
        public int Id { get; set; }
        /// <summary>
        /// 生成url模板
        /// </summary>
        public string TemplateUrl { get; set; }
        /// <summary>
        /// 生成表
        /// </summary>
        public string FieldsAndTables { get; set; }
    }
}
