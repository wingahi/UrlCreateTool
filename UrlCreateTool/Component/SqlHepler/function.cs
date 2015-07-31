using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
namespace UrlCreateTool.Component.SqlHepler
{
    public class function
    {
        public static string sqlstr(string sql)
        {
            string SqlStr = "exec |insert |select |delete |update |truncate |declare |' or '|drop table|creat table|--|' and|and '";
            string[] anySqlStr = SqlStr.Split('|');
            foreach (string ss in anySqlStr)
            {
                if (sql.ToLower().IndexOf(ss) >= 0)
                {

                    sql = sql.ToLower().Replace("'", "''");
                    conn.exesql("insert into sqlfilter(sqlsql,ip,url,addtime) values('" + sql + "','','','" + DateTime.Now + "')");
                    sql = sql.Replace(ss, "");
                }
            }
            return sql;
        }
        public static DataTable SplitDataTable(DataTable dt, int PageIndex, int PageSize)//DataTable分页
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Clone();
            //newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        public static DataTable getDataTable(DataTable dt, string tiaojian, string order)//根据条件和排序筛选数据
        {
            DataRow[] dr = dt.Select(tiaojian);
            if (dr.Length > 0)
            {
                dt = dr.CopyToDataTable();
                if (order != "")
                {
                    dt.DefaultView.Sort = order;
                    dt = dt.DefaultView.ToTable();
                }

            }
            else
            {
                dt=dt.Clone();
            }
            return dt;
        }

        public static DataTable getDataTableGroupBy(DataTable dt, int ii, string ziduan)//根据字段分组DataTable取几条数据
        {

            DataTable dtnew = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                if (dtnew.Select(ziduan + "='" + dr[ziduan] + "'").Length < ii)
                    dtnew.Rows.Add(dr.ItemArray);
            }
            return dtnew;
        }



