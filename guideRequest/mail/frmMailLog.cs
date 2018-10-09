using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using guideRequest.common;

namespace guideRequest.mail
{
    public partial class frmMailLog : Form
    {
        public frmMailLog()
        {
            InitializeComponent();
        }

        guideDataSetTableAdapters.メール送受信記録TableAdapter adp = new guideDataSetTableAdapters.メール送受信記録TableAdapter();
        guideDataSet dts = new guideDataSet();

        string cDate = "col1";
        string cAddress = "col2";
        string cSubject = "col3";
        string cBody = "col4";

        string txtDefault = "宛先・差出人の検索";
        Color defauleColor = SystemColors.WindowText;
        Color grayText = Color.Gray;


        ///---------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///---------------------------------------------------------------------
        private void dgSetting(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する
                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("Meiryo UI", 9, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 522;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                //各列幅指定
                tempDGV.Columns.Add(cDate, "送信日時");
                tempDGV.Columns.Add(cAddress, "差出人");
                tempDGV.Columns.Add(cSubject, "件名");
                tempDGV.Columns.Add(cBody, "内容");

                tempDGV.Columns[cDate].Width = 160;
                tempDGV.Columns[cAddress].Width = 300;
                tempDGV.Columns[cSubject].Width = 300;
                tempDGV.Columns[cBody].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = false;

                // 編集不可とする
                tempDGV.ReadOnly = true;

                // 追加行表示しない
                tempDGV.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                tempDGV.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                tempDGV.AllowUserToOrderColumns = false;

                // 列サイズの変更
                tempDGV.AllowUserToResizeColumns = true;

                // 行サイズ変更禁止
                tempDGV.AllowUserToResizeRows = false;

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMailLog_Load(object sender, EventArgs e)
        {
            utility.WindowsMaxSize(this, this.Width, this.Height);
            utility.WindowsMinSize(this, this.Width, this.Height);

            // データグリッドビュー定義
            dgSetting(dg);

            // データ読み込み
            adp.Fill(dts.メール送受信記録);

            // コンボボックス初期値
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            // 検索テキストボックス
            txtsAddress.Text = txtDefault;
            txtsAddress.ForeColor = grayText;
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     データグリッドに送受信記録を表示します </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        /// <param name="sIdx">
        ///     Rowsインデックス</param>
        ///-------------------------------------------------------------------------
        private void showData(DataGridView g, int sIdx, string sAdd)
        {
            g.Rows.Clear();

            int iX = 0;

            var s = dts.メール送受信記録.OrderByDescending(a => a.日時);
            
            if (sIdx == 0)
            {
                // 受信メールのみ
                s = s.Where(a => a.送受信区分 == global.MLLOG_REC).OrderByDescending(a => a.日時);
                g.Columns[cAddress].HeaderText = "差出人";
            }
            else if (sIdx == 1)
            {
                // 送信メールのみ
                s = s.Where(a => a.送受信区分 == global.MLLOG_SEND).OrderByDescending(a => a.日時);
                g.Columns[cAddress].HeaderText = "宛先";
            }

            foreach (var t in s)
            {
                string gAddress = string.Empty;

                // データグリッドヘッダー変更
                if (t.送受信区分 == global.MLLOG_SEND)
                {
                    gAddress = t.受信者 + " " + t.受信アドレス;
                }
                else if (t.送受信区分 == global.MLLOG_REC)
                {
                    gAddress = t.送信者 + " " + t.送信アドレス;
                }

                // 宛先/差出人検索
                if (!gAddress.Contains(sAdd))
                {
                    // 該当なし
                    continue;
                }

                // データグリッドビューに表示
                g.Rows.Add();

                g[cDate, iX].Value = t.日時.ToString("yyyy/MM/dd HH:ss:dd (ddd)");
                g[cAddress, iX].Value = gAddress;
                g[cSubject, iX].Value = t.件名;
                g[cBody, iX].Value = t.本文;

                iX++;
            }

            if (g.Rows.Count > 0)
            {
                g.CurrentCell = null;
                lblCnt.Text = g.Rows.Count.ToString("#,##0") + " 件";
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridDataShow();
        }

        private void txtsAddress_TextChanged(object sender, EventArgs e)
        {
            gridDataShow();
        }

        private void gridDataShow()
        {
            string sTxt = string.Empty;

            // 検索文字列
            if (txtsAddress.Text == txtDefault)
            {
                sTxt = string.Empty;
            }
            else
            {
                sTxt = txtsAddress.Text;
            }

            // データ抽出
            showData(dg, comboBox1.SelectedIndex, sTxt);
        }


        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sDate = dg[cDate, e.RowIndex].Value.ToString();
            string sAddres = dg[cAddress, e.RowIndex].Value.ToString();
            string sSubject = dg[cSubject, e.RowIndex].Value.ToString();
            string sBody = dg[cBody, e.RowIndex].Value.ToString();

            // メール画面表示
            frmMailBody frm = new frmMailBody(sDate, sAddres, sSubject, sBody);
            frm.ShowDialog();
        }

        private void txtsAddress_Enter(object sender, EventArgs e)
        {
            if (txtsAddress.Text == txtDefault)
            {
                txtsAddress.Text = string.Empty;
                txtsAddress.ForeColor = defauleColor;
            }
        }

        private void txtsAddress_Leave(object sender, EventArgs e)
        {
            if (txtsAddress.Text.Length <= 0 || txtsAddress.Text == txtDefault)
            {
                txtsAddress.Text = txtDefault;
                txtsAddress.ForeColor = grayText;
            }
        }
    }
}
