using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlCreateTool.Common
{
    /// <summary>
    /// 数据类型转换
    /// </summary>
    public static class DataConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this string input)
        {
            int outint;
            int.TryParse(input, out outint);
            return outint;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToDate(this string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                return date;
            }
            return DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStr(this object input)
        {
            if (input==null)
            {
                return string.Empty;
            }
            return input.ToString();
        }
        public static bool ToBoolean(this object input)
        {
            if (input == null) return false;
            if(input.ToStr()=="true" || input.ToStr().ToInt32()==1)
            {
                return true;
            }
            return false;
        }
        public static double ToDouble(this object input)
        {
            if (input == null) return 0;
            double result = 0;
            double.TryParse(input.ToStr(), out result);
            return result;
        }
        /// <summary>
        /// 检验逗号整数串是否合法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ValidateSpitString(this object input)
        {
            string[] temp = input.ToStr().Split(',');
            if(temp.Length<=0)
            {
                return false;
            }
            else
            {
                return temp.All(s => s.ToInt32() != 0);
            }
        }
        public static short ToShort(this string input)
        {
            short outint;
            short.TryParse(input, out outint);
            return outint;
        }
        public static Single ToSingle(this string input)
        {
            Single outresult;
            Single.TryParse(input, out outresult);
            return outresult;
        }
        public static decimal ToDecimal(this string input)
        {
            decimal outresult;
            decimal.TryParse(input, out outresult);
            return outresult;
        }
        public static char ToChar(this string input)
        {
            char outresult;
            char.TryParse(input, out outresult);
            return outresult;
        }
        public static Int64 ToInt64(this string input)
        {
            Int64 outresult;
            Int64.TryParse(input, out outresult);
            return outresult;
        }
        public static UInt64 ToUInt64(this string input)
        {
            UInt64 outresult;
            UInt64.TryParse(input, out outresult);
            return outresult;
        }
        public static UInt32 ToUInt32(this string input)
        {
            UInt32 outresult;
            UInt32.TryParse(input, out outresult);
            return outresult;
        }
        public static UInt16 ToUInt16(this string input)
        {
            UInt16 outresult;
            UInt16.TryParse(input, out outresult);
            return outresult;
        }

        /// <summary>
        /// 验证字符串是否为NullOrEmpty或input.Length<=0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool _IsNullOrEmpty(this string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length > 0)
            {
                return false;
            }
            return true;
        }
    }
}
