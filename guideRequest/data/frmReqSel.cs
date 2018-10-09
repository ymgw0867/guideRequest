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
    public partial class frmReqSel : Form
    {
        public frmReqSel()
        {
            InitializeComponent();

            adp.Fill(dts.ガイド依頼名);
            gAdp.Fill(dts.ガイド依頼対象者);
        }

        // ガイド依頼名テーブルアダプター生成
        guideDataSetTableAdapters.ガイド依頼名TableAdapter adp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
        guideDataSetTableAdapters.ガイド依頼対象者TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        // 選択された依頼番号
        public long reNum { get; set; }

        private void frmReqSel_Load(object sender, EventArgs e)
        {
            // フォーム最大サイズ
            utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小サイズ
            utility.WindowsMinSize(this, this.Width, this.Height);

            // データグリッド定義
            GridViewSetting(dg);

            // データグリッドビューにデータを表示します
            GridViewShow(dg);

            reNum = 0;
        }

        //カラム定義
        string C_ID = "col1";
        string C_Name = "col2";
        string C_Memo = "col3";
        string C_rKbn = "col4";
        string C_irainin = "col5";
        string C_rDate = "col6";
        string C_aDate = "col7";
        string C_uDate = "col8";
        string C_iDate = "col9";
        string C_dCount = "col10";
        string C_Sel = "col11";

        /// <summary>
        /// データグリッドビューの定義を行います
        /// </summary>
        /// <param name="tempDGV">データグリッドビューオブジェクト</param>
        private void GridViewSetting(DataGridView tempDGV)
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
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 321;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                DataGridViewButtonColumn bc = new DataGridViewButtonColumn();
                bc.UseColumnTextForButtonValue = true;
                bc.Text = "選択";
                tempDGV.Columns.Add(bc);
                tempDGV.Columns[0].Name = C_Sel;
                tempDGV.Columns[C_Sel].HeaderText = "";

                tempDGV.Columns.Add(C_ID, "番号");
                tempDGV.Columns.Add(C_rDate, "受付日");
                tempDGV.Columns.Add(C_irainin, "依頼元");
                tempDGV.Columns.Add(C_Name, "内容");
                tempDGV.Columns.Add(C_iDate, "依頼日");
                tempDGV.Columns.Add(C_dCount, "対象者数");
                tempDGV.Columns.Add(C_Memo, "備考");

                tempDGV.Columns[C_ID].Visible = false;
                tempDGV.Columns[C_Sel].Width = 60;
                tempDGV.Columns[C_Name].Width = 200;
                tempDGV.Columns[C_Memo].Width = 300;
                tempDGV.Columns[C_irainin].Width = 160;
                tempDGV.Columns[C_rDate].Width = 110;
                tempDGV.Columns[C_iDate].Width = 110;
                tempDGV.Columns[C_dCount].Width = 80;

                tempDGV.Columns[C_rDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_iDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_dCount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                //tempDGV.Columns[C_Memo].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

                // 編集不可とする
                tempDGV.ReadOnly = false;
                foreach (DataGridViewColumn c in tempDGV.Columns)
                {
                    if (c.Name == C_Sel)
                    {
                        tempDGV.Columns[c.Name].ReadOnly = false;
                    }
                    else
                    {
                        tempDGV.Columns[c.Name].ReadOnly = true;
                    }
                }

                // 追加行表示しない
                tempDGV.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                tempDGV.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                tempDGV.AllowUserToOrderColumns = false;

                // 列サイズ変更可
                tempDGV.AllowUserToResizeColumns = true;

                // 行サイズ変更禁止
                tempDGV.AllowUserToResizeRows = false;

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                //TAB動作
                tempDGV.StandardTab = true;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// ----------------------------------------------------------
        /// <summary>
        ///     データグリッドビューにデータを表示します </summary>
        /// <param name="tempGrid">
        ///     データグリッドビューオブジェクト名</param>
        /// ----------------------------------------------------------
        private void GridViewShow(DataGridView gv)
        {
            try
            {
                gv.RowCount = 0;
                int iX = 0;
                
                foreach (var t in dts.ガイド依頼名.Where(a => a.Getガイド依頼対象者Rows().Count(c => c.Is依頼メール送信日時Null()) > 0).OrderBy(a => a.ID))
                {
                    gv.Rows.Add();
                    gv[C_rDate, iX].Value = t.受付日時.ToShortDateString();
                    gv[C_ID, iX].Value = t.ID;

                    if (t.Is依頼日Null())
                    {
                        gv[C_iDate, iX].Value = string.Empty;
                    }
                    else
                    {
                        gv[C_iDate, iX].Value = t.依頼日.ToShortDateString();
                    }

                    gv[C_Name, iX].Value = t.依頼内容;
                    gv[C_Memo, iX].Value = t.備考;
                    gv[C_irainin, iX].Value = t.依頼元;                    
                    gv[C_dCount, iX].Value = t.Getガイド依頼対象者Rows().Count(a => a.Is依頼メール送信日時Null()).ToString();

                    iX++;
                }
                
                if (gv.RowCount > 0)
                {
                    gv.CurrentCell = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (reqSelect(e.RowIndex))
                    {
                        // 閉じる
                        this.Close();
                    }
                }
            }
        }

        private bool reqSelect(int r)
        {
            string iDate = dg[C_iDate, r].Value.ToString();
            string n = dg[C_irainin, r].Value.ToString();

            if (MessageBox.Show(iDate + " " + n + " が選択されました。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            // 依頼番号
            reNum = long.Parse(dg[C_ID, r].Value.ToString());
            return true;
        }

        private void frmReqSel_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
