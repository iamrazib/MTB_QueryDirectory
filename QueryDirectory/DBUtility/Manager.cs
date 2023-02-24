using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDirectory.DBUtility
{
    class Manager
    {
        //---- UAT 
        //static string connectionString = "7yd4MP3VGCgFvFyV4t81hL13K9EFP3ubA88Gd4uAdysuaCtpV0CK3mj4X0YfgCkRE4ad1mnn1ECxiq1QgI1r7tVhQ9e9nIP1HUv8HI7YY2NCKU9i2TVkr/bvUkDecYpN/J9uhGk2hAEvWthtUexvem8BZXc7uTUZaTSLnOfDYIK1QuhvcNqLRo0JXBj7T5n5X4mptCP/RvBx9MBRTBeO+kJSg1Ghvavd";
        //Data Source=192.168.81.53;Initial Catalog=RemittanceDB;User ID=sa;Password=mtbadmin

        //static string connectionString = "7yd4MP3VGCgFvFyV4t81hL13K9EFP3ubA88Gd4uAdysuaCtpV0CK3mj4X0YfgCkRE4ad1mnn1ECxiq1QgI1r7tVhQ9e9nIP1HUv8HI7YY2PCwHYvHdGPT5Shgbz2/MxI8B8nRwiDXQ0rZOnn4cXKkL1trbjnruYd8a2arAPjkD1OncSTVmUELnrFu9W7nov/f0+yvp1UEAMpC8xgc37ONA==";
        //Data Source=192.168.81.53;Initial Catalog=NRBWork;User ID=sa;Password=mtbadmin

        //--- New Live 10.45.10.106
        static string connectionString = "7yd4MP3VGCgFvFyV4t81hL13K9EFP3ubmIfV6arMv+BwEcytVleK79E65XBpWfELZ9kv1vYl3SSGBDiZUxFvCZXWTbb3yIdh39Kk7C+7mG0RBYFvZrFwKxiGwe82cAM2CvRj6bB2aok0UlP/RKm2RafoU6fBX3I42TZzCj7hH585++AECeXTFxrFYHxALYsUBjsaxa18AIbW3uRXv64WjhVHIiMnx5eT";
        //string dbcon = "Data Source=10.45.10.106;Initial Catalog=NRBWork;User ID=nrbwork;Password=Mtb@1234";


        public string nrbworkConnectionString = Utility.DecryptString(connectionString);

        MTBDBManager dbManager = null;
        

        internal DataTable GetModeOfTxnList()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();
                string query = "SELECT [mode_id],[mode_name] FROM [NRBWork].[dbo].[QueryDirectoryTransactionMode] ORDER BY [mode_id] ";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) {   }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return dt;
        }

        internal DataTable GetExchList()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                //string query = "SELECT [exhId],[exhName] FROM [NRBWork].[dbo].[ExchangeHousesList] WHERE [isActive]=1 ORDER BY [exhName] ";
                //string query = "SELECT [AutoId],[ExchangeHouseFullName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] WHERE [isActive]=1 ORDER BY [ExchangeHouseFullName] ";

                string query = "SELECT [AutoId],[ExchangeHouseShortName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] WHERE [isActive]=1 ORDER BY [ExchangeHouseFullName] ";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return dt;
        }

        internal DataTable GetTodaysInputData(DateTime dtValue)
        {
            DataTable dt = new DataTable();
            string dtVal = dtValue.ToString("yyyy-MM-dd");

            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId] ID, "
                    + " convert(varchar, [QueryDate], 104) QueryDate, "
                    + " [PINNumber], "
                    + " isnull([AccountNo],'') AccountNo, "
                    + " [Amount], "
                    + " (SELECT [mode_name] FROM [NRBWork].[dbo].[QueryDirectoryTransactionMode] where [mode_id]=qd.[TransactionModeId]) 'Payment Mode', "
                    + " (SELECT [ExchangeHouseFullName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] where [AutoId]=qd.[ExchangeHouseId]) 'Exchange House', "
                    + " (SELECT [bankStatusName] FROM [NRBWork].[dbo].[QueryDirectoryBeneBankStatus] where [bankStatusId]=qd.[BeneBankStatusId] ) 'Bank Status',"
                    + " (SELECT [mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] where [mtbQueryStatusId]=qd.[MTBQueryStatusId]) 'MTB Status',"
                    + " CASE WHEN convert(varchar, [CreditDate], 104)='01.01.1900' THEN '' ELSE convert(varchar, [CreditDate], 104) END AS CreditDate, "
                    + " convert(varchar, [TxnInsertDate], 120) InputDate, "
                    + " convert(varchar, [TxnUpdateDate], 120) UpdateDate, "
                    + " isnull(qd.[BeneBankStatusId],'0') bbsId, "
                    + " isnull(qd.[MTBQueryStatusId],'0') mqsId, "
                    + " isnull(qd.[Remarks],'') Remarks, "
                    + " qd.ExchangeHouseId ExchId, "
                    + " qd.TransactionModeId ModeId "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd "
                    + " WHERE (convert(varchar, [TxnInsertDate], 23) ='" + dtVal + "' OR convert(varchar, [TxnUpdateDate], 23)='"+dtVal+"') AND [isDeleted]=0  ORDER BY [AutoId] asc ";

                    //+ " WHERE [QueryDate]='" + dtValue + "' AND [isDeleted]=0  ORDER BY [AutoId] asc ";

                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }

            return dt;
        }

        internal bool saveQueryData(string vdate, string pin, string amt, string modeId, string exchId, string loggedUser)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdSaveData = new SqlCommand();
            string saveData = "";
            bool insertSuccess = false;

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) {  _sqlConnection.Open();  }

                saveData = "INSERT into [NRBWork].[dbo].[QueryDirectoryData]([QueryDate],[PINNumber],[Amount],[TransactionModeId],[ExchangeHouseId],[queryUser])"
                    + " VALUES (@QueryDate,@PINNumber,@Amount,@TransactionModeId,@ExchangeHouseId,@queryUser)";

                cmdSaveData.CommandText = saveData;
                cmdSaveData.Connection = _sqlConnection;

                cmdSaveData.Parameters.Add("@QueryDate", SqlDbType.VarChar).Value = vdate;
                cmdSaveData.Parameters.Add("@PINNumber", SqlDbType.VarChar).Value = pin.Trim();
                cmdSaveData.Parameters.Add("@Amount", SqlDbType.VarChar).Value = amt;
                cmdSaveData.Parameters.Add("@TransactionModeId", SqlDbType.VarChar).Value = modeId;
                cmdSaveData.Parameters.Add("@ExchangeHouseId", SqlDbType.VarChar).Value = exchId;
                cmdSaveData.Parameters.Add("@queryUser", SqlDbType.VarChar).Value = loggedUser;
                
                try
                {
                    cmdSaveData.ExecuteNonQuery();
                    insertSuccess = true;
                }
                catch (Exception ec)
                {
                    insertSuccess = false;
                    throw ec;
                }
            }
            catch (Exception ex)
            {
                insertSuccess = false;
                throw ex;
            }
            finally
            {
                try
                {
                    if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open)
                    {
                        _sqlConnection.Close();
                    }
                }
                catch (SqlException sqlException)
                {
                    throw sqlException;
                }
            }

            return insertSuccess;
        }

        internal bool isExistsThisPinEarlier(string pin)
        {
            DataTable dt = new DataTable();
            bool isExists = false;

            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [QueryDate],[PINNumber],[Amount] FROM [NRBWork].[dbo].[QueryDirectoryData] WHERE [PINNumber] like '%" + pin.Trim() + "%'";

                dt = dbManager.GetDataTable(query.Trim());
                if (dt.Rows.Count > 0)
                {
                    isExists = true;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }

            return isExists;
        }

        internal DataTable GetBeneBankStatusList()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [bankStatusId],[bankStatusName] FROM [NRBWork].[dbo].[QueryDirectoryBeneBankStatus] "
                    + " WHERE [isActive]=1 ORDER BY [bankStatusId] ";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return dt;
        }

        internal DataTable GetMtbQueryStatusList()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [mtbQueryStatusId],[mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] "
                    + " WHERE [isActive]=1 ORDER BY [mtbQueryStatusId] ";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return dt;
        }

        internal DataTable GetQueryDataByPin(string pinOrAccount)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId] ID, "
                    + " convert(varchar, [QueryDate], 104) QueryDate, "
                    + " [PINNumber], "
                    + " isnull([AccountNo],'') AccountNo, "
                    + " [Amount], "
                    + " (SELECT [mode_name] FROM [NRBWork].[dbo].[QueryDirectoryTransactionMode] where [mode_id]=qd.[TransactionModeId]) 'Payment Mode', "
                    + " (SELECT [ExchangeHouseFullName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] where [AutoId]=qd.[ExchangeHouseId]) 'Exchange House', "
                    + " (SELECT [bankStatusName] FROM [NRBWork].[dbo].[QueryDirectoryBeneBankStatus] where [bankStatusId]=qd.[BeneBankStatusId] ) 'Bank Status',"
                    + " (SELECT [mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] where [mtbQueryStatusId]=qd.[MTBQueryStatusId]) 'MTB Status',"
                    + " CASE WHEN convert(varchar, [CreditDate], 104)='01.01.1900' THEN '' ELSE convert(varchar, [CreditDate], 104) END AS CreditDate, "
                    + " convert(varchar, [TxnInsertDate], 120) InputDate, "
                    + " convert(varchar, [TxnUpdateDate], 120) UpdateDate, "
                    + " isnull(qd.[BeneBankStatusId],'0') bbsId, "
                    + " isnull(qd.[MTBQueryStatusId],'0') mqsId, "
                    + " isnull(qd.[Remarks],'') Remarks, "
                    + " qd.ExchangeHouseId ExchId, "
                    + " qd.TransactionModeId ModeId "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd "
                    + " WHERE ([PINNumber] like '%" + pinOrAccount + "%' or [AccountNo] like '%" + pinOrAccount + "%') AND [isDeleted]=0 ";

                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }

            return dt;
        }

        internal bool updateQueryData(string pin, string amt, string beneBankStatusId, string mtbQueryStatusId, string accountNo, string dtCreditDate, string remks,
                    string modeId, string exchId)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdUpdateData = new SqlCommand();
            string updateData = "";
            bool updateSuccess = false;

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)){ _sqlConnection.Open(); }

                updateData = "UPDATE [NRBWork].[dbo].[QueryDirectoryData] SET [Amount]=@Amount, [BeneBankStatusId]=@BeneBankStatusId, [MTBQueryStatusId]=@MTBQueryStatusId, "
                    + " [TxnUpdateDate]=@TxnUpdateDate, [AccountNo]=@AccountNo, [CreditDate]=@CreditDate, [Remarks]=@Remarks, [TransactionModeId]=@ModeId, [ExchangeHouseId]=@ExchId "
                    +" WHERE [PINNumber] like '%" + pin.Trim() + "%' AND [isDeleted]=0 ";

                cmdUpdateData.CommandText = updateData;
                cmdUpdateData.Connection = _sqlConnection;

                //cmdUpdateData.Parameters.Add("@PINNumber", SqlDbType.VarChar).Value = pin;
                cmdUpdateData.Parameters.Add("@Amount", SqlDbType.VarChar).Value = amt;
                cmdUpdateData.Parameters.Add("@BeneBankStatusId", SqlDbType.VarChar).Value = beneBankStatusId;
                cmdUpdateData.Parameters.Add("@MTBQueryStatusId", SqlDbType.VarChar).Value = mtbQueryStatusId;
                cmdUpdateData.Parameters.Add("@TxnUpdateDate", SqlDbType.DateTime).Value = DateTime.Now;
                cmdUpdateData.Parameters.Add("@AccountNo", SqlDbType.VarChar).Value = accountNo;
                cmdUpdateData.Parameters.Add("@CreditDate", SqlDbType.VarChar).Value = dtCreditDate;
                cmdUpdateData.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = remks.Length > 300 ? remks.Substring(0, 297) : remks;
                cmdUpdateData.Parameters.Add("@ModeId", SqlDbType.Int).Value = modeId;
                cmdUpdateData.Parameters.Add("@ExchId", SqlDbType.Int).Value = exchId;

                try
                {
                    cmdUpdateData.ExecuteNonQuery();
                    updateSuccess = true;
                }
                catch (Exception ec)
                {
                    updateSuccess = false;
                    throw ec;
                }

            }
            catch (Exception ex)
            {
                updateSuccess = false;
                throw ex;
            }
            finally
            {
                try
                {
                    if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open)
                    {
                        _sqlConnection.Close();
                    }
                }
                catch (SqlException sqlException)
                {
                    throw sqlException;
                }
            }

            return updateSuccess;
        }

        internal bool DeleteRecord(int autoId)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdDeleteData = new SqlCommand();
            string deleteDataQry = "";
            bool deleteSuccess = false;

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);

                if (_sqlConnection.State.Equals(ConnectionState.Closed))
                {
                    _sqlConnection.Open();
                }

                //deleteDataQry = "UPDATE [" + databaseName + "].[dbo].[QueryDirectoryData] SET [isDeleted]=1 WHERE [AutoId]=@AutoId";

                deleteDataQry = "DELETE FROM [NRBWork].[dbo].[QueryDirectoryData] WHERE [AutoId]=@AutoId";

                cmdDeleteData.CommandText = deleteDataQry;
                cmdDeleteData.Connection = _sqlConnection;
                cmdDeleteData.Parameters.Add("@AutoId", SqlDbType.Int).Value = autoId;

                try
                {
                    cmdDeleteData.ExecuteNonQuery();
                    deleteSuccess = true;
                }
                catch (Exception ec)
                {
                    deleteSuccess = false;
                    throw ec;
                }

            }
            catch (Exception ex)
            {
                deleteSuccess = false;
                throw ex;
            }
            finally
            {
                try
                {
                    if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open)
                    {
                        _sqlConnection.Close();
                    }
                }
                catch (SqlException sqlException)
                {
                    throw sqlException;
                }
            }

            return deleteSuccess;
        }

        internal DataTable GetReportDataByDates(string fromDate, string toDate)
        {
            DataTable dt = new DataTable();

            DateTime dtValueFrm = DateTime.ParseExact(fromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dtValueTo = DateTime.ParseExact(toDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            string dtValFrom = dtValueFrm.ToString("yyyy-MM-dd");
            string dtValTo = dtValueTo.ToString("yyyy-MM-dd");

            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT "
                    + " convert(varchar, [QueryDate], 104) QueryDate, "
                    + " [PINNumber], "
                    + " isnull([AccountNo],'') AccountNo, "
                    + " [Amount], "
                    + " (SELECT [mode_name] FROM [NRBWork].[dbo].[QueryDirectoryTransactionMode] where [mode_id]=qd.[TransactionModeId]) 'Payment Mode', "
                    + " (SELECT [ExchangeHouseFullName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] where [AutoId]=qd.[ExchangeHouseId]) 'Exchange House', "
                    + " isnull((SELECT [bankStatusName] FROM [NRBWork].[dbo].[QueryDirectoryBeneBankStatus] where [bankStatusId]=qd.[BeneBankStatusId] ),'') 'Bank Status',"
                    + " isnull((SELECT [mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] where [mtbQueryStatusId]=qd.[MTBQueryStatusId]),'') 'MTB Status',"
                    + " CASE WHEN convert(varchar, [CreditDate], 104)='01.01.1900' THEN '' ELSE convert(varchar, [CreditDate], 104) END AS CreditDate, "
                    + " convert(varchar, [TxnInsertDate], 120) InputDate, "
                    + " isnull(convert(varchar, [TxnUpdateDate], 120),'') UpdateDate, "
                    + " isnull(qd.[Remarks],'') Remarks "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd "
                    + " WHERE convert(varchar, [TxnUpdateDate], 23) >='" + dtValFrom + "' AND convert(varchar, [TxnUpdateDate], 23)<='" + dtValTo + "'  ORDER BY [AutoId] desc ";

                    //+ " WHERE [QueryDate]>='" + fromDate + "' AND [QueryDate]<='" + toDate + "'  ORDER BY [AutoId] desc ";

                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }

            return dt;
        }

        internal bool SendMail(DataTable dtReportData, DataTable dtReportSummaryData, string fromDate, string toDate)
        {
            MailManager mailManager = new MailManager();

            string subject = "", dateVal = "", tomail = "", ccmail = "";
            int serialNo = 1;

            //tomail = "razibul.islam@mutualtrustbank.com";
            //ccmail = "iamrazib@gmail.com";

            tomail = "mtbremittance@mutualtrustbank.com";
            ccmail = "zahid@mutualtrustbank.com";
                        

            if (fromDate.Equals(toDate))
            {
                subject = "Daily Query Report : " + fromDate;
                dateVal = fromDate;
            }
            else
            {
                subject = "Daily Query Report : " + fromDate + " to " + toDate;
                dateVal = fromDate + " to " + toDate;
            }
            
            string emailbody = "";
            
            emailbody += "<span style=\"font-weight:bold\">Query Performance Report</span>";
            emailbody += "<br>";
            emailbody += "<span style=\"font-weight:bold\">Date: </span>" + dateVal;
            emailbody += "<br>";
            emailbody += "<span style=\"font-weight:bold\">Total Records : </span>" + dtReportData.Rows.Count;

            emailbody += "<br><br><br>";

            //---------- Summary Block --------------
            emailbody += "<style type=\"text/css\"> "
                + " .tSummary  {border-collapse:collapse;border-spacing:0;margin:0px auto;} "
                + " .tSummary td{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px; overflow:hidden;padding:3px 5px;word-break:normal;} "
                + " .tSummary th{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;font-weight:normal;overflow:hidden;padding:3px 5px;word-break:normal;} "
                + " .tSummary .tSummary-baqh{text-align:center;vertical-align:top}"
                + " .tSummary .tSummary-0lax{text-align:left;vertical-align:top} "
                + " </style> "
                + " <table class=\"tSummary\"> "
                + " <thead> "
                  + " <tr> "
                    + " <th class=\"tSummary-0lax\"><span style=\"font-weight:bold\">MTB Status</span></th> "
                    + " <th class=\"tSummary-baqh\"><span style=\"font-weight:bold\">No of Query</span></th> "
                  + " </tr> "
                + " </thead> "
                + " <tbody> ";

            DataRow drowSumr;

            if (dtReportSummaryData.Rows.Count > 0)
            {
                for (int rw = 1; rw <= dtReportSummaryData.Rows.Count; rw++)
                {
                    drowSumr = dtReportSummaryData.Rows[rw - 1];

                    emailbody += " <tr> "
                        + " <td class=\"tSummary-0lax\">" + drowSumr[0].ToString() + "</td> "
                        + " <td class=\"tSummary-baqh\">" + drowSumr[1].ToString() + "</td> "
                        + "</tr>";
                }
                emailbody += "</tbody></table>";
            }

            //---------- Summary Block --------------

            //---------- Detail Block --------------
            emailbody += "<br><br><br>";

            emailbody += "<style type=\"text/css\"> "
                + " .tg  {border-collapse:collapse;border-spacing:0;} "
                + " .tg td{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px; "
                + "   overflow:hidden;padding:3px 5px;word-break:normal;} "
                + " .tg th{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px; "
                + "   font-weight:normal;overflow:hidden;padding:3px 5px;word-break:normal;} "
                + " .tg .tg-0lax{text-align:left;vertical-align:top} "
                + " </style> "
                + " <table class=\"tg\"> "
                + " <thead> "
                  + " <tr> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Sl.</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Query Date</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">PIN Number</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Account No</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Amount</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Payment Mode</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Exchange House</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Bank Status</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">MTB Status</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Credit / Differ Date</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Input Time</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Update Time</span></th> "
                    + " <th class=\"tg-0lax\"><span style=\"font-weight:bold\">Remarks</span></th> "
                  + " </tr> "
                + " </thead> "
                + " <tbody>";

            DataRow drow;

            if (dtReportData.Rows.Count > 0)
            {
                for (int rw = 1; rw <= dtReportData.Rows.Count; rw++)
                {
                    drow = dtReportData.Rows[rw - 1];

                    emailbody += " <tr> "
                        + " <td class=\"tg-0lax\">" + serialNo + "</td> "    
                        + " <td class=\"tg-0lax\">" + drow["QueryDate"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["PINNumber"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["AccountNo"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["Amount"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["Payment Mode"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["Exchange House"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["Bank Status"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["MTB Status"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["CreditDate"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["InputDate"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["UpdateDate"].ToString() + "</td> "
                        + " <td class=\"tg-0lax\">" + drow["Remarks"].ToString() + "</td> "
                        + "</tr>";

                    serialNo++;
                }

                emailbody += "</tbody>";
                emailbody += "</table>";
            }

            return mailManager.SendMail(tomail, ccmail, subject, emailbody);            
        }

        internal DataTable GetReportSummaryDataByDates(string fromDate, string toDate)
        {
            DataTable dt = new DataTable();

            DateTime dtValueFrm = DateTime.ParseExact(fromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dtValueTo = DateTime.ParseExact(toDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            string dtValFrom = dtValueFrm.ToString("yyyy-MM-dd");
            string dtValTo = dtValueTo.ToString("yyyy-MM-dd");

            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT "
                    + " isnull((SELECT [mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] where [mtbQueryStatusId]=p.mtbsts),'') MTBStatus, p.cnt NoOfQuery "
                    + " FROM ( "
                    + " select [MTBQueryStatusId] mtbsts, count(*) cnt "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd  "
                    + " WHERE convert(varchar, [TxnUpdateDate], 23) >='" + dtValFrom + "' AND convert(varchar, [TxnUpdateDate], 23) <='" + dtValTo + "' "
                    + " group by [mtbQueryStatusId] "
                    + " )p ";

                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }

            return dt;
        }
                
        internal DataTable GetUserList()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [UserId]+' - '+[UserName] FROM [NRBWork].[dbo].[QueryDirectoryUserCredential] WHERE [isActive]=1";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) {  }
            finally {  dbManager.CloseDatabaseConnection();  }
            return dt;
        }

        internal bool isPasswordMatch(string userId, string pass)
        {
            bool isFound = false;
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string queryDateCheck = "SELECT * FROM [NRBWork].[dbo].[QueryDirectoryUserCredential]  WHERE [UserId]='" + userId + "' AND [UserPassword]='" + pass + "'";
                DataTable dt = dbManager.GetDataTable(queryDateCheck);
                if (dt.Rows.Count > 0)
                    isFound = true;
                else
                    isFound = false;
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return isFound;
        }

        internal bool ChangeUserPassword(string loggedUserChPass, string currPass, string newPass)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdUpdateData = new SqlCommand();
            bool isSuccess = false;

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) { _sqlConnection.Open(); }

                string queryUpdate = "UPDATE [NRBWork].[dbo].[QueryDirectoryUserCredential] SET [UserPassword]='" + newPass + "' WHERE [UserId]='" + loggedUserChPass + "' AND [UserPassword]='" + currPass + "'";

                cmdUpdateData.CommandText = queryUpdate;
                cmdUpdateData.Connection = _sqlConnection;

                try
                {
                    int updateOk = cmdUpdateData.ExecuteNonQuery();
                    if (updateOk == 0)
                        isSuccess = false;
                    else
                        isSuccess = true;
                }
                catch (Exception ec){ throw ec; }
            }
            catch (Exception ex){  isSuccess = false;  }
            finally
            {
                try{ if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open) { _sqlConnection.Close(); }  }
                catch (SqlException sqlException) {  throw sqlException; }
            }
            return isSuccess;        
        }


        #region ROUTING INFO PAGE
        internal DataTable GetBdAllBanklist()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT distinct [BankCode],[BankName] FROM [NRBWork].[dbo].[BD_BANK_BRANCH] order by [BankName]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal DataTable GetBdAllDistrictlist()
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT distinct [District] FROM [NRBWork].[dbo].[BD_BANK_BRANCH] order by District";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal DataTable GetBranchListByBankCode(string bankCode)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                //string query = "SELECT distinct Branch_code, Branch_Name  FROM [NRBWork].[dbo].[BANK_BRANCH] WHERE Bank_code='" + bankCode + "' order by Branch_Name";
                string query = "SELECT distinct [BranchName]  FROM [NRBWork].[dbo].[BD_BANK_BRANCH] WHERE [BankCode]='" + bankCode + "' order by [BranchName]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal DataTable GetRoutingInfosByWhereClause(string whereClause)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId] Sl, [BankCode], [BankName], [BranchName], [District],[RoutingNumber] Routing, [Email],[MobileNumber]  FROM [NRBWork].[dbo].[BD_BANK_BRANCH] " + whereClause + " order by BranchName, District";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal bool IsRoutingNumberAlreadyExists(string routingNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT * FROM [BD_BANK_BRANCH] WHERE [RoutingNumber]='" + routingNo + "' ";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }

            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        internal bool UpdateRoutingInfo(int idVal, string rtNum, string email, string contactNo, string loggedUser)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdUpdateData = new SqlCommand();
            SqlCommand cmdInsertData = new SqlCommand();
            string updateData = "", insertQry = "";
            bool updateSuccess = false;
            DataTable dtBankBrn = new DataTable();

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) { _sqlConnection.Open(); }

                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId],[MTBSlNo],[MTBCode],[BankCode],[BankName],[BranchName],[District],[RoutingNumber],[Country],[ContactDetails],[Email],[MobileNumber]  FROM [NRBWork].[dbo].[BD_BANK_BRANCH] WHERE [AutoId]=" + idVal;
                dtBankBrn = dbManager.GetDataTable(query.Trim());

                insertQry = "INSERT INTO [dbo].[BD_BANK_BRANCH_UPDATE_HISTORY]([BD_BANK_BRANCH_AutoId],[MTBSlNo],[MTBCode],[BankCode],[BankName],[BranchName],[District],[RoutingNumber],[Country],[ContactDetails],[Email],[MobileNumber],[NewOrUpdate],[UserId]) "
                    + " VALUES (@BD_BANK_BRANCH_AutoId,@MTBSlNo,@MTBCode,@BankCode,@BankName,@BranchName,@District,@RoutingNumber,@Country,@ContactDetails,@Email,@MobileNumber,@NewOrUpdate,@UserId)";

                cmdInsertData.CommandText = insertQry;
                cmdInsertData.Connection = _sqlConnection;
                cmdInsertData.Parameters.Add("@BD_BANK_BRANCH_AutoId", SqlDbType.Int).Value = Convert.ToInt32(dtBankBrn.Rows[0]["AutoId"]);
                cmdInsertData.Parameters.Add("@MTBSlNo", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MTBSlNo"];
                cmdInsertData.Parameters.Add("@MTBCode", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MTBCode"];
                cmdInsertData.Parameters.Add("@BankCode", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BankCode"];
                cmdInsertData.Parameters.Add("@BankName", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BankName"];
                cmdInsertData.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BranchName"];
                cmdInsertData.Parameters.Add("@District", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["District"];
                cmdInsertData.Parameters.Add("@RoutingNumber", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["RoutingNumber"];
                cmdInsertData.Parameters.Add("@Country", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["Country"];
                cmdInsertData.Parameters.Add("@ContactDetails", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["ContactDetails"];
                cmdInsertData.Parameters.Add("@Email", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["Email"];
                cmdInsertData.Parameters.Add("@MobileNumber", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MobileNumber"];
                cmdInsertData.Parameters.Add("@NewOrUpdate", SqlDbType.VarChar).Value = "UPDATE";
                cmdInsertData.Parameters.Add("@UserId", SqlDbType.VarChar).Value = loggedUser;



                updateData = "UPDATE [NRBWork].[dbo].[BD_BANK_BRANCH] SET [RoutingNumber]=@Routing_No, [Email]=@EMAIL, [MobileNumber]=@MobileNumber WHERE [AutoId] = @AutoId ";

                cmdUpdateData.CommandText = updateData;
                cmdUpdateData.Connection = _sqlConnection;
                cmdUpdateData.Parameters.Add("@Routing_No", SqlDbType.VarChar).Value = rtNum;
                cmdUpdateData.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;
                cmdUpdateData.Parameters.Add("@MobileNumber", SqlDbType.VarChar).Value = contactNo;
                cmdUpdateData.Parameters.Add("@AutoId", SqlDbType.Int).Value = idVal;

                try
                {
                    cmdUpdateData.ExecuteNonQuery();
                    cmdInsertData.ExecuteNonQuery();
                    updateSuccess = true;
                }
                catch (Exception ec)
                {
                    updateSuccess = false;
                    throw ec;
                }
            }
            catch (Exception ex)
            {
                updateSuccess = false;
                throw ex;
            }
            finally
            {
                try{ if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open){ _sqlConnection.Close(); }  }
                catch (SqlException sqlException){ throw sqlException; }
            }

            return updateSuccess;
        }

        internal int GetLastRecordNumber()
        {
            DataTable dt = new DataTable();
            int slNo = 0;
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT max(CAST([MTBSlNo] AS int)) FROM [NRBWork].[dbo].[BD_BANK_BRANCH]";
                dt = dbManager.GetDataTable(query.Trim());
                slNo = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return slNo;
        }

        internal bool SaveRoutingInfo(int slNo, string bankCode, string bankName, string brName, string districtName, string rtNum, string email, string contactNo)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdInsertData = new SqlCommand();
            string insertQry = "";
            bool insertSuccess = false;            

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) { _sqlConnection.Open(); }

                insertQry = "INSERT INTO [dbo].[BD_BANK_BRANCH]([MTBSlNo],[MTBCode],[BankCode],[BankName],[BranchName],[District],[RoutingNumber],[Country],[ContactDetails],[Email],[MobileNumber]) "
                    + " VALUES (@MTBSlNo,@MTBCode,@BankCode,@BankName,@BranchName,@District,@RoutingNumber,@Country,@ContactDetails,@Email,@MobileNumber)";

                cmdInsertData.CommandText = insertQry;
                cmdInsertData.Connection = _sqlConnection;
                cmdInsertData.Parameters.Add("@MTBSlNo", SqlDbType.VarChar).Value = slNo.ToString();
                cmdInsertData.Parameters.Add("@MTBCode", SqlDbType.VarChar).Value = slNo.ToString();
                cmdInsertData.Parameters.Add("@BankCode", SqlDbType.VarChar).Value = bankCode;
                cmdInsertData.Parameters.Add("@BankName", SqlDbType.VarChar).Value = bankName;
                cmdInsertData.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = brName;
                cmdInsertData.Parameters.Add("@District", SqlDbType.VarChar).Value = districtName;
                cmdInsertData.Parameters.Add("@RoutingNumber", SqlDbType.VarChar).Value = rtNum;
                cmdInsertData.Parameters.Add("@Country", SqlDbType.VarChar).Value = "Bangladesh";
                cmdInsertData.Parameters.Add("@ContactDetails", SqlDbType.VarChar).Value = "mtbremittance@mutualtrustbank.com";
                cmdInsertData.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                cmdInsertData.Parameters.Add("@MobileNumber", SqlDbType.VarChar).Value = contactNo;
                
                try
                {
                    cmdInsertData.ExecuteNonQuery();
                    insertSuccess = true;
                }
                catch (Exception ec)
                {
                    insertSuccess = false;
                    throw ec;
                }
            }
            catch (Exception ex)
            {
                insertSuccess = false;
                throw ex;
            }
            finally
            {
                try { if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open) { _sqlConnection.Close(); } }
                catch (SqlException sqlException) { throw sqlException; }
            }
            return insertSuccess;
        }

        internal bool UpdateRoutingHistoryTable(string rtNum, string loggedUser)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdInsertData = new SqlCommand();
            string insertQry = "";
            bool insertSuccess = false;
            DataTable dtBankBrn = new DataTable();

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) { _sqlConnection.Open(); }

                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId],[MTBSlNo],[MTBCode],[BankCode],[BankName],[BranchName],[District],[RoutingNumber],[Country],[ContactDetails],[Email],[MobileNumber]  FROM [NRBWork].[dbo].[BD_BANK_BRANCH] WHERE [RoutingNumber]='" + rtNum + "'";
                dtBankBrn = dbManager.GetDataTable(query.Trim());

                insertQry = "INSERT INTO [dbo].[BD_BANK_BRANCH_UPDATE_HISTORY]([BD_BANK_BRANCH_AutoId],[MTBSlNo],[MTBCode],[BankCode],[BankName],[BranchName],[District],[RoutingNumber],[Country],[ContactDetails],[Email],[MobileNumber],[NewOrUpdate],[UserId]) "
                    + " VALUES (@BD_BANK_BRANCH_AutoId,@MTBSlNo,@MTBCode,@BankCode,@BankName,@BranchName,@District,@RoutingNumber,@Country,@ContactDetails,@Email,@MobileNumber,@NewOrUpdate,@UserId)";

                cmdInsertData.CommandText = insertQry;
                cmdInsertData.Connection = _sqlConnection;
                cmdInsertData.Parameters.Add("@BD_BANK_BRANCH_AutoId", SqlDbType.Int).Value = Convert.ToInt32(dtBankBrn.Rows[0]["AutoId"]);
                cmdInsertData.Parameters.Add("@MTBSlNo", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MTBSlNo"];
                cmdInsertData.Parameters.Add("@MTBCode", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MTBCode"];
                cmdInsertData.Parameters.Add("@BankCode", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BankCode"];
                cmdInsertData.Parameters.Add("@BankName", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BankName"];
                cmdInsertData.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["BranchName"];
                cmdInsertData.Parameters.Add("@District", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["District"];
                cmdInsertData.Parameters.Add("@RoutingNumber", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["RoutingNumber"];
                cmdInsertData.Parameters.Add("@Country", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["Country"];
                cmdInsertData.Parameters.Add("@ContactDetails", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["ContactDetails"];
                cmdInsertData.Parameters.Add("@Email", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["Email"];
                cmdInsertData.Parameters.Add("@MobileNumber", SqlDbType.VarChar).Value = dtBankBrn.Rows[0]["MobileNumber"];
                cmdInsertData.Parameters.Add("@NewOrUpdate", SqlDbType.VarChar).Value = "INSERT";
                cmdInsertData.Parameters.Add("@UserId", SqlDbType.VarChar).Value = loggedUser;
                
                try
                {
                    cmdInsertData.ExecuteNonQuery();
                    insertSuccess = true;
                }
                catch (Exception ec)
                {
                    insertSuccess = false;
                    throw ec;
                }
            }
            catch (Exception ex)
            {
                insertSuccess = false;
                throw ex;
            }
            finally
            {
                try { if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open) { _sqlConnection.Close(); } }
                catch (SqlException sqlException) { throw sqlException; }
            }
            return insertSuccess;
        }

        #endregion

        internal DataTable GetSummaryDataByDates(string fromDate, string toDate)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT (SELECT qs.[mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] qs where qs.[mtbQueryStatusId]=qd.[MTBQueryStatusId]) StatusName,  count(*) 'No of Txn' "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd WHERE convert(varchar, [TxnInsertDate], 23) between '" + fromDate + "' AND '" + toDate + "' "
                    + " and isDeleted=0 and qd.[MTBQueryStatusId] is not null group by [MTBQueryStatusId] order by [MTBQueryStatusId]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbManager.CloseDatabaseConnection();
            }
            return dt;
        }

        internal DataTable GetBACHContactDetailsByWhereClause(string whereClause)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT [AutoId] SL, [BankCode], [BankName], [ContactNo], [EmailAddress] FROM [NRBWork].[dbo].[BACHContactDetails] " + whereClause + " order by BankName";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal bool UpdateBACHInformation(int idVal, string email, string contactNo, string loggedUser)
        {
            SqlConnection _sqlConnection = null;
            SqlCommand cmdUpdateData = new SqlCommand();
            string updateData = "";
            bool updateSuccess = false;

            try
            {
                _sqlConnection = new SqlConnection(nrbworkConnectionString);
                if (_sqlConnection.State.Equals(ConnectionState.Closed)) { _sqlConnection.Open(); }

                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                updateData = "UPDATE [NRBWork].[dbo].[BACHContactDetails] SET [EmailAddress]=@EmailAddress, [ContactNo]=@ContactNo WHERE [AutoId] = @AutoId ";

                cmdUpdateData.CommandText = updateData;
                cmdUpdateData.Connection = _sqlConnection;
                cmdUpdateData.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = email;
                cmdUpdateData.Parameters.Add("@ContactNo", SqlDbType.VarChar).Value = contactNo;
                cmdUpdateData.Parameters.Add("@AutoId", SqlDbType.Int).Value = idVal;

                try
                {
                    cmdUpdateData.ExecuteNonQuery();
                    updateSuccess = true;
                }
                catch (Exception ec){ updateSuccess = false;  throw ec; }
            }
            catch (Exception ex) { updateSuccess = false; throw ex; }
            finally
            {
                try { if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Open) { _sqlConnection.Close(); } }
                catch (SqlException sqlException) { throw sqlException; }
            }
            return updateSuccess;
        }

        internal DataTable GetDistrictListByBankCode(string bankCode)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();
                string query = "SELECT distinct [District]  FROM [NRBWork].[dbo].[BD_BANK_BRANCH] WHERE [BankCode]='" + bankCode + "' order by [District]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }

        internal DataTable GetBranchListByBankCodeAndDistrictName(string bankCode, string distName)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();
                string query = "SELECT [BranchName] FROM [NRBWork].[dbo].[BD_BANK_BRANCH] WHERE [BankCode]='" + bankCode + "' and [District]='" + distName + "' order by [BranchName]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex) { }
            finally { dbManager.CloseDatabaseConnection(); }
            return dt;
        }


        internal DataTable GetStopPaymentDataByDates(string fromDate, string toDate)
        {
            DataTable dt = new DataTable();

            //string dtValFrom = DateTime.ParseExact(fromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //string dtValTo = DateTime.ParseExact(toDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT "
                    + " convert(varchar, [QueryDate], 104) QueryDate, "
                    + " [PINNumber], "
                    + " isnull([AccountNo],'') AccountNo, "
                    + " round(CONVERT(float, [Amount]),2) Amount, "
                    + " (SELECT [mode_name] FROM [NRBWork].[dbo].[QueryDirectoryTransactionMode] where [mode_id]=qd.[TransactionModeId]) 'Payment Mode', "
                    + " (SELECT [ExchangeHouseFullName] FROM [NRBWork].[dbo].[ExchangeHouseInfoList] where [AutoId]=qd.[ExchangeHouseId]) 'Exchange House', "
                    + " isnull((SELECT [bankStatusName] FROM [NRBWork].[dbo].[QueryDirectoryBeneBankStatus] where [bankStatusId]=qd.[BeneBankStatusId] ),'') 'Bank Status',"
                    + " isnull((SELECT [mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] where [mtbQueryStatusId]=qd.[MTBQueryStatusId]),'') 'MTB Status',"
                    + " CASE WHEN convert(varchar, [CreditDate], 104)='01.01.1900' THEN '' ELSE convert(varchar, [CreditDate], 104) END AS CreditDate, "
                    + " convert(varchar, [TxnInsertDate], 120) InputDate, "
                    + " isnull(convert(varchar, [TxnUpdateDate], 120),'') UpdateDate, "
                    + " isnull(qd.[Remarks],'') Remarks "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd "
                    + " WHERE qd.MTBQueryStatusId=4 AND convert(varchar, [TxnUpdateDate], 23) >='" + fromDate + "' AND convert(varchar, [TxnUpdateDate], 23)<='" + toDate + "'  ORDER BY [AutoId] desc ";
                                
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex){  }
            finally{  dbManager.CloseDatabaseConnection();  }
            return dt;
        }

        internal DataTable GetSummaryDataByUserAndDates(string userId, string fromDate, string toDate)
        {
            DataTable dt = new DataTable();
            try
            {
                dbManager = new MTBDBManager(MTBDBManager.DatabaseType.SqlServer, nrbworkConnectionString);
                dbManager.OpenDatabaseConnection();

                string query = "SELECT (SELECT qs.[mtbQueryStatusName] FROM [NRBWork].[dbo].[QueryDirectoryMTBQueryStatus] qs where qs.[mtbQueryStatusId]=qd.[MTBQueryStatusId]) StatusName,  count(*) 'No of Txn' "
                    + " FROM [NRBWork].[dbo].[QueryDirectoryData] qd WHERE convert(varchar, [TxnInsertDate], 23) between '" + fromDate + "' AND '" + toDate + "' "
                    + " and [queryUser]='" + userId + "' and isDeleted=0 and qd.[MTBQueryStatusId] is not null group by [MTBQueryStatusId] order by [MTBQueryStatusId]";
                dt = dbManager.GetDataTable(query.Trim());
            }
            catch (Exception ex){       }
            finally{ dbManager.CloseDatabaseConnection();  }
            return dt;
        }
    }
}
