using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

public partial class demo2_PostV2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string errorMessage = "";
        string _pageKey = "v2.aspx";
        bool isSuccess = ValidationSugar.PostValidation(_pageKey, out errorMessage);
        Response.Clear();
        if (isSuccess)
        {
            //提交
            Response.Write("提交成功！");
        }
        else
        {
            //提交
            Response.Write("对不起请求数据格式不正确，请检查浏览器是否支持JavaScript！");
            // throw new HttpException("对不起请求数据格式不正确，请检查浏览器是否支持JavaScript！");
        }
        Response.End();
    }
}