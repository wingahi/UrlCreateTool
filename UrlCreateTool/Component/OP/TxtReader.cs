using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UrlCreateTool.Common;

namespace UrlCreateTool.Component.OP
{
    /// <summary>
    /// 文本编辑器
    /// </summary>
    public class TxtReader
    {
        public static string path = "";
        public static readonly object lockObj=new object();
        public static string Path
        {
            get {
                if (path._IsNullOrEmpty())
                {
                    string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
                    if (!basePath.EndsWith("\\"))
                    {
                        basePath += "\\";
                    }
                    basePath += "Data\\datafile.txt";
                    path = basePath;
                }
                return path;
            }
            set {
                path = value;
            }
        }
        /// <summary>
        /// 从文本取数据，单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str_LineData"></param>
        /// <returns></returns>
        public static T AddData<T>(string str_LineData)
            where T:new()
        {
            string[] str_Data = str_LineData.Split(new string[]{"-|-"},StringSplitOptions.RemoveEmptyEntries);
            T model = new T();
            for (int i = 0; i < str_Data.Length; i++)
            {
                string[] property = str_Data[i].Split(new string[]{"||"},StringSplitOptions.RemoveEmptyEntries);
                PropertyInfo propertyInfo = model.GetType().GetProperty(property[0]);
                if (propertyInfo != null && property.Length==2)
                    propertyInfo.SetValue(model, GetDecodstr(property[1]), null);
            }
            return model;
        }
        /// <summary>
        /// 从文本取数据，批量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str_MultiData"></param>
        /// <returns></returns>
        public static IList<T> AddList<T>(string str_MultiData)
             where T:new()
        {
            IList<T> _List=new List<T>();
            string[] str_Data = str_MultiData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var item in str_Data)
            {
                T info = AddData<T>(item);
                _List.Add(info);
            }
            return _List;
        }
        /// <summary>
        /// 取数据，批量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> GetData<T>()
            where T:new()
        {
            string str_Data = ReadStr();
            return AddList<T>(str_Data);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void Add<T>(T obj)
            where T:new()
        {
            lock (lockObj)
            {
                string str = ConvetToEncodeStr<T>(obj);
                SaveStr(str);
            }
        }
        /// <summary>
        /// 对数据进行编码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str_LineData"></param>
        /// <returns></returns>
        public static T GetEncodeData<T>(string str_LineData)
            where T : new()
        {
            string[] str_Data = str_LineData.Split(new string[] { "-|-" }, StringSplitOptions.RemoveEmptyEntries);
            T model = new T();
            for (int i = 0; i < str_Data.Length; i++)
            {
                string[] property = str_Data[i].Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                PropertyInfo propertyInfo = model.GetType().GetProperty(property[0]);
                if (propertyInfo != null && property.Length==2)
                    propertyInfo.SetValue(model, GetEncodStr(property[1]), null);
            }
            return model;
        }
        /// <summary>
        /// 将实体进行编码，返回字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvetToEncodeStr<T>(T obj)
            where T:new()
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            string result = string.Empty;
            foreach (var item in propertyInfos)
            {
                result += item.Name + "||" + GetEncodStr(item.GetValue(obj, null).ToStr()) + "-|-";
            }
            return result;
        }
        /// <summary>
        /// 将实体进行编码，返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvetToEncode<T>(T obj)
            where T : new()
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (var item in propertyInfos)
            {
                item.SetValue(obj, item.GetValue(obj,null), null);
            }
            return obj;
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <param name="FiledName"></param>
        public static void Delete<T>(string Id,string FiledName)
            where T:new()
        {
            string result = string.Empty;
            lock (lockObj)
            {
                using (StreamReader sr = new StreamReader(Path, System.Text.ASCIIEncoding.UTF8))
                {
                    string str = null;
                    while ((str = sr.ReadLine()) != null)
                    {
                        if (str.Length > 0)
                        {
                            T _t = AddData<T>(str);
                            PropertyInfo pinfo = _t.GetType().GetProperty(FiledName);
                            if (pinfo != null)
                            {
                                object ibj = pinfo.GetValue(_t, null);
                                if (!Id.Equals(ibj.ToStr()))
                                {
                                    result += ConvetToEncodeStr<T>(_t) + "\r\n";
                                }
                            }
                        }
                    }
                }
            }
            _SaveStr(result);
        }

        private static T ReadSimpleRecord<T>(string Id, string FiledName) where T : new()
        {
            string result = string.Empty;
            lock (lockObj)
            {
                using (StreamReader sr = new StreamReader(Path, System.Text.ASCIIEncoding.UTF8))
                {
                    string str = null;
                    while ((str = sr.ReadLine()) != null)
                    {
                        if (str.Length > 0)
                        {
                            T _t = AddData<T>(str);
                            PropertyInfo pinfo = _t.GetType().GetProperty(FiledName);
                            if (pinfo != null)
                            {
                                object ibj = pinfo.GetValue(_t, null);
                                if (Id.Equals(ibj.ToStr()))
                                {
                                    return _t;
                                }
                            }
                        }
                    }
                }
            }
            return default(T);
        }
        public static string GetEncodStr(string _str)
        {
            return Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(_str)).ToStr();
        }
        public static string GetDecodstr(string str)
        {
            return System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64CharArray(str.ToArray(), 0, str.Length)).ToStr();
        }
        public static string ReadStr()
        {
            using (StreamReader sr = new StreamReader(Path, System.Text.ASCIIEncoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
        public static void SaveStr(string str)
        {
            using (StreamWriter sw = new StreamWriter(Path, true, System.Text.ASCIIEncoding.UTF8))
            {
                sw.WriteLine(str);
            }
        }
        public static void _SaveStr(string str)
        {
            using (StreamWriter sw = new StreamWriter(Path, false, System.Text.ASCIIEncoding.UTF8))
            {
                sw.WriteLine(str);
            }
        }
    }
}
