using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using guideRequest.common;

namespace guideRequest.mail
{
    public partial class frmMailComment : Form
    {
        // マスター名
        string msName = "メール本文";

        // フォームモードインスタンス
        utility.formMode fMode = new utility.formMode();

        // ガイド依頼名テーブルアダプター生成
        guideDataSetTableAdapters.メール定型文TableAdapter adp = new guideDataSetTableAdapters.メール定型文TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        public frmMailComment()
        {
            InitializeComponent();

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
            GridViewShow(dg);

            // 種別コンボボックス
            for (int i = 0; i < global.shArray.Length; i++)
            {
                comboBox1.Items.Add(global.shArray[i]);
            }
            
            // 画面初期化
            DispInitial();
        }

        //カラム定義
        string colID = "col1";
        string colComment = "col2";
        string colComment2 = "col3";
        string colUDate = "col4";
        string colShubetsu = "col5";

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
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 260;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                tempDGV.Columns.Add(colID, "番号");
                tempDGV.Columns.Add(colShubetsu, "種別");
                tempDGV.Columns.Add(colComment, "前文");
                tempDGV.Columns.Add(colComment2, "後文");
                tempDGV.Columns.Add(colUDate, "更新年月日");

                tempDGV.Columns[colID].Width = 60;
                tempDGV.Columns[colShubetsu].Width = 80;
                tempDGV.Columns[colComment].Width = 300;
                tempDGV.Columns[colComment2].Width = 300;
                tempDGV.Columns[colUDate].Width = 150;

                tempDGV.Columns[colUDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

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

                foreach (var t in dts.メール定型文.OrderBy(a => a.ID))
                {
                    gv.Rows.Add();
                    gv[colID, iX].Value = t.ID;

                    if (t.種別 > 0 && t.種別 <= global.shArray.Length)
                    {
                        gv[colShubetsu, iX].Value = global.shArray[t.種別 - 1];
                    }
                    else
                    {
                        gv[colShubetsu, iX].Value = string.Empty;
                    }

                    gv[colComment, iX].Value = t.前文;
                    gv[colComment2, iX].Value = t.後文;
                    gv[colUDate, iX].Value = t.更新年月日;

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
            txtSubject.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
            txtComment.Text = string.Empty;
            txtComment2.Text = string.Empty;

            txtComment.ScrollBars = ScrollBars.Both;
            txtComment2.ScrollBars = ScrollBars.Both;
            
            linkLabel4.Visible = true;
            linkLabel2.Enabled = false;
            linkLabel1.Enabled = false;
            //linkLabel2.Visible = false;
            //linkLabel1.Visible = false;
        }

        // 登録データチェック
        private Boolean fDataCheck()
        {
            try
            {
                // 件名
                if (txtSubject.Text.Trim().Length < 1)
                {
                    txtSubject.Focus();
                    throw new Exception("件名を入力してください");
                }

                // 種別
                if (comboBox1.SelectedIndex == -1)
                {
                    comboBox1.Focus();
                    throw new Exception("定型文の種別を選択してください");
                }

                // 本文
                if (txtComment.Text.Trim().Length < 1 && txtComment2.Text.Trim().Length < 1)
                {
                    txtComment.Focus();
                    throw new Exception("前文または後文を入力してください");
                }

                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, msName + "作成", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
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
        private void ShowData(guideDataSet.メール定型文Row s)
        {
            fMode.ID = s.ID;

            txtSubject.Text = s.件名;

            if (s.種別 > 0)
            {
                comboBox1.SelectedIndex = s.種別 - 1;
            }

            txtComment.Text = s.前文;
            txtComment2.Text = s.後文;
            
            //linkLabel2.Visible = true;
            //linkLabel1.Visible = true;
            linkLabel2.Enabled = true;
            linkLabel1.Enabled = true;
        }

        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GridEnter();
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // データセットの内容をデータベースへ反映させます
            adp.Update(dts.メール定型文);

            this.Dispose();
        }

        private void frmKintaiKbn_Shown(object sender, EventArgs e)
        {
            txtComment.Focus();
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

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // フォームを閉じます
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DispInitial();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // 確認
                if (MessageBox.Show("削除してよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                // 削除
                guideDataSet.メール定型文Row r = dts.メール定型文.Single(a => a.ID == fMode.ID);
                r.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("データの削除に失敗しました" + Environment.NewLine + ex.Message);
            }
            finally
            {
                // 削除をコミット
                adp.Update(dts.メール定型文);

                // データテーブルにデータを読み込む
                adp.Fill(dts.メール定型文);

                // データグリッドビューにデータを再表示します
                GridViewShow(dg);

                // 画面データ消去
                DispInitial();
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //エラーチェック
            if (!fDataCheck()) return;

            switch (fMode.Mode)
            {
                // 新規登録
                case global.FORM_ADDMODE:

                    // 確認
                    if (MessageBox.Show("メール定型文を登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return;

                    // データセットにデータを追加します
                    dts.メール定型文.Addメール定型文Row(txtSubject.Text, txtComment.Text, txtComment2.Text, 0, comboBox1.SelectedIndex + 1, DateTime.Now, DateTime.Now);

                    break;

                // 更新処理
                case global.FORM_EDITMODE:

                    // 確認
                    if (MessageBox.Show("データを更新します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return;

                    // データセット更新
                    var r = dts.メール定型文.Single(a => a.ID == fMode.ID);

                    if (!r.HasErrors)
                    {
                        r.件名 = txtSubject.Text;
                        r.前文 = txtComment.Text;
                        r.後文 = txtComment2.Text;
                        r.種別 = comboBox1.SelectedIndex + 1;
                        r.更新年月日 = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show(fMode.ID64.ToString() + "がキー不在です：データの更新に失敗しました", "更新エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    break;

                default:
                    break;
            }

            // データベース更新
            adp.Update(dts.メール定型文);

            // データテーブルにデータを読み込む
            adp.Fill(dts.メール定型文);

            // グリッドデータ再表示
            GridViewShow(dg);

            // 画面データ消去
            DispInitial();
        }
    }
}
