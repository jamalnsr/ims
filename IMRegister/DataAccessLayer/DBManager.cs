using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;

namespace DAL
{
    public static class DBManager
    {
        /// <summary>
        /// FILE: clsDbManager.cs
        /// ABSTRACT:
        ///   This class is used for connectivity with Database
        ///   additionally it have functions to ealsy add update delete or get
        ///   data from database.
        /// </summary>
        /// 

        private static SQLiteConnection dbConnection;
        private static SQLiteCommand dbCommand;
        ///private static SQLiteDataAdapter dbAdapter;
        private static SQLiteTransaction dbTransaction;

        public static DataTable GetDataTable(String strQuery)
        {
            // create the DataSet
            DataSet ds = new DataSet();
            try
            {
                ConnectToDatabase();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(strQuery, dbConnection);
                adapter.Fill(ds);

                DisconnectToDataBase();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                DisconnectToDataBase();
            }
            return ds.Tables[0];
        }
        public static int ExecuteQuery(string Query)
        {
            int Result = 0;

            try
            {
                ConnectToDatabase();
                dbCommand.CommandText = Query;
                Result = dbCommand.ExecuteNonQuery();
                DisconnectToDataBase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisconnectToDataBase();
            }

            return Result;
        }
        public static string ExecuteScalar(string Query)
        {
            string ResultString = "";

            try
            {
                ConnectToDatabase();
                dbCommand.CommandText = Query;
                ResultString = Convert.ToString(dbCommand.ExecuteScalar());
                DisconnectToDataBase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisconnectToDataBase();
            }


            return ResultString;
        }

        internal static void ConnectToDatabase()
        {
            if (dbConnection == null)
            {
                /*String connectionString = @" Data Source=" + Environment.CurrentDirectory + "\\Database\\RIQS.db"
                                        + @";Version=3;New=False;Compress=True;";*/

                String connectionString = @" Data Source=D:\All Projects\IMRegister\Database\IMRgister.db;version=3;New=False;Compress=True;";
                //constr = ConfigurationManager.ConnectionStrings["ZaaSConString"].ToString();
                dbConnection = new SQLiteConnection(connectionString);
                dbCommand = new SQLiteCommand("", dbConnection);
            }

            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }
        internal static void DisconnectToDataBase()
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
        }

        public static void StartTransaction()
        {
            dbTransaction = dbConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }
        public static void CommitTransaction()
        {
            dbTransaction.Commit();
        }
        public static void RollBackTransaction()
        {
            dbTransaction.Rollback();
        }

        #region Excel Sheet Query

       /* public static DataTable GetDataFromExcel(String strQuery)
        {
            String connString = "";
            String strFileType = "";

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".XLS";
            dlg.Filter = "Excel Docs (.XLS)|*.XLS|(.XLSX)|*.XLSX";

            if (dlg.ShowDialog() == true)
            {
                String Path1 = dlg.FileName;
                strFileType = Path1.Substring(Path1.Length - 4, 4);

                if (strFileType.Trim() == ".xls")
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path1 + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
                else if (strFileType.Trim() == "xlsx")
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path1 + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

                OleDbConnection conn = new OleDbConnection(connString);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                OleDbCommand cmd = new OleDbCommand(strQuery, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                conn.Close();
                conn.Dispose();
                return ds.Tables[0];

            }
            return new DataTable();
        }*/

        public static DataTable GetSheetDataFromExcel(String strPath, String strSheetName)
        {
            String connString = "";
            String strFileType = strPath.Substring(strPath.Length - 4, 4);
            String strQuery = "SELECT True as Sel, * ,'' as Message,'' as Status FROM [" + strSheetName + "]";

            if (strFileType.Trim() == ".xls")
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
            else if (strFileType.Trim() == "xlsx")
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

            OleDbConnection conn = new OleDbConnection(connString);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (strSheetName == "")
                strQuery = "SELECT True as Sel, * ,'' as Message,'' as Status FROM [" + GetExcelSheets(strPath).Rows[0]["TABLE_NAME"].ToString() + "]";

            OleDbCommand cmd = new OleDbCommand(strQuery, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            conn.Close();
            conn.Dispose();
            return ds.Tables[0];
        }

        public static DataTable GetExcelSheets(String strPath)
        {
            String connString = "";
            String strFileType = strPath.Substring(strPath.Length - 4, 4);

            if (strFileType.Trim() == ".xls")
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
            else if (strFileType.Trim() == "xlsx")
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

            OleDbConnection conn = new OleDbConnection(connString);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            return conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        }
        #endregion

    }
}

//public static class DBManager
//{
//    /// <summary>
//    /// FILE: clsDbManager.cs
//    /// ABSTRACT:
//    ///   This class is used for connectivity with Database
//    ///   additionally it have functions to ealsy add update delete or get
//    ///   data from database.
//    /// </summary>
//    /// 

//    static OleDbConnection dbConnection;
//    static OleDbTransaction dbTransaction;
//    static OleDbCommand dbCommand;
//    public static DataTable GetDataTable(String strQuery)
//    {
//        // create the DataSet
//        DataSet ds = new DataSet();
//        try
//        {
//            ConnectToDatabase();

