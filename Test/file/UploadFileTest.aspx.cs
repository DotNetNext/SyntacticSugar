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

            /*可选参数*/
            uf.SetIsUseOldFileName(true);//是否使用原始文件名作为新文件的文件名(默认:true),true原始文件名,false系统生成新文件名
            uf.SetFileDirectory(Server.MapPath("/file/temp3/"));//文件保存路径(默认:/upload)
            uf.SetFileType("*");//允许上传的文件类型, 逗号分割,必须全部小写! *表示所有 (默认值: .pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif )  
            uf.SetIsRenameSameFile(false);//重命名同名文件？ 


            //文件以时间分目录保存
            var message = uf.Save(Request.Files["Fileupload1"]); //  “/file/temp3/2015/4/xx.jpg”

            //文件以编号分目录保存
            var message2 = uf.Save(Request.Files["Fileupload1"], "001" /*编号*/);  //   “/file/temp3/001/xx.jpg”


            //返回信息
            var isError = message.Error;//判段是否上传成功
            var webPath = message.WebFilePath;//返回web路径
            var msg = message.Message;//返回上传信息
            var filePath = message.FilePath;//反回文件路径
            var isSuccess = message.Error == false;
        }
    }
}