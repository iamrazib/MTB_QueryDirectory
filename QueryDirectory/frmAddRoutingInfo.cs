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

namespace QueryDirectory
{
    public partial class frmAddRoutingInfo : Form
    {
        public string loggedUser = "";
        static Manager mg = new Manager();

        public frmAddRoutingInfo()
        {
            InitializeComponent();
        }

        private void frmAddRoutingInfo_Load(object sender, EventArgs e)
        {
            LoadBankInfoIntoRoutingInfoPage();
            LoadDistrictInfoIntoRoutingInfoPage();
        }

        private void LoadDistrictInfoIntoRoutingInfoPage()
        {
            cbDistrictName.Items.Clear();
            DataTable BdAllDistrictlist = mg.GetBdAllDistrictlist();
            cbDistrictName.Items.Add("---- Select ----");

            for (int rw = 0; rw < BdAllDistrictlist.Rows.Count; rw++)
            {
                cbDistrictName.Items.Add(BdAllDistrictlist.Rows[rw][0].ToString());
            }
            cbDistrictName.SelectedIndex = 0;
        }

        private void LoadBankInfoIntoRoutingInfoPage()
        {
            cbBankName.Items.Clear();
            DataTable BdAllBanklist = mg.GetBdAllBanklist();
            cbBankName.Items.Add("---- Select ----");
            cbBankCode.Items.Clear();
            cbBankCode.Items.Add("--Select--");

            for (int rw = 0; rw < BdAllBanklist.Rows.Count; rw++)
            {
                cbBankName.Items.Add(BdAllBanklist.Rows[rw][1].ToString());
                cbBankCode.Items.Add(BdAllBanklist.Rows[rw][0].ToString());
            }
            cbBankName.SelectedIndex = 0;
            cbBankCode.SelectedIndex = 0;
        }

        private void cbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBankCode.SelectedIndex = cbBankName.SelectedIndex;
        }

        private void btnSaveRoutingInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRoutingNo.Text.Trim().Length != 9)
                {
                    MessageBox.Show("Routing Number is not in proper length", "Routing Length Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtBranchName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Enter Branch Name", "Empty Branch Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (mg.IsRoutingNumberAlreadyExists(txtRoutingNo.Text.Trim()))
                {
                    MessageBox.Show("Routing Number Already Exists", "Exist Routing No.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(cbDistrictName.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select District", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int lastSlNo = mg.GetLastRecordNumber();
                    if (lastSlNo != 0)
                    {
                        int slNo = lastSlNo + 1;
                        string bankCode = cbBankCode.SelectedItem.ToString();
                        string bankName = cbBankName.SelectedItem.ToString();
                        string brName = txtBranchName.Text.Trim().ToUpper();
                        string districtName = cbDistrictName.SelectedItem.ToString().Trim().ToUpper();
                        string rtNum = txtRoutingNo.Text.Trim();
                        string email = txtEmailValue.Text.Trim();
                        string contactNo = txtMobileNo.Text.Trim();

                        bool status = mg.SaveRoutingInfo(slNo, bankCode, bankName, brName, districtName, rtNum, email, contactNo);
                        if (status)
                        {
                            bool statHist = mg.UpdateRoutingHistoryTable(rtNum, loggedUser);
                            MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error:" + exc.ToString());
            }
        }

        private void btnCancelRoutingInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
