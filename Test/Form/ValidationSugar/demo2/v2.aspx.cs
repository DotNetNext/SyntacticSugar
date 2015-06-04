using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class demo2_v2 : System.Web.UI.Page
{
    private string _pageKey = "v2.aspx";
    protected string bindScript = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //MVC中可以使用ViewBag 扔到body最下面 ，可以使用变量，这里只是测试不考虑性能
            bindScript = ValidationSugar.GetInitScript(_pageKey, new List<ValidationSugar.OptionItem>()
                {
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Mail, IsRequired=false, Placeholder="邮箱", Tip="格式为:xx@xx.xx", FormFiledName="email"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Regex, Pattern="^.{5,10}$", IsRequired=true, Placeholder="姓名", Tip="5-10个字符", FormFiledName="name"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Regex, Pattern="^0|1$", IsRequired=true, Placeholder="姓别", Tip="请选择姓别", FormFiledName="sex"},
                    new ValidationSugar.OptionItem(){  Type=ValidationSugar.OptionItemType.Regex, Pattern=@"^\d$", IsRequired=true, Placeholder="爱好", Tip="请选择爱好", FormFiledName="hobbies",IsMultiselect=true /*多选一定要加将这属性设为true*/}
                         
                });
        }
    }


    
}