//            OleDbDataAdapter adapter = new OleDbDataAdapter(strQuery, dbConnection);
//            adapter.Fill(ds);

//            DisconnectToDataBase();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//        finally
//        {
//            DisconnectToDataBase();
//        }
//        return ds.Tables[0];
//    }

//    public static int ExecuteQuery(string Query)
//    {
//        int Result = 0;

//        try
//        {
//            ConnectToDatabase();
//            dbCommand.CommandText = Query;
//            Result = dbCommand.ExecuteNonQuery();
//            DisconnectToDataBase();
//        }
//        catch (Exception ex)
//        {
//            throw ex;
//        }
//        finally
//        {
//            DisconnectToDataBase();
//        }

//        return Result;
//    }
//    public static string ExecuteScalar(string Query)
//    {
//        string ResultString = "";

//        try
//        {
//            ConnectToDatabase();
//            dbCommand.CommandText = Query;
//            ResultString = Convert.ToString(dbCommand.ExecuteScalar());
//            DisconnectToDataBase();
//        }
//        catch (Exception ex)
//        {
//            throw ex;
//        }
//        finally
//        {
//            DisconnectToDataBase();
//        }


//        return ResultString;
//    }

//    internal static void ConnectToDatabase()
//    {
//        if (dbConnection == null)
//        {
//            String connectionString =
//                             @"Provider=Microsoft.ACE.OLEDB.12.0;Data"
//                           + @" Source=" + Environment.CurrentDirectory + "\\HalqaReport.accdb";

//            //constr = ConfigurationManager.ConnectionStrings["ZaaSConString"].ToString();
//            dbConnection = new OleDbConnection(connectionString);
//            dbCommand = new OleDbCommand("", dbConnection);
//        }

//        if (dbConnection.State == ConnectionState.Closed)
//        {
//            dbConnection.Open();
//        }
//    }
//    internal static void DisconnectToDataBase()
//    {
//        if (dbConnection.State == ConnectionState.Open)
//            dbConnection.Close();
//    }

//    public static void StartTransaction()
//    {
//        dbTransaction = dbConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
//    }
//    public static void CommitTransaction()
//    {
//        dbTransaction.Commit();
//    }
//    public static void RollBackTransaction()
//    {
//        dbTransaction.Rollback();
//    }

//    #region Excel Sheet Query

//    public static DataTable GetDataFromExcel(String strQuery)
//    {
//        String connString = "";
//        String strFileType = "";

//        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

//        // Set filter for file extension and default file extension
//        dlg.DefaultExt = ".XLS";
//        dlg.Filter = "Excel Docs (.XLS)|*.XLS|(.XLSX)|*.XLSX";

//        if (dlg.ShowDialog() == true)
//        {
//            String Path1 = dlg.FileName;
//            strFileType = Path1.Substring(Path1.Length - 4, 4);

//            if (strFileType.Trim() == ".xls")
//                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path1 + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
//            else if (strFileType.Trim() == "xlsx")
//                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path1 + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

//            OleDbConnection conn = new OleDbConnection(connString);

//            if (conn.State == ConnectionState.Closed)
//                conn.Open();

//            OleDbCommand cmd = new OleDbCommand(strQuery, conn);
//            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
//            DataSet ds = new DataSet();
//            da.Fill(ds);
//            da.Dispose();
//            conn.Close();
//            conn.Dispose();
//            return ds.Tables[0];

//        }
//        return new DataTable();
//    }

//    public static DataTable GetSheetDataFromExcel(String strPath, String strSheetName)
//    {
//        String connString = "";
//        String strFileType = strPath.Substring(strPath.Length - 4, 4);
//        String strQuery = "SELECT True as Sel, * ,'' as Message,'' as Status FROM [" + strSheetName + "]";

//        if (strFileType.Trim() == ".xls")
//            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
//        else if (strFileType.Trim() == "xlsx")
//            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

//        OleDbConnection conn = new OleDbConnection(connString);

//        if (conn.State == ConnectionState.Closed)
//            conn.Open();

//        DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

//        if (strSheetName == "")
//            strQuery = "SELECT True as Sel, * ,'' as Message,'' as Status FROM [" + GetExcelSheets(strPath).Rows[0]["TABLE_NAME"].ToString() + "]";

//        OleDbCommand cmd = new OleDbCommand(strQuery, conn);
//        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
//        DataSet ds = new DataSet();
//        da.Fill(ds);
//        da.Dispose();
//        conn.Close();
//        conn.Dispose();
//        return ds.Tables[0];
//    }

//    public static DataTable GetExcelSheets(String strPath)
//    {
//        String connString = "";
//        String strFileType = strPath.Substring(strPath.Length - 4, 4);

//        if (strFileType.Trim() == ".xls")
//            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"";
//        else if (strFileType.Trim() == "xlsx")
//            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties= \"Excel 12.0;HDR=Yes;IMEX=2\"";

//        OleDbConnection conn = new OleDbConnection(connString);

//        if (conn.State == ConnectionState.Closed)
//            conn.Open();

//        return conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
//    }
//    #endregion

//}