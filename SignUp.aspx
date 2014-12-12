<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignUp.aspx.cs" Inherits="SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="~/Style/SignStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="logMain">
        <h1>登陆</h1>
        <form class="flp" runat="server">
            <div>
                <input type="text" id="username" name="username" />
                <label for="username">用户名</label>
            </div>
            <div>
                <input type="password" id="password" name="password" />
                <label for="password">密码</label>
            </div>
            <div>
                <asp:Button ID="logBtn" CssClass="btn gray" Text="登陆" runat="server" OnClick="logBtn_Click"/>
                <asp:Button ID="regBtn" CssClass="btn gray" Text="注册" runat="server" OnClick="regBtn_Click"/>
            </div>
        </form>
    </div>
    <script src="Script/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="Script/jquery.easing.min.js" type="text/javascript"></script>
    <script src="Script/SignScript.js" type="text/javascript"></script>
</body>
</html>