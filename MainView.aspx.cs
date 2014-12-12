using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using maintableController;
using xml_MaintableController;
using sqlHelper;

public partial class MainView : System.Web.UI.Page
{
    List<string> heads = new List<string>();
    DataTable uniteTable;
    MaintableController tableController;
    XML_MaintableController XML_tableController;
    protected void Page_Load(object sender, EventArgs e){
        if (!IsPostBack){
            MaintableBind();
        }
    }

    protected void MaintableBind() {
        string username = Request.QueryString["username"];
        string userId = GetUserId();
        XML_tableController = new XML_MaintableController(username, "UserID", userId, "Id", "dbo.XMLTest", "dbo.users");
        tableController = XML_tableController;
        string sqlStr = "SELECT * FROM dbo.XMLTest WHERE UserID=" + userId;
        uniteTable = tableController.GetMainTable(sqlStr);
        Session["uniteTable"] = uniteTable;
        Session["tableController"] = XML_tableController;
        MainTable.DataSource = uniteTable;
        MainTable.DataBind();
    }

    public string GetUserId()
    {
        string username = Request.QueryString["username"];
        string userId = null;
        string sqlStr = "SELECT Id FROM dbo.users WHERE Name=@username";
        SqlParameter[] param = {
                                   new SqlParameter("@username",username)};
        SqlDataReader userIdReader = SQLHelper.ExecuteReader(sqlStr, param);
        if (userIdReader.Read())
        {
            userId = userIdReader["Id"].ToString();
        }
        return userId;
    }

    /*
    protected DataTable UniteTable(DataTable DBTable, DataTable XMLTableOrig){
        if (XMLTableOrig != null) {
            DataTable XMLTable = XMLTableOrig.Clone();
            XMLTable.Columns["Id"].DataType = typeof(Int32);
            foreach (DataRow row in XMLTableOrig.Rows)
            {
                XMLTable.ImportRow(row);
            }
            DataColumn[] primaryKeys = new DataColumn[1];
            primaryKeys[0] = XMLTable.Columns["Id"];
            XMLTable.PrimaryKey = primaryKeys;
            DBTable.Merge(XMLTable);
        }
        DBTable.PrimaryKey = null;
        DBTable.Columns.Remove("Id");
        DBTable.Columns.Remove("UserId");
        return DBTable;
    }
     */

    protected void addNewRow_Click(object sender, EventArgs e){
        uniteTable = (DataTable)Session["uniteTable"];
        DataRow newRow = uniteTable.NewRow();
        uniteTable.Rows.Add(newRow);
        MainTable.EditIndex = uniteTable.Rows.Count - 1;
        MainTable.DataSource = uniteTable;
        MainTable.DataBind();
    }
    protected void CreateCol_Click(object sender, EventArgs e){
        uniteTable = (DataTable)Session["uniteTable"];
        tableController = (MaintableController)Session["tableController"];
        DataColumn newColumn = new DataColumn(nameTbx.Text);
        newColumn.DefaultValue = defTbx.Text;
        uniteTable.Columns.Add(newColumn);
        MainTable.DataSource = uniteTable;
        MainTable.DataBind();
        string username = Request.QueryString["username"];
        tableController.AddTableColumn(nameTbx.Text, defTbx.Text);
    }
    protected void MainTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        MainTable.EditIndex = -1;
        MaintableBind();
    }
    protected void MainTable_RowEditing(object sender, GridViewEditEventArgs e)
    {
        MainTable.EditIndex = e.NewEditIndex;
        MaintableBind();
    }
    protected void MainTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow updateRow = MainTable.Rows[e.RowIndex];
        tableController = (MaintableController)Session["tableController"];
        List<string> DBHead = tableController.GetHeadFromDB("dbo.XMLTest");
        if (e.RowIndex != MainTable.Rows.Count - 1)
        {
            tableController.UpdateTableRow(updateRow, DBHead);
        }
        else
        {
            tableController.InsertTableRow(updateRow, DBHead);
        }
        MainTable.EditIndex = -1;
        MaintableBind();
    }
    /*
    protected void EditDBAndXML(GridViewRow updateRow, int RowIndex){
        string username = Request.QueryString["username"];
        string xmlPath = XMLHelper.GetXMLPath(username);
        DataTable XMLTable = XMLHelper.GetDataFromXML(xmlPath);
        int Id;
        int.TryParse(XMLTable.Rows[RowIndex]["Id"].ToString(), out Id);

        //DB insert
        string sqlStr = "UPDATE dbo.XMLTest SET name=@name, age=@age WHERE Id=@Id";
        string name = ((TextBox)updateRow.Cells[1].Controls[0]).Text;
        string age = ((TextBox)updateRow.Cells[2].Controls[0]).Text;
        SqlParameter[] param = { 
                               new SqlParameter("@Id",Id),
                               new SqlParameter("@name",name),
                               new SqlParameter("@age",age)};
        SQLHelper.ExecuteSql(sqlStr, param);
        //XML insert
        List<string> parameters = new List<string>();
        List<string> nameParams = new List<string>();
        for (int i = 3; i < updateRow.Cells.Count; i++)
        {
            nameParams.Add(MainTable.HeaderRow.Cells[i].Text);
            parameters.Add(((TextBox)updateRow.Cells[i].Controls[0]).Text);
        }
        XMLController.EditNodeValue(xmlPath,"Id", Id.ToString(), nameParams, parameters);
    }
    protected void InsertDBAndXML(GridViewRow updateRow)
    {
        string username = Request.QueryString["username"];
        string userId = DBController.GetUserId("dbo.users", "username", username, "Id");
        string xmlPath = XMLController.GetXMLPath(username);
        //DB insert
        string sqlStr = "INSERT INTO dbo.XMLTest(UserID,Name,Age) VALUES(@userId,@name,@age)";
        string name = ((TextBox)updateRow.Cells[1].Controls[0]).Text;
        string age = ((TextBox)updateRow.Cells[2].Controls[0]).Text;
        SqlParameter[] param = { 
                               new SqlParameter("@userId",userId),
                               new SqlParameter("@name",name),
                               new SqlParameter("@age",age)};
        SQLHelper.ExecuteSql(sqlStr, param);
        //XML insert;
        int uniqueId = GetUniqueId();
        List<string> parameters = new List<string>();
        List<string> nameParams = new List<string>();
        parameters.Add(uniqueId.ToString());
        nameParams.Add("Id");
        for (int i = 3; i < updateRow.Cells.Count; i++)
        {
            nameParams.Add(MainTable.HeaderRow.Cells[i].Text);
            parameters.Add(((TextBox)updateRow.Cells[i].Controls[0]).Text);
        }
        XMLController.AddNodeValue(xmlPath,nameParams, parameters);
    }
    
    protected int GetUniqueId()
    {
        int uniqueId = 0;
        string sqlStr = "SELECT IDENT_CURRENT('XMLTest') AS currentIdentity";
        SqlDataReader uniqueIdReader = SQLHelper.ExecuteReader(sqlStr);
        if (uniqueIdReader.Read())
        {
            string uniqueIdStr = uniqueIdReader["currentIdentity"].ToString();
            int.TryParse(uniqueIdStr, out uniqueId);
        }
        return uniqueId;
    }
    */

    protected void MainTable_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        MainTable.EditIndex = -1;
    }
    protected void MainTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow updateRow = MainTable.Rows[e.RowIndex];
        string username = Request.QueryString["username"];
        tableController = (MaintableController)Session["tableController"];
        tableController.DeleteTableRow(updateRow);
        MaintableBind();
    }
}