using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Security
{
    public partial class CenerateRandomStringTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Write(RandomSugar.GetRandChinese(3) + "<br>");
                   Response.Write(RandomSugar.GetRandomNum(3));
      ;
        }
    }
}