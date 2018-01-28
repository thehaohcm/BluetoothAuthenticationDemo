namespace BluetoothAuthenticationDemo
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.nameDevice = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.typeDevice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.statusDevice = new System.Windows.Forms.Label();
            this.scandevicebtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.sttDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathFolderDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autoLockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.folderlockDataSet4 = new BluetoothAuthenticationDemo.folderlockDataSet4();
            this.folderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.removeEleBtn = new System.Windows.Forms.Button();
            this.unlockBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.forgetPassBtn = new System.Windows.Forms.Button();
            this.sttDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathFolderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sttDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.onoffAutoLockBtn = new System.Windows.Forms.Button();
            this.folderTableAdapter1 = new BluetoothAuthenticationDemo.folderlockDataSet4TableAdapters.folderTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderlockDataSet4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Device\'s name: ";
            // 
            // nameDevice
            // 
            this.nameDevice.AutoSize = true;
            this.nameDevice.Location = new System.Drawing.Point(138, 9);
            this.nameDevice.Name = "nameDevice";
            this.nameDevice.Size = new System.Drawing.Size(47, 20);
            this.nameDevice.TabIndex = 3;
            this.nameDevice.Text = "None";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type of Device: ";
            // 
            // typeDevice
            // 
            this.typeDevice.AutoSize = true;
            this.typeDevice.Location = new System.Drawing.Point(138, 49);
            this.typeDevice.Name = "typeDevice";
            this.typeDevice.Size = new System.Drawing.Size(47, 20);
            this.typeDevice.TabIndex = 5;
            this.typeDevice.Text = "None";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Available : ";
            // 
            // statusDevice
            // 
            this.statusDevice.AutoSize = true;
            this.statusDevice.Location = new System.Drawing.Point(138, 90);
            this.statusDevice.Name = "statusDevice";
            this.statusDevice.Size = new System.Drawing.Size(48, 20);
            this.statusDevice.TabIndex = 7;
            this.statusDevice.Text = "False";
            // 
            // scandevicebtn
            // 
            this.scandevicebtn.Location = new System.Drawing.Point(12, 113);
            this.scandevicebtn.Name = "scandevicebtn";
            this.scandevicebtn.Size = new System.Drawing.Size(301, 47);
            this.scandevicebtn.TabIndex = 1;
            this.scandevicebtn.Text = "Scan Device...";
            this.scandevicebtn.UseVisualStyleBackColor = true;
            this.scandevicebtn.Click += new System.EventHandler(this.scandevicebtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sttDataGridViewTextBoxColumn2,
            this.pathFolderDataGridViewTextBoxColumn1,
            this.autoLockDataGridViewTextBoxColumn,
            this.lockedDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.folderBindingSource1;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridView1.Location = new System.Drawing.Point(16, 245);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(643, 217);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // sttDataGridViewTextBoxColumn2
            // 
            this.sttDataGridViewTextBoxColumn2.DataPropertyName = "stt";
            this.sttDataGridViewTextBoxColumn2.HeaderText = "stt";
            this.sttDataGridViewTextBoxColumn2.Name = "sttDataGridViewTextBoxColumn2";
            this.sttDataGridViewTextBoxColumn2.ReadOnly = true;
            this.sttDataGridViewTextBoxColumn2.Width = 30;
            // 
            // pathFolderDataGridViewTextBoxColumn1
            // 
            this.pathFolderDataGridViewTextBoxColumn1.DataPropertyName = "pathFolder";
            this.pathFolderDataGridViewTextBoxColumn1.HeaderText = "pathFolder";
            this.pathFolderDataGridViewTextBoxColumn1.Name = "pathFolderDataGridViewTextBoxColumn1";
            this.pathFolderDataGridViewTextBoxColumn1.ReadOnly = true;
            this.pathFolderDataGridViewTextBoxColumn1.Width = 250;
            // 
            // autoLockDataGridViewTextBoxColumn
            // 
            this.autoLockDataGridViewTextBoxColumn.DataPropertyName = "Auto Lock";
            this.autoLockDataGridViewTextBoxColumn.HeaderText = "Auto Lock";
            this.autoLockDataGridViewTextBoxColumn.Name = "autoLockDataGridViewTextBoxColumn";
            this.autoLockDataGridViewTextBoxColumn.ReadOnly = true;
            this.autoLockDataGridViewTextBoxColumn.Width = 50;
            // 
            // lockedDataGridViewTextBoxColumn
            // 
            this.lockedDataGridViewTextBoxColumn.DataPropertyName = "Locked";
            this.lockedDataGridViewTextBoxColumn.HeaderText = "Locked";
            this.lockedDataGridViewTextBoxColumn.Name = "lockedDataGridViewTextBoxColumn";
            this.lockedDataGridViewTextBoxColumn.ReadOnly = true;
            this.lockedDataGridViewTextBoxColumn.Width = 50;
            // 
            // folderBindingSource1
            // 
            this.folderBindingSource1.DataMember = "folder";
            this.folderBindingSource1.DataSource = this.folderlockDataSet4;
            // 
            // folderlockDataSet4
            // 
            this.folderlockDataSet4.DataSetName = "folderlockDataSet4";
            this.folderlockDataSet4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(178, 468);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 47);
            this.button2.TabIndex = 5;
            this.button2.Text = "Add Folder...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // removeEleBtn
            // 
            this.removeEleBtn.Location = new System.Drawing.Point(503, 468);
            this.removeEleBtn.Name = "removeEleBtn";
            this.removeEleBtn.Size = new System.Drawing.Size(156, 47);
            this.removeEleBtn.TabIndex = 7;
            this.removeEleBtn.Text = "Remove Element";
            this.removeEleBtn.UseVisualStyleBackColor = true;
            this.removeEleBtn.Click += new System.EventHandler(this.removeEleBtn_Click);
            // 
            // unlockBtn
            // 
            this.unlockBtn.Location = new System.Drawing.Point(16, 468);
            this.unlockBtn.Name = "unlockBtn";
            this.unlockBtn.Size = new System.Drawing.Size(156, 47);
            this.unlockBtn.TabIndex = 4;
            this.unlockBtn.Text = "Lock/Unlock";
            this.unlockBtn.UseVisualStyleBackColor = true;
            this.unlockBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // forgetPassBtn
            // 
            this.forgetPassBtn.Location = new System.Drawing.Point(12, 164);
            this.forgetPassBtn.Name = "forgetPassBtn";
            this.forgetPassBtn.Size = new System.Drawing.Size(301, 45);
            this.forgetPassBtn.TabIndex = 2;
            this.forgetPassBtn.Text = "Forgot / Lost Device";
            this.forgetPassBtn.UseVisualStyleBackColor = true;
            this.forgetPassBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // sttDataGridViewTextBoxColumn
            // 
            this.sttDataGridViewTextBoxColumn.DataPropertyName = "stt";
            this.sttDataGridViewTextBoxColumn.HeaderText = "stt";
            this.sttDataGridViewTextBoxColumn.Name = "sttDataGridViewTextBoxColumn";
            // 
            // pathFolderDataGridViewTextBoxColumn
            // 
            this.pathFolderDataGridViewTextBoxColumn.DataPropertyName = "pathFolder";
            this.pathFolderDataGridViewTextBoxColumn.HeaderText = "pathFolder";
            this.pathFolderDataGridViewTextBoxColumn.Name = "pathFolderDataGridViewTextBoxColumn";
            // 
            // sttDataGridViewTextBoxColumn1
            // 
            this.sttDataGridViewTextBoxColumn1.DataPropertyName = "stt";
            this.sttDataGridViewTextBoxColumn1.HeaderText = "stt";
            this.sttDataGridViewTextBoxColumn1.Name = "sttDataGridViewTextBoxColumn1";
            // 
            // onoffAutoLockBtn
            // 
            this.onoffAutoLockBtn.Location = new System.Drawing.Point(341, 468);
            this.onoffAutoLockBtn.Name = "onoffAutoLockBtn";
            this.onoffAutoLockBtn.Size = new System.Drawing.Size(156, 47);
            this.onoffAutoLockBtn.TabIndex = 6;
            this.onoffAutoLockBtn.Text = "On/Off Auto Lock";
            this.onoffAutoLockBtn.UseVisualStyleBackColor = true;
            this.onoffAutoLockBtn.Click += new System.EventHandler(this.onoffAutoLockBtn_Click);
            // 
            // folderTableAdapter1
            // 
            this.folderTableAdapter1.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(358, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(301, 45);
            this.button1.TabIndex = 8;
            this.button1.Text = "Change Device";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(358, 166);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(301, 45);
            this.button3.TabIndex = 9;
            this.button3.Text = "Change Secret Question";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(674, 530);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.onoffAutoLockBtn);
            this.Controls.Add(this.unlockBtn);
            this.Controls.Add(this.removeEleBtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.scandevicebtn);
            this.Controls.Add(this.statusDevice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.typeDevice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nameDevice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.forgetPassBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Lock Folder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderlockDataSet4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label nameDevice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label typeDevice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label statusDevice;
        private System.Windows.Forms.Button scandevicebtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button removeEleBtn;
        private System.Windows.Forms.Button unlockBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button forgetPassBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathFolderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttDataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button onoffAutoLockBtn;
        private System.Windows.Forms.BindingSource folderBindingSource;
        private folderlockDataSet4 folderlockDataSet4;
        private System.Windows.Forms.BindingSource folderBindingSource1;
        private folderlockDataSet4TableAdapters.folderTableAdapter folderTableAdapter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sttDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathFolderDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn autoLockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockedDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
    }
}

