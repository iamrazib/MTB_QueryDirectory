using QueryDirectory.DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


namespace QueryDirectory
{
    public partial class frmBatchUpload : Form
    {
        static Manager mg = new Manager();
        object[,] batchDataFileArray = new object[500, 2];
        int batchDataFileIndex = -1;

        public string loggedUser = "";
        public string loggedUserIdAndName = "";


        public frmBatchUpload()
        {
            InitializeComponent();
        }

        private void frmBatchUpload_Load(object sender, EventArgs e)
        {
            LoadExhouseDropDownList();
        }

        private void LoadExhouseDropDownList()
        {
            cbExchBatchData.Items.Clear();
            DataTable Exchlist = mg.GetExchList();
            cbExchBatchData.Items.Add("---- Select Exhouse ----");

            for (int rw = 0; rw < Exchlist.Rows.Count; rw++)
            {
                cbExchBatchData.Items.Add(Exchlist.Rows[rw][1].ToString() + " - " + Exchlist.Rows[rw][0].ToString());
            }

            cbExchBatchData.SelectedIndex = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Files",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog1.FileName;
            }
        }

        private void btnFetchBatchData_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Excel.Application xlAppDataFile = null;
            Excel.Workbooks xlWorkBooksDataFile = null;
            Excel.Workbook xlWorkBookDataFile = null;
            Excel.Sheets _xlSheetsDataFile = null;
            Excel.Worksheet xlWorkSheetDataFile = null;
            Excel.Range rangeDataFile = null;
            int rw = 0;
            int rCnt, cCnt;

            xlAppDataFile = new Excel.Application();
            xlWorkBooksDataFile = xlAppDataFile.Workbooks;
            xlWorkBookDataFile = xlWorkBooksDataFile.Open(txtFilePath.Text, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            _xlSheetsDataFile = xlWorkBookDataFile.Worksheets;
            xlWorkSheetDataFile = _xlSheetsDataFile.get_Item(1);

            rangeDataFile = xlWorkSheetDataFile.UsedRange;
            rw = rangeDataFile.Rows.Count;

            object pinNo, paymentMode;

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                pinNo = rangeDataFile.Cells[rCnt, 1].Value2;
                paymentMode = rangeDataFile.Cells[rCnt, 2].Value2;

                if (pinNo != null)
                {
                    batchDataFileIndex++;

                    for (cCnt = 1; cCnt <= 2; cCnt++)
                    {
                        batchDataFileArray[batchDataFileIndex, (cCnt - 1)] = "" + rangeDataFile.Cells[rCnt, cCnt].Value2;
                    }
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();


            DataTable dtBatchData = CreateDataTableBatchData();
            DataRow drow;

            if (batchDataFileIndex >= 0)
            {
                for (int rwc = 0; rwc <= batchDataFileIndex; rwc++)
                {
                    drow = dtBatchData.NewRow();
                    drow["PIN"] = batchDataFileArray[rwc, 0];
                    drow["PayMode"] = batchDataFileArray[rwc, 1];
                    dtBatchData.Rows.Add(drow);
                }

                dataGridViewBatchData.DataSource = null;
                dataGridViewBatchData.DataSource = dtBatchData;
            }
        }

        private DataTable CreateDataTableBatchData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PIN");
            dt.Columns.Add("PayMode");
            return dt;
        }

        private void btnUploadBatchData_Click(object sender, EventArgs e)
        {
            if (batchDataFileIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Are You Sure to Upload ?", "Upload Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    if (cbExchBatchData.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Select Exchange House !!!", "Error in Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string vdate = dTPickerBatch.Text;
                        string exch = cbExchBatchData.SelectedItem.ToString();
                        string exchId = exch.Split('-')[1].Trim();

                        string pin = "", paymode = "", modeId;
                        bool isExistsThisPinEarlier = false, status;

                        for (int rwc = 0; rwc <= batchDataFileIndex; rwc++)
                        {
                            pin = (string)batchDataFileArray[rwc, 0];
                            paymode = (string)batchDataFileArray[rwc, 1];
                            modeId = GetPaymentModeId(paymode);

                            isExistsThisPinEarlier = mg.isExistsThisPinEarlier(pin.Trim());

                            if (!isExistsThisPinEarlier)
                            {
                                status = mg.saveQueryData(vdate, pin, "", modeId, exchId, loggedUser);
                            }
                        }

                        MessageBox.Show("Upload Success !!!");
                    }
                }
            }
        }

        private string GetPaymentModeId(string paymode)
        {
            if (paymode.ToLower().Contains("other"))
                return "3";
            else if (paymode.ToLower().Contains("cash"))
                return "2";
            else if (paymode.ToLower().Contains("wallet") || paymode.ToLower().Contains("mobile"))
                return "4";
            else return "1";
        }

        private void frmBatchUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Form1 fm1 = new Form1();
            //fm1.LoadTodaysInputIntoGrid(dTPickerBatch.Text);
            //fm1.LoadQueryDataIntoUpdateRecordGrid(dTPickerBatch.Text);
        }

        private void linkLabelFileFormat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmFileFormat ff = new frmFileFormat();
            ff.ShowDialog();
        }


    }
}
