<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ValidationSugarTest.aspx.cs"
    Inherits="Test.Form.ValidationSugarTest" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="http://www.52mvc.com/javascript/jquery.js"></script>
    <link href="jsLib/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" class="contact_form">
    <ul>
        <li>
            <h2>
                表单验证</h2>
            <span class="required_notification">* 表示必填项</span> </li>
        <li>
            <label for="name">
                姓名:</label>
            <input type="text" name="name" />
        </li>
        <li>
            <label>
                姓别:</label>
            <input type="radio" value="1" name="sex" />男
            <input type="radio" value="0" name="sex" />女 </li>
        <li>
            <label for="email">
                电子邮件:</label>
            <input type="email" name="email" />
        </li>
        <li>
            <label for="website">
                手 机:</label>
            <input type="text" name="phone" />
        </li>
        <li>
            <label for="website">
                学 历:</label>
            <select name="education" >
                <option value="">==请选择==</option>
                <option value="1">大学</option>
            </select>
        </li>
        <li>
            <label for="message">
                备注:</label>
            <textarea name="remark" cols="40" rows="6"></textarea>
        </li>
        <li></li>
    </ul>
    <br />
    <asp:Button ID="Button1" runat="server" Text="submit" CssClass="submit" OnClick="Button1_Click" />
    </form>
    <%=ViewState["intiscript"]%>
</body>
</html>
