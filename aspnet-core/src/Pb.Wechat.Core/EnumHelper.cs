using Newtonsoft.Json;
using Pb.Wechat.Attrbutes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Pb.Wechat
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 根据类型获取枚举
        /// </summary>
        /// <param name="emType"></param>
        /// <returns></returns>
        public static List<Tuple<string, string>> GetEnum(Type emType, bool setEmpty = false, string empytValue = "", string emptyText = "")
        {
            List<Tuple<string,string>> dic = new List<Tuple<string, string>>();
            if (setEmpty)
                dic.Insert(0, new Tuple<string, string>(empytValue, emptyText));
            foreach (var item in emType.GetFields())
            {
                var arr = item.GetCustomAttributes(typeof(DescriptionAttribute), true).ToList();
                if (arr.Count > 0)
                    dic.Add(new Tuple<string, string>(item.Name, ((DescriptionAttribute)arr[0]).Description));
            }

            return dic;
        }
        /// <summary>
        /// 根据类型获取枚举
        /// </summary>
        /// <param name="emType"></param>
        /// <returns></returns>
        public static List<Tuple<string, string>> GetIntEnum(Type emType, bool setEmpty = false, string empytValue = "", string emptyText = "")
        {
            List<Tuple<string, string>> dic = new List<Tuple<string, string>>();
            if (setEmpty)
                dic.Insert(0, new Tuple<string, string>(empytValue, emptyText));
            foreach (var item in emType.GetFields())
            {
                var arr = item.GetCustomAttributes(typeof(IntDescriptionAttribute), true).ToList();
                if (arr.Count > 0)
                    dic.Add(new Tuple<string, string>(((IntDescriptionAttribute)arr[0]).IntValue.ToString(), ((IntDescriptionAttribute)arr[0]).Description));
            }

            return dic;
        }

        /// <summary>
        /// 获取枚举的文本值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="value">枚举键</param>
        /// <returns></returns>
        public static string GetEnumText(List<Tuple<string, string>> dic, string value)
        {
            return dic.Where(c => c.Item1 == value).FirstOrDefault()?.Item2;
        }

        /// <summary>
        /// 获取枚举的文本值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="value">枚举键</param>
        /// <returns></returns>
        public static string GetEnumText(Type emType, string value)
        {
            return GetEnumText(GetEnum(emType), value);
        }

        /// <summary>
        /// 获取枚举的键值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="text">枚举文本</param>
        /// <returns></returns>
        public static string GetEnumValue(Type emType, string text)
        {
            return GetEnum(emType).Where(c => c.Item2 == text).FirstOrDefault()?.Item1;
        }

        public static string GetEnumJson(Type emType, string enumName = "", bool setEmpty = false, string empytValue = "", string emptyText = "")
        {
            if (string.IsNullOrEmpty(enumName))
                enumName = emType.Name;
            var dic = GetEnum(emType);
            if (setEmpty)
                dic.Insert(0, new Tuple<string, string>(empytValue, emptyText));
            return string.Format("var {0} = {1};", enumName, JsonConvert.SerializeObject(dic.ToDictionary(c => c.Item1, c => c.Item2)));
        }

        public static string GetIntEnumJson(Type emType, string enumName = "", bool setEmpty = false, string empytValue = "", string emptyText = "")
        {
            if (string.IsNullOrEmpty(enumName))
                enumName = emType.Name;
            var dic = GetIntEnum(emType);
            if (setEmpty)
                dic.Insert(0, new Tuple<string, string>(empytValue, emptyText));
            return string.Format("var {0} = {1};", enumName, JsonConvert.SerializeObject(dic.ToDictionary(c => c.Item1, c => c.Item2)));
        }
    }
}
