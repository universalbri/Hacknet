using System.Windows.Forms;

namespace NETHelper
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblTerminalHeader = new System.Windows.Forms.Label();
            this.lblNetMap = new System.Windows.Forms.Label();
            this.lblRAM = new System.Windows.Forms.Label();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.picGear = new System.Windows.Forms.PictureBox();
            this.picX = new System.Windows.Forms.PictureBox();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.webDisplay = new System.Windows.Forms.WebBrowser();
            this.txtAppData = new AlphaBlendControls.ABTextBox();
            this.txtNetMap = new AlphaBlendControls.ABTextBox();
            this.txtBoxHistory = new AlphaBlendControls.ABTextBox();
            this.txtBoxCommand = new AlphaBlendControls.ABTextBox();
            this.txtDisplay = new AlphaBlendControls.ABTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picGear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTerminalHeader
            // 
            this.lblTerminalHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(184)))));
            this.lblTerminalHeader.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerminalHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblTerminalHeader.Location = new System.Drawing.Point(924, 395);
            this.lblTerminalHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblTerminalHeader.Name = "lblTerminalHeader";
            this.lblTerminalHeader.Padding = new System.Windows.Forms.Padding(2);
            this.lblTerminalHeader.Size = new System.Drawing.Size(740, 13);
            this.lblTerminalHeader.TabIndex = 17;
            this.lblTerminalHeader.Text = "TERMINAL";
            // 
            // lblNetMap
            // 
            this.lblNetMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(184)))));
            this.lblNetMap.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetMap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblNetMap.Location = new System.Drawing.Point(924, 28);
            this.lblNetMap.Margin = new System.Windows.Forms.Padding(0);
            this.lblNetMap.Name = "lblNetMap";
            this.lblNetMap.Padding = new System.Windows.Forms.Padding(2);
            this.lblNetMap.Size = new System.Drawing.Size(740, 13);
            this.lblNetMap.TabIndex = 20;
            this.lblNetMap.Text = "netMap v1.7";
            // 
            // lblRAM
            // 
            this.lblRAM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(184)))));
            this.lblRAM.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRAM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblRAM.Location = new System.Drawing.Point(1669, 28);
            this.lblRAM.Margin = new System.Windows.Forms.Padding(0);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Padding = new System.Windows.Forms.Padding(2);
            this.lblRAM.Size = new System.Drawing.Size(249, 13);
            this.lblRAM.TabIndex = 22;
            this.lblRAM.Text = "RAM";
            // 
            // lblDisplay
            // 
            this.lblDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(184)))));
            this.lblDisplay.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblDisplay.Location = new System.Drawing.Point(2, 28);
            this.lblDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Padding = new System.Windows.Forms.Padding(2);
            this.lblDisplay.Size = new System.Drawing.Size(917, 13);
            this.lblDisplay.TabIndex = 24;
            this.lblDisplay.Text = "DISPLAY";
            // 
            // picGear
            // 
            this.picGear.BackColor = System.Drawing.Color.Transparent;
            this.picGear.Image = ((System.Drawing.Image)(resources.GetObject("picGear.Image")));
            this.picGear.Location = new System.Drawing.Point(27, 4);
            this.picGear.Name = "picGear";
            this.picGear.Size = new System.Drawing.Size(19, 19);
            this.picGear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGear.TabIndex = 25;
            this.picGear.TabStop = false;
            // 
            // picX
            // 
            this.picX.Image = ((System.Drawing.Image)(resources.GetObject("picX.Image")));
            this.picX.Location = new System.Drawing.Point(4, 5);
            this.picX.Name = "picX";
            this.picX.Size = new System.Drawing.Size(18, 18);
            this.picX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picX.TabIndex = 26;
            this.picX.TabStop = false;
            this.picX.Click += new System.EventHandler(this.picX_Click);
            // 
            // picSave
            // 
            this.picSave.Image = ((System.Drawing.Image)(resources.GetObject("picSave.Image")));
            this.picSave.Location = new System.Drawing.Point(52, 4);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(19, 19);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 27;
            this.picSave.TabStop = false;
            // 
            // picDisplay
            // 
            this.picDisplay.BackColor = System.Drawing.Color.Transparent;
            this.picDisplay.Location = new System.Drawing.Point(4, 44);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(911, 1029);
            this.picDisplay.TabIndex = 28;
            this.picDisplay.TabStop = false;
            this.picDisplay.Visible = false;
            // 
            // webDisplay
            // 
            this.webDisplay.Location = new System.Drawing.Point(4, 43);
            this.webDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this.webDisplay.Name = "webDisplay";
            this.webDisplay.ScriptErrorsSuppressed = true;
            this.webDisplay.Size = new System.Drawing.Size(911, 1030);
            this.webDisplay.TabIndex = 29;
            this.webDisplay.Visible = false;
            // 
            // txtAppData
            // 
            this.txtAppData.BackAlpha = 15;
            this.txtAppData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(75)))));
            this.txtAppData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAppData.Enabled = false;
            this.txtAppData.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppData.ForeColor = System.Drawing.Color.White;
            this.txtAppData.IsPasswordChar = false;
            this.txtAppData.Location = new System.Drawing.Point(1671, 43);
            this.txtAppData.Margin = new System.Windows.Forms.Padding(2);
            this.txtAppData.Multiline = true;
            this.txtAppData.Name = "txtAppData";
            this.txtAppData.ReadOnly = true;
            this.txtAppData.Size = new System.Drawing.Size(243, 1030);
            this.txtAppData.TabIndex = 21;
            this.txtAppData.TabStop = false;
            this.txtAppData.Enter += new System.EventHandler(this.txtMemory_Enter);
            // 
            // txtNetMap
            // 
            this.txtNetMap.BackAlpha = 50;
            this.txtNetMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(75)))));
            this.txtNetMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNetMap.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNetMap.ForeColor = System.Drawing.Color.White;
            this.txtNetMap.IsPasswordChar = false;
            this.txtNetMap.Location = new System.Drawing.Point(926, 43);
            this.txtNetMap.Margin = new System.Windows.Forms.Padding(2);
            this.txtNetMap.Multiline = true;
            this.txtNetMap.Name = "txtNetMap";
            this.txtNetMap.ReadOnly = true;
            this.txtNetMap.Size = new System.Drawing.Size(734, 350);
            this.txtNetMap.TabIndex = 19;
            this.txtNetMap.TabStop = false;
            // 
            // txtBoxHistory
            // 
            this.txtBoxHistory.BackAlpha = 15;
            this.txtBoxHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(75)))));
            this.txtBoxHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxHistory.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxHistory.ForeColor = System.Drawing.Color.White;
            this.txtBoxHistory.IsPasswordChar = false;
            this.txtBoxHistory.Location = new System.Drawing.Point(926, 410);
            this.txtBoxHistory.Margin = new System.Windows.Forms.Padding(2);
            this.txtBoxHistory.Multiline = true;
            this.txtBoxHistory.Name = "txtBoxHistory";
            this.txtBoxHistory.ReadOnly = true;
            this.txtBoxHistory.Size = new System.Drawing.Size(734, 644);
            this.txtBoxHistory.TabIndex = 16;
            this.txtBoxHistory.TabStop = false;
            this.txtBoxHistory.Enter += new System.EventHandler(this.txtBoxHistory_Enter);
            this.txtBoxHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxHistory_MouseDown);
            // 
            // txtBoxCommand
            // 
            this.txtBoxCommand.BackAlpha = 15;
            this.txtBoxCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(75)))));
            this.txtBoxCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxCommand.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCommand.ForeColor = System.Drawing.Color.White;
            this.txtBoxCommand.IsPasswordChar = false;
            this.txtBoxCommand.Location = new System.Drawing.Point(926, 1055);
            this.txtBoxCommand.Margin = new System.Windows.Forms.Padding(0);
            this.txtBoxCommand.Multiline = true;
            this.txtBoxCommand.Name = "txtBoxCommand";
            this.txtBoxCommand.Size = new System.Drawing.Size(734, 18);
            this.txtBoxCommand.TabIndex = 15;
            this.txtBoxCommand.Text = "This is a test";
            this.txtBoxCommand.WordWrap = false;
            this.txtBoxCommand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxCommand_KeyPress);
            this.txtBoxCommand.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxCommand_MouseDown);
            this.txtBoxCommand.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtBoxCommand_MouseMove);
            this.txtBoxCommand.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtBoxCommand_MouseUp);
            // 
            // txtDisplay
            // 
            this.txtDisplay.BackAlpha = 15;
            this.txtDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(75)))));
            this.txtDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplay.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDisplay.ForeColor = System.Drawing.Color.White;
            this.txtDisplay.IsPasswordChar = false;
            this.txtDisplay.Location = new System.Drawing.Point(4, 43);
            this.txtDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.ReadOnly = true;
            this.txtDisplay.Size = new System.Drawing.Size(911, 1030);
            this.txtDisplay.TabIndex = 23;
            this.txtDisplay.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.ControlBox = false;
            this.Controls.Add(this.webDisplay);
            this.Controls.Add(this.picSave);
            this.Controls.Add(this.picX);
            this.Controls.Add(this.picGear);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.lblRAM);
            this.Controls.Add(this.txtAppData);
            this.Controls.Add(this.lblNetMap);
            this.Controls.Add(this.txtNetMap);
            this.Controls.Add(this.lblTerminalHeader);
            this.Controls.Add(this.txtBoxHistory);
            this.Controls.Add(this.txtBoxCommand);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.picDisplay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "test";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMain_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.picGear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void AbTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private Label lblTerminalHeader;
        private AlphaBlendControls.ABTextBox txtBoxHistory;
        internal AlphaBlendControls.ABTextBox txtBoxCommand;
        private Label lblNetMap;
        private AlphaBlendControls.ABTextBox txtNetMap;
        private Label lblRAM;
        private AlphaBlendControls.ABTextBox txtAppData;
        private Label lblDisplay;
        private PictureBox picGear;
        private PictureBox picX;
        private PictureBox picSave;
        private PictureBox picDisplay;
        private AlphaBlendControls.ABTextBox txtDisplay;
        private WebBrowser webDisplay;
    }
}

