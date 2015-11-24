using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Http
{
    public partial class HttpTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["name"] != null)
            {
                Response.Write("<br>name:{0}".ToFormat(Request["name"]));
            }
            if (Request["id"] != null)
            {
                Response.Write("<br>id:{0}".ToFormat(Request["id"]));
            }
        }
    }
}