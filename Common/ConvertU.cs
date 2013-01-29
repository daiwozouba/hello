using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ConvertU
    {
        /// <summary>
        /// 
        /// </summary>
        public ConvertU() { }

        /// <summary>
        /// DataTable 转为json 序列
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Dt2Json(System.Data.DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (System.Data.DataRow r in dt.Rows)
            {
                System.Collections.Generic.Dictionary<string, string> drow = new System.Collections.Generic.Dictionary<string, string>();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    drow.Add(col.ColumnName, r[col.ColumnName].ToString());
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic);
        }

        /// <summary>
        /// DataSet 转为json 序列
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string Ds2Json(System.Data.DataSet ds)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.Collections.ArrayList dicSet = new System.Collections.ArrayList();
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                System.Collections.ArrayList dic = new System.Collections.ArrayList();
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    System.Collections.Generic.Dictionary<string, string> drow = new System.Collections.Generic.Dictionary<string, string>();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        drow.Add(col.ColumnName, r[col.ColumnName].ToString());
                    }
                    dic.Add(drow);
                }
                dicSet.Add(dic);
            }
            return jss.Serialize(dicSet);
        }
    }
}
