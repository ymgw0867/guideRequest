using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using guideRequest.common;

namespace guideRequest.data
{
    public partial class frmRequestList : Form
    {
        // マスター名
        string msName = "ガイド依頼";

        // フォームモードインスタンス
        utility.formMode fMode = new utility.formMode();

        // ガイド依頼名テーブルアダプター生成
        guideDataSetTableAdapters.ガイド依頼名TableAdapter adp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
        guideDataSetTableAdapters.ガイド依頼対象者TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();
        guideDataSetTableAdapters.会員情報TableAdapter mAdp = new guideDataSetTableAdapters.会員情報TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        public frmRequestList()
        {
            InitializeComponent();

            // データテーブルにデータを読み込む
            adp.Fill(dts.ガイド依頼名);
            gAdp.Fill(dts.ガイド依頼対象者);
            mAdp.Fill(dts.会員情報);
        }

        private void frm_Load(object sender, EventArgs e)
        {
            // フォーム最大サイズ
            utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小サイズ
            utility.WindowsMinSize(this, this.Width, this.Height);

            // データグリッド定義
            GridViewSetting(dg);
            GridViewSetting2(dg2);

            // データグリッドビューにデータを表示します
            GridViewShow(dg);

            lblID.Text = string.Empty;
            lblIraiMoto.Text = string.Empty;
            lblDate.Text = string.Empty;
        }

        // カラム定義
        string C_ID = "col1";
        string C_Name = "col2";
        string C_Memo = "col3";
        string C_rKbn = "col4";
        string C_irainin = "col5";
        string C_rDate = "col6";
        string C_aDate = "col7";
        string C_uDate = "col8";
        string C_iDate = "col9";
        string C_Status = "col10";
        string C_Select = "col11";
        string C_kName = "col12";
        string C_kNum = "col13";
        string C_MailAddress = "col14";
        string C_address = "col15";
        string C_zipCode = "col16";

        ///--------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------------------
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
                tempDGV.Height = 262;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                tempDGV.Columns.Add(C_ID, "番号");
                tempDGV.Columns.Add(C_rDate, "受付日");
                tempDGV.Columns.Add(C_irainin, "依頼元");
                tempDGV.Columns.Add(C_Name, "内容");
                tempDGV.Columns.Add(C_iDate, "稼働日");
                tempDGV.Columns.Add(C_Status, "ステータス");

