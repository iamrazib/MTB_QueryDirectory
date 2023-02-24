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
    public partial class frmEditRoutingInfo : Form
    {
        public int routingAutoId;
        public string loggedUser = "";
        static Manager mg = new Manager();

        public frmEditRoutingInfo()
        {
            InitializeComponent();
        }

        private void frmEditRoutingInfo_Load(object sender, EventArgs e)
        {
            string whereClause = " WHERE [AutoId]=" + routingAutoId;
            DataTable dtRoutingInfos = mg.GetRoutingInfosByWhereClause(whereClause);

            txtAutoId.Text = dtRoutingInfos.Rows[0]["Sl"].ToString();
            txtBankName.Text = dtRoutingInfos.Rows[0]["BankName"].ToString();
            txtBranchName.Text = dtRoutingInfos.Rows[0]["BranchName"].ToString();
            txtDistrictName.Text = dtRoutingInfos.Rows[0]["District"].ToString();
            txtRoutingNo.Text = dtRoutingInfos.Rows[0]["Routing"].ToString();
            txtEmailValue.Text = dtRoutingInfos.Rows[0]["Email"].ToString();
            txtMobileNo.Text = dtRoutingInfos.Rows[0]["MobileNumber"].ToString();
        }

        private void btnUpdateRoutingInfo_Click(object sender, EventArgs e)
        {
            if (txtRoutingNo.Text.Trim().Length != 9)
            {
                MessageBox.Show("Routing Number is not in proper length", "Routing Length Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //else if (mg.IsRoutingNumberAlreadyExists(txtRoutingNo.Text.Trim()))
            //{
            //    MessageBox.Show("Routing Number Already Exists", "Exist Routing No.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            else
            {
                string rtNum = txtRoutingNo.Text.Trim();
                string email = txtEmailValue.Text.Trim();
                string contactNo = txtMobileNo.Text.Trim();
                int idVal = Convert.ToInt32(txtAutoId.Text);

                bool status = mg.UpdateRoutingInfo(idVal, rtNum, email, contactNo, loggedUser);
                if (status)
                {
                    MessageBox.Show("Update Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void btnCancelUpdateRoutingInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