        public static double getSimilarity1(String doc1, String doc2)			//两篇文章相似度
        {
            if (doc1 == null || doc2 == null || doc1.Trim().Length == 0 || doc2.Trim().Length == 0)
            {
                //throw new Exception();
                return 0;
            }
            System.Collections.Hashtable algorithMap = new System.Collections.Hashtable();
            char[] doc1Chars = doc1.ToCharArray();
            char[] doc2Chars = doc2.ToCharArray();
            for (int i = 0; i < doc1.Length; i++)
            {
                char d1 = doc1Chars[i];
                if (isHanZi(d1))
                {
                    int charIndex = getGB2312Id(d1);
                    if (charIndex != -1)
                    {
                        int[] fq = null;
                        try
                        {
                            fq = (int[])algorithMap[charIndex];
                        }
                        catch (Exception exx)
                        {
                            Console.WriteLine(exx.Message);
                        }
                        finally
                        {
                            if (fq != null && fq.Length == 2)
                            {
                                fq[0]++;
                            }
                            else
                            {
                                fq = new int[2];
                                fq[0] = 1;
                                fq[1] = 0;
                                algorithMap.Add(charIndex, fq);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < doc2.Length; i++)
            {
                char d2 = doc2Chars[i];
                if (isHanZi(d2))
                {
                    int charIndex = getGB2312Id(d2);
                    if (charIndex != -1)
                    {
                        int[] fq = null;
                        try
                        {
                            fq = (int[])algorithMap[charIndex];
                        }
                        catch (Exception eee)
                        {
                            Console.WriteLine(eee.Message);
                        }
                        finally
                        {
                            if (fq != null && fq.Length == 2)
                            {
                                fq[1]++;
                            }
                            else
                            {
                                fq = new int[2];
                                fq[0] = 0;
                                fq[1] = 1;
                                algorithMap.Add(charIndex, fq);
                            }
                        }
                    }
                }
            }
            double sqdoc1 = 0;
            double sqdoc2 = 0;
            double denominator = 0;
            foreach (System.Collections.DictionaryEntry par in algorithMap)
            {
                int[] c = (int[])par.Value;
                denominator += c[0] * c[1];
                sqdoc1 += c[0] * c[0];
                sqdoc2 += c[1] * c[1];
            }
            if (sqdoc1 != 0 && sqdoc2 != 0)
            {
                return denominator / Math.Sqrt(sqdoc1 * sqdoc2);
            }
            else return 0;
        }


        public static bool isHanZi(char ch)
        {
            // 判断是否汉字  
            //return (ch >= 0x4E00 && ch <= 0x9FA5);
            return true;
        }

        /** 
         * 根据输入的Unicode字符，获取它的GB2312编码或者ascii编码， 
         *  
         * @param ch 
         *            输入的GB2312中文字符或者ASCII字符(128个) 
         * @return ch在GB2312中的位置，-1表示该字符不认识 
         */
        public static short getGB2312Id(char ch)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(ch.ToString());
                if (buffer.Length != 2)
                {
                    // 正常情况下buffer应该是两个字节，否则说明ch不属于GB2312编码，故返回'?'，此时说明不认识该字符  
                    return -1;
                }
                int b0 = (int)(buffer[0] & 0x0FF) - 161; // 编码从A1开始，因此减去0xA1=161  
                int b1 = (int)(buffer[1] & 0x0FF) - 161; // 第一个字符和最后一个字符没有汉字，因此每个区只收16*6-2=94个汉字  
                return (short)(b0 * 94 + b1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }
        public static string pagesql(int pagesize, int p, string ziduan, string biao, string tiaojian, string px, string id)    //最新完美版
        {
            //ziduan 查询字段
            //biao 表名
            //tiaojian 查询条件
            //pagesize 分页大小
            //p 当前页
            //pxzd 排序字段
            //px 排序条件

            string str = "select ";
            if (pagesize > 0) str += "top " + pagesize + " ";
            if (ziduan != "") str += ziduan + " ";
            else str += "* ";
            str += "from " + biao + " where ";
            if (tiaojian != "") str += tiaojian + " and ";
            if (p > 1)
            {
                str += id + "id not in(select top " + pagesize * (p - 1) + " " + id + "id from " + biao + " ";
                if (tiaojian != "") str += "where " + tiaojian + " ";
                if (px != "") str += "order by " + px + " ";
                str += ")";
            }
            else str += " 1=1 ";
            if (px != "") str += "order by " + px + " ";
            return str;
        }

        public static string page(int count, int n, string url, string url1, int o, string u1, int pagesize) //分页 有上一页下一页
        {
            //count 总页数
            //n 当前页
            //url 网址前缀
            //url1 网址后缀
            //u1 第一页网址
            //o  第几页作为第一页
            //pagesize 一页显示多少个

            string str = "";
            //str += "第(" + n + "/" + count + ")页 ";
            if (count > 1)
            {
                if (o > 0 && n != o) str += "<a href=\"" + u1 + "\" rel=\"nofollow\">首页</a> "; ;
                if (n != 1)
                {
                    if (n == 2) str += "<a href=\"" + u1 + "\" rel=\"nofollow\">上一页</a> ";
                    else str += "<a href=\"" + url + (n - 1) + url1 + "\" rel=\"nofollow\">上一页</a> ";
                }
                if (count <= pagesize)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        if (i == n)
                            str += " <a class=\"action\">" + i + "</a> ";
                        else if (i == o)
                            str += "<a href=\"" + u1 + "\">" + i + "</a> ";
                        else
                            str += "<a href=\"" + url + i + url1 + "\">" + i + "</a> ";
                    }
                }
                else
                {
                    if (n < (pagesize / 2 + 1))
                    {
                        if (n <= pagesize - n)
                        {
                            for (int i = 1; i <= pagesize; i++)
                            {
                                if (i == n)
                                    str += " <a class=\"action\">" + i + "</a> ";
                                else
                                    if (i == o)
                                        str += "<a href=\"" + u1 + "\">" + i + "</a> ";
                                    else
                                        str += "<a href=\"" + url + i + url1 + "\">" + i + "</a> ";
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= n + (pagesize / 2 + 1); i++)
                            {
                                if (i == n)
                                    str += " <a class=\"action\">" + i + "</a> ";
                                else
                                    if (i == o)
                                        str += "<a href=\"" + u1 + "\">" + i + "</a> ";
                                    else
                                        str += "<a href=\"" + url + i + url1 + "\">" + i + "<a> ";
                            }
                        }
                    }
                    else
                    {
                        if (count - n < (pagesize / 2))
                        {
                            for (int i = n - (pagesize / 2); i <= count; i++)
                            {
                                if (i == n)
                                    str += " <a class=\"action\">" + i + "</a> ";
                                else
                                    if (i == o)
                                        str += "<a href=\"" + u1 + "\">" + i + "</a> ";
                                    else
                                        str += "<a href=\"" + url + i + url1 + "\">" + i + "</a> ";
                            }
                        }
                        else
                        {
                            for (int i = n - (pagesize / 2); i < n + (pagesize / 2 + 1); i++)
                            {
                                if (i == n)
                                    str += " <a class=\"action\">" + i + "</a> ";
                                else
                                    if (i == o)
                                        str += "<a href=\"" + u1 + "\">" + i + "</a> ";
                                    else
                                        str += "<a href=\"" + url + i + url1 + "\">" + i + "</a> ";
                            }
                        }
                    }
                }

                if (n != count)
                {
                    str += " <a href=\"" + url + (n + 1) + url1 + "\" rel=\"nofollow\">下一页</a>";
                    str += " <a href=\"" + url + count + url1 + "\" rel=\"nofollow\">末页</a>";
                }
            }
            else str += " <a class=\"action\">1</a> ";
            return str;
        }
        public static string addurl(string content)
        {
            string str = "";
            string rex = "(?:[\\\"\\'\\=\\>\\s])?((http|https|ftp)\\://)?([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&amp;%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&amp;%\\$#\\=~_\\-\\u4e00-\\u9fa5\\/]+))*(?:[\\\"\\'\\<\\/])?";
            Regex url = new Regex(@rex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = url.Matches(content);

            for (int k = 0; k < mc.Count; k++)
            {
                string murl = mc[k].ToString();
                if (murl.IndexOf("\"") > -1 || murl.IndexOf("\'") > -1)
                {
                    str += function.subfront(content, murl).Replace("没有数据", "");
                    content = function.movefront(content, murl).Replace("没有数据", "");
                    str += murl;
                }
                else
                {
                    if (murl.Substring(0, 1) == "=")
                    {
                        str += function.subfront(content, murl).Replace("没有数据", "");
                        content = function.movefront(content, murl).Replace("没有数据", "");
                        str += murl;
                    }
                    else
                    {
                        if (murl.Substring(0, 1) == ">")
                        {
                            murl = murl.Remove(0, 1);
                        }
                        if (murl.Substring(murl.Length - 1, 1) == "<")
                        {
                            murl = murl.Substring(0, murl.Length - 1);
                        }

                        if (murl.ToLower().EndsWith("jpg") || murl.ToLower().EndsWith("gif") || murl.ToLower().EndsWith("png"))
                        {
                            str += function.subfront(content, murl).Replace("没有数据", "");
                            content = function.movefront(content, murl).Replace("没有数据", "");
                            str += "<a href=\"" + murl + "\" target=\"_blank\"><img src=\"" + murl + "\" /></a>";

                        }
                        else
                        {
                            str += function.subfront(content, murl).Replace("没有数据", "");
                            content = function.movefront(content, murl).Replace("没有数据", "");
                            str += "<a href=\"" + murl + "\" target=\"_blank\">" + murl + "</a>";

                        }
                    }
                }

            }
            str += content;
            return str;
        }
        public static string movefront(string s1, string s2)		//去掉前面的 包含
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Remove(0, s1.IndexOf(s2) + s2.Length);
            else s = "没有数据";
            return s;
        }

        public static string movebehind(string s1, string s2)	//去掉后面的 包含
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Remove(s1.IndexOf(s2), s1.Length - s1.IndexOf(s2));
            else s = "没有数据";
            return s;
        }

        public static string movefront1(string s1, string s2)	//去掉前面的
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Remove(0, s1.IndexOf(s2));
            else s = "没有数据";
            return s;
        }

