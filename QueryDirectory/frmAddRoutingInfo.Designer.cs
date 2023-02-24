namespace QueryDirectory
{
    partial class frmAddRoutingInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBankName = new System.Windows.Forms.ComboBox();
            this.txtBranchName = new System.Windows.Forms.TextBox();
            this.cbDistrictName = new System.Windows.Forms.ComboBox();
            this.txtRoutingNo = new System.Windows.Forms.TextBox();
            this.txtEmailValue = new System.Windows.Forms.TextBox();
            this.txtMobileNo = new System.Windows.Forms.TextBox();
            this.cbBankCode = new System.Windows.Forms.ComboBox();
            this.btnCancelRoutingInfo = new System.Windows.Forms.Button();
            this.btnSaveRoutingInfo = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(37, 219);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Mobile No :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(37, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Email :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(37, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Routing No :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(37, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "District Name :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(37, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Branch Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Bank Name :";
            // 
            // cbBankName
            // 
            this.cbBankName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBankName.FormattingEnabled = true;
            this.cbBankName.Location = new System.Drawing.Point(141, 68);
            this.cbBankName.Name = "cbBankName";
            this.cbBankName.Size = new System.Drawing.Size(290, 21);
            this.cbBankName.TabIndex = 19;
            this.cbBankName.SelectedIndexChanged += new System.EventHandler(this.cbBankName_SelectedIndexChanged);
            // 
            // txtBranchName
            // 
            this.txtBranchName.Location = new System.Drawing.Point(141, 100);
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(290, 20);
            this.txtBranchName.TabIndex = 20;
            // 
            // cbDistrictName
            // 
            this.cbDistrictName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDistrictName.FormattingEnabled = true;
            this.cbDistrictName.Location = new System.Drawing.Point(141, 133);
            this.cbDistrictName.Name = "cbDistrictName";
            this.cbDistrictName.Size = new System.Drawing.Size(290, 21);
            this.cbDistrictName.TabIndex = 21;
            // 
            // txtRoutingNo
            // 
            this.txtRoutingNo.Location = new System.Drawing.Point(141, 165);
            this.txtRoutingNo.MaxLength = 9;
            this.txtRoutingNo.Name = "txtRoutingNo";
            this.txtRoutingNo.Size = new System.Drawing.Size(118, 20);
            this.txtRoutingNo.TabIndex = 22;
            // 
            // txtEmailValue
            // 
            this.txtEmailValue.Location = new System.Drawing.Point(141, 192);
            this.txtEmailValue.Name = "txtEmailValue";
            this.txtEmailValue.Size = new System.Drawing.Size(290, 20);
            this.txtEmailValue.TabIndex = 23;
            // 
            // txtMobileNo
            // 
            this.txtMobileNo.Location = new System.Drawing.Point(141, 219);
            this.txtMobileNo.Name = "txtMobileNo";
            this.txtMobileNo.Size = new System.Drawing.Size(290, 20);
            this.txtMobileNo.TabIndex = 24;
            // 
            // cbBankCode
            // 
            this.cbBankCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBankCode.FormattingEnabled = true;
            this.cbBankCode.Location = new System.Drawing.Point(437, 68);
            this.cbBankCode.Name = "cbBankCode";
            this.cbBankCode.Size = new System.Drawing.Size(78, 21);
            this.cbBankCode.TabIndex = 25;
            this.cbBankCode.Visible = false;
            // 
            // btnCancelRoutingInfo
            // 
            this.btnCancelRoutingInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelRoutingInfo.Location = new System.Drawing.Point(307, 284);
            this.btnCancelRoutingInfo.Name = "btnCancelRoutingInfo";
            this.btnCancelRoutingInfo.Size = new System.Drawing.Size(124, 34);
            this.btnCancelRoutingInfo.TabIndex = 27;
            this.btnCancelRoutingInfo.Text = "Cancel";
            this.btnCancelRoutingInfo.UseVisualStyleBackColor = true;
            this.btnCancelRoutingInfo.Click += new System.EventHandler(this.btnCancelRoutingInfo_Click);
            // 
            // btnSaveRoutingInfo
            // 
            this.btnSaveRoutingInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveRoutingInfo.Location = new System.Drawing.Point(141, 284);
            this.btnSaveRoutingInfo.Name = "btnSaveRoutingInfo";
            this.btnSaveRoutingInfo.Size = new System.Drawing.Size(124, 34);
            this.btnSaveRoutingInfo.TabIndex = 26;
            this.btnSaveRoutingInfo.Text = "Save";
            this.btnSaveRoutingInfo.UseVisualStyleBackColor = true;
            this.btnSaveRoutingInfo.Click += new System.EventHandler(this.btnSaveRoutingInfo_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(175, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "Add Routing Information";
            // 
            // frmAddRoutingInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 357);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCancelRoutingInfo);
            this.Controls.Add(this.btnSaveRoutingInfo);
            this.Controls.Add(this.cbBankCode);
            this.Controls.Add(this.txtMobileNo);
            this.Controls.Add(this.txtEmailValue);
            this.Controls.Add(this.txtRoutingNo);
            this.Controls.Add(this.cbDistrictName);
            this.Controls.Add(this.txtBranchName);
            this.Controls.Add(this.cbBankName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "frmAddRoutingInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Routing Info";
            this.Load += new System.EventHandler(this.frmAddRoutingInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBankName;
        private System.Windows.Forms.TextBox txtBranchName;
        private System.Windows.Forms.ComboBox cbDistrictName;
        private System.Windows.Forms.TextBox txtRoutingNo;
        private System.Windows.Forms.TextBox txtEmailValue;
        private System.Windows.Forms.TextBox txtMobileNo;
        private System.Windows.Forms.ComboBox cbBankCode;
        private System.Windows.Forms.Button btnCancelRoutingInfo;
        private System.Windows.Forms.Button btnSaveRoutingInfo;
        private System.Windows.Forms.Label label8;
    }
}