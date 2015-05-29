<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageStringTest.aspx.cs" Inherits="Test.UI.PageStringTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
       .pagination .click {cursor:pointer}
        .pagination a{text-decoration: none;border: 1px solid #AAE;color: #15B;font-size: 13px;border-radius: 2px;}
        .pagination span{ color:#666;font-size:13px;display: inline-block;border: 1px solid #ccc;padding: 0.2em 0.6em;}
        .pagination span.pagetext{ border:none}
        .pagination a:hover{background: #26B;color: #fff;}
        .pagination a{display: inline-block;padding: 0.2em 0.6em;}
        .pagination .page_current{background: #26B;color: #fff;border: 1px solid #AAE;margin-right: 5px;}
        .pagination{margin-top: 20px;}
        .pagination .current.prev, .pagination .current.next{color: #999;border-color: #999;background: #fff;}
    </style>

    <script>
        function ajaxPage(pageIndex) {
            alert(pageIndex);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
