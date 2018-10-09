using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqToExcel;
using guideRequest.common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;

namespace guideRequest.data
{
    public partial class frmPickup : Form
    {
        public frmPickup()
        {
            InitializeComponent();

            // データテーブルにデータを読み込む
            adp.Fill(dts.ガイド依頼名);
            kAdp.Fill(dts.会員稼働予定);
            gAdp.Fill(dts.ガイド依頼対象者);
            sAdp.Fill(dts.会員情報);
            mAdp.Fill(dts.メール設定);
            cAdp.Fill(dts.メール定型文);
            lAdp.Fill(dts.メール送受信記録);
        }
        
        // テーブルアダプター生成
        guideDataSetTableAdapters.ガイド依頼名TableAdapter adp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
        guideDataSetTableAdapters.会員稼働予定TableAdapter kAdp = new guideDataSetTableAdapters.会員稼働予定TableAdapter();
        guideDataSetTableAdapters.ガイド依頼対象者TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();
        guideDataSetTableAdapters.会員情報TableAdapter sAdp = new guideDataSetTableAdapters.会員情報TableAdapter();
        guideDataSetTableAdapters.メール設定TableAdapter mAdp = new guideDataSetTableAdapters.メール設定TableAdapter();
        guideDataSetTableAdapters.メール定型文TableAdapter cAdp = new guideDataSetTableAdapters.メール定型文TableAdapter();
        guideDataSetTableAdapters.メール送受信記録TableAdapter lAdp = new guideDataSetTableAdapters.メール送受信記録TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        const string TAGOK = "0"; 
        const string TAGCANCEL = "1";

        //カラム定義
        string C_ID = "col1";
        string C_Name = "col2";
        string C_jfgDate = "col3";
        string C_Tel = "col4";
        string C_Fax = "col5";
        string C_Address = "col6";
        string C_Mail = "col7";
        string C_Furi = "col8";
        string C_Key = "col9";

        // 選択依頼番号
        long selNum = 0;

        /// -----------------------------------------------------------------------------
        /// <summary> 
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        /// -----------------------------------------------------------------------------
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
                tempDGV.Height = 361;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;

                //各列幅指定
                DataGridViewCheckBoxColumn ck = new DataGridViewCheckBoxColumn();
                tempDGV.Columns.Add(ck);
                tempDGV.Columns[0].Name = "C_Check";
                tempDGV.Columns[0].HeaderText = "Mail";
                tempDGV.Columns.Add(C_ID, "組合員番号");
                tempDGV.Columns.Add(C_Name, "氏名");
                tempDGV.Columns.Add(C_Furi, "フリガナ");
                tempDGV.Columns.Add(C_jfgDate, "加入年");
                tempDGV.Columns.Add(C_Tel, "TEL");
                tempDGV.Columns.Add(C_Fax, "FAX");
                tempDGV.Columns.Add(C_Mail, "メールアドレス");
                tempDGV.Columns.Add(C_Address, "住所");
                tempDGV.Columns.Add(C_Key, "sID");

                tempDGV.Columns[C_Key].Visible = false;

                tempDGV.Columns["C_Check"].Width = 50;
                tempDGV.Columns[C_ID].Width = 100;
                tempDGV.Columns[C_Name].Width = 120;
                tempDGV.Columns[C_Furi].Width = 100;
                tempDGV.Columns[C_jfgDate].Width = 100;
                tempDGV.Columns[C_Tel].Width = 120;
                tempDGV.Columns[C_Fax].Width = 120;
                tempDGV.Columns[C_Mail].Width = 220;
                tempDGV.Columns[C_Address].Width = 300;

