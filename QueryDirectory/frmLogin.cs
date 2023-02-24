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
    public partial class frmLogin : Form
    {
        static Manager mg = new Manager();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select your user");
            }
            else
            {
                string userIdName = comboBoxUser.SelectedItem.ToString();
                string pass = textBoxPass.Text;

                string userId = userIdName.Split('-')[0].Trim();
                bool passMatch = mg.isPasswordMatch(userId, pass);
                if (passMatch)
                {
                    Form1 frm1 = new Form1();
                    frm1.loggedUser = userId;
                    frm1.loggedUserIdAndName = userIdName;
                    frm1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password Do Not Match, Please Try Again !!!");
                }
            }
        }
    }
}
