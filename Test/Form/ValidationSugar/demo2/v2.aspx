<%@ Page Language="C#" AutoEventWireup="true" CodeFile="v2.aspx.cs" Inherits="demo2_v2" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="jquery-validation-1.13.1/lib/jquery-1.9.1.js"></script>
    <script src="jquery-validation-1.13.1/dist/jquery.validate.js"></script>
    <script src="ValidationSugar.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var factory = new validateFactory($("form"));
            factory.init();


            $("#btnSubmit").click(function () {

                //factory.submit(); //直接提交表单

                //异步方式
                factory.ajaxSubmit(function () {
                    $("form").submit();//可以用ajax
                });
            });

        });
    </script>
    <style>
        .form_hint
        {
            display: none;
        }
        #form1 .error
        {
            color: Red;
        }
    </style>
</head>
<body>
    <h3>
        基于jquery.validate的前后台双验证</h3>
    <form method="post" action="PostV2.aspx" id="form1">
    <p>
        name:
        <input type="text" name="name">
    </p>
    <p>
        sex :<input type="radio" name="sex" value="0" />男
        <input type="radio" name="sex" value="1" />女
    </p>
    <p>
        email:
        <input type="text" name="email" />
    </p>
    <p>
        爱好:
        <input type="checkbox" value="1" name="hobbies">游戏
        <input type="checkbox" value="3" name="hobbies">下棋
        <input type="checkbox" value="2" name="hobbies">打酱油
    </p>
    <button id="btnSubmit" type="submit">
        提交</button>
    <%=bindScript %>
    </form>
</body>
</html>
