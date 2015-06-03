using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Form
{
    public partial class ValidationSugarTest : System.Web.UI.Page
    {
        private string _pageKey = "ValidationSugarTest.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //MVC中可以使用ViewBag 扔到body最下面 ，可以使用变量，这里只是测试不考虑性能
                ViewState["intiscript"] = ValidationSugar.GetInitScript(_pageKey, new List<ValidationSugar.OptionItem>()
                {
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Mail, IsRequired=true, Placeholder="邮箱", Tip="格式为:xx@xx.xx", FormFiledName="email"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Regex, Pattern="^.{5,10}$", IsRequired=true, Placeholder="姓名", Tip="5-10个字符", FormFiledName="name"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Mobile, IsRequired=true, Placeholder="手机", Tip="格式为:18994211791" , FormFiledName="phone"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Regex,Pattern="^0|1$", IsRequired=true, Placeholder="姓别", Tip="格式为:男或女" , FormFiledName="sex"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Int, IsRequired=true, Placeholder="学历", Tip="格式为:大学" , FormFiledName="education"}

                });
            }
        }


        //提交表单,MVC一样用法
        protected void Button1_Click(object sender, EventArgs e)
        {
            string errorMessage = "";
            bool isSuccess = ValidationSugar.PostValidation(_pageKey, out errorMessage);
            if (isSuccess)
            {
                //提交
                Response.Write("<script>alert('提交成功！');window.location.href='" + _pageKey + "'</script>");
            }
            else
            {
                //提交
                Response.Write("<script>alert('对不起请求数据格式不正确，请检查浏览器是否支持JavaScript！');window.location.href='" + _pageKey + "'</script>");
                // throw new HttpException("对不起请求数据格式不正确，请检查浏览器是否支持JavaScript！");
            }
        }
    }
}