using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UrlCreateTool.LiteDB.Model;
using UrlCreateTool.Common;
using UrlCreateTool.Service;
using UrlCreateTool.Component.SqlHepler;
using System.IO;
using System.Threading;

namespace UrlCreateTool
{
    public partial class Form1 : Form
    {
        private IList<Record> recordList = null;
        public string foldName = "Article";
        public int fileId = 1;
        public int maxCountPage = 1000;
        private int totalCount = 0,completeCount = 0;
        public IList<Thread> threadList = new List<Thread>();
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadData();
        }

       
        private void LoadData()
        {
            recordList = DataProccess.GetRecordList();
            if (recordList != null)
            {
                foreach (var item in recordList)
                {
                    txtotalUrlTemplate.AppendText(item.TemplateUrl + "--" + item.FieldsAndTables + "\r\n");
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            IList<Record> _list = DealString(txtotalUrlTemplate.Text.Trim());
            foreach (var item in _list)
            {
                Util util = new Util();
                util.extendName = txtExtendName.Text.Trim();
                util.item = item;
                util.foldName = txtDir.Text.Trim();
                util.maxCountPage = txtSimgleCount.Text.Trim().ToInt32();
                util.showProccessState = ShowState;
                Thread th = new Thread(util.StartRunning);
                threadList.Add(th);
                th.Start();
            }
        }
        public void ShowState(int totalcount, int completeCount, bool isover)
        {
            txtLogs.Text = "总数：" + totalcount + "\r\n完成数：" + completeCount + "\r\n" + (isover?"已完成":"生成中");
        }
        private void BtnAddNewRecord_Click(object sender, EventArgs e)
        {
            string urlTemplate = txtUrlTemplate.Text.Trim();
            string fieldAndTables = txtTables.Text.Trim();
            if (urlTemplate._IsNullOrEmpty() || fieldAndTables._IsNullOrEmpty())
            {
                MessageBox.Show("url模板或表和字段为空！");
                return;
            }
            Record record = new Record();
            record.TemplateUrl = urlTemplate;
            record.FieldsAndTables = fieldAndTables;
            int docId = DataProccess.Add(record);
            if (docId > 0)
            {
                MessageBox.Show("添加成功！");
            }
            else
            {
                MessageBox.Show("添加失败！");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private IList<Record> DealString(string strText)
        {
            IList<Record> recordList = null;
            if (!strText._IsNullOrEmpty())
            {
                string[] _recordList = strText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (_recordList != null && _recordList.Count() > 0)
                {
                    recordList = new List<Record>();
                    foreach (var item in _recordList)
                    {
                        string[] itemArray = item.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
                        if (itemArray.Length == 2)
                        {
                            Record record = new Record();
                            record.TemplateUrl = itemArray[0];
                            record.FieldsAndTables = itemArray[1];
                            recordList.Add(record);
                        }
                    }
                }
            }
            return recordList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in threadList)
            {
                if (item.IsAlive)
                {
                    item.Abort();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //private string FromSql(Record record)
        //{
        //    StringBuilder sbsql = new StringBuilder();

        //    if(!record.FieldsAndTables._IsNullOrEmpty())
        //    {
        //        string[] sqlArray = record.FieldsAndTables.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        //        if (sqlArray.Length > 0)
        //        {
        //            foreach (var item in sqlArray)
        //            {
        //                string[] sqlItem = item.Split('-');
        //                if (sqlItem.Length == 2)
        //                {
        //                    sbsql.AppendLine(SqlContact(sqlItem[0],sqlItem[1]));
        //                }
        //                else if (sqlItem.Length == 3)
        //                {
        //                    sbsql.AppendLine(SqlContact(sqlItem[0], sqlItem[1],sqlItem[2]));
        //                }
        //            }
        //        }
        //    }
        //    return sbsql.ToStr();
        //}
        //private string SqlContact(string fields, string tables, string where = "", int page=1, int count=1)
        //{
        //    //return function.pagesql(count, page, fields, tables, "", "", "");
        //    return "select " + fields + " from " + tables + (where._IsNullOrEmpty() ? "" : " where " + where) + ";";
        //}
        //private DataTable[] GetData(string sqlstr)
        //{
        //    DataSet ds = null;
        //    DataTable[] dtArray=null;
        //    using (conn con = new conn())
        //    {
        //        ds = con.dataset(sqlstr);
        //    }
        //    if (ds != null)
        //    {
        //        dtArray = new DataTable[ds.Tables.Count];
        //        int index = 0;
        //        totalCount = 1;
        //        foreach (DataTable dtitem in ds.Tables)
        //        {
        //            if (dtitem.Rows.Count > 0)
        //            {
        //                totalCount *= dtitem.Rows.Count;
        //            }
        //            dtArray[index++] = dtitem;
        //        }
        //    }
        //    return dtArray;
        //}
        ///// <summary>
        ///// 生成方法
        ///// </summary>
        ///// <param name="urlTemplate"></param>
        ///// <param name="dtArray"></param>
        //private void CreateUrl(string urlTemplate,DataTable[] dtArray)
        //{
        //    if (urlTemplate._IsNullOrEmpty()) return;
        //    StringBuilder sburl=new StringBuilder();
        //    IList<string> listUrl = new List<string>();
        //    int appendCount = 0;
        //    if (dtArray != null && dtArray.Count()>0)
        //    {
        //        int[] index = new int[dtArray.Count()];
        //        string[] colNameArray = null;
        //        colNameArray = GetColumnName(dtArray[0]);
        //        for (int _index = 0; _index < dtArray[0].Rows.Count; _index++)
        //        {
        //            if (colNameArray != null)
        //            {
        //                string url = urlTemplate;
        //                foreach (var item in colNameArray)
        //                {
        //                    url = url.Replace("$" + item, dtArray[0].Rows[_index][item].ToStr());
        //                }
        //                listUrl.Add(url);
        //            }
        //            //遍历其他表
        //            for (int _index1 = 1; _index1 < index.Length;_index1++)
        //            {
        //                colNameArray = GetColumnName(dtArray[_index1]);
        //                int count = listUrl.Count;

        //                for (int url_index = 0; url_index < count; url_index++)
        //                {
        //                    foreach (DataRow dataRow in dtArray[_index1].Rows)
        //                    {
        //                        //foreach(var item_urlTempalte in listUrl)

        //                        if (colNameArray != null)
        //                        {
        //                            string url = listUrl[url_index];
        //                            foreach (var item in colNameArray)
        //                            {
        //                                if (listUrl.FirstOrDefault(s => s.Contains("$" + item)) != null)
        //                                {
        //                                    url = url.Replace("$" + item, dataRow[item].ToStr());
        //                                }
        //                            }
        //                            listUrl.Add(url);
        //                            if (listUrl.Count > maxCountPage)
        //                            {
        //                                var _listUrl = listUrl.Where(s => !s.Contains("$")).ToList();
        //                                if (_listUrl != null && _listUrl.Count > maxCountPage)
        //                                {
        //                                    listUrl = listUrl.Where(s => s.Contains("$")).ToList();
        //                                    appendCount = 0;
        //                                    completeCount += _listUrl.Count;
        //                                    for (int tindex = 0; tindex < _listUrl.Count; tindex++)
        //                                    {
        //                                        sburl.AppendLine(_listUrl[tindex]);
        //                                        appendCount++;
        //                                        if (appendCount / maxCountPage == 0 || tindex == _listUrl.Count)
        //                                        {
        //                                            WriteFile(sburl.ToStr(), foldName);
        //                                            sburl.Clear();
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                dtArray[_index1] = null;
        //                foreach (var item in colNameArray)
        //                {
        //                    listUrl = listUrl.Where(s => !s.Contains("$" + item)).ToList();
        //                }
        //            }
                    
        //            var _listUrl1 = listUrl.Where(s => !s.Contains("$")).ToList();
        //            if (_listUrl1 != null && _listUrl1.Count > maxCountPage)
        //            {
        //                listUrl = listUrl.Where(s => s.Contains("$")).ToList();
        //                completeCount += _listUrl1.Count;
        //                foreach (var itemurl in _listUrl1)
        //                {
        //                    sburl.AppendLine(itemurl);
        //                }
        //                appendCount++;
        //                WriteFile(sburl.ToStr(), foldName);
        //                sburl.Clear();
        //            }
        //        }
        //        listUrl = listUrl.Where(s => !s.Contains("$")).ToList();
        //        if (listUrl != null && listUrl.Count > 0)
        //        {
        //            completeCount += listUrl.Count;
        //            foreach (var itemurl in listUrl)
        //            {
        //                sburl.AppendLine(itemurl);
        //            }
        //            appendCount++;
        //            WriteFile(sburl.ToStr(), foldName);
        //            sburl.Clear();
        //        }
        //    }

        //}
        //private string[] GetColumnName(DataTable dt)
        //{
        //    string[] colNameArray = null;
        //    if (dt != null)
        //    {
        //         DataColumnCollection column = dt.Columns;
        //         colNameArray = new string[column.Count];
        //         for (int index = 0; index < column.Count; index++)
        //         {
        //             colNameArray[index] = column[index].ColumnName;
        //         }
        //    }
        //    return colNameArray;
        //}
        ///// <summary>
        ///// 保存文件
        ///// </summary>
        ///// <param name="html"></param>
        //private bool WriteFile(string contents,string foldName)
        //{
        //    try
        //    {
        //        if (!System.IO.Directory.Exists(foldName))
        //        {
        //            DirectoryInfo DirInfo = Directory.CreateDirectory(foldName); //创建目录
        //            DirInfo.Attributes = FileAttributes.Normal;
        //        }
        //        string fullpath = foldName + "//" + fileId + ".txt";
        //        if (File.Exists(fullpath))
        //        {
        //            File.Delete(fullpath);
        //        }
        //        fileId++;
        //        File.WriteAllText(fullpath, contents);
        //        ShowState myDelegate = delegate(string totalcount, string completedCount)
        //        {
        //            txtLogs.Text += "总共数：" + totalcount + "\r\n完成数：" + completedCount;
        //        };
        //        txtLogs.Invoke(myDelegate, totalCount, completeCount);
        //        return true;
        //    }
        //    catch
        //    {
        //        ShowState myDelegate = delegate(string totalcount, string errorCount)
        //        {
        //            txtLogs.Text += "总共数：" + totalcount + "\r\n出现错误！";
        //        };
        //        txtLogs.Invoke(myDelegate, totalCount, 0);
        //        using (StreamWriter sw = new StreamWriter(DateTime.Now.ToString("yyyyMMdd") + ".txt"))
        //        {
        //            sw.Write(contents);
        //        }
        //        return false;
        //    }
        //}
    }
}
