<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImageTest.aspx.cs" Inherits="Test.file.UploadImageTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1"
            runat="server" />
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
        <br />
        <br />
        1<br />
        <asp:Image ID="Image1" runat="server" />
        <br />
        <br />
        2<br />
        <asp:Image ID="Image2" runat="server" />
    </div>
    </form>
</body>
</html>
