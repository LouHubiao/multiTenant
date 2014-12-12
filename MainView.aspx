﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainView.aspx.cs" Inherits="MainView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Script/jquery-2.1.1.min.js" type="text/javascript"></script>
    <link href="~/Style/MainView.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="addNewRow" CssClass="tbEditBtn" runat="server" Text="创建新行" OnClick="addNewRow_Click" />
        </div>
        <div>
            <asp:Button ID="CreateCol" Text="创建新列" OnClick="CreateCol_Click" runat="server" />
            <asp:Label ID="Label1" CssClass="newCol" Text="列名" runat="server"></asp:Label>
            <asp:TextBox ID="nameTbx" runat="server"></asp:TextBox>
            <!--
            <asp:Label ID="Label2" CssClass="newCol" Text="可空" runat="server"></asp:Label>
            <asp:DropDownList ID="canNullDpl" runat="server">
                <asp:ListItem>NOT NULL</asp:ListItem>
                <asp:ListItem>NULL</asp:ListItem>
            </asp:DropDownList>
            -->
            <asp:Label ID="defVal" CssClass="newCol" Text="默认值" runat="server"></asp:Label>
            <asp:TextBox ID="defTbx" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:GridView ID="MainTable" runat="server" CssClass="MainTable" Width="800" AutoGenerateDeleteButton="true" AutoGenerateEditButton="true" OnRowEditing="MainTable_RowEditing"
                OnRowCancelingEdit="MainTable_RowCancelingEdit" OnRowUpdating="MainTable_RowUpdating" OnRowUpdated="MainTable_RowUpdated" OnRowDeleting="MainTable_RowDeleting">
            </asp:GridView>
        </div>
    </form>
</body>
</html>