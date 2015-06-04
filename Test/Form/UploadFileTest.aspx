
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFileTest.aspx.cs" Inherits="Test.file.UploadFileTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:fileupload ID="Fileupload1" runat="server"></asp:fileupload>
    <asp:button ID="Button1" runat="server" text="Button" OnClick="Unnamed1_Click" />


    </div>
    </form>
</body>
</html>
