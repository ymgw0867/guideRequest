using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mailReceive.common;

namespace mailReceive.mail
{
    public partial class frmMailComSelect : Form
    {
        // マスター名
        string msName = "メール定型文選択";

        // フォームモードインスタンス
        utility.formMode fMode = new utility.formMode();

        // ガイド依頼名テーブルアダプター生成
        guideDataSetTableAdapters.メール定型文TableAdapter adp = new guideDataSetTableAdapters.メール定型文TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        // 選択された定型文のID
        public int mID { get; set; }

        int _cKbn = 0;

        public frmMailComSelect(int cKbn)
        {
            InitializeComponent();

            _cKbn = cKbn;

            // データテーブルにデータを読み込む
            adp.Fill(dts.メール定型文);
        }

        private void frm_Load(object sender, EventArgs e)
        {
            // フォーム最大サイズ
            utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小サイズ
            utility.WindowsMinSize(this, this.Width, this.Height);

            // データグリッド定義
            GridViewSetting(dg);

            // データグリッドビューにデータを表示します
            GridViewShow(dg, _cKbn);

            // 画面初期化
            DispInitial();
        }

        //カラム定義
        string colSel = "col0";
        string colID = "col1";
        string colComment = "col2";

        /// ---------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        /// ---------------------------------------------------------------------
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
                //tempDGV.RowTemplate.Height = 200;
                tempDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

                // 全体の高さ
                //tempDGV.Height = 536;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                DataGridViewButtonColumn bc = new DataGridViewButtonColumn();
                bc.UseColumnTextForButtonValue = true;
                bc.Text = "選択";
                tempDGV.Columns.Add(bc);
                tempDGV.Columns[0].Name = colSel;
                tempDGV.Columns[colSel].HeaderText = "";

                tempDGV.Columns.Add(colID, "番号");
                tempDGV.Columns.Add(colComment, "内容");

                tempDGV.Columns[colSel].Width = 60;
                tempDGV.Columns[colID].Width = 60;
                tempDGV.Columns[colComment].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tempDGV.Columns[colComment].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

                // 編集不可とする
                tempDGV.ReadOnly = true;

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
        private void GridViewShow(DataGridView gv, int kbn)
        {
            try
            {
                gv.RowCount = 0;
                int iX = 0;

                foreach (var t in dts.メール定型文.Where(a => a.種別 == kbn).OrderByDescending(a => a.ID))
                {
                    gv.Rows.Add();
                    gv[colID, iX].Value = t.ID;
                    gv[colComment, iX].Value = t.前文 + Environment.NewLine + t.後文;

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

        /// ----------------------------------------------------------
        /// <summary>
        ///     画面の初期化 </summary>
        /// ----------------------------------------------------------
        private void DispInitial()
        {
            mID = 0;
        }

        /// ---------------------------------------------------------------------
        /// <summary>
        ///     グリッドビュー行選択時処理 </summary>
        /// ---------------------------------------------------------------------
        private void GridEnter()
        {
            int rx = dg.SelectedRows[0].Index;
            
            // IDを取得
            int rID = utility.StrtoZero(dg[colID, rx].Value.ToString());

            // 対象となるデータテーブルROWを取得します
            guideDataSet.メール定型文Row sQuery = dts.メール定型文.Single(a => a.ID == rID);
            
            if (!sQuery.HasErrors)
            {
                // モードステータスを「編集モード」にします
                fMode.Mode = global.FORM_EDITMODE;
            }
            else
            {
                MessageBox.Show(dg[0, rx].Value.ToString() + "がキー不在です：データの読み込みに失敗しました", "データ取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        
        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GridEnter();
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
            mID = 0;

            // フォームを閉じます
            this.Close();
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    mID = int.Parse(dg[colID, e.RowIndex].Value.ToString());

                    // 閉じる
                    this.Close();
                }
            }
        }
    }
}
