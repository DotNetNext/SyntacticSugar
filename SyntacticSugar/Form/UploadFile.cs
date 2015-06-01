using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;


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

        private ParamsModel _params;
        private string _Number { get; set; }

        public UploadFile()
        {
            _params = new ParamsModel()
            {
                FileDirectory = "/upload",
                FileType = ".pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif",
                MaxSizeM = 10,
                PathSaveType = PathSaveType.dateTimeNow,
                IsRenameSameFile=true
            };
        }

        /// <summary>
        /// 文件保存路径(默认:/upload)
        /// </summary>
        public void SetFileDirectory(string fileDirectory)
        {
            if (fileDirectory == null)
            {
                throw new ArgumentNullException("fileDirectory");
            }

            var isMapPath = Regex.IsMatch(fileDirectory, @"[a-z]\:\\", RegexOptions.IgnoreCase);
            if (isMapPath)
            {
                fileDirectory = GetRelativePath(fileDirectory);
            }
            _params.FileDirectory = fileDirectory;
        }

   
        /// <summary>
        /// 是否使用原始文件名作为新文件的文件名(默认:true)
        /// </summary>
        /// <param name="isUseOldFileName">true原始文件名,false系统生成新文件名</param>
        public void SetIsUseOldFileName(bool isUseOldFileName)
        {
            _params.IsUseOldFileName = isUseOldFileName;
        }

        /// <summary>
        /// 允许上传的文件类型, 逗号分割,必须全部小写! *表示所有 (默认值: .pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif )  
        /// </summary>
        public void SetFileType(string fileType)
        {
            _params.FileType = fileType;
        }
        /// <summary>
        /// 允许上传多少大小(单位：M)
        /// </summary>
        public void SetMaxSizeM(double maxSizeM)
        {
            _params.MaxSizeM = maxSizeM;
        }
        /// <summary>
        /// 重命名同名文件？ 
        /// </summary>
        /// <param name="isRenameSameFile">true:重命名,false覆盖</param>
        public void SetIsRenameSameFile(bool isRenameSameFile)
        {
            _params.IsRenameSameFile = isRenameSameFile;
        }


        /// <summary>
        /// 保存表单文件
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <returns></returns>
        public ResponseMessage Save(HttpPostedFile postFile)
        {
            return CommonSave(postFile);
        }



        /// <summary>
        /// 保存表单文件,根据编号创建子文件夹
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <param name="number">编号</param>
        /// <returns></returns>
        public ResponseMessage Save(HttpPostedFile postFile, string number)
        {

            _params.PathSaveType = PathSaveType.code;
            _Number = number;
            return CommonSave(postFile);
        }


        /// <summary>
        /// 保存表单文件,根据HttpPostedFile
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <param name="isRenameSameFile">值为true 同名文件进行重命名，false覆盖原有文件</param>
        /// <param name="fileName">新的文件名</param>
        /// <returns></returns>
        private ResponseMessage CommonSave(HttpPostedFile postFile)
        {

            ResponseMessage reval = new ResponseMessage();
            try
            {
                if (postFile == null || postFile.ContentLength == 0)
                {
                    TryError(reval, "没有文件！");
                    return reval;
                }

                //文件名
                string fileName = _params.IsUseOldFileName ? postFile.FileName : DateTime.Now.ToString("yyyyMMddhhmmssms") + Path.GetExtension(postFile.FileName);

                //验证格式
                this.CheckingType(reval, postFile.FileName);
                //验证大小
                this.CheckSize(reval, postFile);

                if (reval.Error) return reval;


                string webDir = string.Empty;
                // 获取存储目录
                var directory = this.GetDirectory(ref webDir);
                var filePath = directory + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    if (_params.IsRenameSameFile)
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
                reval.FilePath = filePath;
                reval.FilePath = webDir + fileName;
                reval.FileName = fileName;
                reval.WebFilePath = webDir + fileName;
                return reval;
            }
            catch (Exception ex)
            {
                TryError(reval, ex.Message);
                return reval;
            }
        }

        private void CheckSize(ResponseMessage message, HttpPostedFile PostFile)
        {
            if (PostFile.ContentLength / 1024.0 / 1024.0 > _params.MaxSizeM)
            {
                TryError(message, string.Format("对不起上传文件过大，不能超过{0}M！", _params.MaxSizeM));
            }
        }
        /// <summary>
        /// 根据物理路径获取相对路径
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="sever"></param>
        /// <returns></returns>
        private static string GetRelativePath(string fileDirectory)
        {
            var sever = HttpContext.Current.Server;
            fileDirectory = "/" + fileDirectory.Replace(sever.MapPath("~/"), "").TrimStart('/').Replace('\\', '/');
            return fileDirectory;
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <returns></returns>
        private string GetDirectory(ref string webDir)
        {
            var sever = HttpContext.Current.Server;
            // 存储目录
            string directory = _params.FileDirectory;

            // 目录格式
            string childDirectory = DateTime.Now.ToString("yyyy-MM/dd");
            if (_params.PathSaveType == PathSaveType.code)
            {
                childDirectory = _Number;
            }
            webDir = directory.TrimEnd('/') + "/" + childDirectory + '/';
            string dir = sever.MapPath(webDir);
            // 创建目录
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// 验证文件类型)
        /// </summary>
        /// <param name="fileName"></param>
        private void CheckingType(ResponseMessage message, string fileName)
        {
            if (_params.FileType != "*")
            {
                // 获取允许允许上传类型列表
                string[] typeList = _params.FileType.Split(',');

                // 获取上传文件类型(小写)
                string type = Path.GetExtension(fileName).ToLowerInvariant(); ;

                // 验证类型
                if (typeList.Contains(type) == false)
                    this.TryError(message, "文件类型非法!");
            }
        }

        /// <summary>
        /// 抛出错误
        /// </summary>
        /// <param name="Msg"></param>
        private void TryError(ResponseMessage message, string msg)
        {
            message.Error = true;
            message.Message = msg;
        }

        #region models

        private class ParamsModel
        {
            /// <summary>
            /// 文件保存路径
            /// </summary>
            public string FileDirectory { get; set; }
            /// <summary>
            /// 允许上传的文件类型, 逗号分割,必须全部小写!
            /// </summary>
            public string FileType { get; set; }
            /// <summary>
            /// 允许上传多少大小
            /// </summary>
            public double MaxSizeM { get; set; }
            /// <summary>
            /// 路径存储类型
            /// </summary>
            public PathSaveType PathSaveType { get; set; }
            /// <summary>
            /// 重命名同名文件? 
            /// </summary>
            public bool IsRenameSameFile { get; set; }
            /// <summary>
            /// 是否使用原始文件名
            /// </summary>
            public bool IsUseOldFileName { get; set; }
        }


        /// <summary>
        /// 路径存储类型
        /// </summary>
        public enum PathSaveType
        {
            /// <summary>
            /// 根据时间创建存储目录
            /// </summary>
            dateTimeNow = 0,
            /// <summary>
            /// 根据ID编号方式来创建存储目录
            /// </summary>
            code = 1

        }


        /// <summary>
        /// 反回信息
        /// </summary>
        public class ResponseMessage
        {
            /// <summary>
            /// 上传错误
            /// </summary>
            public bool Error { get; set; }
            /// <summary>
            /// 消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 网站路径
            /// </summary>
            public string WebFilePath { get; set; }
            /// <summary>
            /// 获取文件名
            /// </summary>
            public string FileName { get; set; }

        }
        #endregion
    }
}