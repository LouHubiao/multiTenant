using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Data;

/// <summary>
/// XML控制类，XML扩展类型租户使用
/// </summary>
namespace xmlHelper
{
    public class XMLHelper
    {
        /// <summary>
        /// 获取对应租户xml文件路径，默认为Configuration中"XMLsFile"项+username+.xml
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>xml文件路径</returns>
        public static string GetXMLPath(string username)
        {
            string xmlPath = ConfigurationManager.AppSettings["XMLsFile"] + @"\" + username + ".xml";
            return xmlPath;
        }

        /// <summary>
        /// 加载xml文件，未异常处理
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <returns></returns>
        public static XmlDocument LoadXML(string filePath) {
            XmlDocument document;
            document = new XmlDocument();
            document.Load(filePath);    //加异常处理；
            return document;
        }

        /// <summary>
        /// 获取xml->DB的列名序列
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <returns>List-string</returns>
        public static List<string> GetHeadFromXML(string xmlPath)
        {
            List<string> heads = new List<string>();
            DataSet xmlSet = new DataSet();
            xmlSet.ReadXml(xmlPath);
            if (xmlSet.Tables.Count != 0)
            {
                DataTable XMLTable = xmlSet.Tables[0];
                foreach (DataColumn col in XMLTable.Columns)
                {
                    heads.Add(col.ColumnName);
                }
            }
            return heads;
        }

        /// <summary>
        /// 获取xml中的数据至DataTable
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataFromXML(string xmlPath)
        {
            DataSet xmlSet = new DataSet();
            xmlSet.ReadXml(xmlPath);
            DataTable XMLTable = null;
            if (xmlSet.Tables.Count != 0)
            {
                XMLTable = xmlSet.Tables[0];  //只有一个table
            }
            return XMLTable;
        }

        /// <summary>
        /// 将DataTable中添加的行同步更新到xml中
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="nameParams">列名集合</param>
        /// <param name="parameters">列值集合</param>
        public static void AddNodeValue(string filePath, List<string> nameParams, List<string> parameters)
        {
            XmlDocument document = LoadXML(filePath);
            XmlElement newElement = document.CreateElement("row");
            for (int i = 0; i < nameParams.Count; i++)
            {
                XmlElement node = document.CreateElement(nameParams[i]);
                node.InnerText = parameters[i];
                newElement.AppendChild(node);
            }
            document.DocumentElement.AppendChild(newElement);
            document.Save(filePath);
        }
        
        /// <summary>
        /// 将DataTable中修改的行同步更新到xml中
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="PK_Name">主键名</param>
        /// <param name="PK_Value">主键值</param>
        /// <param name="nameParams">其余列名</param>
        /// <param name="parameters">其余列值</param>
        public static void EditNodeValue(string filePath, string PK_Name,string PK_Value,List<string> nameParams, List<string> parameters) {
            XmlDocument document = LoadXML(filePath);
            XmlNodeList children = document.DocumentElement.ChildNodes;
            int find = 0;
            foreach (XmlNode child in children)
            {
                XmlNodeList grandChildren = child.ChildNodes;
                foreach (XmlNode grandChild in grandChildren) {
                    if (grandChild.Name.Equals(PK_Name)&&grandChild.InnerText.Equals(PK_Value)) {
                        find = 1;
                        break;
                    }
                }
                if (find == 1) {
                    for (int i = 0; i < nameParams.Count; i++) {
                        foreach (XmlNode grandChild in grandChildren)
                        {
                            if (grandChild.Name.Equals(nameParams[i])){
                                grandChild.InnerText = parameters[i];
                            }
                        }
                    }
                    find = 0;
                    break;
                }
            }
            document.Save(filePath);
        }
        
        /// <summary>
        /// 将DataTable中修改的行同步更新到xml中
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="PK_Name">主键名</param>
        /// <param name="PK_Value">主键值</param>
        public static void DeleteNodeValue(string filePath, string PK_Name, string PK_Value){
            XmlDocument document = LoadXML(filePath);
            XmlNodeList children = document.DocumentElement.ChildNodes;
            int find = 0;
            XmlNode deleteingNode = null;
            foreach (XmlNode child in children)
            {
                XmlNodeList grandChildren = child.ChildNodes;
                foreach (XmlNode grandChild in grandChildren)
                {
                    if (grandChild.Name.Equals(PK_Name) && grandChild.InnerText.Equals(PK_Value))
                    {
                        find = 1;
                        break;
                    }
                }
                if (find == 1)
                {
                    deleteingNode = child;
                    break;
                }
            }
            document.DocumentElement.RemoveChild(deleteingNode);
            document.Save(filePath);
        }

        /// <summary>
        /// 将DataTable中添加的列同步更新到xml中
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="colName">新增列名</param>
        /// <param name="colDef">新增列默认值</param>
        public static void AddNewColumn(string filePath,string colName,string colDef)
        {
            XmlDocument document = LoadXML(filePath);
            XmlNodeList children = document.DocumentElement.ChildNodes;
            foreach (XmlNode child in children) {
                XmlElement newElement = document.CreateElement(colName);
                newElement.InnerText = colDef;
                child.AppendChild(newElement);
            }
            document.Save(filePath);
        }

        /// <summary>
        /// 从XML中删除列
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="colName">删除列名</param>
        public static void DeleteNewColumn(string filePath, string colName)
        {
            XmlDocument document = LoadXML(filePath);
            XmlNodeList children = document.DocumentElement.ChildNodes;
            foreach (XmlNode child in children)
            {
                XmlNodeList grandChildren = child.ChildNodes;
                foreach (XmlNode grandChild in grandChildren)
                {
                    if (grandChild.Name.Equals(colName))
                    {
                        child.RemoveChild(grandChild);
                        break;
                    }
                }
            }
            document.Save(filePath);
        }
    }
}