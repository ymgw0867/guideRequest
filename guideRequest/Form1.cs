using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using guideRequest.config;
using guideRequest.data;

namespace guideRequest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            utility.WindowsMaxSize(this, this.Width, this.Height);
            utility.WindowsMinSize(this, this.Width, this.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRequestMs frm = new frmRequestMs();
            frm.ShowDialog();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //mail.frmMailReceive frm = new mail.frmMailReceive();
            //frm.ShowDialog();
            //this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPickup frm = new frmPickup();
            frm.ShowDialog();
            this.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            data.frmRequestList frm = new frmRequestList();
            frm.ShowDialog();
            this.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            mail.frmMailLog frm = new mail.frmMailLog();
            frm.ShowDialog();
            this.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            mail.frmMailComment frm = new mail.frmMailComment();
            frm.ShowDialog();
            this.Show();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmMailSet frm = new frmMailSet();
            frm.ShowDialog();
            this.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 保存期間経過したデータの削除
            dataDelete();

            // メッセージID削除
            mIdDelete();

            // 閉じる
            this.Close();
        }

        /// <summary>
        ///     保存期間経過したデータの削除 </summary>
        private void dataDelete()
        {
            guideDataSet dts = new guideDataSet();
            guideDataSetTableAdapters.メール設定TableAdapter adp = new guideDataSetTableAdapters.メール設定TableAdapter();
            guideDataSetTableAdapters.ガイド依頼名TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
            guideDataSetTableAdapters.ガイド依頼対象者TableAdapter mAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();

            this.Cursor = Cursors.WaitCursor;

            // データ読み込み
            adp.Fill(dts.メール設定);
            gAdp.Fill(dts.ガイド依頼名);
            mAdp.Fill(dts.ガイド依頼対象者);
            
            DateTime dt;

            if (dts.メール設定.Any(a => a.ID == guideRequest.common.global.mailKey))
            {
                var s = dts.メール設定.Single(a => a.ID == guideRequest.common.global.mailKey);

                if (s.データ保存月数 != 0)
                {
                    // 基準年月日
                    dt = DateTime.Today.AddMonths(-1 * s.データ保存月数);

                    // ガイド依頼対象者データ削除
                    foreach (var g in dts.ガイド依頼対象者
                           .Where(a => a.ガイド依頼名Row != null && a.ガイド依頼名Row.登録年月日 < dt) )
                    {
                        g.Delete();
                    }

                    // ガイド依頼名データ削除
                    foreach (var t in dts.ガイド依頼名.Where(a => a.登録年月日 < dt))
                    {
                        // ガイド依頼データ削除
                        t.Delete();
                    }

                    // データベース更新
                    gAdp.Update(dts.ガイド依頼名);
                    mAdp.Update(dts.ガイド依頼対象者);
                }
            }

            this.Cursor = Cursors.Default;
        }


        /// <summary>
        ///     メッセージIDデータを削除します </summary>
        private void mIdDelete()
        {
            guideDataSet dts = new guideDataSet();
            guideDataSetTableAdapters.messageIDTableAdapter adp = new guideDataSetTableAdapters.messageIDTableAdapter();
            
            this.Cursor = Cursors.WaitCursor;

            // データ読み込み
            adp.Fill(dts.messageID);
            
            DateTime dt;
            
            int delM = Properties.Settings.Default.mIdDelSpan;

            if (delM != 0)
            {
                // 基準年月日
                dt = DateTime.Today.AddMonths(-1 * delM);

                // メッセージIDデータ削除
                foreach (var g in dts.messageID.Where(a => a.受信日時 < dt))
                {
                    g.Delete();
                }

                // データベース更新
                adp.Update(dts.messageID);
            }

            this.Cursor = Cursors.Default;
        }
    }
}
