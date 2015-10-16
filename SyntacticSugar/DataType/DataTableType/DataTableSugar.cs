using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SyntacticSugar
{

    /// <summary>
    /// ** 描述：dataTable操作类
    /// ** 创始时间：2015-10-16
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** qq：610262374 欢迎交流,共同提高 ,命名语法等写的不好的地方欢迎大家的给出宝贵建议
    /// ** 使用说明: 
    /// </summary>
    public class DataTableSugar
    {
        /// <summary>
        /// 创建简单的dataTable
        /// </summary>
        /// <param name="columnNameArray"></param>
        /// <returns></returns>
        public static DataTable CreateEasyTable(params string[] columnNameArray)
        {
            DataTable dt = new DataTable();
            if (columnNameArray.Length > 0)
                foreach (var it in columnNameArray)
                {
                    dt.Columns.Add(new DataColumn(it));
                }
            return dt;
        }
    }

    public static class DataTableExtension
    {
        /// <summary>
        /// 添加DataRow
        /// </summary>
        /// <param name="columnNameArray"></param>
        /// <returns></returns>
        public static DataTable AddRow(this DataTable dt, params Object[] rowValues)
        {
            var row = dt.NewRow();
            int i = 0;
            if (rowValues.Length > 0)
            {
                foreach (var it in rowValues)
                {
                    row[i] = it;
                    ++i;
                }
            }
            dt.Rows.Add(row);
            return dt;
        }
    }
}
