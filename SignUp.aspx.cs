using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sqlHelper;
using System.Data.SqlClient;

public partial class SignUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) { 
            
        }
    }
    protected void regBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Register.aspx");
    }
    protected void logBtn_Click(object sender, EventArgs e)
    {
        log();
    }
    private void log()
    {
        string name = Request.Form["username"];
        string password = Request.Form["password"];
        string sql = "SELECT Name,Pass,Part FROM dbo.users WHERE Name=@name AND Pass=@password";
        SqlParameter[] parameters = {
                                    new SqlParameter("@name",name),
                                    new SqlParameter("@password",password),
                                    };
        SqlDataReader user = SQLHelper.ExecuteReader(sql, parameters);
        if (user.Read()){
            user.Close();
            Response.Redirect("MainView.aspx?username="+name);
        }
        else {
            Response.Write("用户名或密码错误");
        }
    }
}