using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test.IO
{
    public partial class ReflectionTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           var f=  ReflectionSugar.CreateInstance<FileSugar>("SyntacticSugar.FileSugar", "SyntacticSugarx");
         
        }
    }
}