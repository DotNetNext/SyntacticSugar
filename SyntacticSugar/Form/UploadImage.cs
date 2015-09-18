using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：图片上传类、支持水印、缩略图
    /// ** 创始时间：2015-5-28
    /// ** 修改时间：-
    /// ** 修改人：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4536626.html
    /// </summary>
    public class UploadImage
    {

        #region 属性
        /// <summary>
        /// 允许图片格式 （例如： .jpg|.gif ）
        /// </summary>
        public string SetAllowFormat { get; set; }
        /// <summary>
        /// 允许上传图片大小 (单位:1M)
        /// </summary>
        public double SetAllowSize { get; set; }
        /// <summary>
        /// 文字水印字符
        /// </summary>
        public string SetWordWater { get; set; }
        /// <summary>
        /// 图片水印
        /// </summary>
        public string SetPicWater { get; set; }
        /// <summary>
        /// 水印图片的位置 0居中、1左上角、2右上角、3左下角、4右下角
        /// </summary>
        public int SetPositionWater { get; set; }
        /// <summary>
        /// 缩略图宽度多个逗号格开（例如:200,100）
        /// </summary>
        public string SetSmallImgWidth { get; set; }
        /// <summary>
        /// 缩略图高度多个逗号格开（例如:200,100）
        /// </summary>
        public string SetSmallImgHeight { get; set; }
        /// <summary>
        /// 是否限制最大宽度，默认为true
        /// </summary>
        public bool SetLimitWidth { get; set; }
        /// <summary>
        /// 最大宽度尺寸，默认为600
        /// </summary>
        public int SetMaxWidth { get; set; }
        /// <summary>
        /// 是否剪裁图片，默认true
        /// </summary>
        public bool SetCutImage { get; set; }
        /// <summary>
        /// 限制图片最小宽度，0表示不限制
        /// </summary>
        public int SetMinWidth { get; set; }

        #endregion

        public UploadImage()
        {
            SetAllowFormat = ".jpeg|.jpg|.bmp|.gif|.png";   //允许图片格式
            SetAllowSize = 1;       //允许上传图片大小,默认为1MB
            SetPositionWater = 4;
            SetCutImage = true;
        }



        #region main method


        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="PostedFile">HttpPostedFile控件</param>
        /// <param name="SaveFolder">保存路径【sys.config配置路径】</param>
        /// <param name="oImgWidth">图片宽度</param>
        /// <param name="oImgHeight">图片高度</param>
        /// <param name="cMode">剪切类型</param>
        /// <returns>返回上传信息</returns>
        public ResponseMessage FileCutSaveAs(System.Web.HttpPostedFile PostedFile, string SaveFolder, int oImgWidth, int oImgHeight, CutMode cMode)
        {
            ResponseMessage rm = new ResponseMessage();
            try
            {
                //获取上传文件的扩展名
                string sEx = System.IO.Path.GetExtension(PostedFile.FileName);
                if (!CheckValidExt(SetAllowFormat, sEx))
                {
                    TryError(rm, 2);
                    return rm;
                }

                //获取上传文件的大小
                double PostFileSize = PostedFile.ContentLength / 1024.0 / 1024.0;

                if (PostFileSize > SetAllowSize)
                {
                    TryError(rm, 3);
                    return rm;  //超过文件上传大小
                }
                if (!System.IO.Directory.Exists(SaveFolder))
                {
                    System.IO.Directory.CreateDirectory(SaveFolder);
                }
                //重命名名称
                string NewfileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString("000");
                string fName = "s" + NewfileName + sEx;
                string fullPath = Path.Combine(SaveFolder, fName);
                PostedFile.SaveAs(fullPath);

                //重命名名称
                string sNewfileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString("000");
                string sFName = sNewfileName + sEx;
                rm.IsError = false;
                rm.FileName = sFName;
                string sFullPath = Path.Combine(SaveFolder, sFName);
                rm.filePath = sFullPath;
                rm.WebPath = "/" + sFullPath.Replace(HttpContext.Current.Server.MapPath("~/"), "").Replace("\\", "/");
                CreateSmallPhoto(fullPath, oImgWidth, oImgHeight, sFullPath, SetPicWater, SetWordWater, cMode);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                //压缩
                if (PostFileSize > 100)
                {
                    CompressPhoto(sFullPath, 100);
                }
            }
            catch (Exception ex)
            {
                TryError(rm, ex.Message);
            }
            return rm;
        }



        /// <summary>
        /// 通用图片上传类
        /// </summary>
        /// <param name="PostedFile">HttpPostedFile控件</param>
        /// <param name="SaveFolder">保存路径【sys.config配置路径】</param>
        /// <returns>返回上传信息</returns>
        public ResponseMessage FileSaveAs(System.Web.HttpPostedFile PostedFile, string SaveFolder)
        {
            ResponseMessage rm = new ResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(PostedFile.FileName))
                {
                    TryError(rm, 4);
                    return rm;
                }

                Random rd = new Random();
                int rdInt = rd.Next(1000, 9999);
                //重命名名称
                string NewfileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + rdInt;

                //获取上传文件的扩展名
                string sEx = System.IO.Path.GetExtension(PostedFile.FileName);
                if (!CheckValidExt(SetAllowFormat, sEx))
                {
                    TryError(rm, 2);
                    return rm;
                }

                //获取上传文件的大小
                double PostFileSize = PostedFile.ContentLength / 1024.0 / 1024.0;

                if (PostFileSize > SetAllowSize)
                {
                    TryError(rm, 3);
                    return rm;
                }


                if (!System.IO.Directory.Exists(SaveFolder))
                {
                    System.IO.Directory.CreateDirectory(SaveFolder);
                }

                rm.FileName = NewfileName + sEx;
                string fullPath = SaveFolder.Trim('\\') + "\\" + rm.FileName;
                rm.WebPath = "/" + fullPath.Replace(HttpContext.Current.Server.MapPath("~/"), "").Replace("\\", "/");
                rm.filePath = fullPath;
                rm.Size = PostFileSize;
                PostedFile.SaveAs(fullPath);


                System.Drawing.Bitmap bmp = new Bitmap(fullPath);
                int realWidth = bmp.Width;
                int realHeight = bmp.Height;
                bmp.Dispose();

                #region 检测图片宽度限制
                if (SetMinWidth > 0)
                {
                    if (realWidth < SetMinWidth)
                    {
                        TryError(rm, 7);
                        return rm;
                    }
                }
                #endregion

                #region 监测图片宽度是否超过600，超过的话，自动压缩到600
                if (SetLimitWidth && realWidth > SetMaxWidth)
                {
                    int mWidth = SetMaxWidth;
                    int mHeight = mWidth * realHeight / realWidth;

                    string tempFile = SaveFolder + Guid.NewGuid().ToString() + sEx;
                    File.Move(fullPath, tempFile);
                    CreateSmallPhoto(tempFile, mWidth, mHeight, fullPath, "", "");
                    File.Delete(tempFile);
                }
                #endregion

                #region 压缩图片存储尺寸
                if (sEx.ToLower() != ".gif")
                {
                    CompressPhoto(fullPath, 100);
                }
                #endregion



                //生成缩略图片高宽
                if (string.IsNullOrEmpty(SetSmallImgWidth))
                {
                    rm.Message = "上传成功,无缩略图";
                    return rm;
                }


                string[] oWidthArray = SetSmallImgWidth.Split(',');
                string[] oHeightArray = SetSmallImgHeight.Split(',');
                if (oWidthArray.Length != oHeightArray.Length)
                {
                    TryError(rm, 6);
                    return rm;
                }


                for (int i = 0; i < oWidthArray.Length; i++)
                {
                    if (Convert.ToInt32(oWidthArray[i]) <= 0 || Convert.ToInt32(oHeightArray[i]) <= 0)
                        continue;

                    string sImg = SaveFolder.TrimEnd('\\') + '\\' + NewfileName + "_" + i.ToString() + sEx;

                    //判断图片高宽是否大于生成高宽。否则用原图
                    if (realWidth > Convert.ToInt32(oWidthArray[i]))
                    {
                        if (SetCutImage)
                            CreateSmallPhoto(fullPath, Convert.ToInt32(oWidthArray[i]), Convert.ToInt32(oHeightArray[i]), sImg, "", "");
                        else
                            CreateSmallPhoto(fullPath, Convert.ToInt32(oWidthArray[i]), Convert.ToInt32(oHeightArray[i]), sImg, "", "", CutMode.CutNo);
                    }
                    else
                    {
                        if (SetCutImage)
                            CreateSmallPhoto(fullPath, realWidth, realHeight, sImg, "", "");
                        else
                            CreateSmallPhoto(fullPath, realWidth, realHeight, sImg, "", "", CutMode.CutNo);
                    }
                }

                #region 给大图添加水印
                if (!string.IsNullOrEmpty(SetPicWater))
                    AttachPng(SetPicWater, fullPath);
                else if (!string.IsNullOrEmpty(SetWordWater))
                    AttachText(SetWordWater, fullPath);
                #endregion


            }
            catch (Exception ex)
            {
                TryError(rm, ex.Message);
            }
            return rm;
        }

        #region 验证格式
        /// <summary>
        /// 验证格式
        /// </summary>
        /// <param name="allType">所有格式</param>
        /// <param name="chkType">被检查的格式</param>
        /// <returns>bool</returns>
        public bool CheckValidExt(string allType, string chkType)
        {
            bool flag = false;
            string[] sArray = allType.Split('|');
            foreach (string temp in sArray)
            {
                if (temp.ToLower() == chkType.ToLower())
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        #endregion

        #region 根据需要的图片尺寸，按比例剪裁原始图片
        /// <summary>
        /// 根据需要的图片尺寸，按比例剪裁原始图片
        /// </summary>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="img">原始图片</param>
        /// <returns>剪裁区域尺寸</returns>
        public Size CutRegion(int nWidth, int nHeight, Image img)
        {
            double width = 0.0;
            double height = 0.0;

            double nw = (double)nWidth;
            double nh = (double)nHeight;

            double pw = (double)img.Width;
            double ph = (double)img.Height;

            if (nw / nh > pw / ph)
            {
                width = pw;
                height = pw * nh / nw;
            }
            else if (nw / nh < pw / ph)
            {
                width = ph * nw / nh;
                height = ph;
            }
            else
            {
                width = pw;
                height = ph;
            }

            return new Size(Convert.ToInt32(width), Convert.ToInt32(height));
        }
        #endregion

        #region 等比例缩小图片
        public Size NewSize(int nWidth, int nHeight, Image img)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(img.Width);
            double sh = Convert.ToDouble(img.Height);
            double mw = Convert.ToDouble(nWidth);
            double mh = Convert.ToDouble(nHeight);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = nWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = nHeight;
                w = (h * sw) / sh;
            }

            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
        #endregion

        #region 生成缩略图

        #region 生成缩略图，不加水印
        /// <summary>
        /// 生成缩略图，不加水印
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        public void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile)
        {
            Image img = Image.FromFile(filename);
            ImageFormat thisFormat = img.RawFormat;

            Size CutSize = CutRegion(nWidth, nHeight, img);
            Bitmap outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - CutSize.Width) / 2;
            int nStartY = (img.Height - CutSize.Height) / 2;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                nStartX, nStartY, CutSize.Width, CutSize.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //if (thisFormat.Equals(ImageFormat.Gif))
            //{
            //    Response.ContentType = "image/gif";
            //}
            //else
            //{
            //    Response.ContentType = "image/jpeg";
            //}

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                //outBmp.Save(Response.OutputStream, jpegICI, encoderParams);
                outBmp.Save(destfile, jpegICI, encoderParams);
            }
            else
            {
                //outBmp.Save(Response.OutputStream, thisFormat);
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();
        }
        #endregion

        #region 生成缩略图，加水印
        public void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string sy, int nType)
        {
            if (nType == 0)
                CreateSmallPhoto(filename, nWidth, nHeight, destfile, sy, "");
            else
                CreateSmallPhoto(filename, nWidth, nHeight, destfile, "", sy);
        }
        #endregion

        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        /// <param name="png">图片水印</param>
        /// <param name="text">文本水印</param>
        public void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string png, string text)
        {
            Image img = Image.FromFile(filename);
            ImageFormat thisFormat = img.RawFormat;

            Size CutSize = CutRegion(nWidth, nHeight, img);
            Bitmap outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - CutSize.Width) / 2;
            int nStartY = (img.Height - CutSize.Height) / 2;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                nStartX, nStartY, CutSize.Width, CutSize.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(destfile, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            if (!string.IsNullOrEmpty(png))
                AttachPng(png, destfile);

            if (!string.IsNullOrEmpty(text))
                AttachText(text, destfile);
        }


        public void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string png, string text, CutMode cMode)
        {
            Image img = Image.FromFile(filename);

            if (nWidth <= 0)
                nWidth = img.Width;
            if (nHeight <= 0)
                nHeight = img.Height;

            int towidth = nWidth;
            int toheight = nHeight;

            switch (cMode)
            {
                case CutMode.CutWH://指定高宽缩放（可能变形）               
                    break;
                case CutMode.CutW://指定宽，高按比例                   
                    toheight = img.Height * nWidth / img.Width;
                    break;
                case CutMode.CutH://指定高，宽按比例
                    towidth = img.Width * nHeight / img.Height;
                    break;
                case CutMode.CutNo: //缩放不剪裁
                    int maxSize = (nWidth >= nHeight ? nWidth : nHeight);
                    if (img.Width >= img.Height)
                    {
                        towidth = maxSize;
                        toheight = img.Height * maxSize / img.Width;
                    }
                    else
                    {
                        toheight = maxSize;
                        towidth = img.Width * maxSize / img.Height;
                    }
                    break;
                default:
                    break;
            }
            nWidth = towidth;
            nHeight = toheight;

            ImageFormat thisFormat = img.RawFormat;

            Size CutSize = new Size(nWidth, nHeight);
            if (cMode != CutMode.CutNo)
                CutSize = CutRegion(nWidth, nHeight, img);

            Bitmap outBmp = new Bitmap(CutSize.Width, CutSize.Height);

            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - CutSize.Width) / 2;
            int nStartY = (img.Height - CutSize.Height) / 2;

            //int x1 = (outBmp.Width - nWidth) / 2;
            //int y1 = (outBmp.Height - nHeight) / 2;

            if (cMode != CutMode.CutNo)
                g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                    nStartX, nStartY, CutSize.Width, CutSize.Height, GraphicsUnit.Pixel);
            else
                g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(destfile, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            if (!string.IsNullOrEmpty(png))
                AttachPng(png, destfile);

            if (!string.IsNullOrEmpty(text))
                AttachText(text, destfile);
        }
        #endregion

        #endregion

        #region 添加文字水印
        public void AttachText(string text, string file)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!System.IO.File.Exists(file))
                return;

            System.IO.FileInfo oFile = new System.IO.FileInfo(file);
            string strTempFile = System.IO.Path.Combine(oFile.DirectoryName, Guid.NewGuid().ToString() + oFile.Extension);
            oFile.CopyTo(strTempFile);

            Image img = Image.FromFile(strTempFile);
            ImageFormat thisFormat = img.RawFormat;

            int nHeight = img.Height;
            int nWidth = img.Width;

            Bitmap outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                0, 0, nWidth, nHeight, GraphicsUnit.Pixel);

            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //通过循环这个数组，来选用不同的字体大小
            //如果它的大小小于图像的宽度，就选用这个大小的字体
            for (int i = 0; i < 7; i++)
            {
                //设置字体，这里是用arial，黑体
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = g.MeasureString(text, crFont);

                if ((ushort)crSize.Width < (ushort)nWidth)
                    break;
            }

            //因为图片的高度可能不尽相同, 所以定义了
            //从图片底部算起预留了5%的空间
            int yPixlesFromBottom = (int)(nHeight * .08);

            //现在使用版权信息字符串的高度来确定要绘制的图像的字符串的y坐标

            float yPosFromBottom = ((nHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //计算x坐标
            float xCenterOfImg = (nWidth / 2);

            //把文本布局设置为居中
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //通过Brush来设置黑色半透明
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //绘制版权字符串
            g.DrawString(text,                 //版权字符串文本
                crFont,                                   //字体
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //位置
                StrFormat);

            //设置成白色半透明
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制版权字符串来创建阴影效果
            //记住移动文本的位置1像素
            g.DrawString(text,                 //版权文本
                crFont,                                   //字体
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg, yPosFromBottom),  //位置
                StrFormat);

            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(file, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(file, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            System.IO.File.Delete(strTempFile);
        }
        #endregion

        #region 添加图片水印
        ///<summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="png">水印图片</param>
        /// <param name="file">原文件</param>
        /// <param name="position">水印图片的位置 0居中、1左上角、2右上角、3左下角、4右下角</param>
        public void AttachPng(string png, string file)
        {
            if (string.IsNullOrEmpty(png))
                return;

            if (!System.IO.File.Exists(png))
                return;

            if (!System.IO.File.Exists(file))
                return;

            System.IO.FileInfo oFile = new System.IO.FileInfo(file);
            string strTempFile = System.IO.Path.Combine(oFile.DirectoryName, Guid.NewGuid().ToString() + oFile.Extension);
            oFile.CopyTo(strTempFile);

            Image img = Image.FromFile(strTempFile);
            ImageFormat thisFormat = img.RawFormat;
            int nHeight = img.Height;
            int nWidth = img.Width;

            Bitmap outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                0, 0, nWidth, nHeight, GraphicsUnit.Pixel);

            img.Dispose();

            img = Image.FromFile(png);

            //Bitmap bmpPng = new Bitmap(img);

            //ImageAttributes imageAttr = new ImageAttributes();
            //Color bg = Color.Green;
            //imageAttr.SetColorKey(bg, bg);

            Size pngSize = NewSize(nWidth, nHeight, img);
            int nx = 0;
            int ny = 0;
            int padding = 10;
            if (SetPositionWater == 0)
            {
                nx = (nWidth - pngSize.Width) / 2;
                ny = (nHeight - pngSize.Height) / 2;
            }
            else if (SetPositionWater == 1)
            {
                nx = padding;
                ny = padding;
            }
            else if (SetPositionWater == 2)
            {
                nx = (nWidth - pngSize.Width) - padding;
                ny = padding;
            }
            else if (SetPositionWater == 3)
            {
                nx = padding;
                ny = (nHeight - pngSize.Height) - padding;
            }
            else
            {
                nx = (nWidth - pngSize.Width) - padding;
                ny = (nHeight - pngSize.Height) - padding;
            }
            g.DrawImage(img, new Rectangle(nx, ny, pngSize.Width, pngSize.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (jpegICI != null)
            {
                outBmp.Save(file, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(file, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            System.IO.File.Delete(strTempFile);
        }
        #endregion

        #region 得到指定mimeType的ImageCodecInfo
        /// <summary>
        /// 保存JPG时用
        /// </summary>
        /// <param name="mimeType"> </param>
        /// <returns>得到指定mimeType的ImageCodecInfo </returns>
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        #endregion

        #region 保存为JPEG格式，支持压缩质量选项
        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="SourceFile"></param>
        /// <param name="FileName"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public bool KiSaveAsJPEG(string SourceFile, string FileName, int Qty)
        {
            Bitmap bmp = new Bitmap(SourceFile);

            try
            {
                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);

                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
                ps.Param[0] = p;

                bmp.Save(FileName, GetCodecInfo("image/jpeg"), ps);

                bmp.Dispose();

                return true;
            }
            catch
            {
                bmp.Dispose();
                return false;
            }

        }
        #endregion

        #region 将图片压缩到指定大小
        /// <summary>
        /// 将图片压缩到指定大小
        /// </summary>
        /// <param name="FileName">待压缩图片</param>
        /// <param name="size">期望压缩后的尺寸</param>
        public void CompressPhoto(string FileName, int size)
        {
            if (!System.IO.File.Exists(FileName))
                return;

            int nCount = 0;
            System.IO.FileInfo oFile = new System.IO.FileInfo(FileName);
            long nLen = oFile.Length;
            while (nLen > size * 1024 && nCount < 10)
            {
                string dir = oFile.Directory.FullName;
                string TempFile = System.IO.Path.Combine(dir, Guid.NewGuid().ToString() + "." + oFile.Extension);
                oFile.CopyTo(TempFile, true);

                KiSaveAsJPEG(TempFile, FileName, 70);

                try
                {
                    System.IO.File.Delete(TempFile);
                }
                catch { }

                nCount++;

                oFile = new System.IO.FileInfo(FileName);
                nLen = oFile.Length;
            }
        }
        #endregion

        #endregion


        #region common method

        /// <summary>
        /// 图片上传错误编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetCodeMessage(int code)
        {
            var dic = new Dictionary<int, string>(){
        {0,"系统配置错误"},
        {1,"上传图片成功"},
        {2,string.Format( "对不起，上传格式错误！请上传{0}格式图片",SetAllowFormat)},
        {3,string.Format("超过文件上传大小,不得超过{0}M",SetAllowSize)},
        {4,"未上传文件"},
        {5,""},
        {6,"缩略图长度和宽度配置错误"},
        {7,"检测图片宽度限制"}
         };
            return dic[code];
        }
        private void TryError(ResponseMessage rm, int code)
        {
            rm.IsError = true;
            rm.Message = GetCodeMessage(code);
        }
        private void TryError(ResponseMessage rm, string message)
        {
            rm.IsError = true;
            rm.Message = message;
        }
        #endregion

        #region models
        public enum CutMode
        {
            /// <summary>
            /// 根据高宽剪切
            /// </summary>
            CutWH = 1,
            /// <summary>
            /// 根据宽剪切
            /// </summary>
            CutW = 2,
            /// <summary>
            /// 根据高剪切
            /// </summary>
            CutH = 3,
            /// <summary>
            /// 缩放不剪裁
            /// </summary>
            CutNo = 4
        }
        public class ResponseMessage
        {
            /// <summary>
            /// 是否遇到错误
            /// </summary>
            public bool IsError { get; set; }
            /// <summary>
            /// web路径
            /// </summary>
            public string WebPath { get; set; }
            /// <summary>
            /// 文件物理路径
            /// </summary>
            public string filePath { get; set; }
            /// <summary>
            /// 反回消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 文件大小
            /// </summary>
            public double Size { get; set; }
            /// <summary>
            /// 图片名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 图片目录
            /// </summary>
            public string Directory
            {
                get
                {
                    if (WebPath == null) return null;
                    return WebPath.Replace(FileName, "");
                }
            }
            /// <summary>
            /// 缩略图路径
            /// </summary>
            public string SmallPath(int index)
            {
                return string.Format("{0}{1}_{2}{3}", Directory, Path.GetFileNameWithoutExtension(FileName), index, Path.GetExtension(FileName));
            }
        }
        #endregion
    }

}