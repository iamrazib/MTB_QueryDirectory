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
    public partial class frmEditBACHInfo : Form
    {
        public int bachInfoEditId;
        public string loggedUser = "";
        static Manager mg = new Manager();

        public frmEditBACHInfo()
        {
            InitializeComponent();
        }

        private void frmEditBACHInfo_Load(object sender, EventArgs e)
        {
            string whereClause = " WHERE [AutoId]=" + bachInfoEditId;
            DataTable dtBACHContactDetails = mg.GetBACHContactDetailsByWhereClause(whereClause);

            txtAutoId.Text = dtBACHContactDetails.Rows[0]["Sl"].ToString();
            txtBankCode.Text = dtBACHContactDetails.Rows[0]["BankCode"].ToString();
            txtBankName.Text = dtBACHContactDetails.Rows[0]["BankName"].ToString();
            txtEmailValue.Text = dtBACHContactDetails.Rows[0]["EmailAddress"].ToString();
            txtContactNo.Text = dtBACHContactDetails.Rows[0]["ContactNo"].ToString();
        }

        private void btnUpdateRoutingInfo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Update ?", "Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string email = txtEmailValue.Text.Trim();
                string contactNo = txtContactNo.Text.Trim();
                int idVal = Convert.ToInt32(txtAutoId.Text);

                bool status = mg.UpdateBACHInformation(idVal, email, contactNo, loggedUser);
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
