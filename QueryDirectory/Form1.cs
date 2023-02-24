using QueryDirectory.DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QueryDirectory
{
    public partial class Form1 : Form
    {
        static string formVersion = "Form_Version 1.7,  Date : 27-Jul-2022";
        static Manager mg = new Manager();
        int autoId = 0; // ID to use delete record
        int routingEditId = -1;
        int bachInfoEditId = -1;
        int routingNoValue;
        int summaryDataRowCount = 0;

        public string loggedUser = "";
        public string loggedUserIdAndName = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadModeOfTxnDropDownList();
            LoadExhouseDropDownList();
            LoadBeneBankStatusDropDownList();
            LoadMtbQueryStatusDropDownList();
            //string dtValue = dateTimePicker1.Text;

            DateTime dtValue = DateTime.ParseExact(dateTimePicker1.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            LoadTodaysInputIntoGrid(dtValue);
            LoadQueryDataIntoUpdateRecordGrid(dtValue);

            lblDateValue.Text = "";
            lblExchValue.Text = "";
            lblModeOfTxn.Text = "";
            lblInputDate.Text = "";

            // Routing Info Page
            LoadBankInfoIntoRoutingInfoPage();
            //LoadDistrictInfoIntoRoutingInfoPage();
            //--------------

            //-- BACH info page
            LoadBACHContactDetailsInfo();
            //------------------------

            LoadUserList();

            btnSummaryReportExportToExcel.Enabled = false;

            this.Text = formVersion + " >> Server: " + mg.nrbworkConnectionString.Split(';')[0].Split('=')[1] + " - Database: " + mg.nrbworkConnectionString.Split(';')[1].Split('=')[1];
            this.Text = this.Text + " :: User: " + loggedUserIdAndName;
        }

                       

        public void LoadQueryDataIntoUpdateRecordGrid(DateTime dtValue)
        {
            //DataTable dtTodaysInputData = mg.GetTodaysInputData(dtValue);
            LoadIntoDataGridViewSearchOutput(mg.GetTodaysInputData(dtValue));
        }

        public void LoadTodaysInputIntoGrid(DateTime dtValue)
        {
            DataTable dtTodaysInputData = mg.GetTodaysInputData(dtValue);

            dataGridViewTodaysInput.DataSource = null;
            dataGridViewTodaysInput.DataSource = dtTodaysInputData;

            dataGridViewTodaysInput.Columns["ID"].Width = 50;
            dataGridViewTodaysInput.Columns["QueryDate"].Width = 70;
            dataGridViewTodaysInput.Columns["PINNumber"].Width = 120;
            dataGridViewTodaysInput.Columns["Amount"].Width = 80;
            dataGridViewTodaysInput.Columns["Payment Mode"].Width = 150;
            dataGridViewTodaysInput.Columns["Exchange House"].Width = 200;
            dataGridViewTodaysInput.Columns["CreditDate"].Width = 70;
            dataGridViewTodaysInput.Columns["InputDate"].Width = 110;
            dataGridViewTodaysInput.Columns["UpdateDate"].Width = 110;

            dataGridViewTodaysInput.Columns["bbsId"].Visible = false;
            dataGridViewTodaysInput.Columns["mqsId"].Visible = false;

            //dataGridViewTodaysInput.Columns["ExchId"].Visible = false;
            //dataGridViewTodaysInput.Columns["ModeId"].Visible = false;
        }

        #region DROPDOWN LIST LOAD
        private void LoadMtbQueryStatusDropDownList()
        {
            //cbMtbQueryStatus.Items.Clear();
            //cbMtbQueryStatus.Items.Add("---- Select Query Status ----");
            //cbMtbQueryStatus.Items.Add("Completed and Notified");
            //cbMtbQueryStatus.Items.Add("Differ and Notified");
            //cbMtbQueryStatus.Items.Add("Requested for Internal Amendment");
            //cbMtbQueryStatus.Items.Add("Stop Payment and Cancelation Requested");
            //cbMtbQueryStatus.Items.Add("In Progress");

            cbMtbQueryStatus.Items.Clear();
            DataTable MtbQueryStatuslist = mg.GetMtbQueryStatusList();
            cbMtbQueryStatus.Items.Add("---- Select Query Status ----");

            cbMtbQueryStatusId.Items.Clear();
            cbMtbQueryStatusId.Items.Add("--Select--");

            for (int rw = 0; rw < MtbQueryStatuslist.Rows.Count; rw++)
            {
                cbMtbQueryStatus.Items.Add(MtbQueryStatuslist.Rows[rw][0].ToString() + " - " + MtbQueryStatuslist.Rows[rw][1].ToString());
                cbMtbQueryStatusId.Items.Add(MtbQueryStatuslist.Rows[rw][0].ToString());
            }

            cbMtbQueryStatus.SelectedIndex = 0;
            cbMtbQueryStatusId.SelectedIndex = 0;
        }

        private void LoadBeneBankStatusDropDownList()
        {       
            cbBeneBankStatus.Items.Clear();
            DataTable BeneBankStatuslist = mg.GetBeneBankStatusList();
            cbBeneBankStatus.Items.Add("---- Select Bank Status ----");

            cbBeneBankStatusId.Items.Clear();
            cbBeneBankStatusId.Items.Add("--Select--");

            for (int rw = 0; rw < BeneBankStatuslist.Rows.Count; rw++)
            {
                cbBeneBankStatus.Items.Add(BeneBankStatuslist.Rows[rw][0].ToString() + " - " + BeneBankStatuslist.Rows[rw][1].ToString());
                cbBeneBankStatusId.Items.Add(BeneBankStatuslist.Rows[rw][0].ToString());
            }

            cbBeneBankStatus.SelectedIndex = 0;
            cbBeneBankStatusId.SelectedIndex = 0;
        }

        private void LoadExhouseDropDownList()
        {
            cbExch.Items.Clear();
            cbExchUpdatePage.Items.Clear();

            DataTable Exchlist = mg.GetExchList();
            cbExch.Items.Add("---- Select Exhouse ----");
            cbExchUpdatePage.Items.Add("---- Select Exhouse ----");

            for (int rw = 0; rw < Exchlist.Rows.Count; rw++)
            {
                //cbExch.Items.Add(Exchlist.Rows[rw][0].ToString() + " - " + Exchlist.Rows[rw][1].ToString());
                cbExch.Items.Add(Exchlist.Rows[rw][1].ToString() + " - " + Exchlist.Rows[rw][0].ToString());
                cbExchUpdatePage.Items.Add(Exchlist.Rows[rw][1].ToString() + " - " + Exchlist.Rows[rw][0].ToString());
            }                        

            cbExch.SelectedIndex = 0;
            cbExchUpdatePage.SelectedIndex = 0;
        }

        private void LoadModeOfTxnDropDownList()
        {
            cbModeOfTxn.Items.Clear();
            cbModeOfTxnUpdatePage.Items.Clear();

            DataTable ModeOfTxnlist = mg.GetModeOfTxnList();
            cbModeOfTxn.Items.Add("---- Select Mode ----");
            cbModeOfTxnUpdatePage.Items.Add("---- Select Mode ----");

            for (int rw = 0; rw < ModeOfTxnlist.Rows.Count; rw++)
            {
                cbModeOfTxn.Items.Add(ModeOfTxnlist.Rows[rw][0].ToString() + " - " + ModeOfTxnlist.Rows[rw][1].ToString());
                cbModeOfTxnUpdatePage.Items.Add(ModeOfTxnlist.Rows[rw][0].ToString() + " - " + ModeOfTxnlist.Rows[rw][1].ToString());
            }

            //cbModeOfTxn.Items.Add("---- Select Mode ----");
            //cbModeOfTxn.Items.Add("MTB Account Deposit");
            //cbModeOfTxn.Items.Add("Cash over the Counter");
            //cbModeOfTxn.Items.Add("Other Bank Account Deposit");
            //cbModeOfTxn.Items.Add("Mobile Wallet Deposit");

            cbModeOfTxn.SelectedIndex = 0;
            cbModeOfTxnUpdatePage.SelectedIndex = 0;
        }

        private void LoadUserList()
        {
            comboBoxUser.Items.Clear();
            comboBoxUser.Items.Add("---- Select User ----");

            string usr = "";
            DataTable userlist = mg.GetUserList();

            for (int rw = 0; rw < userlist.Rows.Count; rw++)
            {
                usr = userlist.Rows[rw][0].ToString();
                comboBoxUser.Items.Add(usr);
            }
            comboBoxUser.SelectedIndex = 0;
        }

        #endregion

        private void btnSaveInput_Click(object sender, EventArgs e)
        {
            if (!txtPinNo.Text.Trim().Equals(""))
            {
                
                if (cbModeOfTxn.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select Mode of Transaction !!!", "Error in Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (cbExch.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select Exchange House !!!", "Error in Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    bool isExistsThisPinEarlier = mg.isExistsThisPinEarlier(txtPinNo.Text.Trim());

                    if (!isExistsThisPinEarlier)
                    {
                        string vdate = dateTimePicker1.Text;
                        string pin = txtPinNo.Text.Trim();
                        string amt = "";// txtAmount.Text.Trim();
                        string modeOfTxn = cbModeOfTxn.SelectedItem.ToString();
                        string exch = cbExch.SelectedItem.ToString();
                        string modeId = modeOfTxn.Split('-')[0].Trim();
                        string exchId = exch.Split('-')[1].Trim();

                        bool status = mg.saveQueryData(vdate, pin, amt, modeId, exchId, loggedUser);

                        txtPinNo.Text = "";
                        txtAmount.Text = "";
                        //ReloadPageValue();
                        Form1_Load(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("This PIN already Exists. Please view from 'Update Record'");
                        txtPinNo.Text = "";
                        txtAmount.Text = "";
                        //ReloadPageValue();
                        Form1_Load(sender, e);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("PIN/Ref No. can not be Empty !!!", "Error in INPUT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void ReloadPageValue()
        //{
        //    LoadModeOfTxnDropDownList();
        //    LoadExhouseDropDownList();
        //    LoadBeneBankStatusDropDownList();
        //    LoadMtbQueryStatusDropDownList();
        //    LoadTodaysInputIntoGrid(dateTimePicker1.Text);
        //    LoadQueryDataIntoUpdateRecordGrid(dateTimePicker1.Text);

        //    lblDateValue.Text = "";
        //    lblExchValue.Text = "";
        //    lblModeOfTxn.Text = "";
        //    lblInputDate.Text = "";

        //    this.Text = " >> Server: " + mg.remittanceConnectionString.Split(';')[0].Split('=')[1] + " - Database: " + mg.remittanceConnectionString.Split(';')[1].Split('=')[1];
        //    this.Text = this.Text + " :: User: " + loggedUserIdAndName;
        //}

        private void btnSearchDataInput_Click(object sender, EventArgs e)
        {
            //string dtValue = dateTimePicker1.Text;
            DateTime dtValue = DateTime.ParseExact(dateTimePicker1.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            LoadTodaysInputIntoGrid(dtValue);
            LoadQueryDataIntoUpdateRecordGrid(dtValue);

            dtpickerCreditDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

        private void btnSearchUpdateRecord_Click(object sender, EventArgs e)
        {
            autoId = 0;
            DataTable dtInputtedData = new DataTable();
            string pin = txtRefNoUpdatePage.Text.Trim();
            
            if (pin.Equals(""))
            {
                DateTime dtValue = DateTime.ParseExact(dateTimePicker1.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                dtInputtedData = mg.GetTodaysInputData(dtValue);

                lblDateValue.Text = "";
                lblModeOfTxn.Text = "";
                lblExchValue.Text = "";
                lblInputDate.Text = "";
                txtAmount.Text = "";
                txtAccountNo.Text = "";

                LoadIntoDataGridViewSearchOutput(dtInputtedData);                
                LoadBeneBankStatusDropDownList();
                LoadMtbQueryStatusDropDownList();
            }
            else
            {
                dtInputtedData = mg.GetQueryDataByPin(pin);

                if (dtInputtedData.Rows.Count > 0)
                {                    
                    lblDateValue.Text = (string)dtInputtedData.Rows[0][1];
                    lblModeOfTxn.Text = (string)dtInputtedData.Rows[0][5];
                    lblExchValue.Text = (string)dtInputtedData.Rows[0][6];
                    lblInputDate.Text = (string)dtInputtedData.Rows[0][10];
                    txtAmount.Text = Convert.ToString(dtInputtedData.Rows[0][4]);
                    txtAccountNo.Text = (string)dtInputtedData.Rows[0][3];
                    txtRemarks.Text = (string)dtInputtedData.Rows[0][14];

                    string creditDt = (string)dtInputtedData.Rows[0][9];
                    if (creditDt.Equals("") || creditDt.Equals("01.01.1900"))
                    {
                        dtpickerCreditDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        DateTime oDate = DateTime.ParseExact(creditDt, "dd.MM.yyyy", null);
                        dtpickerCreditDate.Text = oDate.ToShortDateString();
                    }

                    LoadIntoDataGridViewSearchOutput(dtInputtedData);
                }
                else
                {
                    lblDateValue.Text = "";
                    lblModeOfTxn.Text = "";
                    lblExchValue.Text = "";
                    lblInputDate.Text = "";
                    txtAmount.Text = "";
                    txtAccountNo.Text = "";

                    MessageBox.Show("No Data Found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string pin = txtRefNoUpdatePage.Text.Trim();
            if (!pin.Equals(""))
            {

                bool isExistsThisPin = mg.isExistsThisPinEarlier(txtRefNoUpdatePage.Text.Trim());
                if (isExistsThisPin)
                {
                    string amt = txtAmount.Text.Trim();
                    amt = amt.Replace(",", "");
                    string beneBankStatus = cbBeneBankStatus.SelectedItem.ToString();
                    string mtbQueryStatus = cbMtbQueryStatus.SelectedItem.ToString();

                    string beneBankStatusId = beneBankStatus.Split('-')[0].Trim();
                    string mtbQueryStatusId = mtbQueryStatus.Split('-')[0].Trim();

                    string modeId = cbModeOfTxnUpdatePage.SelectedItem.ToString().Split('-')[0].Trim();
                    string exchId = cbExchUpdatePage.SelectedItem.ToString().Split('-')[1].Trim();

                    string accountNo = txtAccountNo.Text.Trim();
                    string remks = txtRemarks.Text.Trim();
                    string dtCreditDate = "";

                    if (beneBankStatusId.Equals("1") || beneBankStatusId.Equals("4") || beneBankStatusId.Equals("5") || beneBankStatusId.Equals("6"))
                    {
                        dtCreditDate = dtpickerCreditDate.Text;
                    }

                    if (!mtbQueryStatusId.Equals("") && Convert.ToInt32(mtbQueryStatusId) == 6)
                    {
                        amt = "0";
                    }

                    if (!mtbQueryStatusId.Equals("") && Convert.ToInt32(mtbQueryStatusId) != 6 && (txtAmount.Text.Trim().Equals("") || txtAmount.Text.Trim().Equals("0")))
                    {
                        MessageBox.Show("Amount can not be Empty/Zero !!!", "Error in INPUT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        bool status = mg.updateQueryData(pin, amt, beneBankStatusId, mtbQueryStatusId, accountNo, dtCreditDate, remks, modeId, exchId);

                        if (status)
                        {
                            DataTable dtInputtedData = mg.GetQueryDataByPin(pin);
                            LoadIntoDataGridViewSearchOutput(dtInputtedData);

                            MessageBox.Show("Update Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("PIN/Reference Number NOT Exists in the system");
                }
                               
            }
            else
            {
                MessageBox.Show("Please Input PIN/Reference Number");
            }
        }

        private void LoadIntoDataGridViewSearchOutput(DataTable dtInputtedData)
        {
            dataGridViewSearchOutput.DataSource = null;
            dataGridViewSearchOutput.DataSource = dtInputtedData;

            dataGridViewSearchOutput.Columns["ID"].Width = 50;
            dataGridViewSearchOutput.Columns["QueryDate"].Width = 70;
            dataGridViewSearchOutput.Columns["PINNumber"].Width = 110;
            dataGridViewSearchOutput.Columns["Amount"].Width = 70;
            dataGridViewSearchOutput.Columns["Payment Mode"].Width = 150;
            dataGridViewSearchOutput.Columns["Exchange House"].Width = 180;
            dataGridViewSearchOutput.Columns["CreditDate"].Width = 70;
            dataGridViewSearchOutput.Columns["InputDate"].Width = 110;
            dataGridViewSearchOutput.Columns["UpdateDate"].Width = 110;
        
            dataGridViewSearchOutput.Columns["bbsId"].Visible = false;
            dataGridViewSearchOutput.Columns["mqsId"].Visible = false;

            //dataGridViewSearchOutput.Columns["ExchId"].Visible = false;
            //dataGridViewSearchOutput.Columns["ModeId"].Visible = false;
        }

        private void dataGridViewSearchOutput_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            autoId = Convert.ToInt32(dataGridViewSearchOutput.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (autoId == 0)
                {
                    MessageBox.Show("Please select record to delete ...");
                }
                else
                {
                    //MessageBox.Show("" + autoId);
                    bool status = mg.DeleteRecord(autoId);
                    if (status)
                    {
                        MessageBox.Show("Record Deleted Success ..");

                        //DataTable dtInputtedData = mg.GetQueryDataByPin(txtRefNoUpdatePage.Text.Trim());
                        //dataGridViewSearchOutput.DataSource = null;
                        //dataGridViewSearchOutput.DataSource = dtInputtedData;

                        //dataGridViewSearchOutput.Columns["QueryDate"].Width = 70;
                        //dataGridViewSearchOutput.Columns["PINNumber"].Width = 100;
                        //dataGridViewSearchOutput.Columns["Amount"].Width = 70;
                        //dataGridViewSearchOutput.Columns["Payment Mode"].Width = 150;
                        //dataGridViewSearchOutput.Columns["Exchange House"].Width = 180;
                        //dataGridViewSearchOutput.Columns["InputDate"].Width = 110;
                        //dataGridViewSearchOutput.Columns["UpdateDate"].Width = 100;

                        //string dtValue = dateTimePicker1.Text;
                        DateTime dtValue = DateTime.ParseExact(dateTimePicker1.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                        LoadTodaysInputIntoGrid(dtValue);
                    }
                }//else block

            }
        }

        private void dataGridViewTodaysInput_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            autoId = Convert.ToInt32(dataGridViewTodaysInput.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void btnClrPinField_Click(object sender, EventArgs e)
        {
            txtRefNoUpdatePage.Text = "";
            lblDateValue.Text = "";
            lblModeOfTxn.Text = "";
            lblExchValue.Text = "";
            lblInputDate.Text = "";
            txtAmount.Text = "";
            txtAccountNo.Text = "";
            txtRemarks.Text = "";
            dtpickerCreditDate.Text = DateTime.Now.ToShortDateString();

            LoadBeneBankStatusDropDownList();
            LoadMtbQueryStatusDropDownList();
            LoadModeOfTxnDropDownList();
            LoadExhouseDropDownList();

            btnSearchUpdateRecord_Click(sender, e);
        }

        private void dataGridViewTodaysInput_CellClick(object sender, DataGridViewCellEventArgs e)
        { }

        private void dataGridViewSearchOutput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Value at: " + e.ColumnIndex + " :: " + dataGridViewSearchOutput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

            if (e.ColumnIndex == 2)  // select at PIN column
            {
                txtRefNoUpdatePage.Text = dataGridViewSearchOutput.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                                
                string bbsId = dataGridViewSearchOutput.Rows[e.RowIndex].Cells[12].Value.ToString();
                string mqsId = dataGridViewSearchOutput.Rows[e.RowIndex].Cells[13].Value.ToString();

                string exchId = dataGridViewSearchOutput.Rows[e.RowIndex].Cells[15].Value.ToString();
                string modeId = dataGridViewSearchOutput.Rows[e.RowIndex].Cells[16].Value.ToString();                                       
                
                DataTable Exchlist = mg.GetExchList();
                for (int ii = 0; ii < Exchlist.Rows.Count; ii++)
                {
                    if (Exchlist.Rows[ii][0].Equals(Convert.ToInt32(exchId)))
                    {
                        cbExchUpdatePage.SelectedIndex = ii + 1;
                        break;
                    }
                }

                cbModeOfTxnUpdatePage.SelectedIndex = Convert.ToInt32(modeId);
                cbBeneBankStatus.SelectedIndex = Convert.ToInt32(bbsId);
                cbMtbQueryStatus.SelectedIndex = Convert.ToInt32(mqsId);
                
                btnSearchUpdateRecord_Click(sender, e);
            }
        }

        private void cbBeneBankStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBeneBankStatusId.SelectedIndex = cbBeneBankStatus.SelectedIndex;
        }

        private void cbMtbQueryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMtbQueryStatusId.SelectedIndex = cbMtbQueryStatus.SelectedIndex;
        }

        private void btnBatchUpload_Click(object sender, EventArgs e)
        {
            frmBatchUpload batchUpld = new frmBatchUpload();
            batchUpld.loggedUser = this.loggedUser;
            batchUpld.loggedUserIdAndName = this.loggedUserIdAndName;
            batchUpld.ShowDialog();
        }

        private void btnReportDataSearch_Click(object sender, EventArgs e)
        {
            string fromDate = dtpickerFromRpt.Text;
            string toDate = dtpickerToRpt.Text;
                       
            DataTable dtReportData = mg.GetReportDataByDates(fromDate, toDate);

            dataGridViewReportData.DataSource = null;
            dataGridViewReportData.DataSource = dtReportData;
            
            //dataGridViewReportData.Columns["ID"].Width = 30;
            dataGridViewReportData.Columns["QueryDate"].Width = 70;
            dataGridViewReportData.Columns["PINNumber"].Width = 120;
            dataGridViewReportData.Columns["Amount"].Width = 80;
            dataGridViewReportData.Columns["Payment Mode"].Width = 150;
            dataGridViewReportData.Columns["Exchange House"].Width = 200;
            dataGridViewReportData.Columns["CreditDate"].Width = 70;
            dataGridViewReportData.Columns["InputDate"].Width = 110;
            dataGridViewReportData.Columns["UpdateDate"].Width = 110;
                        
            lblTotalRecords.Text = "Total Records : " + dtReportData.Rows.Count;
        }

        private void btnReportMail_Click(object sender, EventArgs e)
        {
            string fromDate = dtpickerFromRpt.Text;
            string toDate = dtpickerToRpt.Text;

            DialogResult result = MessageBox.Show("Are You Sure to Send Mail ?", "Mail Send Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataTable dtReportData = mg.GetReportDataByDates(fromDate, toDate);
                DataTable dtReportSummaryData = mg.GetReportSummaryDataByDates(fromDate, toDate);

                if (dtReportData.Rows.Count > 0)
                {
                    bool status = mg.SendMail(dtReportData, dtReportSummaryData, fromDate, toDate);
                    if (status)
                    {
                        MessageBox.Show("Mail Sent Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mail Sent ERROR !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No Data Found to Send Mail", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword chps = new frmChangePassword();
            chps.loggedUserChPass = this.loggedUser;
            chps.loggedUserIdAndNameChPass = this.loggedUserIdAndName;
            chps.ShowDialog();
        }


        #region Routing_Info

        private void cbBankNameRoutingInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bankCode = "";
            cbBankCode.SelectedIndex = cbBankNameRoutingInfo.SelectedIndex;

            if (cbBankNameRoutingInfo.SelectedIndex != 0)
            {
                bankCode = cbBankCode.SelectedItem.ToString().Trim();
                LoadDistrictInfoByBankCodeIntoRoutingInfoPage(bankCode);
                //LoadBranchInfoByBankCodeIntoRoutingInfoPage(bankCode);
            }
            else
            {
                cbDistrictNameRoutingInfo.Items.Clear();
            }
        }

        private void LoadDistrictInfoByBankCodeIntoRoutingInfoPage(string bankCode)
        {
            cbDistrictNameRoutingInfo.Items.Clear();
            DataTable BdAllDistrictlist = mg.GetDistrictListByBankCode(bankCode);
            cbDistrictNameRoutingInfo.Items.Add("---- Select ----");

            for (int rw = 0; rw < BdAllDistrictlist.Rows.Count; rw++)
            {
                cbDistrictNameRoutingInfo.Items.Add(BdAllDistrictlist.Rows[rw][0].ToString());
            }
            cbDistrictNameRoutingInfo.SelectedIndex = 0;
        }

        private void cbDistrictNameRoutingInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDistrictNameRoutingInfo.SelectedIndex != 0)
            {
                string bankCode = cbBankCode.SelectedItem.ToString().Trim();
                string distName = cbDistrictNameRoutingInfo.SelectedItem.ToString().Trim();
                DataTable Branchlist = mg.GetBranchListByBankCodeAndDistrictName(bankCode, distName);

                cbBranchNameRoutingInfo.Items.Clear();
                cbBranchNameRoutingInfo.Items.Add("---- Select ----");
                for (int rw = 0; rw < Branchlist.Rows.Count; rw++)
                {
                    cbBranchNameRoutingInfo.Items.Add(Branchlist.Rows[rw][0].ToString());
                }
                cbBranchNameRoutingInfo.SelectedIndex = 0;
            }
            else
            {
                cbBranchNameRoutingInfo.Items.Clear();
            }
        }

        //private void LoadBranchInfoByBankCodeIntoRoutingInfoPage(string bankCode)
        //{
        //    cbBranchNameRoutingInfo.Items.Clear();
        //    DataTable Branchlist = mg.GetBranchListByBankCode(bankCode);
        //    cbBranchNameRoutingInfo.Items.Add("---- Select ----");
            
        //    for (int rw = 0; rw < Branchlist.Rows.Count; rw++)
        //    {
        //        cbBranchNameRoutingInfo.Items.Add(Branchlist.Rows[rw][0].ToString());
        //    }

        //    cbBranchNameRoutingInfo.SelectedIndex = 0;
        //}             

        private void btnSearchRouting_Click(object sender, EventArgs e)
        {
            string bankCode = "", branchName = "", districtName = "", whereClause = "";
            routingEditId = -1;

            if (cbBankNameRoutingInfo.SelectedIndex != 0)
            {
                bankCode = cbBankCode.SelectedItem.ToString().Trim();
                whereClause = " WHERE [BankCode]='" + bankCode + "' ";

                if (cbDistrictNameRoutingInfo.SelectedIndex != 0)
                {
                    districtName = cbDistrictNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper();
                    whereClause += " AND upper([District])='" + districtName + "' ";
                }

                if (cbBranchNameRoutingInfo.SelectedIndex > 0)
                {
                    branchName = cbBranchNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper();
                    whereClause += " AND upper([BranchName])='" + branchName + "' ";
                }

                if (txtRoutingNo.Text.Trim().Length > 0)
                {
                    try
                    {
                        routingNoValue = Int32.Parse(txtRoutingNo.Text.Trim());
                        whereClause += " AND [RoutingNumber] like '" + txtRoutingNo.Text.Trim() + "%'";
                    }
                    catch (Exception ex)  // wrote branch name
                    {
                        whereClause += " AND UPPER([BranchName]) like UPPER('" + txtRoutingNo.Text.Trim() + "%')";
                    }
                }
                               
                DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                LoadRoutingInfoDataIntoGridView(dtRoutingInfos);  
            }
            else
            {
                dgridViewRoutingInfos.DataSource = null;
                MessageBox.Show("Error in Bank Selection !!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadRoutingInfoDataIntoGridView(DataTable dtRoutingInfos)
        {
            dgridViewRoutingInfos.DataSource = null;
            dgridViewRoutingInfos.DataSource = dtRoutingInfos;

            if (dtRoutingInfos.Rows.Count > 0)
            {
                dgridViewRoutingInfos.Columns["Sl"].Width = 40;
                dgridViewRoutingInfos.Columns["BankCode"].Width = 70;
                dgridViewRoutingInfos.Columns["BankName"].Width = 230;
                dgridViewRoutingInfos.Columns["BranchName"].Width = 150;
                dgridViewRoutingInfos.Columns["Email"].Width = 240;
                dgridViewRoutingInfos.Columns["Routing"].Width = 80;
                dgridViewRoutingInfos.Columns["MobileNumber"].Width = 300;
            }
        }

        private void dgridViewRoutingInfos_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            routingEditId = Convert.ToInt32(dgridViewRoutingInfos.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void btnEditRoutingInfo_Click(object sender, EventArgs e)
        {            
            if (routingEditId > -1)
            {                
                frmEditRoutingInfo edtri = new frmEditRoutingInfo();
                edtri.routingAutoId = routingEditId;
                edtri.loggedUser = this.loggedUser;
                edtri.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select Record By Clicking Row Header", "Error in Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }               

        private void txtRoutingNo_TextChanged(object sender, EventArgs e)
        {
            if (txtRoutingNo.Text.Trim().Length > 0)
            {
                string bankCode = "", whereClause = "";

                try
                {
                    routingNoValue = Int32.Parse(txtRoutingNo.Text.Trim());

                    if (cbBankNameRoutingInfo.SelectedIndex != 0)
                    {
                        bankCode = cbBankCode.SelectedItem.ToString().Trim();
                        whereClause = " WHERE [BankCode]='" + bankCode + "' ";

                        if (cbDistrictNameRoutingInfo.SelectedIndex != 0)
                        {
                            whereClause += " AND upper([District])='" + cbDistrictNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                        }
                        if (cbBranchNameRoutingInfo.SelectedIndex > 0)
                        {
                            whereClause += " AND upper([BranchName])='" + cbBranchNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                        }
                        if (txtRoutingNo.Text.Trim().Length > 0)
                        {
                            whereClause += " AND [RoutingNumber] like '" + txtRoutingNo.Text.Trim() + "%'";
                        }

                        DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                        LoadRoutingInfoDataIntoGridView(dtRoutingInfos);
                    }
                    else  // bank not selected
                    {
                        whereClause = " WHERE [RoutingNumber] like '" + txtRoutingNo.Text.Trim() + "%'";
                        DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                        LoadRoutingInfoDataIntoGridView(dtRoutingInfos);
                    }
                }
                catch (Exception ex)  // wrote branch name
                {
                    if (cbBankNameRoutingInfo.SelectedIndex != 0)
                    {
                        bankCode = cbBankCode.SelectedItem.ToString().Trim();
                        whereClause = " WHERE [BankCode]='" + bankCode + "' ";

                        if (cbDistrictNameRoutingInfo.SelectedIndex != 0)
                        {
                            whereClause += " AND upper([District])='" + cbDistrictNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                        }
                        if (cbBranchNameRoutingInfo.SelectedIndex > 0)
                        {
                            whereClause += " AND upper([BranchName])='" + cbBranchNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                        }

                        if (txtRoutingNo.Text.Trim().Length > 0)
                        {
                            whereClause += " AND UPPER([BranchName]) like UPPER('" + txtRoutingNo.Text.Trim() + "%')";
                        }

                        DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                        LoadRoutingInfoDataIntoGridView(dtRoutingInfos);
                    }
                    else  // bank not selected
                    {
                        whereClause = " WHERE UPPER([BranchName]) like UPPER('%" + txtRoutingNo.Text.Trim() + "%')";
                        DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                        LoadRoutingInfoDataIntoGridView(dtRoutingInfos);
                    }
                }                
            }
            else  // display all list based 
            {
                string bankCode = "", whereClause = "";
                if (cbBankNameRoutingInfo.SelectedIndex != 0)
                {
                    bankCode = cbBankCode.SelectedItem.ToString().Trim();
                    whereClause = " WHERE [BankCode]='" + bankCode + "' ";

                    if (cbDistrictNameRoutingInfo.SelectedIndex != 0)
                    {
                        whereClause += " AND upper([District])='" + cbDistrictNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                    }
                    if (cbBranchNameRoutingInfo.SelectedIndex > 0)
                    {
                        whereClause += " AND upper([BranchName])='" + cbBranchNameRoutingInfo.SelectedItem.ToString().Trim().ToUpper() + "' ";
                    }                    

                    DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);
                    LoadRoutingInfoDataIntoGridView(dtRoutingInfos);
                }
                else
                {
                    dgridViewRoutingInfos.DataSource = null;
                }
            }
        }

        private void btnAddNewRoutingInfo_Click(object sender, EventArgs e)
        {
            frmAddRoutingInfo addrt = new frmAddRoutingInfo();
            addrt.loggedUser = this.loggedUser;
            addrt.ShowDialog();
        }

        private void LoadBankInfoIntoRoutingInfoPage()
        {
            cbBankNameRoutingInfo.Items.Clear();
            DataTable BdAllBanklist = mg.GetBdAllBanklist();
            cbBankNameRoutingInfo.Items.Add("---- Select ----");

            cbBankCode.Items.Clear();
            cbBankCode.Items.Add("--Select--");

            for (int rw = 0; rw < BdAllBanklist.Rows.Count; rw++)
            {
                cbBankNameRoutingInfo.Items.Add(BdAllBanklist.Rows[rw][1].ToString());
                cbBankCode.Items.Add(BdAllBanklist.Rows[rw][0].ToString());
            }

            cbBankNameRoutingInfo.SelectedIndex = 0;
            cbBankCode.SelectedIndex = 0;
        }

        //private void LoadDistrictInfoIntoRoutingInfoPage()
        //{
        //    cbDistrictNameRoutingInfo.Items.Clear();
        //    DataTable BdAllDistrictlist = mg.GetBdAllDistrictlist();
        //    cbDistrictNameRoutingInfo.Items.Add("---- Select ----");

        //    for (int rw = 0; rw < BdAllDistrictlist.Rows.Count; rw++)
        //    {
        //        cbDistrictNameRoutingInfo.Items.Add(BdAllDistrictlist.Rows[rw][0].ToString());
        //    }
        //    cbDistrictNameRoutingInfo.SelectedIndex = 0;
        //}

        #endregion

        #region SUMMARY TAB

        private void btnSearchSumr_Click(object sender, EventArgs e)
        {            
            DateTime dateTime1 = DateTime.ParseExact(dtPickerFromSumr.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(dtPickerToSumr.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            string fromDate = dateTime1.ToString("yyyy-MM-dd");
            string toDate = dateTime2.ToString("yyyy-MM-dd");

            DataTable dtSummaryData = mg.GetSummaryDataByDates(fromDate, toDate);

            dataGridViewSummary.DataSource = null;
            dataGridViewSummary.DataSource = dtSummaryData;

            summaryDataRowCount = dtSummaryData.Rows.Count;

            dataGridViewSummary.Columns["StatusName"].Width = 250;

            if (summaryDataRowCount > 0)
            {
                btnSummaryReportExportToExcel.Enabled = true;
            }
            else
            {
                btnSummaryReportExportToExcel.Enabled = false;
            }
        }

        private void btnSummaryReportExportToExcel_Click(object sender, EventArgs e)
        {            
            DateTime dateTime1 = DateTime.ParseExact(dtPickerFromSumr.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(dtPickerToSumr.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            string fromDate = dateTime1.ToString("yyyy-MM-dd");
            string toDate = dateTime2.ToString("yyyy-MM-dd");

            string filePath = "";
            SaveFileDialog saveDlg = new SaveFileDialog();
            //saveDlg.InitialDirectory = @"D:\";
            saveDlg.Filter = "Excel files (*.xls)|*.xls";
            saveDlg.FilterIndex = 0;
            saveDlg.RestoreDirectory = true;
            saveDlg.Title = "Export Excel File To";
            saveDlg.FileName = "Query_Summary_" + fromDate + "_to_" + toDate + ".xls";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                filePath = saveDlg.FileName;
                Cursor.Current = Cursors.WaitCursor;
                DataTable dtSummaryData = mg.GetSummaryDataByDates(fromDate, toDate);
                GenerateExcelSummaryReport(filePath, fromDate, toDate, dtSummaryData);
                MessageBox.Show("File saved at: " + filePath);
                Cursor.Current = Cursors.Default;
            }
        }

        private void GenerateExcelSummaryReport(string filePath, string fromDate, string toDate, DataTable dtSummaryData)
        {
            //DataTable dtSummaryData = mg.GetSummaryDataByDates(fromDate, toDate);
            if (dtSummaryData.Rows.Count > 0)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application _excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks _workbooks = _excelApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook _workbook = _workbooks.Add();
                    Microsoft.Office.Interop.Excel.Worksheet _worksheet = _workbook.Worksheets[1];
                    _worksheet.Name = "Sheet1";
                    Microsoft.Office.Interop.Excel.Range _workSheetRange = _worksheet.get_Range("A1", "B1");

                    TOPHeaderSUMMARY(_worksheet, _workSheetRange, fromDate, toDate);

                    int row_num = 3;
                    ADD_HEADER_ROW_SUMMARY(row_num, _worksheet, _workSheetRange);
                    row_num++;
                    int firstTimeEmptyRow = 0;
                    string statusName;
                    int noOfTxn = 0;

                    for (int rCnt = 0; rCnt < dtSummaryData.Rows.Count; rCnt++)
                    {
                        if (firstTimeEmptyRow == 0)
                        {
                            addSumrDataMain(row_num, 1, "", "A" + row_num, "A" + row_num, "@", _worksheet, _workSheetRange);
                            addSumrDataMain(row_num, 2, "", "B" + row_num, "B" + row_num, "@", _worksheet, _workSheetRange);                            
                            row_num++;
                            firstTimeEmptyRow = 1;
                        }

                        statusName = dtSummaryData.Rows[rCnt][0].ToString();
                        noOfTxn = Convert.ToInt32(dtSummaryData.Rows[rCnt][1].ToString());

                        addSumrDataMain(row_num, 1, statusName, "A" + row_num, "A" + row_num, "@", _worksheet, _workSheetRange);
                        addSumrDataMain(row_num, 2, noOfTxn, "B" + row_num, "B" + row_num, "@", _worksheet, _workSheetRange);
                        row_num++;
                    }//for end

                    try
                    {
                        File.Delete(filePath);
                        _excelApp.ActiveWorkbook.SaveCopyAs(filePath);
                        _excelApp.ActiveWorkbook.Saved = true;
                        //---------------- remove extra empty row -----------------------
                        RemoveExtraEmptyRowFromSheetForMainData(filePath, "A4:B4");
                        //---------------------------------------------------------------
                        this.Text = "";
                    }
                    catch (IOException ioe) { MessageBox.Show(ioe.ToString()); }

                    try{  _workbook.Close(true, null, null); _excelApp.Quit(); }
                    finally
                    {
                        if (_worksheet != null){  Marshal.FinalReleaseComObject(_worksheet); _worksheet = null; }
                        if (_workbook != null) {  Marshal.FinalReleaseComObject(_workbook);  _workbook = null;  }
                        if (_excelApp != null) {  Marshal.FinalReleaseComObject(_excelApp);  _excelApp = null;  }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception exc)
                {
                    string err = exc.ToString();
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void TOPHeaderSUMMARY(Excel.Worksheet _worksheet, Excel.Range _workSheetRange, string fromDate, string toDate)
        {
            _worksheet.Cells[1, 2] = "Query Report";
            _workSheetRange = _worksheet.get_Range("A1", "B1");
            _workSheetRange.Merge(0);
            _workSheetRange.Interior.Color = Color.Gainsboro.ToArgb();
            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Bold = true;
            _workSheetRange.Font.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Name = "Century Gothic";
            _worksheet.get_Range("A1", "B1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            _workSheetRange.Font.Size = 12;

            _worksheet.Cells[2, 2] = "From " + fromDate + " To " + toDate;
            _workSheetRange = _worksheet.get_Range("A2", "B2");
            _workSheetRange.Merge(0);
            _workSheetRange.Interior.Color = Color.Gainsboro.ToArgb();
            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Bold = true;
            _workSheetRange.Font.Color = Color.Black.ToArgb();
            _worksheet.get_Range("A2", "B2").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        private void RemoveExtraEmptyRowFromSheetForMainData(string filePath, string haveToDeleteCell)
        {
            Excel.Application _app = new Excel.Application();
            Excel.Workbooks _books;
            Excel.Workbook _book = null;
            Excel.Sheets _sheets;
            Excel.Worksheet _sheet;

            _books = _app.Workbooks;
            _book = _books.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _sheets = _book.Worksheets;

            _sheet = (Excel.Worksheet)_sheets[1];
            _sheet.Select(Type.Missing);

            Excel.Range rangeDD = _sheet.get_Range(haveToDeleteCell, Type.Missing);
            rangeDD.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(rangeDD);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_sheet);
            _book.Save();
            _book.Close(false, Type.Missing, Type.Missing);

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_book);
            _app.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_app);

            Marshal.FinalReleaseComObject(_sheet);
            Marshal.FinalReleaseComObject(_book);
            Marshal.FinalReleaseComObject(_app);
            GC.Collect();
        }

        private void addSumrDataMain(int row, int col, object data, string cell1, string cell2, string format,
            Excel.Worksheet _worksheet, Excel.Range _workSheetRange)
        {
            _worksheet.Cells[row, col] = data;
            _workSheetRange = _worksheet.get_Range(cell1, cell2);
            _workSheetRange.Borders.Color = System.Drawing.Color.Black.ToArgb();
            _workSheetRange.EntireColumn.NumberFormat = format;

            if (col == 2)
            {
                _workSheetRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
        }

        private void ADD_HEADER_ROW_SUMMARY(int rownum, Excel.Worksheet _worksheet, Excel.Range _workSheetRange)
        {
            createHeaders(rownum, 1, "Status Name", "A" + rownum, "A" + rownum, 0, true, 30, _worksheet, _workSheetRange);
            createHeaders(rownum, 2, "No of Txn", "B" + rownum, "B" + rownum, 0, true, 15, _worksheet, _workSheetRange);
        }

        private void createHeaders(int row, int col, string htext, string cell1, string cell2, int mergeColumns, bool fontBold, int columnSize,
                Excel.Worksheet _worksheet, Excel.Range _workSheetRange)
        {
            _worksheet.Cells[row, col] = htext;
            _workSheetRange = _worksheet.get_Range(cell1, cell2);
            _workSheetRange.Merge(mergeColumns);
            _workSheetRange.Interior.Color = System.Drawing.Color.Gainsboro.ToArgb();

            _workSheetRange.Borders.Color = System.Drawing.Color.Black.ToArgb();
            _workSheetRange.Font.Bold = fontBold;
            _workSheetRange.ColumnWidth = columnSize;
            _workSheetRange.Font.Color = System.Drawing.Color.Black.ToArgb();
            _workSheetRange.Font.Size = 10;
        }

        #endregion

        #region BACH Info
        
        private void LoadBACHContactDetailsInfo()
        {
            string whereClause = "";
            DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);
            LoadBACHContactDetailsIntoGridView(dtBACHContactDetails);
        }

        private void LoadBACHContactDetailsIntoGridView(DataTable dtBACHContactDetails)
        {
            dgridViewBACHContacts.DataSource = null;
            dgridViewBACHContacts.DataSource = dtBACHContactDetails;

            dgridViewBACHContacts.Columns["SL"].Width = 40;
            dgridViewBACHContacts.Columns["BankCode"].Width = 70;
            dgridViewBACHContacts.Columns["BankName"].Width = 230;
            dgridViewBACHContacts.Columns["ContactNo"].Width = 400;
            dgridViewBACHContacts.Columns["EmailAddress"].Width = 350;
        }
        
        private void txtBankNameBACHpage_TextChanged(object sender, EventArgs e)
        {
            string whereClause = "";
            if (txtBankNameBACHpage.Text.Trim().Length > 0)
            {
                whereClause = " WHERE upper([BankName]) like upper('%" + txtBankNameBACHpage.Text.Trim() + "%')";
                DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);
                LoadBACHContactDetailsIntoGridView(dtBACHContactDetails);
            }
            else
            {
                whereClause = "";
                DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);
                LoadBACHContactDetailsIntoGridView(dtBACHContactDetails);
            }
        }

        private void btnRefreshBACHinfo_Click(object sender, EventArgs e)
        {
            string whereClause = "";
            if (txtBankNameBACHpage.Text.Trim().Length > 0)
            {
                whereClause = " WHERE upper([BankName]) like upper('%" + txtBankNameBACHpage.Text.Trim() + "%')";
                DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);
                LoadBACHContactDetailsIntoGridView(dtBACHContactDetails);
            }
            else
            {
                whereClause = "";
                DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);
                LoadBACHContactDetailsIntoGridView(dtBACHContactDetails);
            }
        }

        private void btnEditBACHinfo_Click(object sender, EventArgs e)
        {
            if (bachInfoEditId > -1)
            {
                frmEditBACHInfo edtbi = new frmEditBACHInfo();
                edtbi.bachInfoEditId = bachInfoEditId;
                edtbi.loggedUser = this.loggedUser;
                edtbi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select Record By Clicking Row Header", "Error in Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgridViewBACHContacts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bachInfoEditId = Convert.ToInt32(dgridViewBACHContacts.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutMe abt = new frmAboutMe();
            abt.ShowDialog();
        }

        private void btnSearchStopPay_Click(object sender, EventArgs e)
        {
            string fromDate = DateTime.ParseExact(dTPickerStopPayFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string toDate = DateTime.ParseExact(dTPickerStopPayTo.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            DataTable dtReportData = mg.GetStopPaymentDataByDates(fromDate, toDate);

            dataGridViewStopPayment.DataSource = null;
            dataGridViewStopPayment.DataSource = dtReportData;

            lblTotalStopPaymentRecords.Text = "Total Records : " + dtReportData.Rows.Count;
        }

        private void btnStopPayExport_Click(object sender, EventArgs e)
        {
            string fromDate = DateTime.ParseExact(dTPickerStopPayFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string toDate = DateTime.ParseExact(dTPickerStopPayTo.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string filePath = "";
            SaveFileDialog saveDlg = new SaveFileDialog();
            //saveDlg.InitialDirectory = @"D:\";
            saveDlg.Filter = "Excel files (*.xls)|*.xls";
            saveDlg.FilterIndex = 0;
            saveDlg.RestoreDirectory = true;
            saveDlg.Title = "Export Excel File To";
            saveDlg.FileName = "StopPayment_data_" + fromDate + "_to_" + toDate + ".xls";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                filePath = saveDlg.FileName;
                Cursor.Current = Cursors.WaitCursor;
                GenerateStopPaymentExcelReport(filePath, fromDate, toDate);
                MessageBox.Show("File saved at: " + filePath);
                Cursor.Current = Cursors.Default;
            }
        }

        private void GenerateStopPaymentExcelReport(string filePath, string fromDate, string toDate)
        {
            DataTable dtReportData = mg.GetStopPaymentDataByDates(fromDate, toDate);
            if (dtReportData.Rows.Count > 0)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application _excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks _workbooks = _excelApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook _workbook = _workbooks.Add();
                    Microsoft.Office.Interop.Excel.Worksheet _worksheet = _workbook.Worksheets[1];
                    _worksheet.Name = "Sheet1";
                    Microsoft.Office.Interop.Excel.Range _workSheetRange = _worksheet.get_Range("A1", "L1");

                    TOPHeaderSPSUMMARY(_worksheet, _workSheetRange, fromDate, toDate);

                    int row_num = 3;
                    ADD_HEADER_ROW_SP_SUMMARY(row_num, _worksheet, _workSheetRange);
                    row_num++;
                    int firstTimeEmptyRow = 0;
                    //string statusName;
                    //int noOfTxn = 0;

                    for (int rCnt = 0; rCnt < dtReportData.Rows.Count; rCnt++)
                    {
                        if (firstTimeEmptyRow == 0)
                        {
                            addSPDataMain(row_num, 1, "", "A" + row_num, "A" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 2, "", "B" + row_num, "B" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 3, "", "C" + row_num, "C" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 4, "", "D" + row_num, "D" + row_num, "###.##", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 5, "", "E" + row_num, "E" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 6, "", "F" + row_num, "F" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 7, "", "G" + row_num, "G" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 8, "", "H" + row_num, "H" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 9, "", "I" + row_num, "I" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 10, "", "J" + row_num, "J" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 11, "", "K" + row_num, "K" + row_num, "@", _worksheet, _workSheetRange);
                            addSPDataMain(row_num, 12, "", "L" + row_num, "L" + row_num, "@", _worksheet, _workSheetRange);
                            row_num++;
                            firstTimeEmptyRow = 1;
                        }
                                               
                        addSPDataMain(row_num, 1, Convert.ToString(dtReportData.Rows[rCnt][0]), "A" + row_num, "A" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 2, Convert.ToString(dtReportData.Rows[rCnt][1]), "B" + row_num, "B" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 3, Convert.ToString(dtReportData.Rows[rCnt][2]), "C" + row_num, "C" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 4, String.Format("{0:0.00}", Convert.ToDouble(dtReportData.Rows[rCnt][3])), "D" + row_num, "D" + row_num, "###.##", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 5, Convert.ToString(dtReportData.Rows[rCnt][4]), "E" + row_num, "E" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 6, Convert.ToString(dtReportData.Rows[rCnt][5]), "F" + row_num, "F" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 7, Convert.ToString(dtReportData.Rows[rCnt][6]), "G" + row_num, "G" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 8, Convert.ToString(dtReportData.Rows[rCnt][7]), "H" + row_num, "H" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 9, Convert.ToString(dtReportData.Rows[rCnt][8]), "I" + row_num, "I" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 10, Convert.ToString(dtReportData.Rows[rCnt][9]), "J" + row_num, "J" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 11, Convert.ToString(dtReportData.Rows[rCnt][10]), "K" + row_num, "K" + row_num, "@", _worksheet, _workSheetRange);
                        addSPDataMain(row_num, 12, Convert.ToString(dtReportData.Rows[rCnt][11]), "L" + row_num, "L" + row_num, "@", _worksheet, _workSheetRange);
                        row_num++;

                    }//for end

                    try
                    {
                        File.Delete(filePath);
                        _excelApp.ActiveWorkbook.SaveCopyAs(filePath);
                        _excelApp.ActiveWorkbook.Saved = true;
                        //---------------- remove extra empty row -----------------------
                        RemoveExtraEmptyRowFromSheetForMainData(filePath, "A4:L4");
                        //---------------------------------------------------------------
                        //this.Text = "";
                    }
                    catch (IOException ioe) { MessageBox.Show(ioe.ToString()); }

                    try { _workbook.Close(true, null, null); _excelApp.Quit(); }
                    finally
                    {
                        if (_worksheet != null) { Marshal.FinalReleaseComObject(_worksheet); _worksheet = null; }
                        if (_workbook != null) { Marshal.FinalReleaseComObject(_workbook); _workbook = null; }
                        if (_excelApp != null) { Marshal.FinalReleaseComObject(_excelApp); _excelApp = null; }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception exc)
                {
                    string err = exc.ToString();
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void addSPDataMain(int row, int col, object data, string cell1, string cell2, string format,
            Excel.Worksheet _worksheet, Excel.Range _workSheetRange)
        {
            _worksheet.Cells[row, col] = data;
            _workSheetRange = _worksheet.get_Range(cell1, cell2);
            _workSheetRange.Borders.Color = System.Drawing.Color.Black.ToArgb();
            _workSheetRange.EntireColumn.NumberFormat = format;

            if (col == 4)
            {
                _workSheetRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
        }

        private void ADD_HEADER_ROW_SP_SUMMARY(int rownum, Excel.Worksheet _worksheet, Excel.Range _workSheetRange)
        {
            createHeaders(rownum, 1, "QueryDate", "A" + rownum, "A" + rownum, 0, true, 12, _worksheet, _workSheetRange);
            createHeaders(rownum, 2, "PINNumber", "B" + rownum, "B" + rownum, 0, true, 20, _worksheet, _workSheetRange);
            createHeaders(rownum, 3, "AccountNo", "C" + rownum, "C" + rownum, 0, true, 10, _worksheet, _workSheetRange);
            createHeaders(rownum, 4, "Amount", "D" + rownum, "D" + rownum, 0, true, 12, _worksheet, _workSheetRange);
            createHeaders(rownum, 5, "Payment Mode", "E" + rownum, "E" + rownum, 0, true, 25, _worksheet, _workSheetRange);
            createHeaders(rownum, 6, "Exchange House", "F" + rownum, "F" + rownum, 0, true, 25, _worksheet, _workSheetRange);
            createHeaders(rownum, 7, "Bank Status", "G" + rownum, "G" + rownum, 0, true, 20, _worksheet, _workSheetRange);
            createHeaders(rownum, 8, "MTB Status", "H" + rownum, "H" + rownum, 0, true, 25, _worksheet, _workSheetRange);
            createHeaders(rownum, 9, "CreditDate", "I" + rownum, "I" + rownum, 0, true, 12, _worksheet, _workSheetRange);
            createHeaders(rownum, 10, "InputDate", "J" + rownum, "J" + rownum, 0, true, 18, _worksheet, _workSheetRange);
            createHeaders(rownum, 11, "UpdateDate", "K" + rownum, "K" + rownum, 0, true, 18, _worksheet, _workSheetRange);
            createHeaders(rownum, 12, "Remarks", "L" + rownum, "L" + rownum, 0, true, 35, _worksheet, _workSheetRange);
        }

        private void TOPHeaderSPSUMMARY(Excel.Worksheet _worksheet, Excel.Range _workSheetRange, string fromDate, string toDate)
        {            
            _worksheet.Cells[1, 2] = "Stop Payment Query Report";
            _workSheetRange = _worksheet.get_Range("A1", "L1");
            _workSheetRange.Merge(0);
            _workSheetRange.Interior.Color = Color.Gainsboro.ToArgb();
            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Bold = true;
            _workSheetRange.Font.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Name = "Century Gothic";
            _worksheet.get_Range("A1", "L1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            _workSheetRange.Font.Size = 12;

            _worksheet.Cells[2, 2] = "From " + fromDate + " To " + toDate;
            _workSheetRange = _worksheet.get_Range("A2", "L2");
            _workSheetRange.Merge(0);
            _workSheetRange.Interior.Color = Color.Gainsboro.ToArgb();
            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Bold = true;
            _workSheetRange.Font.Color = Color.Black.ToArgb();
            _worksheet.get_Range("A2", "L2").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;        
        }

        private void btnSearchUserWiseSumr_Click(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select User");
            }
            else
            {
                string userIdName = comboBoxUser.SelectedItem.ToString();
                string userId = userIdName.Split('-')[0].Trim();

                DateTime dateTime1 = DateTime.ParseExact(dtPFromSumrUserWise.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                DateTime dateTime2 = DateTime.ParseExact(dtPToSumrUserWise.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                string fromDate = dateTime1.ToString("yyyy-MM-dd");
                string toDate = dateTime2.ToString("yyyy-MM-dd");

                DataTable dtSummaryData = mg.GetSummaryDataByUserAndDates(userId, fromDate, toDate);

                dataGridViewUserWiseSummary.DataSource = null;
                dataGridViewUserWiseSummary.DataSource = dtSummaryData;

                summaryDataRowCount = dtSummaryData.Rows.Count;

                dataGridViewUserWiseSummary.Columns["StatusName"].Width = 250;

                if (summaryDataRowCount > 0)
                {
                    btnExportUserWiseSummary.Enabled = true;
                }
                else
                {
                    btnExportUserWiseSummary.Enabled = false;
                }
            }
        }

        private void btnExportUserWiseSummary_Click(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select User");
            }
            else
            {
                string userIdName = comboBoxUser.SelectedItem.ToString();
                string userId = userIdName.Split('-')[0].Trim();

                DateTime dateTime1 = DateTime.ParseExact(dtPFromSumrUserWise.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                DateTime dateTime2 = DateTime.ParseExact(dtPToSumrUserWise.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                string fromDate = dateTime1.ToString("yyyy-MM-dd");
                string toDate = dateTime2.ToString("yyyy-MM-dd");

                string filePath = "";
                SaveFileDialog saveDlg = new SaveFileDialog();
                //saveDlg.InitialDirectory = @"D:\";
                saveDlg.Filter = "Excel files (*.xls)|*.xls";
                saveDlg.FilterIndex = 0;
                saveDlg.RestoreDirectory = true;
                saveDlg.Title = "Export Excel File To";
                saveDlg.FileName = userId + "_Query_Summary_" + fromDate + "_to_" + toDate + ".xls";

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveDlg.FileName;
                    Cursor.Current = Cursors.WaitCursor;
                    DataTable dtSummaryData = mg.GetSummaryDataByUserAndDates(userId, fromDate, toDate);
                    GenerateExcelSummaryReport(filePath, fromDate, toDate, dtSummaryData);
                    MessageBox.Show("File saved at: " + filePath);
                    Cursor.Current = Cursors.Default;
                }
            }
        }

       


    }
}
