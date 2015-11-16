using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Http
{
    public partial class ResponseSugarTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var path= Server.MapPath(@"~\TestFIle\ok.docx");
            var pathName = FileSugar.GetFileName(path);
            ResponseSugar.ResponseFile(path, pathName);
        }
    }
}