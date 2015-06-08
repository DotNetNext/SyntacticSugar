using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Extenions
{
    public partial class EnumSugarExtenions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var attr = MyType.b.GetName();
            var value = MyType.b.GetValue<int>();

            var type = MyType.a.GetAttribute();
        }
    }

    public enum MyType
    {
        [Desc("阿asfd")]
        a = 0,
        [Desc("逼")]
        b = 1
    }
}