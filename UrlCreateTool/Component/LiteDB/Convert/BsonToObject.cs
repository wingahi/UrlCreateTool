﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UrlCreateTool.LiteDB.Convert
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public class BsonToObject
    {
        public static T ConvertTo<T>(BsonDocument bd)
            where T:new()
        {
            T model = new T();
            if (bd != null)
            {
                PropertyInfo[] propertyInfos = model.GetType().GetProperties();
                foreach (var property in propertyInfos)
                {
                    if (property != null && bd.ContainsKey(property.Name))
                    {
                        property.SetValue(model, bd[property.Name].RawValue, null);
                    }
                }
            }
            return model;
        }
        public static IList<T> ConvertToList<T>(List<BsonDocument> dbList)
            where T:new()
        {
            IList<T> _List = new List<T>();
            foreach(var bd in dbList)
            {
                T _t=ConvertTo<T>(bd);
                if(_t!=null)
                {
                    _List.Add(_t);
                }
            }
            return _List;
        }
    }
}
