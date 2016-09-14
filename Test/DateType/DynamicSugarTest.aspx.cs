using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.DateType
{
    public partial class DynamicSugarTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var d = new Dictionary<string, object>();
            d.Add("a", "n");
            var dy=DynamicSugar.DictionaryToDynamic(d);

            var dic = DynamicSugar.DynamicToDictionary(dy);

            var dy2 = new { id = 1 };
            var dic2 = DynamicSugar.DynamicToDictionary(d);
        }
    }
}