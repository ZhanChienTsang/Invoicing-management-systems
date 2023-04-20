namespace ChienTsang
{
    partial class PurOrd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurOrd));
            this.dgvM = new System.Windows.Forms.DataGridView();
            this.dgvD = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnTop = new System.Windows.Forms.ToolStripButton();
            this.btnPrior = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSeek = new System.Windows.Forms.ToolStripButton();
            this.btnSeekDo = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.label5 = new System.Windows.Forms.Label();
            this.LbTbName = new System.Windows.Forms.Label();
            this.Leave = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvD)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvM
            // 
            this.dgvM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvM.Location = new System.Drawing.Point(14, 270);
            this.dgvM.Name = "dgvM";
            this.dgvM.Size = new System.Drawing.Size(835, 220);
            this.dgvM.TabIndex = 5;
            this.dgvM.Leave += new System.EventHandler(this.dgvM_Leave);
            // 
            // dgvD
            // 
            this.dgvD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvD.Location = new System.Drawing.Point(14, 505);
            this.dgvD.Name = "dgvD";
            this.dgvD.Size = new System.Drawing.Size(835, 195);
            this.dgvD.TabIndex = 18;
            this.dgvD.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvD_CellDoubleClick);
            this.dgvD.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvD_CellEndEdit);
            this.dgvD.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvD_PreviewKeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(363, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 38;
            this.label3.Text = "記錄數";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label2.Font = new System.Drawing.Font("新細明體", 12.25F);
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(190, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 17);
            this.label2.TabIndex = 39;
            this.label2.Text = "　";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(235)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(14, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 195);
            this.panel1.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(20, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(316, 21);
            this.label4.TabIndex = 62;
            this.label4.Text = "※主檔完成輸入後,請點選tBox末欄！";
            this.label4.Visible = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnClose,
            this.Leave});
            this.toolStrip2.Location = new System.Drawing.Point(682, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(183, 54);
            this.toolStrip2.TabIndex = 60;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnOpen
            // 
            this.btnOpen.AutoSize = false;
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = global::ChienTsang.Properties.Resources.OpenFile_48;
            this.btnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(52, 52);
            this.btnOpen.Text = "開  檔";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::ChienTsang.Properties.Resources.CloseFile_48;
            this.btnClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(52, 52);
            this.btnClose.Text = "關  檔";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTop,
            this.btnPrior,
            this.btnNext,
            this.btnLast,
            this.toolStripSeparator1,
            this.btnAdd,
            this.btnDel,
            this.btnEdit,
            this.btnCancel,
            this.btnUpdate,
            this.toolStripSeparator2,
            this.btnSeek,
            this.btnSeekDo,
            this.btnRefresh,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(14, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(667, 54);
            this.toolStrip1.TabIndex = 59;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnTop
            // 
            this.btnTop.AutoSize = false;
            this.btnTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTop.Image = ((System.Drawing.Image)(resources.GetObject("btnTop.Image")));
            this.btnTop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(52, 52);
            this.btnTop.Text = "第一筆";
            this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
            // 
            // btnPrior
            // 
            this.btnPrior.AutoSize = false;
            this.btnPrior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrior.Image = ((System.Drawing.Image)(resources.GetObject("btnPrior.Image")));
            this.btnPrior.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPrior.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrior.Name = "btnPrior";
            this.btnPrior.Size = new System.Drawing.Size(52, 52);
            this.btnPrior.Text = "上一筆";
            this.btnPrior.Click += new System.EventHandler(this.btnPrior_Click);
            // 
            // btnNext
            // 
            this.btnNext.AutoSize = false;
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(52, 52);
            this.btnNext.Text = "下一筆";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.AutoSize = false;
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(52, 52);
            this.btnLast.Text = "最末筆";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 54);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = false;
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::ChienTsang.Properties.Resources.add_48;
            this.btnAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(52, 52);
            this.btnAdd.Text = "新  增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.AutoSize = false;
            this.btnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDel.Image = global::ChienTsang.Properties.Resources.delete_48;
            this.btnDel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(52, 52);
            this.btnDel.Text = "刪  除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = false;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(230)))));
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = global::ChienTsang.Properties.Resources.update_48;
            this.btnEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(52, 52);
            this.btnEdit.Text = "編  修";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = false;
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancel.Image = global::ChienTsang.Properties.Resources.cancel_48;
            this.btnCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 52);
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.AutoSize = false;
            this.btnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUpdate.Image = global::ChienTsang.Properties.Resources.correct_48;
            this.btnUpdate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(52, 52);
            this.btnUpdate.Text = "寫  入";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 54);
            // 
            // btnSeek
            // 
            this.btnSeek.AutoSize = false;
            this.btnSeek.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSeek.Image = ((System.Drawing.Image)(resources.GetObject("btnSeek.Image")));
            this.btnSeek.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSeek.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSeek.Name = "btnSeek";
            this.btnSeek.Size = new System.Drawing.Size(52, 52);
            this.btnSeek.Text = "查  詢";
            this.btnSeek.Click += new System.EventHandler(this.btnSeek_Click);
            // 
            // btnSeekDo
            // 
            this.btnSeekDo.AutoSize = false;
            this.btnSeekDo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSeekDo.Image = ((System.Drawing.Image)(resources.GetObject("btnSeekDo.Image")));
            this.btnSeekDo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSeekDo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSeekDo.Name = "btnSeekDo";
            this.btnSeekDo.Size = new System.Drawing.Size(52, 52);
            this.btnSeekDo.Text = "執  行";
            this.btnSeekDo.Click += new System.EventHandler(this.btnSeekDo_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSize = false;
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(52, 52);
            this.btnRefresh.Text = "重  置";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 54);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(30, 670);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(444, 21);
            this.label5.TabIndex = 63;
            this.label5.Text = "※請直接在明細檔的FGNO欄位點二下,作型號選擇！";
            this.label5.Visible = false;
            // 
            // LbTbName
            // 
            this.LbTbName.AutoSize = true;
            this.LbTbName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.LbTbName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbTbName.ForeColor = System.Drawing.Color.MediumBlue;
            this.LbTbName.Location = new System.Drawing.Point(868, 27);
            this.LbTbName.Name = "LbTbName";
            this.LbTbName.Size = new System.Drawing.Size(80, 16);
            this.LbTbName.TabIndex = 70;
            this.LbTbName.Text = "TableName";
            // 
            // Leave
            // 
            this.Leave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Leave.Image = global::ChienTsang.Properties.Resources.go_out_48;
            this.Leave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.Leave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Leave.Name = "Leave";
            this.Leave.Size = new System.Drawing.Size(52, 51);
            this.Leave.Text = "toolStripButton1";
            this.Leave.Click += new System.EventHandler(this.Leave_Click);
            // 
            // PurOrd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 711);
            this.Controls.Add(this.LbTbName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvD);
            this.Controls.Add(this.dgvM);
            this.Name = "PurOrd";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FDouble_FormClosing);
            this.Load += new System.EventHandler(this.FDouble_Load);
            this.SizeChanged += new System.EventHandler(this.FDouble_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FDouble_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvD)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvM;
        private System.Windows.Forms.DataGridView dgvD;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnTop;
        private System.Windows.Forms.ToolStripButton btnPrior;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDel;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSeek;
        private System.Windows.Forms.ToolStripButton btnSeekDo;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LbTbName;
        private System.Windows.Forms.ToolStripButton Leave;
    }
}

