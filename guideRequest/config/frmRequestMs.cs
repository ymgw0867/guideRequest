using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using guideRequest.common;

namespace guideRequest.config
{
    public partial class frmRequestMs : Form
    {
        // マスター名
        string msName = "ガイド依頼";

        // フォームモードインスタンス
        utility.formMode fMode = new utility.formMode();

        // ガイド依頼名テーブルアダプター生成
        guideDataSetTableAdapters.ガイド依頼名TableAdapter adp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        public frmRequestMs()
        {
            InitializeComponent();

            // データテーブルにデータを読み込む
            adp.Fill(dts.ガイド依頼名);
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
            GridViewShow(dg);

            // 画面初期化
            DispInitial();
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
                tempDGV.Columns.Add(C_ID, "番号");
                tempDGV.Columns.Add(C_rDate, "受付日");
                tempDGV.Columns.Add(C_irainin, "依頼元");
                tempDGV.Columns.Add(C_Name, "内容");
                tempDGV.Columns.Add(C_iDate, "依頼日");
                tempDGV.Columns.Add(C_rKbn, "依頼区分");
                tempDGV.Columns.Add(C_Memo, "備考");
                tempDGV.Columns.Add(C_aDate, "登録年月日");
                tempDGV.Columns.Add(C_uDate, "更新年月日");

                tempDGV.Columns[C_ID].Visible = false;
                tempDGV.Columns[C_Name].Width = 200;
                tempDGV.Columns[C_Memo].Width = 300;
                tempDGV.Columns[C_rKbn].Width = 100;
                tempDGV.Columns[C_irainin].Width = 160;
                tempDGV.Columns[C_rDate].Width = 110;
                tempDGV.Columns[C_iDate].Width = 110;
                tempDGV.Columns[C_aDate].Width = 150;
                tempDGV.Columns[C_uDate].Width = 150;

                tempDGV.Columns[C_rDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_iDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_rKbn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                //tempDGV.Columns[C_Memo].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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

                foreach (var t in dts.ガイド依頼名.OrderByDescending(a => a.受付日時))
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
                    gv[C_rKbn, iX].Value = t.依頼区分;
                    gv[C_irainin, iX].Value = t.依頼元;
                    gv[C_aDate, iX].Value = t.登録年月日;
                    gv[C_uDate, iX].Value = t.更新年月日;

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
            fMode.Mode = global.FORM_ADDMODE;
            cmbKbn.SelectedIndex = 0;
            txtName.Text = string.Empty;
            txtMemo.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            btnUpdate.Enabled = true;
            btnDel.Enabled = false;
            btnClear.Enabled = false;

            cmbIrai.Focus();

            // 依頼元コンボボックス
            cmbIrai.Items.Clear();

            var s = dts.ガイド依頼名.Select(a => new
            {
                依頼元 = a.依頼元
            })
            .Distinct()
            .OrderBy(a => a.依頼元);

            foreach (var t in s)
            {
                cmbIrai.Items.Add(t.依頼元);
            }

            cmbIrai.SelectedIndex = -1;
            cmbIrai.Text = string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //エラーチェック
            if (!fDataCheck()) return;

            switch (fMode.Mode)
            {
                // 新規登録
                case global.FORM_ADDMODE:

                    // 確認
                    if (MessageBox.Show(txtName.Text + "を登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) 
                        return;

                    // データセットにデータを追加します
                    //dts.ガイド依頼名.Addガイド依頼名Row(txtName.Text, txtMemo.Text, cmbKbn.SelectedIndex, cmbIrai.Text, dateTimePicker2.Value, dateTimePicker1.Value, DateTime.Now, DateTime.Now);
                    
                    break;

                // 更新処理
                case global.FORM_EDITMODE:

                    // 確認
                    if (MessageBox.Show("データを更新します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) 
                        return;

                    // データセット更新
                    var r = dts.ガイド依頼名.Single(a => a.ID == fMode.ID64);
                    
                    if (!r.HasErrors)
                    {
                        r.依頼元 = cmbIrai.Text;
                        r.依頼内容 = txtName.Text;
                        r.依頼区分 = cmbKbn.SelectedIndex;
                        r.備考 = txtMemo.Text;
                        r.依頼日 = dateTimePicker2.Value;
                        r.受付日時 = dateTimePicker1.Value;
                        r.更新年月日 = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show(fMode.ID64.ToString() + "がキー不在です：データの更新に失敗しました","更新エラー",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }

                    break;

                default:
                    break;
            }

            // グリッドデータ再表示
            GridViewShow(dg);

            // 画面データ消去
            DispInitial();      
        }

        // 登録データチェック
        private Boolean fDataCheck()
        {
            try
            {
                // 依頼元チェック
                if (cmbIrai.Text.Trim().Length < 1)
                {
                    cmbIrai.Focus();
                    throw new Exception("依頼元を入力してください");
                }

                // 依頼内容チェック
                if (txtName.Text.Trim().Length < 1)
                {
                    txtName.Focus();
                    throw new Exception("内容を入力してください");
                }
                
                // 依頼区分
                if (cmbKbn.SelectedIndex == -1)
                {
                    cmbKbn.Focus();
                    throw new Exception("依頼区分を選択してください");
                }

                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, msName + "保守", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        /// ---------------------------------------------------------------------
        /// <summary>
        ///     グリッドビュー行選択時処理 </summary>
        /// ---------------------------------------------------------------------
        private void GridEnter()
        {
            string msgStr;
            int rx = dg.SelectedRows[0].Index;

            // 選択確認
            msgStr = "";
            msgStr += dg[C_rDate, rx].Value.ToString() + " " + dg[C_irainin, rx].Value.ToString() + "が選択されました。よろしいですか？";

            if (MessageBox.Show(msgStr, "選択", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            // IDを取得
            int rID = utility.StrtoZero(dg[C_ID, rx].Value.ToString());

            // 対象となるデータテーブルROWを取得します
            guideDataSet.ガイド依頼名Row sQuery = (guideDataSet.ガイド依頼名Row)dts.ガイド依頼名.Single(a => a.ID == rID);
            
            if (!sQuery.HasErrors)
            {
                // 編集画面に表示
                ShowData(sQuery);

                // モードステータスを「編集モード」にします
                fMode.Mode = global.FORM_EDITMODE;
            }
            else
            {
                MessageBox.Show(dg[0, rx].Value.ToString() + "がキー不在です：データの読み込みに失敗しました", "データ取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// -------------------------------------------------------
        /// <summary>
        ///     マスターの内容を画面に表示する </summary>
        /// <param name="sTemp">
        ///     マスターインスタンス</param>
        /// -------------------------------------------------------
        private void ShowData(guideDataSet.ガイド依頼名Row s)
        {
            fMode.ID64 = s.ID;
            cmbKbn.SelectedIndex = s.依頼区分;
            cmbIrai.Text = s.依頼元;
            txtName.Text = s.依頼内容;
            txtMemo.Text = s.備考;

            if (!s.Is依頼日Null())
            {
                dateTimePicker2.Value = s.依頼日;
            }

            dateTimePicker1.Value = s.受付日時;

            btnDel.Enabled = true;
            btnClear.Enabled = true;
        }

        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GridEnter();
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
            // フォームを閉じます
            this.Close();
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // データセットの内容をデータベースへ反映させます
            adp.Update(dts.ガイド依頼名);

            this.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DispInitial();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                // 確認
                if (MessageBox.Show("削除してよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                // 削除
                guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == fMode.ID64);
                r.Delete();                
            }
            catch (Exception ex)
            {
                MessageBox.Show("データの削除に失敗しました" + Environment.NewLine + ex.Message);
            }
            finally
            {
                // 削除をコミット
                adp.Update(dts.ガイド依頼名);

                // データテーブルにデータを読み込む
                adp.Fill(dts.ガイド依頼名);

                // データグリッドビューにデータを再表示します
                GridViewShow(dg);

                // 画面データ消去
                DispInitial();
            }
        }

        private void frmKintaiKbn_Shown(object sender, EventArgs e)
        {
            cmbIrai.Focus();
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
    }
}
