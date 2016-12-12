using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：分页类
    /// ** 创始时间：2015-5-29
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明: http://www.cnblogs.com/sunkaixuan/p/4538593.html
    public class PageString
    {
        private ParamsModel _params;
        public PageString()
        {
            _params = new ParamsModel()
            {
                IsEnglish = false,
                IsShowText = true,
                TextFormat = "<span class=\"pagetext\"><strong>总共</strong>:{0} 条 <strong>当前</strong>:{1}/{2}</span> ",
                ClassName = "pagination",
                PageIndexName = "pageIndex",
                IsAjax = false
            };
        }

        #region  set method
        /// <summary>
        /// 是否是英文      (默认：false)
        /// </summary>
        public void SetIsEnglish(bool isEnglish)
        {
            _params.IsEnglish = isEnglish;
        }
        /// <summary>
        /// 是否显示分页文字(默认：true)
        /// </summary>
        public void SetIsShowText(bool isShowText)
        {
            _params.IsShowText = isShowText;
        }
        /// <summary>
        /// 样式            (默认:"pagination")
        /// </summary>
        public void SetClassName(string className)
        {
            _params.ClassName = className;
        }
        /// <summary>
        /// 分页参数名      (默认:"pageIndex")
        /// </summary>
        public void SetPageIndexName(string pageIndexName)
        {
            _params.PageIndexName = pageIndexName;

        }
        /// <summary>
        /// 是否是异步 同步 href='' 异步 onclick=ajaxPage()    (默认:false)
        /// </summary>
        public void SetIsAjax(bool isAjax)
        {
            _params.IsAjax = isAjax;
        }

        /// <summary>
        /// 自定义文字
        /// string.Format("{0}{1}{2}","总记录数","当前页数","总页数")
        /// 默认值：《span class=\"pagetext\"》《strong》总共《/strong》:{0} 条 《strong》当前《/strong》:{1}/{2}《/span》
        /// </summary>
        public void SetTextFormat(string textFormat)
        {
            _params.TextFormat = textFormat;
        }


        #endregion

        /*免费的样式 
        .pagination .click {cursor:pointer}
        .pagination a{text-decoration: none;border: 1px solid #AAE;color: #15B;font-size: 13px;border-radius: 2px;}
        .pagination span{ color:#666;font-size:13px;display: inline-block;border: 1px solid #ccc;padding: 0.2em 0.6em;}
        .pagination span.pagetext{ border:none}
        .pagination a:hover{background: #26B;color: #fff;}
        .pagination a{display: inline-block;padding: 0.2em 0.6em;}
        .pagination .page_current{background: #26B;color: #fff;border: 1px solid #AAE;margin-right: 5px;}
        .pagination{margin-top: 20px;}
        .pagination .current.prev, .pagination .current.next{color: #999;border-color: #999;background: #fff;}
         * */

        #region main method
        /// <summary>
        /// 分页算法＜一＞共20页 首页 上一页  1  2  3  4  5  6  7  8  9  10  下一页  末页 
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// <returns></returns>
        public string ToPageString(int total, int pageSize, int pageIndex, string query_string)
        {

            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            StringBuilder pagestr = new StringBuilder();
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            pagestr.AppendFormat("<div class=\"{0}\" >", _params.ClassName);
            if (pageIndex < 1) { pageIndex = 1; }
            //计算总页数
            if (pageSize != 0)
            {
                allpage = (total / pageSize);
                allpage = ((total % pageSize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = pageIndex + 1;
            pre = pageIndex - 1;
            startcount = (pageIndex + 5) > allpage ? allpage - 9 : pageIndex - 4;//中间页起始序号
            //中间页终止序号
            endcount = pageIndex < 5 ? 10 : pageIndex + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内

            bool isFirst = pageIndex == 1;
            bool isLast = pageIndex == allpage;

            if (_params.IsShowText)
                pagestr.AppendFormat(_params.TextFormat, total, pageIndex, allpage);

            if (isFirst)
            {
                pagestr.Append("<span>首页</span> <span>上一页</span>");
            }
            else
            {
                pagestr.AppendFormat("<a href=\"{0}pageIndex=1\">首页</a>  <a href=\"{0}pageIndex={1}\">上一页</a>", query_string, pre);
            }
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                bool isCurent = pageIndex == i;
                if (isCurent)
                {
                    pagestr.Append("  <a class=\"page_current page_number\">" + i + "</a>");
                }
                else
                {
                    pagestr.Append("   <a class=\"page_number\"  href=\"" + query_string + "pageIndex=" + i + "\">" + i + "</a>");
                }

            }
            if (isLast)
            {
                pagestr.Append("<span>下一页</span> <span>末页</span>");
            }
            else
            {
                pagestr.Append("  <a  href=\"" + query_string + "pageIndex=" + next + "\">下一页</a>  <a href=\"" + query_string + "pageIndex=" + allpage + "\">末页</a>");
            }
            pagestr.AppendFormat("</div>");
            return ConversionData(pagestr.ToString());
        }

        #endregion

        #region private method
        private string ConversionData(string page)
        {
            if (_params.IsEnglish)
            {
                page = page.Replace("上一页", "Previous").Replace("下一页", "Next").Replace("总共", "total").Replace("当前", "Current").Replace("条", "records").Replace("首页", "First").Replace("末页", "Last");
            }
            if (_params.IsAjax)
            {
                var matches = Regex.Matches(page, @"href\="".*?""", RegexOptions.Singleline);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match m in matches)
                    {
                        page = page.Replace(m.Value, string.Format("class=\"click\" onclick=\"ajaxPage('{0}')\"", Regex.Match(m.Value, string.Format(@"{0}\=(\d+)", _params.PageIndexName)).Groups[1].Value));
                    }
                }
            }
            return page;

        } 
        #endregion

        #region model
        private class ParamsModel
        {
            /// <summary>
            /// 是否是英文      (默认：false)
            /// </summary>
            public bool IsEnglish { get; set; }
            /// <summary>
            /// 是否显示分页文字(默认：true)
            /// </summary>
            public bool IsShowText { get; set; }
            /// <summary>
            /// 样式            (默认:"pagination")
            /// </summary>
            public string ClassName { get; set; }
            /// <summary>
            /// 分页参数名      (默认:"pageIndex")
            /// </summary>
            public string PageIndexName { get; set; }
            /// <summary>
            /// 是否是异步 同步 href='' 异步 onclick=ajaxPage()    (默认:false)
            /// </summary>
            public bool IsAjax { get; set; }

            /// <summary>
            /// 自定义文字
            /// string.Format("{0}{1}{2}","总记录数","当前页数","总页数")
            /// 默认值：《span class=\"pagetext\"》《strong》总共《/strong》:{0} 条 《strong》当前《/strong》:{1}/{2}《/span》
            /// </summary>
            public string TextFormat { get; set; }
        } 
        #endregion

    }

}
