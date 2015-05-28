using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：html解析类
    /// ** 创始时间：2015-4-23
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** qq：610262374 欢迎交流,共同提高 ,命名语法等写的不好的地方欢迎大家的给出宝贵建议
    /// ** 使用说明:http://www.cnblogs.com/sunkaixuan/p/4482121.html
    /// </summary>
    public class XHtmlElement
    {
        private string _html;
        public XHtmlElement(string html)
        {
            _html = html;
        }

        /// <summary>
        /// 获取最近的相同层级的HTML元素
        /// </summary>
        /// <param name="elementName">等于null为所有元素</param>
        /// <returns></returns>
        public List<HtmlInfo> Descendants(string elementName = null)
        {
            if (_html == null)
            {
                throw new ArgumentNullException("html不能这空！");
            }
            var allList = RootDescendants(_html);
            var reval = allList.Where(c => elementName == null || c.TagName.ToLower() == elementName.ToLower()).ToList();
            if (reval == null || reval.Count == 0)
            {
                reval = GetDescendantsSource(allList, elementName);
            }
            return reval;
        }


        /// <summary>
        /// 获取第一级元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public List<HtmlInfo> RootDescendants(string html = null)
        {
            /*
             * 业务逻辑:
                         * 1、获取第一个html标签一直找结尾标签，如果在这个过程中遇到相同的标签收尾标签就要加1
                         * 2、第一个标签取到后继续第一步操作，找第2个元素 。。第N个元素
             */
            if (html == null) html = _html;
            var firstTag = Regex.Match(html, "<.+?>");

            List<string> eleList = new List<string>();
            List<HtmlInfo> reval = new List<HtmlInfo>();
            GetElementsStringList(html, ref eleList);
            foreach (var r in eleList)
            {
                HtmlInfo data = new HtmlInfo();
                data.OldFullHtml = r;
                data.SameLeveHtml = html;
                data.TagName = Regex.Match(r, @"(?<=\s{1}|\<)[a-z,A-Z]+(?=\>|\s)", RegexOptions.IgnoreCase).Value;
                data.InnerHtml = Regex.Match(r, @"(?<=\>).+(?=<)", RegexOptions.Singleline).Value;
                var eleBegin = Regex.Match(r, "<.+?>").Value;
                var attrList = Regex.Matches(eleBegin, @"[a-z,A-Z]+\="".+?""").Cast<Match>().Select(c => new { key = c.Value.Split('=').First(), value = c.Value.Split('=').Last().TrimEnd('"').TrimStart('"') }).ToList();
                data.Attributes = new Dictionary<string, string>();
                if (attrList != null && attrList.Count > 0)
                {
                    foreach (var a in attrList)
                    {
                        data.Attributes.Add(a.key, a.value);
                    }
                }
                reval.Add(data);
            }
            return reval;

        }





        #region private
        private List<HtmlInfo> GetDescendantsSource(List<HtmlInfo> allList, string elementName)
        {
            foreach (var r in allList)
            {
                if (r.InnerHtml == null || !r.InnerHtml.Contains("<")) continue;
                var childList = RootDescendants(r.InnerHtml).Where(c => elementName == null || c.TagName.ToLower() == elementName.ToLower()).ToList();
                if (childList == null || childList.Count == 0)
                {
                    childList = GetDescendantsSource(RootDescendants(r.InnerHtml), elementName);
                    if (childList != null && childList.Count > 0)
                        return childList;
                }
                else
                {
                    return childList;
                }
            }
            return null;
        }

        private void GetElementsStringList(string html, ref List<string> eleList)
        {
            HtmlInfo info = new HtmlInfo();
            info.TagName = Regex.Match(html, @"(?<=\<\s{0,5}|\<)([a-z,A-Z]+|h\d{1})(?=\>|\s)", RegexOptions.IgnoreCase).Value;
            string currentTagBeginReg = @"<\s{0,10}" + info.TagName + @".*?>";//获取当前标签元素开始标签正则
            string currentTagEndReg = @"\<\/" + info.TagName + @"\>";//获取当前标签元素收尾标签正则
            if (string.IsNullOrEmpty(info.TagName)) return;

            string eleHtml = "";
            //情况1 <a/>
            //情况2 <a></a>
            //情况3 <a> 错误格式
            //情况4endif
            if (Regex.IsMatch(html, @"<\s{0,10}" + info.TagName + "[^<].*?/>"))//单标签
            {
                eleHtml = Regex.Match(html, @"<\s{0,10}" + info.TagName + "[^<].*?/>").Value;
            }
            else if (!Regex.IsMatch(html, currentTagEndReg))//没有收尾
            {
                if (Regex.IsMatch(html, @"\s{0,10}\<\!\-\-\[if"))
                {
                    eleHtml = GetElementString(html, @"\s{0,10}\<\!\-\-\[if", @"\[endif\]\-\-\>", 1);
                }
                else
                {
                    eleHtml = Regex.Match(html, currentTagBeginReg,RegexOptions.Singleline).Value;
                }
            }
            else
            {
                eleHtml = GetElementString(html, currentTagBeginReg, currentTagEndReg, 1);
            }


            try
            {
                eleList.Add(eleHtml);
                html = html.Replace(eleHtml, "");
                html = Regex.Replace(html, @"<\!DOCTYPE.*?>", "");
                if (!Regex.IsMatch(html, @"^\s*$"))
                {
                    GetElementsStringList(html, ref eleList);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SORRY,您的HTML格式不能解析！！！");

            }

        }

        private string GetElementString(string html, string currentTagBeginReg, string currentTagEndReg, int i)
        {

            string newHtml = GetRegNextByNum(html, currentTagBeginReg, currentTagEndReg, i);
            var currentTagBeginMatches = Regex.Matches(newHtml, currentTagBeginReg, RegexOptions.Singleline).Cast<Match>().Select(c => c.Value).ToList();
            var currentTagEndMatches = Regex.Matches(newHtml, currentTagEndReg).Cast<Match>().Select(c => c.Value).ToList();
            if (currentTagBeginMatches.Count == currentTagEndMatches.Count)
            { //两个签标元素相等
                return newHtml;
            }
            return GetElementString(html, currentTagBeginReg, currentTagEndReg, ++i);
        }

        private string GetRegNextByNum(string val, string currentTagBeginReg, string currentTagEndReg, int i)
        {
            return Regex.Match(val, currentTagBeginReg + @"((.*?)" + currentTagEndReg + "){" + i + "}?", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
        }
        #endregion



    }
    public static class XHtmlElementExtendsion
    {
        /// <summary>
        /// 获取最近的相同层级的HTML元素
        /// </summary>
        /// <param name="elementName">等于null为所有元素</param>
        /// <returns></returns>
        public static List<HtmlInfo> Descendants(this  IEnumerable<HtmlInfo> htmlInfoList, string elementName = null)
        {
            var html = htmlInfoList.First().InnerHtml;
            XHtmlElement xhe = new XHtmlElement(html);
            return xhe.Descendants(elementName);
        }
        /// <summary>
        /// 获取下级元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static List<HtmlInfo> ChildDescendants(this  IEnumerable<HtmlInfo> htmlInfoList, string elementName = null)
        {
            var html = htmlInfoList.First().InnerHtml;
            XHtmlElement xhe = new XHtmlElement(html);
            return xhe.RootDescendants(html).Where(c => elementName == null || c.TagName == elementName).ToList();
        }

        /// <summary>
        /// 获取父级
        /// </summary>
        /// <param name="htmlInfoList"></param>
        /// <returns></returns>
        public static List<HtmlInfo> ParentDescendant(this  IEnumerable<HtmlInfo> htmlInfoList,string fullHtml)
        {
            var saveLeveHtml = htmlInfoList.First().SameLeveHtml;
            string replaceGuid=Guid.NewGuid().ToString();
            fullHtml = fullHtml.Replace(saveLeveHtml,replaceGuid);
            var parentHtml = Regex.Match(fullHtml, @"<[^<]+?>[^<]*?" + replaceGuid + @".*?<\/.+?>").Value;
            parentHtml = parentHtml.Replace(replaceGuid, saveLeveHtml);
            XHtmlElement xhe = new XHtmlElement(parentHtml);
            return xhe.RootDescendants();
        }
    }
    /// <summary>
    /// html信息类
    /// </summary>
    public class HtmlInfo
    {
        /// <summary>
        /// 元素名
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 元素属性
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; }
        /// <summary>
        /// 元素内部html
        /// </summary>
        public string InnerHtml { get; set; }

        public string OldFullHtml { get; set; }

        public string SameLeveHtml { get; set; }

        /// <summary>
        /// 得到元素的html
        /// </summary>
        /// <returns></returns>
        public string FullHtml
        {
            get
            {
                StringBuilder reval = new StringBuilder();
                string attributesString = string.Empty;
                if (Attributes != null && Attributes.Count > 0)
                {
                    attributesString = string.Join(" ", Attributes.Select(c => string.Format("{0}=\"{1}\"", c.Key, c.Value)));
                }
                reval.AppendFormat("<{0} {2}>{1}</{0}>", TagName, InnerHtml, attributesString);
                return reval.ToString();
            }
        }
    }
}
复制代码