        public static string movebehind1(string s1, string s2)	//去掉后面的 
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Remove(s1.IndexOf(s2) + s2.Length, s1.Length - s1.IndexOf(s2) - s2.Length);
            else s = "没有数据";
            return s;
        }

        public static string subfront(string s1, string s2)		//截取前面的 不包含
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Substring(0, s1.IndexOf(s2));
            else s = "没有数据";
            return s;
        }
        public static string subfront1(string s1, string s2)		//截取前面的
        {
            string s = "";
            if (s1.IndexOf(s2) > -1)
                s = s1.Substring(0, s1.IndexOf(s2) + s2.Length);
            else s = "没有数据";
            return s;
        }

        public static string subbehind(string s1, string s2)	//截取后面的 包含
        {
            string s = "";
            if (s1.LastIndexOf(s2) > -1)
                s = s1.Substring(s1.LastIndexOf(s2) + s2.Length, s1.Length - s1.LastIndexOf(s2) - s2.Length);
            else s = "没有数据";
            return s;
        }

        public static string subbehind1(string s1, string s2)	//截取后面的
        {
            string s = "";
            if (s1.LastIndexOf(s2) > -1)
                s = s1.Substring(s1.LastIndexOf(s2), s1.Length - s1.LastIndexOf(s2));
            else s = "没有数据";
            return s;
        }

        public static string substring(string s0, string s1, string s2)	//截取字符串 
        {
            string s = "";
            if (s0.IndexOf(s1) > -1 && s0.IndexOf(s2) > -1 && s0.IndexOf(s1) < s0.IndexOf(s2))
                s = s0.Substring(s0.IndexOf(s1) + s1.Length, s0.IndexOf(s2) - s0.IndexOf(s1) - s1.Length);
            else
            {
                s = "没有数据";
            }
            return s;
        }

        public static string substring1(string s0, string s1, string s2)	//截取字符串
        {
            string s = "";
            if (s0.IndexOf(s1) > -1 && s0.IndexOf(s2) > -1 && s0.IndexOf(s1) < s0.IndexOf(s2))
                s = s0.Substring(s0.IndexOf(s1), s0.IndexOf(s2) - s0.IndexOf(s1) + s2.Length);
            else
            {
                s = "没有数据";
            }
            return s;
        }

        public static string htmlcode(string str)	//去除html标签
        {
            str = str.Replace("&nbsp;", "").Replace("&amp;nbsp;", "").Replace("&#160;", "");
            int sum = 0, sum1 = -1;
            char c;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                c = Convert.ToChar(str.Substring(i, 1));
                if (c == '<' && sum == 0)
                {
                    str = str.Remove(i, str.Length - i);
                    sum = 0;
                }
                if (c == '>' && sum == 0)
                {
                    sum1 = i;
                    sum = 1;
                }
                if (c == '<' && sum == 1)
                {
                    str = str.Remove(i, sum1 - i + 1);
                    sum = 0;
                }



            }
            return str;
        }
        public static string rand(int ii, int l)
        {
            int number;
            char code;
            string str = String.Empty;

            System.Random random = new Random();
            for (int i = 0; i < ii; i++)
            {
                number = random.Next();
                if (l == 0)
                {
                    if (number % 2 == 0)
                        code = (char)('0' + (char)(number % 10));
                    else
                        code = (char)('A' + (char)(number % 26));
                    str += code.ToString();
                }
                else if (l == 1)
                {
                    code = (char)('0' + (char)(number % 10));
                    str += code.ToString();
                }
                else if (l == 2)
                {
                    code = (char)('A' + (char)(number % 26));
                    str += code.ToString();
                }
                else if (l == 3)
                {
                    code = (char)('a' + (char)(number % 26));
                    str += code.ToString();
                }
            }

            return str;
        }
        public static string enc(string Text)		//加密
        {
            string sKey = "zfy4jfl0";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public static string dec(string Text) //解密
        {
            string sKey = "zfy4jfl0";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        public static string enc(string Text, string sKey)		//加密
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public static string dec(string Text, string sKey) //解密
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        public static string getstr1(string str, int len) //获取第一段
        {
            string s = "";

            s = s.Replace("&nbsp;", "");
            if (str.IndexOf("<br/>") > -1)
                s = str.Substring(0, str.IndexOf("<br/>"));
            else s = str;

            return s;

        }
        public static string getstr2(string str, int len) //随机第一段或第二段
        {
            string s = "";

            s = s.Replace("&nbsp;", "");
            if (str.IndexOf("<br/>") > -1)
            {
                Random r = new Random(DateTime.Now.Second);
                if (r.Next() % 2 == 1) s = str.Substring(0, str.IndexOf("<br/>"));
                else
                    s = str.Substring(str.IndexOf("<br/>") + 5, str.Length - str.IndexOf("<br/>") - 5);
            }
            else s = str;

            if (s.Length < 10) s = str;
            return s;

        }
        public static string getstr12(string str, int len) //随机第一段或第二段
        {
            string s = "";

            s = s.Replace("&nbsp;", "");
            if (str.IndexOf("<br/>") > -1)
            {
                Random r = new Random(DateTime.Now.Second);
                if (r.Next() % 2 == 1) s = str.Substring(0, str.IndexOf("<br/>"));
                else s = str.Substring(str.IndexOf("<br/>") + 5, str.Length - str.IndexOf("<br/>") - 5);
            }
            else s = str;

            if (s.Length < 10) s = str;
            return s;

        }
        public static string getstring(string str, int len) //获取字符串有...
        {
            if (len > 0)
                if (str.Length > len) str = str.Substring(0, len) + "…";

            return str;
        }
        public static string getstr(string str, int len) //截取字符串
        {
            if (str != null)
                if (len > 0)
                    if (str.Length > len) str = str.Substring(0, len);

            return str;

        }
        public static int random(int i1, int i2)
        {
            int i = 0;
            Random r = new Random(Guid.NewGuid().GetHashCode());
            i = r.Next(i1, i2);

            return i;
        }
        public static DateTime GetMondayDate(DateTime someDate)			//获取周一
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }
        public static DateTime GetSundayDate(DateTime someDate)			//获取周末
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。 
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }
    }
}