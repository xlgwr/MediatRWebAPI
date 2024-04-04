using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediatRApplication.CategoryCRUD
{
    public abstract class TryParseDD<T>
        where T : class, new()
    {
        public static bool TryParse(string? value, IFormatProvider? provider,
                                out T? tvalue)
        {
            // Format is "(12.3,10.1)"
            var trimmedValue = value?.TrimStart('(').TrimEnd(')');
            var segments = trimmedValue?.Split(',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            tvalue = segments.StringArray2Object<T>();
            return false;
        }
        public static ValueTask<T?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
        {

            var result = context.Request.Query.StringArray2Object<T>();
           

            return ValueTask.FromResult<T?>(result);
        }

    }
    public static class typeExt
    {
        public static T StringArray2Object<T>(this string[] array)
        where T : class, new()
        {
            if (!array.Any())
            {
                return new T();
            }
            Type type = typeof(T);
            object obj = Activator.CreateInstance<T>();
            PropertyInfo[] pis = type.GetProperties().OrderBy(a => a.MetadataToken).ToArray();
            PropertyInfo PInfo = null;
            for (int num = 0; num < array?.Count(); num++)
            {
                try
                {
                    PInfo = pis[num];
                    if (PInfo.PropertyType == typeof(string))
                    {
                        PInfo.SetValue(obj, array[num].ToString());
                    }
                    else if (PInfo.PropertyType == typeof(sbyte) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToSByte(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(byte) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToByte(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(int) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToInt32(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(Int16) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToInt16(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(Int32) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToInt32(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(UInt32) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToUInt32(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(uint) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToUInt32(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(UInt64) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToUInt64(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(short) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToInt16(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(double) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToDouble(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(Decimal) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToDecimal(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(Int64) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToInt64(array[num]));
                    }
                    else if (PInfo.PropertyType == typeof(char) && !string.IsNullOrEmpty(array[num]))
                    {
                        PInfo.SetValue(obj, Convert.ToChar(array[num]));
                    }
                    else
                    {
                        throw new Exception("参数转换出错");
                    }
                }
                catch (Exception ex)
                {
                    string errStr = string.Empty;
                    for (int i = 0; i < array.Length; i++)
                    {
                        errStr += array[i] + ",";
                    }
                    throw new Exception(string.Format("{0}\r\n{1}:{2}" + type.Name, ex.ToString(), num, errStr));
                }
            }
            return (T)obj;
        }

        

        public static T StringArray2Object<T>(this IQueryCollection array)
            where T : class, new ()
        {
            if (!array.Any())
            {
                return new T();
            }
            Type type = typeof(T);
            List<T> list = new List<T>();

            PropertyInfo[] pArray = type.GetProperties().OrderBy(a => a.MetadataToken).ToArray();
            T entity = new T();
            foreach (PropertyInfo p in pArray)
            {
                if (!array.Keys.Contains(p.Name.ToLower()))
                {
                    continue;
                }
                try
                {
                    var currtype = p.PropertyType;
                    array.TryGetValue(p.Name.ToLower(), out var val);

                    //类型转换
                    if (p.SetValueByType(entity, currtype, val))
                    {
                        continue;
                    }

                    p.SetValue(entity, val, null);
                }
                catch (Exception ex)
                {
                    throw new Exception("输换出错"+ex.Message);
                }
            }
            return entity;
        }

        #region 类型转换
        #region typeList
        public static Type typeofString = typeof(string);
        public static Type typeofInt = typeof(int);
        public static Type typeofInt64 = typeof(Int64);
        public static Type typeofsbyte = typeof(sbyte);
        public static Type typeoflong = typeof(long);
        public static Type typeofdouble = typeof(double);
        public static Type typeofdecimal = typeof(decimal);
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <param name="entity"></param>
        /// <param name="currtype"></param>
        /// <param name="currValue"></param>
        /// <returns></returns>
        public static bool SetValueByType<T>(this PropertyInfo p, T entity, Type currtype, object rowValue)
        {

            var currValue = rowValue.ToString().Trim();
            #region 类型转换
            if (currtype == typeofString)
            {
                p.SetValue(entity, currValue, null);
                return true;
            }
            if (string.IsNullOrEmpty( currValue))
            {
                p.SetValue(entity, null, null);
                return true;
            }
            if (currtype == typeofInt)
            {
                p.SetValue(entity, int.Parse(currValue));
                return true;
            }
            if (currtype == typeofdecimal)
            {
                p.SetValue(entity, decimal.Parse(currValue));
                return true;
            }
            if (currtype == typeofInt64 || currtype == typeoflong)
            {
                p.SetValue(entity, Int64.Parse(currValue));
                return true;
            }
            if (currtype == typeofsbyte)
            {
                p.SetValue(entity, sbyte.Parse(currValue));
                return true;
            }
            if (currtype == typeofdouble)
            {
                p.SetValue(entity, double.Parse(currValue));
                return true;
            }
            #endregion
            return false;
        }

        #endregion
    }

}
