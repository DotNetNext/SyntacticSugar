using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;


namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：单文件上传类 (暂时不支持多文件上传)
    /// ** 创始时间：2015-5-27
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4533954.html
    /// </summary>
    public class UploadFile
    {

        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string SetFileDirectory { get; set; }
        /// <summary>
        /// 允许上传的文件类型, 逗号分割,必须全部小写!
        ///
        /// 格式: ".gif,.exe" 或更多
        /// </summary>
        public string SetFileType { get; set; }
        /// <summary>
        /// 允许上传多少大小(单位：M)
        /// </summary>
        public double SetMaxSizeM { get; set; }
        /// <summary>
        /// 上传错误
        /// </summary>
        public bool GetError { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string GetMessage { get; private set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string GetFilePath { get; private set; }
        /// <summary>
        /// 网站路径
        /// </summary>
        public string GetWebFilePath { get; private set; }
        /// <summary>
        /// 获取文件名
        /// </summary>
        public string GetFileName { get; private set; }

        public UploadFile()
        {

            SetFileDirectory = "/upload";
            SetFileType = ".pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif";
            SetMaxSizeM = 10;
        }

        /// <summary>
        /// 单文件上传类
        /// </summary>
        /// <param name="directory">文件保存路径</param>
        /// <param name="fileType">允许上传的文件类型</param>
        /// <param name="maxSizeM">允许上传多少大小(单位：M)</param>
        public UploadFile(string directory, string fileType, double maxSizeM)
        {

            SetFileDirectory = directory;
            SetFileType = fileType;
            SetMaxSizeM = maxSizeM;
        }

        /// <summary>
        /// 保存表单文件,根据file name
        /// </summary>
        /// <param name="formField"></param>
        /// <param name="isRenameSameFile">值为true 同名文件进行重命名，false覆盖原有文件</param>
        /// <param name="fileName">新的文件名</param>
        /// <returns></returns>
        public string Save(string formField, bool isRenameSameFile, string fileName = null)
        {
            var Response = HttpContext.Current.Response;
            var Request = HttpContext.Current.Request;
            // 获取上传的文件
            HttpFileCollection file = Request.Files;
            HttpPostedFile postFile = file[formField];
            return Save(postFile, isRenameSameFile, fileName);
        }

        /// <summary>
        /// 保存表单文件,根据HttpPostedFile
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <param name="isRenameSameFile">值为true 同名文件进行重命名，false覆盖原有文件</param>
        /// <param name="fileName">新的文件名</param>
        /// <returns></returns>
        public string Save(HttpPostedFile postFile, bool isRenameSameFile, string fileName = null)
        {
            try
            {
                //文件名
                fileName = string.IsNullOrEmpty(fileName) ? postFile.FileName : fileName;

                //验证格式
                this.CheckingType(postFile.FileName);
                //验证大小
                this.CheckSize(postFile);

                if (GetError) return string.Empty;


                string webDir = string.Empty;
                // 获取存储目录
                var directory = this.GetDirectory(ref webDir);
                var filePath = directory + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    if (isRenameSameFile)
                    {
                        filePath = directory + DateTime.Now.ToString("yyyyMMddhhssms") + "-" + fileName;
                    }
                    else
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                // 保存文件
                postFile.SaveAs(filePath);
                GetFilePath = filePath;
                GetWebFilePath = webDir + fileName;
                GetFileName = postFile.FileName;
                return filePath;
            }
            catch (Exception ex)
            {
                TryError(ex.Message);
                return string.Empty;
            }
        }

        private void CheckSize(HttpPostedFile PostFile)
        {
            if (PostFile.ContentLength / 1024.0 / 1024.0 > SetMaxSizeM)
            {
                TryError(string.Format("对不起上传文件过大，不能超过{0}M！", SetMaxSizeM));
            }
        }



        /// <summary>
        /// 获取目录
        /// </summary>
        /// <returns></returns>
        private string GetDirectory(ref string webDir)
        {
            // 存储目录
            string directory = this.SetFileDirectory;

            // 目录格式
            string Date = DateTime.Now.ToString("yyyy-MM/dd");
            webDir = directory + "/" + Date + '/';
            string dir = HttpContext.Current.Server.MapPath(webDir);
            // 创建目录
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// 验证文件类型
        /// </summary>
        /// <param name="fileName"></param>
        private void CheckingType(string fileName)
        {
            // 获取允许允许上传类型列表
            string[] typeList = this.SetFileType.Split(',');

            // 获取上传文件类型(小写)
            string type = Path.GetExtension(fileName).ToLowerInvariant(); ;

            // 验证类型
            if (typeList.Contains(type) == false)
                this.TryError("文件类型非法!");
        }

        /// <summary>
        /// 抛出错误
        /// </summary>
        /// <param name="Msg"></param>
        private void TryError(string msg)
        {
            this.GetError = true;
            this.GetMessage = msg;
        }
    }
}