                tempDGV.Columns[C_jfgDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                //tempDGV.Columns[C_Memo].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

                // 編集不可とする
                tempDGV.ReadOnly = false;
                foreach (DataGridViewColumn  c in tempDGV.Columns)
                {
                    if (c.Name == "C_Check")
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

        private void frmPickup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Tag.ToString() == TAGCANCEL)
            {
                if (MessageBox.Show("処理を中止します。よろしいですか", "中止", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    // フォームを閉じるのをキャンセルする
                    e.Cancel = true;
                    return;
                }
                else
                {
                    return;
                }
            }

            // 後片付け
            this.Dispose();
        }

        private void frmPickup_Load(object sender, EventArgs e)
        {
            utility.WindowsMaxSize(this, this.Width, this.Height);
            utility.WindowsMinSize(this, this.Width, this.Height);

            // フォームタグ値初期化
            this.Tag = TAGCANCEL;

            // グリッドビュー書式設定
            GridViewSetting(dg1);

            // 画面の初期化
            DispInitial();

            radioButton1.Checked = true;
            linkLabel1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Tag = TAGOK; 
        }

        /// ----------------------------------------------------------
        /// <summary>
        ///     画面の初期化 </summary>
        /// ----------------------------------------------------------
        private void DispInitial()
        {
            cmbKbn.SelectedIndex = 0;
            txtName.Text = string.Empty;
            txtMemo.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker3.Value = DateTime.Today;

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
            
            linkLabel4.Visible = false;

            // メール内容
            txtFrom.Text = string.Empty;
            txtCc.Text = string.Empty;
            txtBcc.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtText.Text = string.Empty;

            txtFrom.Enabled = false;
            txtCc.Enabled = false;
            txtBcc.Enabled = false;
            txtSubject.Enabled = false;
            txtText.Enabled = false;
            checkBox1.Checked = true;
            checkBox1.Enabled = false;

            txtText.ScrollBars = ScrollBars.Both;
            
            linkLabel5.Visible = false;    // メール送信ボタン
            linkLabel6.Visible = false;    // テスト送信ボタン

            // ピックアップ操作ボタンを有効にします
            button1.Enabled = true;
            linkLabel2.Visible = true;

            // グリッドビュークリア
            dg1.Rows.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pFile = openPickupXls();

            if (pFile!= string.Empty)
            {
                lblPickupFile.Text = pFile;
            }
        }

        ///--------------------------------------------------------------------------------
        /// <summary>
        ///     ピックアップリスト（エクセルシート）の選択 </summary>
        /// <returns>
        ///     ピックアップリストパス</returns>
        ///--------------------------------------------------------------------------------
        private string openPickupXls()
        {
            DialogResult ret;

            //ダイアログボックスの初期設定
            openFileDialog1.Title = "ピックアップリスト選択";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Excelブック(*.xls;*.xlsx)|*.xls;*.xlsx|全てのファイル(*.*)|*.*";

            //ダイアログボックスの表示
            ret = openFileDialog1.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.Cancel)
            {
                return string.Empty;
            }

            if (MessageBox.Show(openFileDialog1.FileName + Environment.NewLine + " が選択されました。よろしいですか?", "呼び出しシート名確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return string.Empty;
            }

            return openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!errCheck())
            {
                return;
            }

            if (MessageBox.Show("対象者抽出を実行しますか？","実行確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // 対象者抽出
            searchData(lblPickupFile.Text, dateTimePicker2.Value, dateTimePicker3.Value);
        }

        ///---------------------------------------------------------------------------------------
        /// <summary>
        ///     対象者表示 </summary>
        /// <param name="sID">
        ///     依頼番号</param>
        ///----------------------------------------------------------------------------------------
        private void pickUpDataList(long sID)
        {
            dg1.RowCount = 0;
            int iX = 0;

            foreach (var t in dts.ガイド依頼対象者.Where(a => a.依頼番号 == sID && a.Is依頼メール送信日時Null()).OrderBy(a => a.ID))
            {
                dg1.Rows.Add();

                dg1["C_Check", iX].Value = true;
                dg1[C_ID, iX].Value = t.会員番号.ToString();
                dg1[C_Name, iX].Value = t.会員情報Row.氏名;
                dg1[C_Furi, iX].Value = t.会員情報Row.フリガナ;
                dg1[C_jfgDate, iX].Value = t.会員情報Row.JFG加入年;
                dg1[C_Tel, iX].Value = t.会員情報Row.自宅電話番号;
                dg1[C_Fax, iX].Value = t.会員情報Row.自宅FAX番号;
                dg1[C_Mail, iX].Value = t.メールアドレス;
                dg1[C_Address, iX].Value = t.会員情報Row.都道府県 + t.会員情報Row.住所1.Trim() + t.会員情報Row.住所2.Trim() + t.会員情報Row.住所3.Trim();
                dg1[C_Key, iX].Value = t.ID.ToString();

                iX++;
            }

            if (dg1.RowCount > 0)
            {
                dg1.CurrentCell = null;

                // メール作成ボタン
                linkLabel4.Visible = true;
            }
        }

        private bool errCheck()
        {
            if (cmbIrai.Text.Trim() == string.Empty)
            {
                MessageBox.Show("依頼元を指定してください","確認",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                cmbIrai.Focus();
                return false;
            }

            // 2016/02/25 稼働期間
            if (dateTimePicker2.Value > dateTimePicker3.Value)
            {
                MessageBox.Show("稼働期間が正しくありません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dateTimePicker3.Focus();
                return false;
            }
            
            if (lblPickupFile.Text == string.Empty)
            {
                MessageBox.Show("ピックアップリストを選択してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                button1.Focus();
                return false;
            }
            
            return true;
        }
        
        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼対象者抽出処理 </summary>
        /// <param name="pFile">
        ///     ピックアップリストパス</param>
        /// <param name="iDt">
        ///     依頼日</param>
        ///----------------------------------------------------------------------------------
        private void searchData(string pFile, DateTime iDt, DateTime iDt2)
        {
            // 依頼番号(ID)を取得
            DateTime dt = DateTime.Now;
            selNum = long.Parse((dt.Year - 2000).ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0'));
            
            // 依頼日（期間） : 2016/02/25
            DateTime sDt = DateTime.Parse(iDt.ToShortDateString());
            DateTime sDt2 = DateTime.Parse(iDt2.ToShortDateString());

            //マウスポインタを待機にする
            this.Cursor = Cursors.WaitCursor;

            Excel.Application oXls = new Excel.Application();

            Excel.Workbook oXlsBook = (Excel.Workbook)(oXls.Workbooks.Open(pFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                           Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                           Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                           Type.Missing, Type.Missing));

            Excel.Worksheet oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[1];

            Excel.Range dRng;
            Excel.Range[] rng = new Microsoft.Office.Interop.Excel.Range[2];

            int dCnt = 0;

            // 列番号
            const int colHotel = 9;
            const int colSonota1 = 10;
            const int colSonota2 = 11;
            const int colNum = 1;
            const int colMail = 8;

            try
            {
                // 読み込み開始行
                int fromRow = 2;

                // 利用領域行数を取得
                int toRow = oxlsSheet.UsedRange.Rows.Count;

                // エクセルシートの行を順次読み込む
                for (int i = fromRow; i <= toRow; i++)
                {
                    string hCode = string.Empty;

                    // 2016/02/02
                    //dRng = (Excel.Range)oxlsSheet.Cells[i, colHotel];
                    //hCode = dRng.Text.ToString().Trim();

                    //// 対象者か？
                    //if (hCode == string.Empty)
                    //{
                    //    continue;
                    //}

                    if (cmbKbn.SelectedIndex == 0)  // 依頼区分：ホテル　2016/02/01
                    {
                        // ホテル業務列の値を取得します
                        dRng = (Excel.Range)oxlsSheet.Cells[i, colHotel];
                        hCode = dRng.Text.ToString().Trim();

                        // 対象者か？
                        if (hCode == string.Empty)
                        {
                            continue;
                        }
                    }
                    else if (cmbKbn.SelectedIndex == 1)  // 依頼区分：その他　2016/02/01
                    {
                        // 団体インセンティブ列の値を取得します
                        dRng = (Excel.Range)oxlsSheet.Cells[i, colSonota1];
                        hCode = dRng.Text.ToString().Trim();

                        // 対象者か？
                        if (hCode == string.Empty)
                        {
                            // 普通の観光列の値を取得します
                            dRng = (Excel.Range)oxlsSheet.Cells[i, colSonota2];
                            hCode = dRng.Text.ToString().Trim();

                            // 対象者か？
                            if (hCode == string.Empty)
                            {
                                continue;
                            }
                        }
                    }


                    // カード番号を取得します
                    dRng = (Excel.Range)oxlsSheet.Cells[i, colNum];
                    string sNum = dRng.Text.ToString().Trim();

                    // メールアドレスを取得します
                    dRng = (Excel.Range)oxlsSheet.Cells[i, colMail];
                    string sMail = dRng.Text.ToString().Trim();

                    // 会員稼働予定テーブルを参照
                    if (getSchedule(double.Parse(sNum), iDt, iDt2))
                    {
                        // 稼働可能のときガイド依頼対象者テーブルに追加
                        guideDataUpdate(double.Parse(sNum), sMail, selNum);
                        dCnt++;
                    }
                }

                if (dCnt > 0)
                {
                    // ガイド依頼名テーブル登録
                    guideNameUpdate(selNum);

                    // データベース更新
                    gAdp.Update(dts.ガイド依頼対象者);
                    adp.Update(dts.ガイド依頼名);

                    // 抽出者一覧表示
                    pickUpDataList(selNum);
                }
                else
                {
                    dg1.Rows.Clear();
                }

                //マウスポインタを元に戻す
                this.Cursor = Cursors.Default;

                //保存処理
                oXls.DisplayAlerts = false;

                // 終了メッセージ
                MessageBox.Show("ガイド依頼対象者の抽出が終了しました。" + Environment.NewLine + Environment.NewLine + dCnt.ToString() + "件のデータを抽出しました。", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エクセルシートオープンエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            finally
            {
                //Bookをクローズ
                oXlsBook.Close(Type.Missing, Type.Missing, Type.Missing);

                //Excelを終了
                oXls.Quit();

                // COM オブジェクトの参照カウントを解放する 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXlsBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXls);

                //マウスポインタを元に戻す
                this.Cursor = Cursors.Default;
            }
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     会員稼働予定テーブルの任意の会員の任意の日付を参照し稼働可能か調べる </summary>
        /// <param name="sNum">
        ///     カード番号</param>
        /// <param name="sDt">
        ///     日付</param>
        /// <returns>
        ///     true:稼働可能, false:稼働不可</returns>
        ///------------------------------------------------------------------------------------
        private bool getSchedule(double sNum, DateTime sDt, DateTime sDt2)
        {
            DateTime iDt = DateTime.Parse(sDt.ToShortDateString());
            DateTime iDt2 = DateTime.Parse(sDt2.ToShortDateString());

            for (DateTime d = iDt; d <= iDt2; d = d.AddDays(1))
            {
                if (!chkSchedule(sNum, d))
                {
                    return false;
                }
            }

            return true;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     会員稼働予定テーブルの任意の会員の任意の日付を参照し稼働可能か調べる </summary>
        /// <param name="sNum">
        ///     カード番号</param>
        /// <param name="sDt">
        ///     期間開始日付</param>
        /// <returns>
        ///     true:稼働可能, false:稼働不可</returns>
        ///------------------------------------------------------------------------------------
        private bool chkSchedule(double sNum, DateTime sDt)
        {
            // 会員稼働予定テーブルを参照
            if (dts.会員稼働予定.Any(a => a.カード番号 == sNum && a.年 == sDt.Year && a.月 == sDt.Month))
            {
                guideDataSet.会員稼働予定Row s = dts.会員稼働予定.Single(a => a.カード番号 == sNum && a.年 == sDt.Year && a.月 == sDt.Month);

                // 依頼日と稼働予定を参照する
                if (sDt.Day == 1 && (s.d1 == "〇" || s.d1 == "◯" || s.d1 == "○" || s.d1 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 2 && (s.d2 == "〇" || s.d2 == "◯" || s.d2 == "○" || s.d2 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 3 && (s.d3 == "〇" || s.d3 == "◯" || s.d3 == "○" || s.d3 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 4 && (s.d4 == "〇" || s.d4 == "◯" || s.d4 == "○" || s.d4 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 5 && (s.d5 == "〇" || s.d5 == "◯" || s.d5 == "○" || s.d5 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 6 && (s.d6 == "〇" || s.d6 == "◯" || s.d6 == "○" || s.d6 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 7 && (s.d7 == "〇" || s.d7 == "◯" || s.d7 == "○" || s.d7 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 8 && (s.d8 == "〇" || s.d8 == "◯" || s.d8 == "○" || s.d8 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 9 && (s.d9 == "〇" || s.d9 == "◯" || s.d9 == "○" || s.d9 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 10 && (s.d10 == "〇" || s.d10 == "◯" || s.d10 == "○" || s.d10 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 11 && (s.d11 == "〇" || s.d11 == "◯" || s.d11 == "○" || s.d11 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 12 && (s.d12 == "〇" || s.d12 == "◯" || s.d12 == "○" || s.d12 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 13 && (s.d13 == "〇" || s.d13 == "◯" || s.d13 == "○" || s.d13 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 14 && (s.d14 == "〇" || s.d14 == "◯" || s.d14 == "○" || s.d14 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 15 && (s.d15 == "〇" || s.d15 == "◯" || s.d15 == "○" || s.d15 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 16 && (s.d16 == "〇" || s.d16 == "◯" || s.d16 == "○" || s.d16 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 17 && (s.d17 == "〇" || s.d17 == "◯" || s.d17 == "○" || s.d17 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 18 && (s.d18 == "〇" || s.d18 == "◯" || s.d18 == "○" || s.d18 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 19 && (s.d19 == "〇" || s.d19 == "◯" || s.d19 == "○" || s.d19 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 20 && (s.d20 == "〇" || s.d20 == "◯" || s.d20 == "○" || s.d20 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 21 && (s.d21 == "〇" || s.d21 == "◯" || s.d21 == "○" || s.d21 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 22 && (s.d22 == "〇" || s.d22 == "◯" || s.d22 == "○" || s.d22 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 23 && (s.d23 == "〇" || s.d23 == "◯" || s.d23 == "○" || s.d23 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 24 && (s.d24 == "〇" || s.d24 == "◯" || s.d24 == "○" || s.d24 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 25 && (s.d25 == "〇" || s.d25 == "◯" || s.d25 == "○" || s.d25 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 26 && (s.d26 == "〇" || s.d26 == "◯" || s.d26 == "○" || s.d26 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 27 && (s.d27 == "〇" || s.d27 == "◯" || s.d27 == "○" || s.d27 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 28 && (s.d28 == "〇" || s.d28 == "◯" || s.d28 == "○" || s.d28 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 29 && (s.d29 == "〇" || s.d29 == "◯" || s.d29 == "○" || s.d29 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 30 && (s.d30 == "〇" || s.d30 == "◯" || s.d30 == "○" || s.d30 == string.Empty))
                {
                    return true;
                }

                if (sDt.Day == 31 && (s.d31 == "〇" || s.d31 == "◯" || s.d31 == "○" || s.d31 == string.Empty))
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼対象者テーブル登録 </summary>
        /// <param name="sNum">
        ///     会員（組合員）番号</param>
        /// <param name="sMail">
        ///     メールアドレス</param>
        /// <param name="sID">
        ///     依頼番号</param>
        ///------------------------------------------------------------------------------------
        private void guideDataUpdate(double sNum, string sMail, long sID)
        {
            guideDataSet.ガイド依頼対象者Row r = dts.ガイド依頼対象者.Newガイド依頼対象者Row();
            r.会員番号 = sNum;
            r.メールアドレス = sMail;
            r.依頼番号 = sID;
            r.結果 = 0;
            r.登録年月日 = DateTime.Now;
            dts.ガイド依頼対象者.Addガイド依頼対象者Row(r);
        }

        ///-------------------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼名テーブル登録 </summary>
        /// <param name="sID">
        ///     依頼番号</param>
        ///-------------------------------------------------------------------------------------
        private void guideNameUpdate(long sID)
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Newガイド依頼名Row();
            r.ID = sID;
            r.依頼元 = cmbIrai.Text;
            r.依頼内容 = txtName.Text;
            r.依頼区分 = cmbKbn.SelectedIndex;
            r.依頼日 = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());
            r.受付日時 = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
            r.備考 = txtMemo.Text;
            r.登録年月日 = DateTime.Now;
            r.更新年月日 = DateTime.Now;
            r.依頼終了 = 0;
            dts.ガイド依頼名.Addガイド依頼名Row(r);
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼名テーブルに依頼メール送信日時を書き込み </summary>
        ///--------------------------------------------------------------------------
        private void iraiMLSendDateUpdate(DateTime nDt)
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == selNum);
            r.依頼メール送信日時 = nDt;
        }
        
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DispInitial();

            if (radioButton2.Checked)
            {
                linkLabel1.Visible = true;
                linkLabel2.Visible = false;
                button1.Enabled = false;
            }
            else
            {
                linkLabel1.Visible = false;
                linkLabel2.Visible = true;
                button1.Enabled = true;
            }
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            frmReqSel frm = new frmReqSel();
            frm.ShowDialog();
            selNum = frm.reNum; 
            
            // 後片付け
            frm.Dispose();

            // データ表示
            if (selNum != 0)
            {
                dataShow();
            }
        }

        private void dataShow()
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == selNum);

            cmbIrai.Text = r.依頼元;
            txtName.Text = r.依頼内容;
            cmbKbn.SelectedIndex = r.依頼区分;
            dateTimePicker2.Value = r.依頼日;
            dateTimePicker1.Value = r.受付日時;
            txtMemo.Text = r.備考;

            // 抽出者一覧表示
            pickUpDataList(selNum);

            // ピックアップ操作ボタンを無効にします
            button1.Enabled = false;
            linkLabel2.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            makeSendMail();
        }

        private void makeSendMail()
        {
            int mCnt = 0;

            for (int i = 0; i < dg1.RowCount; i++)
            {
                if (dg1["C_Check", i].Value.ToString() == "True")
                {
                    mCnt++;
                }
            }

            if (mCnt > 0)
            {
                // メール内容
                txtFrom.Enabled = true;
                txtCc.Enabled = true;
                txtBcc.Enabled = true;
                txtSubject.Enabled = true;
                txtText.Enabled = true;
                checkBox1.Enabled = true;

                //
                guideDataSet.メール設定Row r = dts.メール設定.Single(a => a.ID == global.mailKey);

                if (!r.HasErrors)
                {
                    txtFrom.Text = r.メールアドレス;
                    txtCc.Text = string.Empty;
                    txtBcc.Text = string.Empty;

                    // メール定型文を呼び出す
                    StringBuilder cSB = new StringBuilder(); // メール文面
                    mail.frmMailComSelect frm = new mail.frmMailComSelect(global.MLCOMMENT_IRAI);
                    frm.ShowDialog();
                    int mailComID = frm.mID;

                    if (mailComID != 0)
                    {
                        guideDataSet.メール定型文Row cr = dts.メール定型文.Single(a => a.ID == mailComID);

                        if (!cr.HasErrors)
                        {
                            // 件名
                            txtSubject.Text = cr.件名 + " " + selNum.ToString();

                            // 前文
                            cSB.Append(cr.前文).Append(Environment.NewLine);

                            // 内容
                            cSB.Append("依頼番号：").Append(selNum.ToString()).Append(Environment.NewLine).Append(Environment.NewLine);
                            cSB.Append("稼働日：").Append(dateTimePicker2.Value.ToLongDateString()).Append(Environment.NewLine).Append(Environment.NewLine);
                            cSB.Append("依頼元：").Append(cmbIrai.Text).Append(" 様").Append(Environment.NewLine).Append(Environment.NewLine);
                            cSB.Append("内容：").Append(txtName.Text).Append(Environment.NewLine).Append(Environment.NewLine);
                            cSB.Append("特記事項：").Append(txtMemo.Text).Append(Environment.NewLine).Append(Environment.NewLine);

                            // 後文
                            cSB.Append(cr.後文).Append(Environment.NewLine);

                            // 署名を付加
                            cSB.Append(r.署名);

                            // メール文章
                            txtText.Text = cSB.ToString();

                            // メール送信ボタン
                            linkLabel5.Visible = true;
                            linkLabel6.Visible = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("メール送信対象者がいません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     依頼メール送信確認・認証 </summary>
        /// <returns>
        ///     true:認証成功、false:認証不可、または実行中止</returns>
        ///-------------------------------------------------------------------------
        private bool preMailSend()
        {
            int mCnt = 0;

            for (int i = 0; i < dg1.RowCount; i++)
            {
                if (dg1["C_Check", i].Value.ToString() == "True")
                {
                    mCnt++;
                }
            }
            
            if (mCnt == 0)
            {
                MessageBox.Show("メール送信対象者がいません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // 実行確認
            if (MessageBox.Show("依頼メールを" + mCnt.ToString() + "名の組合員に送信します。よろしいですか","送信確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            // 送信用パスワード認証
            frmSendPW frm = new frmSendPW();
            frm.ShowDialog();
            bool pwStatus = frm.pwAuthen;
            frm.Dispose();

            if (!pwStatus)
            {
                return false;
            }

            // 再確認
            if (MessageBox.Show("依頼メールを組合員に送信します。よろしいですか", "再確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            return true;
        }


        ///-----------------------------------------------------------------------------
        /// <summary>
        ///     依頼メールを送信する </summary>
        /// <param name="toAddress">
        ///     宛先アドレス</param>
        /// <param name="sSubject">
        ///     メール件名</param>
        /// <param name="sMailText">
        ///     メール本文</param>
        /// <param name="SubjectOp">
        ///     </param>
        /// <param name="toStatus">
        ///     true:宛名差し込みする、false:宛名差し込みしない</param>
        /// <param name="testFlg">
        ///     0:本番送信、1:テスト送信</param>
        ///-----------------------------------------------------------------------------
        private void sendRequestMail(string toAddress, string sSubject, string sMailText, string SubjectOp, bool toStatus, int testFlg)
        {
            // メール設定情報
            guideDataSet.メール設定Row r = dts.メール設定.Single(a => a.ID == global.mailKey);
            
            // smtpサーバーを指定する
            SmtpClient client = new SmtpClient();
            client.Host = r.SMTPサーバー;
            client.Port = r.SMTPポート番号;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential(r.ログイン名, r.パスワード);
            client.EnableSsl = false;
            client.Timeout = 10000;

            //メッセージインスタンス生成
            MailMessage message = new MailMessage();

            //送信元
            message.From = new MailAddress(r.メールアドレス, r.メール名称);

            //件名
            message.Subject = sSubject;            

            // Cc
            if (txtCc.Text != string.Empty)
            {
                message.CC.Add(new MailAddress(txtCc.Text));
            }

            // Bcc
            if (txtBcc.Text != string.Empty)
            {
                message.Bcc.Add(new MailAddress(txtBcc.Text));
            }

            // 送信メールカウント
            int mCnt = 0;

            try
            {
                // 送信先テストモード
                if (Properties.Settings.Default.mailTest == global.FLGON)
                {
                    // テスト送信先
                    string[] toAdd = { "kyamagiwa@gmail.com", "yamagiwak@ybb.ne.jp", "e.moshichi-1212@i.softbank.jp" };

                    for (int i = 0; i < toAdd.Length; i++)
                    {
                        // 宛先
                        message.To.Clear();
                        message.To.Add(new MailAddress(toAdd[i]));

                        // 複数送信の時、2件目以降のCc/Bcc設定はクリアする
                        if (mCnt > 0)
                        {
                            message.CC.Clear();
                            message.Bcc.Clear();
                        }

                        // 送信する
                        client.Send(message);

                        // 送信ログ書き込み
                        DateTime nDt = DateTime.Now;
                        mllogUpdate("", toAdd[i], r.メール名称, r.メールアドレス, sSubject, sMailText, nDt);

                        // カウント
                        mCnt++;
                    }
                }
                else if (Properties.Settings.Default.mailTest == global.FLGOFF)
                {
                    // 本番用：グリッドから送付先を読み込む
                    for (int i = 0; i < dg1.RowCount; i++)
                    {
                        // チェック行
                        if (dg1["C_Check", i].Value.ToString() == "True")
                        {
                            // メールアドレスの記入あり
                            if (dg1[C_Mail, i].Value != null)
                            {
                                string toAdd = "";
                                string toName = "";

                                // テスト？本送信？
                                if (testFlg == global.FLGOFF)
                                {
                                    // 本送信
                                    toAdd = dg1[C_Mail, i].Value.ToString();
                                    toName = utility.NulltoStr(dg1[C_Name, i].Value);
                                }
                                else if (testFlg == global.FLGON)
                                {
                                    // テスト送信
                                    toAdd = r.メールアドレス;
                                    toName = r.メール名称;
                                }

                                //宛先
                                message.To.Clear();
                                message.To.Add(new MailAddress(toAdd, toName));

                                // 複数送信の時、2件目以降のCc/Bcc設定はクリアする
                                if (mCnt > 0)
                                {
                                    message.CC.Clear();
                                    message.Bcc.Clear();
                                }

                                //本文
                                if (toStatus)
                                {
                                    // 宛名氏名を差し込む
                                    string toSama = dg1[C_Name, i].Value.ToString() + "　様" + Environment.NewLine + Environment.NewLine;
                                    message.Body = toSama + sMailText;
                                }
                                else
                                {
                                    message.Body = sMailText;
                                }
                                
                                // 送信する
                                client.Send(message);

                                // 本送信のとき
                                if (testFlg == global.FLGOFF)
                                {
                                    DateTime nDt = DateTime.Now;

                                    // ガイド依頼名テーブルに送信日時を書き込む
                                    iraiMLSendDateUpdate(nDt);

                                    // 対象者データに送信日時を書き込む
                                    guideSendUpdate(int.Parse(dg1[C_Key, i].Value.ToString()), nDt);

                                    // 送信ログ書き込み
                                    mllogUpdate(toName, toAdd, r.メール名称, r.メールアドレス, sSubject, sMailText, nDt);

                                }
                                // カウント
                                mCnt++;
                            }
                        } 
                    }
                }
            }
            catch (SmtpException ex)
            //catch (Exception ex)
            {
                //エラーメッセージ
                MessageBox.Show(ex.Message, "メール送信エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                // 送信あり
                if (mCnt > 0)
                {
                    MessageBox.Show(mCnt.ToString() + "件の依頼メールを送信しました");

                    // 本送信の時
                    if (testFlg == global.FLGOFF)
                    {
                        // データベース更新
                        lAdp.Update(dts.メール送受信記録);
                        gAdp.Update(dts.ガイド依頼対象者);
                        adp.Update(dts.ガイド依頼名);
                    }
                }

                message.Dispose();
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     依頼メール送信日時書き込み </summary>
        /// <param name="sID">
        ///     ID</param>
        ///-------------------------------------------------------------------------
        private void guideSendUpdate(int sID, DateTime nDt)
        {
            guideDataSet.ガイド依頼対象者Row r = dts.ガイド依頼対象者.Single(a => a.ID == sID);

            if (!r.HasErrors)
            {
                r.依頼メール送信日時 = nDt;
            }
        }
        
        /// -----------------------------------------------------------------------
        /// <summary>
        ///     送信ログ書き込み </summary>
        /// <param name="sTo">
        ///     受信者</param>
        /// <param name="sToAddress">
        ///     受信者アドレス</param>
        /// <param name="sFrom">
        ///     送信者</param>
        /// <param name="sFromAddress">
        ///     送信者アドレス</param>
        /// <param name="sSubject">
        ///     件名</param>
        /// <param name="sMailBody">
        ///     メール本文</param>
        /// <param name="nDt">
        ///     送信日時</param>
        /// -----------------------------------------------------------------------
        private void mllogUpdate(string sTo, string sToAddress, string sFrom, string sFromAddress, string sSubject, string sMailBody, DateTime nDt)
        {
            // 送信ログ書き込み
            guideDataSet.メール送受信記録Row lr = dts.メール送受信記録.Newメール送受信記録Row();
            lr.日時 = nDt;
            lr.送受信区分 = global.MLLOG_SEND;
            lr.受信者 = sTo;
            lr.受信アドレス = sToAddress;
            lr.送信者 = sFrom;
            lr.送信アドレス = sFromAddress;
            lr.件名 = sSubject;
            lr.本文 = sMailBody;
            lr.登録年月日 = DateTime.Now;

            dts.メール送受信記録.Addメール送受信記録Row(lr);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                button1.Enabled = true;
                linkLabel2.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            frmReqSel frm = new frmReqSel();
            frm.ShowDialog();
            selNum = frm.reNum;

            // 後片付け
            frm.Dispose();

            // データ表示
            if (selNum != 0)
            {
                dataShow();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!errCheck())
            {
                return;
            }

            if (MessageBox.Show("対象者抽出を実行しますか？", "実行確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // 対象者抽出
            searchData(lblPickupFile.Text, dateTimePicker2.Value, dateTimePicker3.Value);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            makeSendMail();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!preMailSend())
            {
                return;
            }

            // メール送信
            sendRequestMail("", txtSubject.Text, txtText.Text, "", checkBox1.Checked, global.FLGOFF);

            // 閉じる
            this.Tag = TAGOK;
            this.Close();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string msg = "";
            msg += "これはテスト送信です。" + Environment.NewLine;
            msg += "テスト送信はすべてのメールを差出人宛に送付します。" + Environment.NewLine + Environment.NewLine;
            msg += "送信先：" + txtFrom.Text + Environment.NewLine + Environment.NewLine;
            msg += "よろしいですか？";

            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // メール送信
            sendRequestMail("", txtSubject.Text, txtText.Text, "", checkBox1.Checked, global.FLGON);
        }
    }
}
