using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace Test.file
{
    public partial class UploadImageTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            UploadImage ui = new UploadImage();

            /***可选参数***/

            ui.SetWordWater = "哈哈";//文字水印
            // ui.SetPicWater = Server.MapPath("2.png");//图片水印(图片和文字都赋值图片有效)
            ui.SetPositionWater = 4;//水印图片的位置 0居中、1左上角、2右上角、3左下角、4右下角

            ui.SetSmallImgHeight = "110,40,20";//设置缩略图可以多个
            ui.SetSmallImgWidth = "100,40,20";

            //保存图片生成缩略图
            var reponseMessage = ui.FileSaveAs(Request.Files[0], Server.MapPath("~/file/temp"));

            //裁剪图片
            var reponseMessage2 = ui.FileCutSaveAs(Request.Files[0], Server.MapPath("~/file/temp2"), 400, 300, UploadImage.CutMode.CutNo);




            /***返回信息***/
            var isError = reponseMessage.IsError;//是否异常
            var webPath = reponseMessage.WebPath;//web路径
            var filePath = reponseMessage.filePath;//物理路径
            var message = reponseMessage.Message;//错误信息
            var directory = reponseMessage.Directory;//目录
            var smallPath1 = reponseMessage.SmallPath(0);//缩略图路径1
            var smallPath2 = reponseMessage.SmallPath(1);//缩略图路径2
            var smallPath3 = reponseMessage.SmallPath(2);//缩略图路径3



            /*test*/
            Image1.ImageUrl = reponseMessage.WebPath;
            Image2.ImageUrl = reponseMessage2.WebPath;
        }
    }
}