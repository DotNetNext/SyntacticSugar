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
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4558205.html
    /// ** git: http://git.oschina.net/sunkaixuan/ValidationSuarMVC
    /// </summary>
    public class ValidationSugar
    {

        private static List<ValidationOption> ValidationOptionList = new List<ValidationOption>();


        public static InitScriptModel CreateOptionItem()
        {
            InitScriptModel model = new InitScriptModel();
            return model;
        }



        public static string GetBindScript(string pageKey, Action init = null)
        {
            if (!ValidationOptionList.Any(c => c.PageKey == pageKey))
            {
                if (init != null)
                    init();
            }
            if (!ValidationOptionList.Any(c => c.PageKey == pageKey))
            {
                throw new ArgumentNullException("ValidationSugar.PostValidation.pageKey");
            }
            return ValidationOptionList.Single(c => c.PageKey == pageKey).Script;
        }



        /// <summary>
        /// 前台注入
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="itemList"></param>
        public static void Init(string pageKey, params OptionItem[] itemList)
        {
            ValidationOption option = new ValidationOption();
            string uk = Guid.NewGuid().ToString().Replace("-", "");//唯一函数名
            string script = string.Empty;
            StringBuilder itemsCode = new StringBuilder();
            foreach (var item in itemList)
            {
                //为script添加name方便动态删除
                script = @"<script name=""scriptValidates"">
var bindValidation{1}=function(name,params,i,validateSize){{
     var selectorObj=$(""[name='""+name+""']"").last();
   if(params.tip!=null)
     selectorObj.attr(""tip""+i,params.tip);
     if(params.pattern!=null)
     selectorObj.attr(""pattern""+i,params.pattern);
     if(params.placeholder!=null)
     selectorObj.attr(""placeholder"",params.placeholder);
     if(params.isRequired==true)
     selectorObj.attr(""required"",params.isRequired);
     selectorObj.attr(""validatesize"",validateSize);
}}
{0}</script>";
                foreach (var itit in item.TypeParams)
                {
                    int index = item.TypeParams.IndexOf(itit);
                    switch (itit.Type)
                    {
                        case OptionItemType.Mail:
                            itit.Pattern = @"^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$";
                            break;
                        case OptionItemType.Int:
                            itit.Pattern = @"^\\d{1,11}$";
                            break;
                        case OptionItemType.Double:
                            itit.Pattern = @"^\\d{1,11}$";
                            break;
                        case OptionItemType.IdCard:
                            itit.Pattern = @"^(\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x))$";
                            break;
                        case OptionItemType.Date:
                            itit.Pattern = @"^(((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(10|12|0?[13578])([-\\/])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(11|0?[469])([-\\/])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\\d{2})|([2-9]\\d{3}))([-\\/])(0?2)([-\\/])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\\/])(0?2)([-\\/])(29)$)|(^([3579][26]00)([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][0][48])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][0][48])([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][2468][048])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][2468][048])([-\\/])(0?2)([-\\/])(29)$)|(^([1][89][13579][26])([-\\/])(0?2)([-\\/])(29)$)|(^([2-9][0-9][13579][26])([-\\/])(0?2)([-\\/])(29))|(((((0[13578])|([13578])|(1[02]))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(3[01])))|((([469])|(11))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(30)))|((02|2)[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9]))))[\\-\\/\\s]?\\d{4})(\\s(((0[1-9])|([1-9])|(1[0-2]))\\:([0-5][0-9])((\\s)|(\\:([0-5][0-9])\\s))([AM|PM|am|pm]{2,2})))?$";
                            break;
                        case OptionItemType.Mobile:
                            itit.Pattern = @"^[0-9]{11}$";
                            break;
                        case OptionItemType.Telephone:
                            itit.Pattern = @"^(\\(\\d{3,4}\\)|\\d{3,4}-|\\s)?\\d{8}$";
                            break;
                        case OptionItemType.Fax:
                            itit.Pattern = @"^[+]{0,1}(\\d){1,3}[ ]?([-]?((\\d)|[ ]){1,12})+$";
                            break;
                        case OptionItemType.Func:
                            itit.Pattern = "myfun:" + itit.Func;
                            break;
                        case OptionItemType.Regex:
                            itit.Pattern = itit.Pattern.TrimStart('^').TrimEnd('$');
                            itit.Pattern = string.Format("^{0}$", itit.Pattern);
                            itit.Pattern = itit.Pattern.Replace(@"\", @"\\");
                            break;
                    }
                    itemsCode.AppendFormat("bindValidation{0}('{1}',{{   tip:'{2}',pattern:'{3}',placeholder:'{4}',isRequired:{5} }},{6},{7})", uk, item.FormFiledName, itit.Tip, itit.Pattern, item.Placeholder, item.IsRequired ? "true" : "false", index, item.TypeParams.Count);
                    itemsCode.AppendLine();
                }
            }
            option.Script = string.Format(script, itemsCode.ToString(), uk);
            script = null;
            itemsCode.Clear();
            option.PageKey = pageKey;
            option.ItemList = itemList.ToList();
            ValidationOptionList.Add(option);

        }


        /// <summary>
        /// 后台验证
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="errorMessage">json格式</param>
        /// <returns></returns>
        public static bool PostValidation(string pageKey, out string errorMessage, Action init = null)
        {
            bool isSuccess = true;
            errorMessage = string.Empty;
            if (!ValidationOptionList.Any(c => c.PageKey == pageKey))
            {
                if (init != null)
                    init();
            }
            if (!ValidationOptionList.Any(c => c.PageKey == pageKey))
            {
                throw new ArgumentNullException("ValidationSugar.PostValidation.pageKey");
            }
            var context = System.Web.HttpContext.Current;
            var itemList = ValidationOptionList.Where(c => c.PageKey == pageKey).Single().ItemList;
            var successItemList = itemList.Where(it =>
            {
                string value = context.Request[it.FormFiledName];
                if (string.IsNullOrEmpty(value) && it.IsRequired == false) return true;
                if (string.IsNullOrEmpty(value) && it.IsRequired) return false;
                if (it.IsMultiselect == true)
                {
                    var errorList = value.Split(',').Where(itit =>
                    {
                        var isNotMatch = it.TypeParams.Any(par => par.Type != ValidationSugar.OptionItemType.Func && !Regex.IsMatch(itit, par.Pattern.Replace(@"\\", @"\")));
                        return isNotMatch;

                    }).ToList();
                    return errorList.Count == 0;
                }
                else
                {
                    return !it.TypeParams.Any(par => par.Type != ValidationSugar.OptionItemType.Func && !Regex.IsMatch(value, par.Pattern.Replace(@"\\", @"\")));
                }
            }
                ).ToList();
            isSuccess = (successItemList.Count == itemList.Count);
            if (!isSuccess)
            {
                errorMessage = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(itemList.Where(it => !successItemList.Any(sit => sit.FormFiledName == it.FormFiledName)));
            }
            return isSuccess;
        }


        public class ValidationOption
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
            Regex = 1000,
            /// <summary>
            /// 函数验证
            /// </summary>
            Func = 1001

        }
        /// <summary>
        /// 验证选项
        /// </summary>
        public class OptionItem
        {
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
            /// 是多选吗? 默认false
            /// </summary>
            public bool IsMultiselect { get; set; }

            /// <summary>
            /// 验证类型参数
            /// </summary>
            public List<OptionItemTypeParams> TypeParams { get; set; }

        }
        /// <summary>
        /// 验证类型参数
        /// </summary>
        public class OptionItemTypeParams
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
            /// 提醒
            /// </summary>
            public string Tip { get; set; }
            /// <summary>
            /// 函数
            /// </summary>
            public string Func { get; set; }
        }
    }

    public static class ValidationSugarExtension
    {
        public static ValidationSugarOptionItemModel Set(this InitScriptModel thisValue, string formFiledName, bool isRequired, string placeholder = "", bool isMultiselect = false)
        {
            ValidationSugarOptionItemModel item = new ValidationSugarOptionItemModel();
            item.ItemOption = new ValidationSugar.OptionItem()
            {
                FormFiledName = formFiledName,
                IsMultiselect = isMultiselect,
                Placeholder = placeholder,
                IsRequired = isRequired
            };
            thisValue = null;
            return item;
        }

        public static ValidationSugarOptionItemModel Add(this ValidationSugarOptionItemModel thisValue, ValidationSugar.OptionItemType type, string tip)
        {
            if (thisValue.ItemOption.TypeParams == null)
            {
                thisValue.ItemOption.TypeParams = new List<ValidationSugar.OptionItemTypeParams>();
            }
            ValidationSugar.OptionItemTypeParams par = new ValidationSugar.OptionItemTypeParams()
            {
                Type = type,
                Tip = tip
            };
            thisValue.ItemOption.TypeParams.Add(par);
            return thisValue;

        }
        public static ValidationSugarOptionItemModel AddRegex(this ValidationSugarOptionItemModel thisValue, string pattern, string tip)
        {
            if (thisValue.ItemOption.TypeParams == null)
            {
                thisValue.ItemOption.TypeParams = new List<ValidationSugar.OptionItemTypeParams>();
            }
            ValidationSugar.OptionItemTypeParams par = new ValidationSugar.OptionItemTypeParams()
            {
                Pattern = pattern,
                Tip = tip,
                Type = ValidationSugar.OptionItemType.Regex
            };
            thisValue.ItemOption.TypeParams.Add(par);
            return thisValue;
        }
        public static ValidationSugarOptionItemModel AddFunc(this ValidationSugarOptionItemModel thisValue, string func, string tip)
        {
            if (thisValue.ItemOption.TypeParams == null)
            {
                thisValue.ItemOption.TypeParams = new List<ValidationSugar.OptionItemTypeParams>();
            }
            ValidationSugar.OptionItemTypeParams par = new ValidationSugar.OptionItemTypeParams()
            {
                Func = func,
                Tip = tip,
                Type = ValidationSugar.OptionItemType.Func
            };
            thisValue.ItemOption.TypeParams.Add(par);
            return thisValue;
        }


        public static ValidationSugar.OptionItem ToOptionItem(this ValidationSugarOptionItemModel thisValue)
        {
            return thisValue.ItemOption;
        }
    }
    public class ValidationSugarOptionItemModel
    {
        public ValidationSugar.OptionItem ItemOption { get; set; }
    }
    public class InitScriptModel
    {
        public ValidationSugar.OptionItem ItemOption { get; set; }
    }
}
