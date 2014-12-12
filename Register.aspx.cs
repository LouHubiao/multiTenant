using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sqlHelper;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void logBtn_Click(object sender, EventArgs e) {
        Response.Redirect("SignUp.aspx");
    }
    protected void regBtn_Click(object sender, EventArgs e)
    {
        Registe();
        Response.Redirect("SignUp.aspx");
    }
    private void Registe() {
        CreateUser();
        CreateXML();
    }
    protected void CreateUser() {
        string name = Request.Form["username"];
        string password = Request.Form["password"];
        string part = Request.Form["part"];
        string sql = "INSERT INTO dbo.users(Name,Pass,Part) VALUES(@name,@password,@part)";
        SqlParameter[] parameters = {
                                    new SqlParameter("@name",name),
                                    new SqlParameter("@password",password),
                                    new SqlParameter("@part",part),
                                    };
        SQLHelper.ExecuteSql(sql, parameters);
    }
    protected void CreateXML() {
        string xmlPath = ConfigurationManager.AppSettings["XMLsFile"] + @"\" + Request.Form["username"] + ".xml";
        XDocument doc = new XDocument(new XElement("extraData"));
        doc.Save(xmlPath);
    }
}