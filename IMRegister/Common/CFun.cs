using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace IMRegister
{
    public class CFun
    {

        #region Data Functions

        public static String fnGetValue(String strQuery)
        {
            try
            {
                return DBManager.GetDataTable(strQuery.ToLower()).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        public static clsResults fnSaveData(String strUserId, String strTableName, String[] strFields, String[] strValues, String strCriteria = "", String strPrimaryKey = "Id", Boolean ConnectToDB = true)
        {
            clsResults oResults = new clsResults();
            if (strFields.Length != strValues.Length)
            {
                oResults.ErrorCode = "1";
                oResults.ErrorMessage = "Fields and Values are not Equal";
                return oResults;
            }

            if (ConnectToDB)
                DBManager.ConnectToDatabase();

            try
            {
                if (strCriteria == "")
                {
                    DBManager.ExecuteQuery(GetInsertQuery(strUserId, strTableName, strFields, strValues));

                    String SlQuery = "select  " + strPrimaryKey + " from " + strTableName + " order by MDAT  DESC";
                    oResults.ErrorCode = "0";
                    oResults.AdditionalInfo = DBManager.GetDataTable(SlQuery).Rows[0][0].ToString();
                }
                else
                {

                    DBManager.ExecuteQuery(GetUpdateQuery(strUserId, strTableName, strFields, strValues, strCriteria));

                    //String SlQuery = "select " + strPrimaryKey + " from " + strTableName + " order by MDAT DESC";
                    oResults.ErrorCode = "0";
                    //oResults.AdditionalInfo = DBManager.ExecuteScalar(SlQuery);
                }
            }
            catch (Exception ex)
            {
                oResults.ErrorCode = "1";
                oResults.ErrorMessage = ex.Message;
                return oResults;

            }
            finally
            {
                if (ConnectToDB)
                {
                    DBManager.DisconnectToDataBase();
                }
            }
            return oResults;
        }

        public static clsResults fnDeleteData(String strTableName, String strCriteria = "")
        {
            String strQuery = "Delete From " + strTableName;
            if (strCriteria != "")
                strQuery += " Where " + strCriteria;

            DBManager.ConnectToDatabase();
            clsResults oResults = new clsResults();

            try
            {
                int result = DBManager.ExecuteQuery(strQuery);

                if (result == 0)
                {
                    oResults.ErrorCode = "1";
                    oResults.ErrorMessage = "No Record Deleted";
                }
                else
                {
                    oResults.ErrorCode = "0";
                }
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction();
                oResults.ErrorCode = "1";
                oResults.ErrorMessage = ex.Message;
                return oResults;
            }

            DBManager.DisconnectToDataBase();

            return oResults;
        }

        public static clsResults fnExecuteQuery(String strQuery, Boolean ConnectToDB = true)
        {
            clsResults oResults = new clsResults();
            if (ConnectToDB)
            {
                DBManager.ConnectToDatabase();
            }

            try
            {
                DBManager.ExecuteQuery(strQuery);
                oResults.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                oResults.ErrorCode = "1";
                oResults.ErrorMessage = ex.Message;
                return oResults;
            }
            finally
            {
                if (ConnectToDB)
                {
                    DBManager.DisconnectToDataBase();
                }
            }
            return oResults;
        }

        public static clsResults fnSelectData(String strTableName, String[] strFields, String strCriteria = "")
        {
            String strQuery = "Select " + String.Join(",", strFields) + " From " + strTableName;
            if (strCriteria != "")
                strQuery += " Where " + strCriteria;

            return fnSelectData(strQuery);
        }

        public static clsResults fnSelectData(String strQuery)
        {
            DBManager.ConnectToDatabase();
            clsResults oResults = new clsResults();

            try
            {
                oResults.dtRecords = DBManager.GetDataTable(strQuery);
                oResults.ErrorCode = "0";
            }
            catch (Exception ex)
            {
                DBManager.DisconnectToDataBase();
                oResults.ErrorCode = "1";
                oResults.ErrorMessage = ex.Message;
                return oResults;
            }

            DBManager.DisconnectToDataBase();

            return oResults;
        }

        #endregion

        #region Registry Values

        public static void SetRegistryValue(String strKey, String strValue)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(strKey);
            key.SetValue(strKey, strValue);
            key.Close();
        }

        public static String GetRegistryValue(String strKey)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(strKey);
            String strValue = key.GetValue(strKey).ToString();
            key.Close();

            return strValue; // (Microsoft.Win32.Registry.CurrentUser.GetValue(strKey) == null ? "" : Microsoft.Win32.Registry.CurrentUser.GetValue(strKey)).ToString();
        }

        #endregion

        #region Genral

        /// <summary>

        /// Method used to get the enumeration values and names in the key value data table. 

        /// It returns Key Value Datatable containing enumeration keys in its value column and enumeration values 

        /// in its key column

        /// </summary>

        /// <param name="enumType"></param>

        /// <returns>Datatable containing enumeration keys in its value column and enumeration values 

        /// in its key column

        /// </returns>      
        public static Dictionary<String, string> GetEnumerationDataTable(Type enumeration, Boolean IncludeEmptyRow = false)
        {
            Dictionary<String, string> dtKeyValue = new Dictionary<String, string>();
            int i = 1;
            if (IncludeEmptyRow)
            {
                dtKeyValue.Add("", " ");
            }
            foreach (FieldInfo Fi in enumeration.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                dtKeyValue.Add(Fi.GetValue(enumeration).ToString(), Fi.GetValue(enumeration).ToString());
            }

            return dtKeyValue;
        }

        public static Boolean ExistsInEnumeration(Type enumeration, String strValue)
        {
            foreach (FieldInfo Fi in enumeration.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (Fi.GetValue(enumeration).ToString().ToUpper() == strValue.ToUpper())
                    return true;
            }
            return false;
        }

        public static int ValueFromEnumeration(Type enumeration, String strValue)
        {
            foreach (FieldInfo Fi in enumeration.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (Fi.GetValue(enumeration).ToString() == strValue)
                    return (int)Fi.GetValue(enumeration);
            }
            return -1;
        }

        public static String GetInsertQuery(String strUserId, String strTableName, String[] strFields, String[] strValues)
        {
            DataTable dt = GetTableStructure(strTableName);
            String strItemUpdationQuery = "INSERT INTO " + strTableName;
            strItemUpdationQuery += " (" + String.Join(",", strFields) + ",MUID,MDAT,RUID,RDAT)  VALUES (";
            for (int i = 0; i < strFields.Length; i++)
            {
                if (i != 0) strItemUpdationQuery += ", ";
                strItemUpdationQuery += GetQueryValue(dt, strFields, strValues, i);
            }
            strItemUpdationQuery += ",'" + strUserId + "','" + DateTime.Now + "','" + strUserId + "','" + DateTime.Now + "')";
            return strItemUpdationQuery;
        }

        public static String GetUpdateQuery(String strUserId, String strTableName, String[] strFields, String[] strValues, String strCriteria = "")
        {
            DataTable dt = GetTableStructure(strTableName);
            String strItemUpdationQuery = "UPDATE " + strTableName + " SET ";
            for (int i = 0; i < strFields.Length; i++)
            {
                if (i != 0) strItemUpdationQuery += ", ";
                strItemUpdationQuery += strFields[i] + " = " + GetQueryValue(dt, strFields, strValues, i) + "";
            }
            strItemUpdationQuery += ",MUID = '" + strUserId + "'";
            strItemUpdationQuery += ",MDAT = '" + DateTime.Now + "'";

            strItemUpdationQuery += " WHERE " + strCriteria;
            return strItemUpdationQuery;
        }

        internal static string fnGetValue(String strTableName, String[] strFields, String strCriteria = "")
        {
            String strQuery = "Select " + String.Join(",", strFields) + " From " + strTableName;
            if (strCriteria != "")
                strQuery += " Where " + strCriteria;

            return fnGetValue(strQuery);
        }

        private static string GetQueryValue(DataTable dt, String[] strFields, String[] strValues, Int32 intIndex)
        {
            String strDataType = dt.Columns[strFields[intIndex]].DataType.Name;

            if (!ExistsInEnumeration(typeof(enStringValues), strDataType))
            {
                if (strValues[intIndex].Trim() == "")
                {
                    return "null";
                }
                else
                    return strValues[intIndex];
            }
            else
            {
                if (strValues[intIndex].Trim() == "")
                {
                    return "null";
                }
                else
                    return "'" + strValues[intIndex] + "'";
            }
        }

        private static DataTable GetTableStructure(string strTableName)
        {
            return DBManager.GetDataTable("select * from " + strTableName + " where 1=2");
        }

        public static bool IsUnicode(string value)
        {
            byte[] ascii = AsciiStringToByteArray(value);
            byte[] unicode = UnicodeStringToByteArray(value);
            string value1 = FromASCIIByteArray(ascii);
            string value2 = FromUnicodeByteArray(unicode);
            if (value1 != value2)
            {
                return true;
            }
            return false;
        }

        public static string FromASCIIByteArray(byte[] characters)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string constructedString = encoding.GetString(characters);
            return constructedString;
        }

        public static string FromUnicodeByteArray(byte[] characters)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            string constructedString = encoding.GetString(characters);
            return constructedString;
        }

        public static byte[] AsciiStringToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public static byte[] UnicodeStringToByteArray(string str)
        {
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            return encoding.GetBytes(str);
        }
        #endregion
         
        #region Logging

        /// <summary>
        /// Log any error received to a log file
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            StreamWriter sw = new StreamWriter
                                (new FileStream(Environment.CurrentDirectory + "\\Errors_Log.txt"
                                    , FileMode.Create, FileAccess.Write));

            sw.WriteLine("------ " + DateTime.Now.ToString("dd, mmm, yyyy hh:MM tt")
                       + " ----------------------------------------------------------------------------------------------------");
            sw.WriteLine(" Error Message :- " + ex.Message);
            sw.WriteLine(" StackTrace    :- " + ex.StackTrace);
            sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");

            sw.Close();
        }

        #endregion
    }
}