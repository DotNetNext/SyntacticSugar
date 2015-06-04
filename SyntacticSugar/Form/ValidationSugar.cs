using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：可以方便实现前后端双验证,基于jquery
    /// ** 创始时间：2015-6-4
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4550580.html
    /// </summary>
    public class ValidationSugar
    {

        private static List<ValidationOption> ValidationOptionList = new List<ValidationOption>();

        /// <summary>
        /// 前台注入
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="itemList"></param>
        public static string GetInitScript(string pageKey, List<OptionItem> itemList)
        {
            //初始化后不在赋值
            if (ValidationOptionList.Any(it => it.PageKey == pageKey))
            {
                return (ValidationOptionList.Single(c => c.PageKey == pageKey).Script);
            }
            else
            {
                ValidationOption option = new ValidationOption();
                string uk = Guid.NewGuid().ToString().Replace("-", "");//唯一函数名
                string script = @"<script>
var bindValidation{1}=function(name,params){{
     var selectorObj=$(""[name='""+name+""']"");
     selectorObj.after(""<span class=\""form_hint\"">""+params.tip+""</span>"");
     if(params.pattern!=null)
     selectorObj.attr(""pattern"",params.pattern);
     if(params.placeholder!=null)
     selectorObj.attr(""placeholder"",params.placeholder);
     if(params.isRequired=true)
     selectorObj.attr(""required"",params.isRequired);
}}
{0}</script>";
                StringBuilder itemsCode = new StringBuilder();
                foreach (var item in itemList)
                {
                    switch (item.Type)
                    {
                        case OptionItemType.Mail:
                            item.Pattern = @"^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$";
                            break;
                        case OptionItemType.Int:
                            item.Pattern = @"^\\d{1,11}$";
                            break;
                        case OptionItemType.Double:
                            item.Pattern = @"^\\d{1,11}$";
                            break;
                        case OptionItemType.IdCard:
                            item.Pattern = @"^(\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x))$";
                            break;
                        case OptionItemType.Date:
                            item.Pattern = @"^(((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(10|12|0?[13578])([-\\/])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(11|0?[469])([-\\/])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(0?2)([-\\/])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\\/])(0?2)([-\\/])(29)$)|(^([3579][26]00)([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][0][48])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][0][48])([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][2468][048])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][2468][048])([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][13579][26])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][13579][26])([-\\/])(0?2)([-\\/])(29))|(((((0[13578])|([13578])|(1[02]))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(3[01])))|((([469])|(11))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(30)))|((02|2)[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9]))))[\\-\\/\\s]?\\d{4})(\\s(((0[1-9])|([1-9])|(1[0-2]))\\:([0-5][0-9])((\\s)|(\\:([0-5][0-9])\\s))([AM|PM|am|pm]{2,2})))?$";
                            break;
                        case OptionItemType.Mobile:
                            item.Pattern = @"^[0-9]{11}$";
                            break;
                        case OptionItemType.Telephone:
                            item.Pattern = @"^(\\(\\d{3,4}\\)|\\d{3,4}-|\\s)?\\d{8}$";
                            break;
                        case OptionItemType.Fax:
                            item.Pattern = @"^[+]{0,1}(\\d){1,3}[ ]?([-]?((\\d)|[ ]){1,12})+$";
                            break;
                        case OptionItemType.Regex:
                            break;
                    }
                    itemsCode.AppendFormat("bindValidation{0}('{1}',{{   tip:'{2}',pattern:'{3}',placeholder:'{4}',isRequired:{5} }})", uk, item.FormFiledName, item.Tip, item.Pattern, item.Placeholder,item.IsRequired?"true":"false");
                    itemsCode.AppendLine();
                }
                option.Script = string.Format(script, itemsCode.ToString(), uk);
                script = null;
                itemsCode.Clear();
                option.PageKey = pageKey;
                option.ItemList = itemList;
                ValidationOptionList.Add(option);
                return (option.Script);
            }
        }

        /// <summary>
        /// 后台验证
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="errorMessage">json格式</param>
        /// <returns></returns>
        public static bool PostValidation(string pageKey, out string errorMessage)
        {
            bool isSuccess = true;
            errorMessage = string.Empty;
            if (!ValidationOptionList.Any(c => c.PageKey == pageKey))
            {
                throw new ArgumentNullException("ValidationSugar.PostValidation.pageKey");
            }
            var context = System.Web.HttpContext.Current;
            var itemList = ValidationOptionList.Where(c => c.PageKey == pageKey).Single().ItemList;
            var successItemList = itemList.Where(it => (it.IsRequired && !string.IsNullOrEmpty(context.Request[it.FormFiledName]) || !it.IsRequired)).Where(it => Regex.IsMatch(context.Request[it.FormFiledName], it.Pattern.Replace(@"\\",@"\"))).ToList();
            isSuccess = (successItemList.Count == itemList.Count);
            if (!isSuccess)
            {
                errorMessage = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(itemList);
            }
            return isSuccess;
        }


        private class ValidationOption
        {
            public string PageKey { get; set; }
            public string Script { get; set; }
            public List<OptionItem> ItemList { get; set; }

        }

        public enum OptionItemType
        {
            Mail = 0,
            Int = 2,
            Double = 3,
            IdCard = 4,
            Date = 5,
            /// <summary>
            /// 移动电话
            /// </summary>
            Mobile = 6,
            /// <summary>
            /// 座机
            /// </summary>
            Telephone = 7,
            Fax = 8,
            /// <summary>
            /// 没有合适的，请使用正则验证
            /// </summary>
            Regex = 1000

        }
        /// <summary>
        /// 验证选项
        /// </summary>
        public class OptionItem
        {
            /// <summary>
            /// 验证类型
            /// </summary>
            public OptionItemType Type { get; set; }
            /// <summary>
            /// 正则
            /// </summary>
            public string Pattern { get; set; }
            /// <summary>
            /// 是否必填
            /// </summary>
            public bool IsRequired { get; set; }
            /// <summary>
            /// 表单字段名(name或者id)
            /// </summary>
            public string FormFiledName { get; set; }
            /// <summary>
            /// 水印
            /// </summary>
            public string Placeholder { get; set; }
            /// <summary>
            /// 提醒
            /// </summary>
            public string Tip { get; set; }

        }
    }
}
