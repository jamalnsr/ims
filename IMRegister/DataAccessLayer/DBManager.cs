using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

        private static SqlConnection  dbConnection;
        private static SqlCommand dbCommand;
        //private static SqlDataAdapter dbAdapter;
        private static SqlTransaction dbTransaction;

        public static DataTable GetDataTable(String strQuery)
        {
            // create the DataSet
            DataSet ds = new DataSet();
            try
            {
                ConnectToDatabase();

                SqlDataAdapter adapter = new SqlDataAdapter(strQuery, dbConnection);
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


                /* var uri = new Uri("sqlserver://wiletrmgcfgrlwwe:NPXJQ8UXoSW8J2SXsLHpZsQW5jAnqVJBBN2Zzg32FL4c3EMnByxXsLru7aNEFFGz@499436a0-4c2d-4186-b01b-a67b00fd9072.sqlserver.sequelizer.com/db499436a04c2d4186b01ba67b00fd9072");
                 var connectionString = new SqlConnectionStringBuilder
                 {
                     DataSource = uri.Host,
                     InitialCatalog = uri.AbsolutePath.Trim('/'),
                     UserID = uri.UserInfo.Split(':').First(),
                     Password = uri.UserInfo.Split(':').Last(),
                 }.ConnectionString;*/

                var uriString = ConfigurationManager.AppSettings["SQLSERVER_URI"];
                var uri = new Uri(uriString);
                var connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = uri.Host,
                    InitialCatalog = uri.AbsolutePath.Trim('/'),
                    UserID = uri.UserInfo.Split(':').First(),
                    Password = uri.UserInfo.Split(':').Last(),
                }.ConnectionString;
                

                //String connectionString = @"Server=.;Database=IMRegister;User Id=sa;Password=admin@123;";
                dbConnection = new SqlConnection(connectionString);
                dbCommand = new SqlCommand("", dbConnection);
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
    }
}