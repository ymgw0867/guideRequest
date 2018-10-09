namespace guideRequest
{
    partial class frmMailSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMailSet));
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.txtFromAddress = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPopPort = new System.Windows.Forms.TextBox();
            this.txtPopServerName = new System.Windows.Forms.TextBox();
            this.txtMailRecSpan = new System.Windows.Forms.TextBox();
            this.txtSmtpServerName = new System.Windows.Forms.TextBox();
            this.txtSmtpPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMailName = new System.Windows.Forms.TextBox();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDataSaveMonth = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMemo
            // 
            this.txtMemo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(143, 305);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(470, 151);
            this.txtMemo.TabIndex = 10;
            // 
            // txtFromAddress
            // 
            this.txtFromAddress.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtFromAddress.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtFromAddress.Location = new System.Drawing.Point(143, 189);
            this.txtFromAddress.Name = "txtFromAddress";
            this.txtFromAddress.Size = new System.Drawing.Size(470, 24);
            this.txtFromAddress.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtPassword.Location = new System.Drawing.Point(143, 160);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(470, 24);
            this.txtPassword.TabIndex = 5;
            // 
            // txtLogin
            // 
            this.txtLogin.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtLogin.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtLogin.Location = new System.Drawing.Point(143, 131);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(470, 24);
            this.txtLogin.TabIndex = 4;
            // 
            // txtPopPort
            // 
            this.txtPopPort.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPopPort.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtPopPort.Location = new System.Drawing.Point(143, 102);
            this.txtPopPort.Name = "txtPopPort";
            this.txtPopPort.Size = new System.Drawing.Size(51, 24);
            this.txtPopPort.TabIndex = 3;
            this.txtPopPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPopPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmtpPort_KeyPress);
            // 
            // txtPopServerName
            // 
            this.txtPopServerName.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPopServerName.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtPopServerName.Location = new System.Drawing.Point(143, 73);
            this.txtPopServerName.Name = "txtPopServerName";
            this.txtPopServerName.Size = new System.Drawing.Size(470, 24);
            this.txtPopServerName.TabIndex = 2;
            // 
            // txtMailRecSpan
            // 
            this.txtMailRecSpan.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMailRecSpan.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtMailRecSpan.Location = new System.Drawing.Point(143, 247);
            this.txtMailRecSpan.MaxLength = 4;
            this.txtMailRecSpan.Name = "txtMailRecSpan";
            this.txtMailRecSpan.Size = new System.Drawing.Size(51, 24);
            this.txtMailRecSpan.TabIndex = 8;
            this.txtMailRecSpan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMailRecSpan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmtpPort_KeyPress);
            // 
            // txtSmtpServerName
            // 
            this.txtSmtpServerName.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSmtpServerName.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSmtpServerName.Location = new System.Drawing.Point(143, 15);
            this.txtSmtpServerName.Name = "txtSmtpServerName";
            this.txtSmtpServerName.Size = new System.Drawing.Size(470, 24);
            this.txtSmtpServerName.TabIndex = 0;
            // 
            // txtSmtpPort
            // 
            this.txtSmtpPort.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSmtpPort.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSmtpPort.Location = new System.Drawing.Point(143, 44);
            this.txtSmtpPort.MaxLength = 3;
            this.txtSmtpPort.Name = "txtSmtpPort";
            this.txtSmtpPort.Size = new System.Drawing.Size(51, 24);
            this.txtSmtpPort.TabIndex = 1;
            this.txtSmtpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSmtpPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmtpPort_KeyPress);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 24);
            this.label1.TabIndex = 33;
            this.label1.Text = "SMTPサーバー";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 24);
            this.label2.TabIndex = 34;
            this.label2.Text = "SMTPポート番号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.SteelBlue;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 24);
            this.label3.TabIndex = 36;
            this.label3.Text = "POPポート番号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.SteelBlue;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 24);
            this.label4.TabIndex = 35;
            this.label4.Text = "POPサーバー";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.SteelBlue;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 24);
            this.label5.TabIndex = 37;
            this.label5.Text = "アカウント ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.SteelBlue;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 24);
            this.label6.TabIndex = 38;
            this.label6.Text = "パスワード";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.SteelBlue;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(12, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 24);
            this.label7.TabIndex = 39;
            this.label7.Text = "メールアドレス";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.SteelBlue;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 24);
            this.label8.TabIndex = 40;
            this.label8.Text = "受信間隔・秒";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.SteelBlue;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(12, 305);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 151);
            this.label9.TabIndex = 41;
            this.label9.Text = "署名";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.SteelBlue;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(12, 218);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 24);
            this.label10.TabIndex = 43;
            this.label10.Text = "アカウント名";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMailName
            // 
            this.txtMailName.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMailName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMailName.Location = new System.Drawing.Point(143, 218);
            this.txtMailName.Name = "txtMailName";
            this.txtMailName.Size = new System.Drawing.Size(470, 24);
            this.txtMailName.TabIndex = 7;
            // 
            // linkLabel4
            // 
            this.linkLabel4.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel4.Image")));
            this.linkLabel4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel4.Location = new System.Drawing.Point(463, 464);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(56, 25);
            this.linkLabel4.TabIndex = 11;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "更新";
            this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel3.Image")));
            this.linkLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.Location = new System.Drawing.Point(535, 464);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(75, 25);
            this.linkLabel3.TabIndex = 12;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "終了する";
            this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.SteelBlue;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(12, 276);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(131, 24);
            this.label11.TabIndex = 93;
            this.label11.Text = "データ保存月数";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDataSaveMonth
            // 
            this.txtDataSaveMonth.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDataSaveMonth.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtDataSaveMonth.Location = new System.Drawing.Point(143, 276);
            this.txtDataSaveMonth.MaxLength = 4;
            this.txtDataSaveMonth.Name = "txtDataSaveMonth";
            this.txtDataSaveMonth.Size = new System.Drawing.Size(51, 24);
            this.txtDataSaveMonth.TabIndex = 9;
            this.txtDataSaveMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDataSaveMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmtpPort_KeyPress);
            // 
            // frmMailSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 495);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtDataSaveMonth);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtMailName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSmtpPort);
            this.Controls.Add(this.txtMailRecSpan);
            this.Controls.Add(this.txtSmtpServerName);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.txtFromAddress);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.txtPopServerName);
            this.Controls.Add(this.txtPopPort);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmMailSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "メール設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSystem_FormClosing);
            this.Load += new System.EventHandler(this.frmSystem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.TextBox txtFromAddress;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPopPort;
        private System.Windows.Forms.TextBox txtPopServerName;
        private System.Windows.Forms.TextBox txtMailRecSpan;
        private System.Windows.Forms.TextBox txtSmtpServerName;
        private System.Windows.Forms.TextBox txtSmtpPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMailName;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDataSaveMonth;
    }
}