                tempDGV.Columns[C_ID].Visible = false;
                tempDGV.Columns[C_rDate].Width = 110;
                tempDGV.Columns[C_irainin].Width = 200;
                tempDGV.Columns[C_Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tempDGV.Columns[C_iDate].Width = 110;
                tempDGV.Columns[C_Status].Width = 100;

                tempDGV.Columns[C_rDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_iDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                
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

        ///--------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------------------
        private void GridViewSetting2(DataGridView tempDGV)
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
                tempDGV.Height = 302;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                tempDGV.Columns.Add(C_Select, "選考");
                tempDGV.Columns.Add(C_kNum, "番号");
                tempDGV.Columns.Add(C_kName, "氏名");
                tempDGV.Columns.Add(C_rDate, "返信受信");
                tempDGV.Columns.Add(C_iDate, "結果通知");
                tempDGV.Columns.Add(C_zipCode, "〒");
                tempDGV.Columns.Add(C_address, "住所");

                tempDGV.Columns[C_Select].Width = 50;
                tempDGV.Columns[C_kNum].Width = 90;
                tempDGV.Columns[C_kName].Width = 130;
                tempDGV.Columns[C_rDate].Width = 136;
                tempDGV.Columns[C_iDate].Width = 136;
                tempDGV.Columns[C_zipCode].Width = 80;
                tempDGV.Columns[C_address].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[C_Select].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_kNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_rDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_iDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[C_zipCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

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

                foreach (var t in dts.ガイド依頼名.Where(a => a.依頼終了 == global.FLGON).OrderByDescending(a => a.受付日時))
                {
                    gv.Rows.Add();
                    gv[C_rDate, iX].Value = t.受付日時.ToShortDateString();
                    gv[C_irainin, iX].Value = t.依頼元;
                    gv[C_Name, iX].Value = t.依頼内容;
                    gv[C_ID, iX].Value = t.ID;

                    if (t.Is依頼日Null())
                    {
                        gv[C_iDate, iX].Value = string.Empty;
                    }
                    else
                    {
                        gv[C_iDate, iX].Value = t.依頼日.ToShortDateString();
                    }

                    if (t.依頼終了 == global.FLGOFF)
                    {
                        gv[C_Status, iX].Value = "受付中";
                    }
                    else
                    {
                        gv[C_Status, iX].Value = "終了";
                    }

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
        
        /// ---------------------------------------------------------------------
        /// <summary>
        ///     グリッドビュー行選択時処理 </summary>
        /// ---------------------------------------------------------------------
        private void GridEnter()
        {
            //string msgStr;
            int rx = dg.SelectedRows[0].Index;

            //// 選択確認
            //msgStr = "";
            //msgStr += dg[C_rDate, rx].Value.ToString() + " " + dg[C_irainin, rx].Value.ToString() + "が選択されました。よろしいですか？";

            //if (MessageBox.Show(msgStr, "選択", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            //{
            //    return;
            //}

            // IDを取得
            long rID = long.Parse(dg[C_ID, rx].Value.ToString());

            // ピックアップ対象者表示
            ShowData(rID, dg2);
        }

        /// -------------------------------------------------------
        /// <summary>
        ///     ピックアップ対象者を表示する </summary>
        /// <param name="sId">
        ///     依頼番号</param>
        /// <param name="gv">
        ///     データグリッドビューオブジェクト</param>
        /// -------------------------------------------------------
        private void ShowData(long sId, DataGridView gv)
        {
            try
            {
                var s = dts.ガイド依頼名.Single(a => a.ID == sId);

                lblID.Text = s.ID.ToString();
                lblIraiMoto.Text = s.依頼元;
                lblDate.Text = "稼働日：" + s.依頼日.ToShortDateString();

                gv.RowCount = 0;
                int iX = 0;

                foreach (var t in dts.ガイド依頼対象者.Where(a => a.依頼番号 == sId)
                    .OrderByDescending(a => a.結果))
                {
                    gv.Rows.Add();

                    if (t.結果 == global.FLGON)
                    {
                        gv[C_Select, iX].Value = "○";
                    }
                    else
                    {
                        gv[C_Select, iX].Value = "-";
                    }

                    if (t.Is返信受信日時Null())
                    {
                        gv[C_rDate, iX].Value = string.Empty;
                    }
                    else
                    {
                        gv[C_rDate, iX].Value = t.返信受信日時.ToString("yyyy/MM/dd HH:mm:ss");
                    }

                    if (t.Is結果送信日時Null())
                    {
                        gv[C_iDate, iX].Value = string.Empty;
                    }
                    else
                    {
                        gv[C_iDate, iX].Value = t.結果送信日時.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    
                    gv[C_kNum, iX].Value = t.会員番号.ToString();


                    if (t.会員情報Row != null)
                    {
                        gv[C_kName, iX].Value = t.会員情報Row.氏名;
                        gv[C_zipCode, iX].Value = t.会員情報Row.郵便番号;
                        gv[C_address, iX].Value = t.会員情報Row.都道府県 + t.会員情報Row.住所1.Replace(" ", "") + t.会員情報Row.住所2.Replace(" ", "") + t.会員情報Row.住所3.Replace(" ", "");
                    }
                    else
                    {
                        gv[C_kName, iX].Value = string.Empty;
                        gv[C_zipCode, iX].Value = string.Empty;
                        gv[C_address, iX].Value = string.Empty;
                    }
                    
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

        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
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

            // 後片付け
            this.Dispose();
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
            GridEnter();
        }

        private void dg_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // フォームを閉じます
            this.Close();
        }
    }
}
