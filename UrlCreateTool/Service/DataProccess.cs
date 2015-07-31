using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlCreateTool.LiteDB.OP;
using UrlCreateTool.LiteDB.Model;

namespace UrlCreateTool.Service
{
    /// <summary>
    /// 数据处理中心
    /// </summary>
    public sealed class DataProccess
    {
        public static IList<Record> GetRecordList()
        {
            return LiteDBOP.FindAll<Record>();
        }
        public static bool Delete(int docId)
        {
            return LiteDBOP.Delete(docId);
        }
        public static Record GetSimpleRecord(int docId)
        {
            return LiteDBOP.FindById<Record>(docId);
        }

        public static int Add(Record model)
        {
            return LiteDBOP.Save<Record>(model);
        }
    }
}
