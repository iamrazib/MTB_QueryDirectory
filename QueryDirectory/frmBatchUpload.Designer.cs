namespace QueryDirectory
{
    partial class frmBatchUpload
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
            this.dTPickerBatch = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnFetchBatchData = new System.Windows.Forms.Button();
            this.dataGridViewBatchData = new System.Windows.Forms.DataGridView();
            this.cbExchBatchData = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUploadBatchData = new System.Windows.Forms.Button();
            this.linkLabelFileFormat = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchData)).BeginInit();
            this.SuspendLayout();
            // 
            // dTPickerBatch
            // 
            this.dTPickerBatch.CustomFormat = "dd-MMM-yyyy";
            this.dTPickerBatch.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPickerBatch.Location = new System.Drawing.Point(63, 23);
            this.dTPickerBatch.Name = "dTPickerBatch";
            this.dTPickerBatch.Size = new System.Drawing.Size(111, 20);
            this.dTPickerBatch.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date :";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(86, 57);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(235, 20);
            this.txtFilePath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Select File:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(336, 55);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnFetchBatchData
            // 
            this.btnFetchBatchData.Location = new System.Drawing.Point(453, 55);
            this.btnFetchBatchData.Name = "btnFetchBatchData";
            this.btnFetchBatchData.Size = new System.Drawing.Size(100, 23);
            this.btnFetchBatchData.TabIndex = 8;
            this.btnFetchBatchData.Text = "Fetch Data";
            this.btnFetchBatchData.UseVisualStyleBackColor = true;
            this.btnFetchBatchData.Click += new System.EventHandler(this.btnFetchBatchData_Click);
            // 
            // dataGridViewBatchData
            // 
            this.dataGridViewBatchData.AllowUserToAddRows = false;
            this.dataGridViewBatchData.AllowUserToDeleteRows = false;
            this.dataGridViewBatchData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBatchData.Location = new System.Drawing.Point(24, 104);
            this.dataGridViewBatchData.Name = "dataGridViewBatchData";
            this.dataGridViewBatchData.ReadOnly = true;
            this.dataGridViewBatchData.Size = new System.Drawing.Size(529, 205);
            this.dataGridViewBatchData.TabIndex = 9;
            // 
            // cbExchBatchData
            // 
            this.cbExchBatchData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExchBatchData.FormattingEnabled = true;
            this.cbExchBatchData.Location = new System.Drawing.Point(262, 22);
            this.cbExchBatchData.Name = "cbExchBatchData";
            this.cbExchBatchData.Size = new System.Drawing.Size(291, 21);
            this.cbExchBatchData.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(191, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Exch House";
            // 
            // btnUploadBatchData
            // 
            this.btnUploadBatchData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadBatchData.Location = new System.Drawing.Point(423, 318);
            this.btnUploadBatchData.Name = "btnUploadBatchData";
            this.btnUploadBatchData.Size = new System.Drawing.Size(130, 26);
            this.btnUploadBatchData.TabIndex = 14;
            this.btnUploadBatchData.Text = "Upload";
            this.btnUploadBatchData.UseVisualStyleBackColor = true;
            this.btnUploadBatchData.Click += new System.EventHandler(this.btnUploadBatchData_Click);
            // 
            // linkLabelFileFormat
            // 
            this.linkLabelFileFormat.AutoSize = true;
            this.linkLabelFileFormat.Location = new System.Drawing.Point(559, 25);
            this.linkLabelFileFormat.Name = "linkLabelFileFormat";
            this.linkLabelFileFormat.Size = new System.Drawing.Size(58, 13);
            this.linkLabelFileFormat.TabIndex = 15;
            this.linkLabelFileFormat.TabStop = true;
            this.linkLabelFileFormat.Text = "File Format";
            this.linkLabelFileFormat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFileFormat_LinkClicked);
            // 
            // frmBatchUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 353);
            this.Controls.Add(this.linkLabelFileFormat);
            this.Controls.Add(this.btnUploadBatchData);
            this.Controls.Add(this.cbExchBatchData);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dataGridViewBatchData);
            this.Controls.Add(this.btnFetchBatchData);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.dTPickerBatch);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "frmBatchUpload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Upload";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBatchUpload_FormClosing);
            this.Load += new System.EventHandler(this.frmBatchUpload_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dTPickerBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnFetchBatchData;
        private System.Windows.Forms.DataGridView dataGridViewBatchData;
        private System.Windows.Forms.ComboBox cbExchBatchData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUploadBatchData;
        private System.Windows.Forms.LinkLabel linkLabelFileFormat;
    }
}