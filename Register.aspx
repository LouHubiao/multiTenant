<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="~/Style/SignStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="regMain">
        <h1>注册</h1>
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
                <input type="password" id="passwordConfirm" name="passwordConfirm" />
                <label for="passwordConfirm">确认密码</label>
            </div>
            <div>
                <input type="radio" id="radio-1-1" class="regular-radio" name="part" value="1" /><label for="radio-1-1"></label>
                <span class="regSpan">XML</span>
                <input type="radio" id="radio-1-2" class="regular-radio" name="part" value="2" /><label for="radio-1-2"></label>
                <span class="regSpan">共享数据库</span>
                <input type="radio" id="radio-1-3" class="regular-radio" name="part" value="3" /><label for="radio-1-3"></label>
                <span class="regSpan">共享表</span>
            </div>
            <div>
                <asp:Button ID="regBtn" CssClass="btn gray" Text="确认注册" runat="server" OnClick="regBtn_Click"/>
                <asp:Button ID="logBtn" CssClass="btn gray" Text="已有账户？" runat="server" OnClick="logBtn_Click"/>
            </div>
        </form>
    </div>
    <script src="Script/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="Script/jquery.easing.min.js" type="text/javascript"></script>
    <script src="Script/SignScript.js" type="text/javascript"></script>
</body>
</html>
