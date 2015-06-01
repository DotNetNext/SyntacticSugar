using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.file
{
    public partial class UploadFileTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            UploadFile uf = new UploadFile();
            uf.SetIsUseOldFileName(false);
            uf.SetFileDirectory(Server.MapPath("/file/temp3/"));
            uf.Save(Request.Files["Fileupload1"]);
        }
    }
}