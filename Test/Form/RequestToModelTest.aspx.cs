using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Form
{
    public class RTModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class RequestToModelTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //允许 value包含逗号的写法
                RequestToModel.SetIsUnvalidatedFrom = key =>
                {
                    var formArray = Request.Form.GetValues(key).Select(it => it == null ? it : it.Replace(",", RequestToModel.COMMAS)).ToArray();
                    string reval = string.Join(",", formArray);
                    return reval;
                };
                var list = RequestToModel.GetListByForm<RTModel>();
            }
        }
    }
}