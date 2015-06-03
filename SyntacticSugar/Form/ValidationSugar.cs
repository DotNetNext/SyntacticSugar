using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntacticSugar.Form
{
    /// <summary>
    /// ** 描述：可以方便实现前后端双验证,基于jquery
    /// ** 创始时间：2015-6-4
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：-
    /// </summary>
    public class ValidationSugar
    {

        /// <summary>
        /// 前台注入
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="itemList"></param>
        public static void WebInit(string pageKey, List<OptionItem> itemList)
        {
            string uk = Guid.NewGuid().ToString().Replace("-", "");//唯一函数名
            string script = @"<script>
var changeInput{1}=function(selector,params){{
     var selectorObj=$(""#""+selector+"",""[name='""+selector+""']"");
     selectorObj.after(""<span class=""form_hint"">""+params.tip+""</span>"");
     if(params.Pattern!=null)
     selectorObj.attr(""pattern"",params.Pattern);
     if(params.Pattern!=null)
     selectorObj.attr(""placeholder"",params.Placeholder);
}}


{0}</script>";
            StringBuilder itemsCode = new StringBuilder();
            foreach (var item in itemList)
            {
                switch (item.Type)
                {
                    case OptioItemType.Mail:
                        item.Pattern=@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
                        break;
                    case OptioItemType.Phone:
                           item.Pattern=@"^\d{11}$";
                        break;
                    case OptioItemType.Int:
                         item.Pattern=@"^\d{1,11}$";
                        break;
                    case OptioItemType.Double:
                        item.Pattern=@"^\d{1,11}$";
                        break;
                    case OptioItemType.IdCard:
                        break;
                    case OptioItemType.Date:
                        break;
                    case OptioItemType.Mobile:
                        break;
                    case OptioItemType.Telephone:
                        break;
                    case OptioItemType.Fax:
                        break;
                    case OptioItemType.Regex:
                        break;
                }   
                itemsCode.AppendFormat("changeInput{0}('{1}',{{   tip:'{2}',pattern:'{3}',placeholder:'{4}'}})", uk, item.FormFiledName, item.Tip, item.Pattern=, item.Placeholder);
                itemsCode.AppendLine();
            }
            script = string.Format(script, itemsCode.ToString(), uk);
            System.Web.HttpContext.Current.Response.Write(script);
            script = null;
            uk = null;
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


            return isSuccess;
        }


        private class ValidationOption
        {
            public string PageKey { get; set; }
            public List<OptionItem> ItemList { get; set; }

        }

        public enum OptioItemType
        {
            Mail = 0,
            Phone = 1,
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
            public OptioItemType Type { get; set; }
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
