using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using sqlHelper;
using System.Web.UI.WebControls;

namespace maintableController
{
    public abstract class MaintableController
    {
        public List<string> GetHeadFromDB(string DBName)
        {
            List<string> heads = new List<string>();
            string sqlStr = "SELECT name FROM syscolumns where id=object_id(N'" + DBName + "')";
            DataTable headTable = SQLHelper.ExecuteDt(sqlStr);
            foreach (DataRow aRow in headTable.Rows)
            {
                heads.Add(aRow["name"].ToString());
            }
            if (heads.Count == 0)
                System.Diagnostics.Debug.Write("GetHeadFromDB:heads.Count == 0");
            return heads;
        }
        public abstract DataTable GetMainTable(string sqlStr);
        public abstract void InsertTableRow(GridViewRow updateRow, List<string> DBHeader);
        public abstract void UpdateTableRow(GridViewRow updateRow, List<string> DBHeader);
        public abstract void DeleteTableRow(GridViewRow updateRow);
        public abstract void AddTableColumn(string colName, string colDef);
    }
}