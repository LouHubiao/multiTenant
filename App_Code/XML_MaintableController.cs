using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using xmlHelper;
using sqlHelper;
using System.Web.UI.WebControls;
using maintableController;

namespace xml_MaintableController
{
    public class XML_MaintableController : MaintableController
    {
        private string username;
        private string userIdName;
        private string userId;
        private string idName;
        private string valueDBName;
        private string userDBName;

        public XML_MaintableController(string username, string userIdName, string userId, string idName, string valueDBName, string userDBName)
        {
            this.username = username;
            this.userIdName = userIdName;
            this.userId = userId;
            this.idName = idName;
            this.valueDBName = valueDBName;
            this.userDBName = userDBName;
        }

        public override DataTable GetMainTable(string sqlStr)
        {
            string xmlPath = XMLHelper.GetXMLPath(username);
            DataTable DBTable = SQLHelper.ExecuteDt(sqlStr);
            DataTable XMLTable = XMLHelper.GetDataFromXML(xmlPath);
            DataTable uniteTable = UniteTable(DBTable, XMLTable);
            return uniteTable;
        }

        public override void InsertTableRow(GridViewRow updateRow, List<string> DBHeader)
        {
            //DBInsert
            List<SqlParameter> paramList = new List<SqlParameter>();
            string sqlStr = "INSERT INTO " + valueDBName + "(";
            foreach (string head in DBHeader)
            {
                if (!head.Equals(idName)) {
                    sqlStr = sqlStr + head + ",";
                }
            }
            sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            sqlStr = sqlStr + ") VALUES(";
            foreach (string head in DBHeader)
            {
                int index = GetIndexFromColName(updateRow, head);
                if (index != updateRow.Cells.Count)
                {
                    string valuePara = ((TextBox)updateRow.Cells[index].Controls[0]).Text;
                    sqlStr = sqlStr + "@" + head + ",";
                    paramList.Add(new SqlParameter("@" + head, valuePara));
                }
            }
            //默认userId不出现在前台
            sqlStr = sqlStr + "@userId)";
            paramList.Add(new SqlParameter("@userId", userId));
            SqlParameter[] param = paramList.ToArray();
            SQLHelper.ExecuteSql(sqlStr, param);
            //XMLInsert
            string xmlPath = XMLHelper.GetXMLPath(username);
            List<string> XMLHeader = XMLHelper.GetHeadFromXML(xmlPath);
            string newId = GetNewId();
            List<string> paraVals = new List<string>();
            List<string> paraNames = new List<string>();
            paraVals.Add(newId);
            foreach (string head in XMLHeader)
            {
                paraNames.Add(head);
                int index = GetIndexFromColName(updateRow, head);
                if (index != updateRow.Cells.Count)
                {
                    paraVals.Add(((TextBox)updateRow.Cells[index].Controls[0]).Text);
                }
            }
            XMLHelper.AddNodeValue(xmlPath, paraNames, paraVals);
        }

        public override void UpdateTableRow(GridViewRow updateRow, List<string> DBHeader)
        {
            string xmlPath = XMLHelper.GetXMLPath(username);
            DataTable XMLTable = XMLHelper.GetDataFromXML(xmlPath);
            string id = XMLTable.Rows[updateRow.RowIndex][idName].ToString();
            //DBUpdate
            string sqlStr = "UPDATE " + valueDBName + " SET ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            foreach (string head in DBHeader)
            {
                int index = 0;
                foreach (DataControlFieldCell cell in updateRow.Cells)
                {
                    if (cell.ContainingField is BoundField)
                        if (((BoundField)cell.ContainingField).DataField.Equals(head))
                            break;
                    index++;
                }
                if (index != updateRow.Cells.Count) {
                    string valuePara = ((TextBox)updateRow.Cells[index].Controls[0]).Text;
                    sqlStr = sqlStr + head + "=@" + head + ",";
                    paramList.Add(new SqlParameter("@" + head, valuePara));
                }
            }
            sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            sqlStr = sqlStr + " WHERE " + idName + "=@id";
            paramList.Add(new SqlParameter("@id", id));
            SqlParameter[] param = paramList.ToArray();
            SQLHelper.ExecuteSql(sqlStr,param);
            //XMLUpdate
            List<string> XMLHeader = XMLHelper.GetHeadFromXML(xmlPath);
            List<string> paraVals = new List<string>();
            List<string> paraNames = new List<string>();
            paraVals.Add(id);
            foreach (string head in XMLHeader)
            {
                paraNames.Add(head);
                int index = GetIndexFromColName(updateRow, head);
                if (index != updateRow.Cells.Count)
                {
                    paraVals.Add(((TextBox)updateRow.Cells[index].Controls[0]).Text);
                }
            }
            XMLHelper.EditNodeValue(xmlPath, idName, id, paraNames, paraVals);
        }

        public override void DeleteTableRow(GridViewRow updateRow)
        {
            string xmlPath = XMLHelper.GetXMLPath(username);
            DataTable XMLTable = XMLHelper.GetDataFromXML(xmlPath);
            string id = XMLTable.Rows[updateRow.RowIndex][idName].ToString();
            //DBDelete
            string sqlStr = "DELETE FROM " + valueDBName + " WHERE " + idName + "=@id";
            SqlParameter[] param = { new SqlParameter("@id", id) };
            SQLHelper.ExecuteSql(sqlStr,param);
            //XMLDelete
            XMLHelper.DeleteNodeValue(xmlPath, idName, id);
        }

        public override void AddTableColumn(string colName, string colDef)
        {
            string xmlPath = XMLHelper.GetXMLPath(username);
            XMLHelper.AddNewColumn(xmlPath, colName, colDef);
        }

        protected DataTable UniteTable(DataTable DBTable, DataTable XMLTableOrig)
        {
            if (XMLTableOrig != null)
            {
                DataTable XMLTable = XMLTableOrig.Clone();
                XMLTable.Columns[idName].DataType = typeof(Int32);
                foreach (DataRow row in XMLTableOrig.Rows)
                {
                    XMLTable.ImportRow(row);
                }
                DataColumn[] XMLprimaryKeys = new DataColumn[1];
                XMLprimaryKeys[0] = XMLTable.Columns[idName];
                XMLTable.PrimaryKey = XMLprimaryKeys;
                DBTable.Merge(XMLTable);
            }
            DBTable.PrimaryKey = null;
            DBTable.Columns.Remove(idName);
            DBTable.Columns.Remove(userIdName);
            return DBTable;
        }

        protected string GetNewId()
        {
            string uniqueId = null;
            string sqlStr = "SELECT IDENT_CURRENT('" + valueDBName + "') AS currentIdentity";
            SqlDataReader uniqueIdReader = SQLHelper.ExecuteReader(sqlStr);
            if (uniqueIdReader.Read())
            {
                uniqueId = uniqueIdReader["currentIdentity"].ToString();
            }
            return uniqueId;
        }

        protected int GetIndexFromColName(GridViewRow updateRow,string head)
        {
            int index = 0;
            foreach (DataControlFieldCell cell in updateRow.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(head))
                        break;
                index++;
            }
            return index;
        }
    }
}
