using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guideRequest.data
{
    public partial class frmSendPW : Form
    {
        public frmSendPW()
        {
            InitializeComponent();
        }

        public bool pwAuthen { get; set; }

        private void btnPw_Click(object sender, EventArgs e)
        {
        }

        private bool pwAuth()
        {
            if (txtPw.Text == Properties.Settings.Default.pw)
            {
                // 有効ステータス
                return true;    
            }
            else
            {
                MessageBox.Show("パスワードの認証に失敗しました。", "認証エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // 無効ステータス
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 無効ステータス
            pwAuthen = false;

            // 閉じる
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pwAuthen = pwAuth();

            // 閉じる
            this.Close();
        }
    }
}
