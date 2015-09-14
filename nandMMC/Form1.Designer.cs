namespace nandMMC
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
            this.incfixed = new System.Windows.Forms.CheckBox();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.Dumper = new System.ComponentModel.BackgroundWorker();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Flasher = new System.ComponentModel.BackgroundWorker();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.LbSize = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Devicelist = new nandMMC.SafeComboBox();
            this.dumpbtn = new nandMMC.SafeButton();
            this.updatebtn = new nandMMC.SafeButton();
            this.flashbtn = new nandMMC.SafeButton();
            this.abortbtn = new nandMMC.SafeButton();
            this.working = new nandMMC.LoadingCircle();
            this.status = new nandMMC.SafeToolStripLabel();
            this.tbNor = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.size.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // incfixed
            // 
            this.incfixed.AutoSize = true;
            this.incfixed.BackColor = System.Drawing.SystemColors.Control;
            this.incfixed.Location = new System.Drawing.Point(10, 91);
            this.incfixed.Name = "incfixed";
            this.incfixed.Size = new System.Drawing.Size(115, 17);
            this.incfixed.TabIndex = 1;
            this.incfixed.Text = "Todos Dispositivos";
            this.incfixed.UseVisualStyleBackColor = false;
            this.incfixed.CheckedChanged += new System.EventHandler(this.IncfixedCheckedChanged);
            // 
            // bw
            // 
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BwDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BwRunWorkerCompleted);
            // 
            // Dumper
            // 
            this.Dumper.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DumperDoWork);
            this.Dumper.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DumperRunWorkerCompleted);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "bin";
            this.sfd.FileName = "Dump.bin";
            this.sfd.Title = "Select where to save your dump";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.statusStrip1.Location = new System.Drawing.Point(0, 276);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(557, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Flasher
            // 
            this.Flasher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.FlasherDoWork);
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "bin";
            this.ofd.FileName = "Nandflash.bin";
            this.ofd.Filter = "Xbox 360 Binary Files|*.bin|Xbox 360 ECC files|*.ecc|All Files|*.*";
            this.ofd.Title = "Select NAND to flash";
            // 
            // LbSize
            // 
            this.LbSize.AutoSize = true;
            this.LbSize.Location = new System.Drawing.Point(17, 35);
            this.LbSize.Name = "LbSize";
            this.LbSize.Size = new System.Drawing.Size(55, 13);
            this.LbSize.TabIndex = 0;
            this.LbSize.Text = "Tamanho:";
            // 
            // size
            // 
            this.size.BackColor = System.Drawing.SystemColors.Control;
            this.size.Controls.Add(this.LbSize);
            this.size.Location = new System.Drawing.Point(349, 12);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(196, 177);
            this.size.TabIndex = 0;
            this.size.TabStop = false;
            this.size.Text = "Informações";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.Controls.Add(this.tbNor);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 79);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Devicelist);
            this.groupBox2.Controls.Add(this.dumpbtn);
            this.groupBox2.Controls.Add(this.updatebtn);
            this.groupBox2.Controls.Add(this.incfixed);
            this.groupBox2.Controls.Add(this.flashbtn);
            this.groupBox2.Controls.Add(this.abortbtn);
            this.groupBox2.Location = new System.Drawing.Point(13, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 118);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Leitura e Gravação";
            // 
            // Devicelist
            // 
            this.Devicelist.BackColor = System.Drawing.SystemColors.Control;
            this.Devicelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Devicelist.FormattingEnabled = true;
            this.Devicelist.Location = new System.Drawing.Point(10, 19);
            this.Devicelist.Name = "Devicelist";
            this.Devicelist.Size = new System.Drawing.Size(306, 21);
            this.Devicelist.Sorted = true;
            this.Devicelist.TabIndex = 1;
            this.Devicelist.SelectedIndexChanged += new System.EventHandler(this.DevicelistSelectedIndexChanged);
            this.Devicelist.TextChanged += new System.EventHandler(this.DevicelistTextChanged);
            // 
            // dumpbtn
            // 
            this.dumpbtn.Location = new System.Drawing.Point(10, 46);
            this.dumpbtn.Name = "dumpbtn";
            this.dumpbtn.Size = new System.Drawing.Size(91, 23);
            this.dumpbtn.TabIndex = 2;
            this.dumpbtn.Text = "Dump";
            this.dumpbtn.UseVisualStyleBackColor = true;
            this.dumpbtn.Click += new System.EventHandler(this.DumpbtnClick);
            // 
            // updatebtn
            // 
            this.updatebtn.BackgroundImage = global::nandMMC.Properties.Resources.refresh;
            this.updatebtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.updatebtn.Location = new System.Drawing.Point(179, 89);
            this.updatebtn.Name = "updatebtn";
            this.updatebtn.Size = new System.Drawing.Size(24, 23);
            this.updatebtn.TabIndex = 0;
            this.updatebtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.updatebtn.UseVisualStyleBackColor = true;
            this.updatebtn.Click += new System.EventHandler(this.UpdatebtnClick);
            // 
            // flashbtn
            // 
            this.flashbtn.Enabled = false;
            this.flashbtn.Location = new System.Drawing.Point(107, 46);
            this.flashbtn.Name = "flashbtn";
            this.flashbtn.Size = new System.Drawing.Size(96, 23);
            this.flashbtn.TabIndex = 2;
            this.flashbtn.Text = "Flash";
            this.flashbtn.UseVisualStyleBackColor = true;
            this.flashbtn.Click += new System.EventHandler(this.FlashbtnClick);
            // 
            // abortbtn
            // 
            this.abortbtn.Enabled = false;
            this.abortbtn.Location = new System.Drawing.Point(209, 46);
            this.abortbtn.Name = "abortbtn";
            this.abortbtn.Size = new System.Drawing.Size(107, 23);
            this.abortbtn.TabIndex = 2;
            this.abortbtn.Text = "Cancelar";
            this.abortbtn.UseVisualStyleBackColor = true;
            this.abortbtn.Click += new System.EventHandler(this.AbortbtnClick);
            // 
            // working
            // 
            this.working.Active = false;
            this.working.BackColor = System.Drawing.SystemColors.Control;
            this.working.Color = System.Drawing.Color.Black;
            this.working.Enabled = false;
            this.working.InnerCircleRadius = 10;
            this.working.Location = new System.Drawing.Point(475, 195);
            this.working.Name = "working";
            this.working.NumberSpoke = 15;
            this.working.OuterCircleRadius = 30;
            this.working.RotationSpeed = 100;
            this.working.Size = new System.Drawing.Size(70, 71);
            this.working.SpokeThickness = 2;
            this.working.TabIndex = 3;
            this.working.Text = "loadingCircle1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(122, 17);
            this.status.Text = "Waiting for user input";
            // 
            // tbNor
            // 
            this.tbNor.Location = new System.Drawing.Point(10, 19);
            this.tbNor.Name = "tbNor";
            this.tbNor.ReadOnly = true;
            this.tbNor.Size = new System.Drawing.Size(221, 20);
            this.tbNor.TabIndex = 0;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(237, 17);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Abrir";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(557, 298);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.working);
            this.Controls.Add(this.size);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "nandMMC v";
            this.size.ResumeLayout(false);
            this.size.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox incfixed;
        private SafeComboBox Devicelist;
        private SafeButton dumpbtn;
        private System.ComponentModel.BackgroundWorker bw;
        private System.ComponentModel.BackgroundWorker Dumper;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private SafeToolStripLabel status;
        private SafeButton abortbtn;
        private SafeButton flashbtn;
        private System.ComponentModel.BackgroundWorker Flasher;
        private System.Windows.Forms.OpenFileDialog ofd;
        private LoadingCircle working;
        private System.Windows.Forms.GroupBox size;
        private SafeButton updatebtn;
        private System.Windows.Forms.Label LbSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox tbNor;
    }
}

