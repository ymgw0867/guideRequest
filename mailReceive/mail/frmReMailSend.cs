using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using LinqToExcel;
using mailReceive.common;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;

namespace mailReceive.mail
{
    public partial class frmReMailSend : Form
    {
        public frmReMailSend(reMeilTo[] rm)
        {
            InitializeComponent();

            //this.Cursor = Cursors.WaitCursor;

            //// データテーブルにデータを読み込む
            //adp.Fill(dts.ガイド依頼名);
            //gAdp.Fill(dts.ガイド依頼対象者);
            //sAdp.Fill(dts.会員情報);
            //mAdp.Fill(dts.メール設定);
            //cAdp.Fill(dts.メール定型文);
            //lAdp.Fill(dts.メール送受信記録);

            _rm = rm;

            //this.Cursor = Cursors.Default;
        }

        reMeilTo[] _rm = null;

        // テーブルアダプター生成
        guideDataSetTableAdapters.ガイド依頼名TableAdapter adp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
        guideDataSetTableAdapters.ガイド依頼対象者TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();
        guideDataSetTableAdapters.会員情報TableAdapter sAdp = new guideDataSetTableAdapters.会員情報TableAdapter();
        guideDataSetTableAdapters.メール設定TableAdapter mAdp = new guideDataSetTableAdapters.メール設定TableAdapter();
        guideDataSetTableAdapters.メール定型文TableAdapter cAdp = new guideDataSetTableAdapters.メール定型文TableAdapter();
        guideDataSetTableAdapters.メール送受信記録TableAdapter lAdp = new guideDataSetTableAdapters.メール送受信記録TableAdapter();

        // データテーブル生成
        guideDataSet dts = new guideDataSet();

        const string TAGOK = "0"; 
        const string TAGCANCEL = "1";
        const string NOT_SEND = "未送信";

        //カラム定義
        string C_Date = "col1";
        string C_ID = "col2";
        string C_Name = "col3";
        string C_Wdays = "col4";
        string C_MailAddress = "col5";
        string C_Address = "col6";
        string C_reqID = "col7";
        
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
                tempDGV.Columns.Add(C_Date, "返信日時");
                tempDGV.Columns.Add(C_ID, "組合員番号");
                tempDGV.Columns.Add(C_Name, "氏名");
                tempDGV.Columns.Add(C_Wdays, DateTime.Today.Year.ToString() + "年稼働日数");
                tempDGV.Columns.Add(C_MailAddress, "メールアドレス");
                tempDGV.Columns.Add(C_Address, "住所");
                tempDGV.Columns.Add(C_reqID, "CID");

                tempDGV.Columns[C_Date].Visible = false;

                tempDGV.Columns[C_Date].Width = 110;
                tempDGV.Columns[C_ID].Width = 100;
                tempDGV.Columns[C_Name].Width = 100;
                tempDGV.Columns[C_Wdays].Width = 120;
                tempDGV.Columns[C_MailAddress].Width = 300;
                tempDGV.Columns[C_Address].Width = 300;

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

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void frmPickup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Tag.ToString() == TAGCANCEL)
            {
                if (MessageBox.Show("結果メール送信処理を中止します。よろしいですか", "中止", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
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

            this.Cursor = Cursors.WaitCursor;

            // データテーブルにデータを読み込む
            adp.Fill(dts.ガイド依頼名);
            gAdp.Fill(dts.ガイド依頼対象者);
            sAdp.Fill(dts.会員情報);
            mAdp.Fill(dts.メール設定);
            cAdp.Fill(dts.メール定型文);
            lAdp.Fill(dts.メール送受信記録);

            // 依頼内容表示
            showIrai();

            // グリッドビュー書式設定
            GridViewSetting(dg1);

            // 画面の初期化
            DispInitial();

            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Tag = TAGOK; 
        }

        ///---------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼内容表示 </summary>
        ///---------------------------------------------------------------
        private void showIrai()
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == _rm[0].iCode);

            txtIraiNum.Text = r.ID.ToString();
            txtIrai.Text = r.依頼元;
            txtName.Text = r.依頼内容;
            txtUdate.Text = r.受付日時.ToShortDateString();
            txtWdate.Text = r.依頼日.ToShortDateString();
            txtMemo.Text = r.備考;

            if (r.Is依頼メール送信日時Null())
            {
                txtIraiDate.Text = NOT_SEND;
            }
            else
            {
                txtIraiDate.Text = r.依頼メール送信日時.ToString("yyyy/MM/dd HH:mm:ss");
            }

            if (r.Is選考メール送信日時Null())
            {
                txtSenkouDate.Text = NOT_SEND;
            }
            else
            {
                txtSenkouDate.Text = r.選考メール送信日時.ToString("yyyy/MM/dd HH:mm:ss");
            }

            if (r.Is断りメール送信日時Null())
            {
                txtKotowariDate.Text = NOT_SEND;
            }
            else
            {
                txtKotowariDate.Text = r.断りメール送信日時.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        /// ----------------------------------------------------------
        /// <summary>
        ///     画面の初期化 </summary>
        /// ----------------------------------------------------------
        private void DispInitial()
        {
            txtName.Text = string.Empty;
            txtMemo.Text = string.Empty;
            
            linkLabel4.Visible = false;

            // メール内容
            txtFrom.Enabled = false;
            txtCc.Enabled = false;
            txtBcc.Enabled = false;
            txtSubject.Enabled = false;
            txtText.Enabled = false;

            txtText.ScrollBars = ScrollBars.Both;
            
            linkLabel6.Visible = false;
            linkLabel5.Visible = false;
        }

        ///---------------------------------------------------------------------------------------
        /// <summary>
        ///     メール送信対象者表示 </summary>
        /// <param name="sID">
        ///     依頼番号</param>
        ///----------------------------------------------------------------------------------------
        private void sendMailList(int sKbn, reMeilTo[] rm)
        {
            dg1.RowCount = 0;
            int iX = 0;

            for (int i = 0; i < rm.Length; i++)
            {
                if (rm[i].mailStatus == sKbn)
                {
                    dg1.Rows.Add();

                    dg1[C_Date, iX].Value = rm[i].reDate;
                    dg1[C_ID, iX].Value = rm[i].saNum.ToString();
                    dg1[C_Name, iX].Value = rm[i].name;
                    dg1[C_Wdays, iX].Value = rm[i].wDays.ToString();
                    dg1[C_MailAddress, iX].Value = rm[i].mailAddress;
                    dg1[C_Address, iX].Value = rm[i].address;
                    dg1[C_reqID, iX].Value = rm[i].reqID.ToString();

                    iX++;
                }
            }

            if (dg1.RowCount > 0)
            {
                dg1.CurrentCell = null;

                // メール作成ボタン
                linkLabel4.Visible = true;
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
            //guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Newガイド依頼名Row();
            //r.ID = sID;
            //r.依頼元 = cmbIrai.Text;
            //r.依頼内容 = txtName.Text;
            //r.依頼区分 = cmbKbn.SelectedIndex;
            //r.依頼日 = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());
            //r.受付日時 = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
            //r.備考 = txtMemo.Text;
            //r.登録年月日 = DateTime.Now;
            //r.更新年月日 = DateTime.Now;
            //r.依頼終了 = 0;
            //dts.ガイド依頼名.Addガイド依頼名Row(r);
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     メール作成 </summary>
        ///----------------------------------------------------------
        private void makeSendMail()
        {
            // メール内容
            txtFrom.Enabled = true;
            txtCc.Enabled = true;
            txtBcc.Enabled = true;
            txtSubject.Enabled = true;
            txtText.Enabled = true;

            //
            guideDataSet.メール設定Row r = dts.メール設定.Single(a => a.ID == global.mailKey);

            if (!r.HasErrors)
            {
                txtFrom.Text = r.メールアドレス;
                txtCc.Text = string.Empty;
                txtBcc.Text = string.Empty;
                    
                // メール定型文を呼び出す
                StringBuilder cSB = new StringBuilder(); // メール文面
                frmMailComSelect frm = null;

                if (comboBox2.SelectedIndex == 0)
                {
                    frm = new frmMailComSelect(global.MLCOMMENT_KETTEI);
                }
                else if (comboBox2.SelectedIndex == 1)
                {
                    frm = new frmMailComSelect(global.MLCOMMENT_KOTOWARI);
                }

                frm.ShowDialog();
                int mailComID = frm.mID;

                if (mailComID != 0)
                {
                    guideDataSet.メール定型文Row cr = dts.メール定型文.Single(a => a.ID == mailComID);

                    if (!cr.HasErrors)
                    {
                        // 件名
                        txtSubject.Text = cr.件名;

                        // 前文
                        cSB.Append(cr.前文).Append(Environment.NewLine);

                        // 内容
                        cSB.Append("依頼番号：").Append(txtIraiNum.Text).Append(Environment.NewLine).Append(Environment.NewLine);
                        cSB.Append("稼働日：").Append(DateTime.Parse(txtWdate.Text).ToLongDateString()).Append(Environment.NewLine).Append(Environment.NewLine);
                        cSB.Append("依頼元：").Append(txtIrai.Text).Append(" 様").Append(Environment.NewLine).Append(Environment.NewLine);
                        cSB.Append("内容：").Append(txtName.Text).Append(Environment.NewLine).Append(Environment.NewLine);
                        cSB.Append("特記事項：").Append(txtMemo.Text).Append(Environment.NewLine).Append(Environment.NewLine);

                        // 後文
                        cSB.Append(cr.後文).Append(Environment.NewLine);

                        // 署名を付加
                        cSB.Append(r.署名);

                        // メール文章
                        txtText.Text = cSB.ToString();

                        // メール送信ボタン
                        linkLabel6.Visible = true;
                        linkLabel5.Visible = true;

                        // 組合員名自動不可
                        checkBox1.Checked = true;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     依頼終了とする </summary>
        /// <param name="sID">
        ///     依頼番号</param>
        ///--------------------------------------------------------------------------
        private bool requestClose(long sID)
        {
            string msg = string.Empty;
            bool rtn = false;

            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == sID);
            
            if (!r.Is選考メール送信日時Null() == !r.Is断りメール送信日時Null())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("選考メール、お断りメール共に送信されました。この依頼案件を終了してよろしいですか？").Append(Environment.NewLine).Append(Environment.NewLine);
                sb.Append("選考メール： ").Append(r.選考メール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました").Append(Environment.NewLine);
                sb.Append("お断りメール： ").Append(r.断りメール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました").Append(Environment.NewLine);

                if (MessageBox.Show(sb.ToString(), "修了確認",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }

                // 依頼案件終了フラグ書き込み
                requestCloseUpdate(sID);
                adp.Update(dts.ガイド依頼名);
                rtn = true;
            }

            return rtn;
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     依頼案件終了フラグ書き込み </summary>
        /// <param name="sID">
        ///     依頼番号</param>
        ///------------------------------------------------------------------------
        private void requestCloseUpdate(long sID)
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == sID);
            r.依頼終了 = global.FLGON;

            // データベース更新
            adp.Update(dts.ガイド依頼名);

            // メッセージ
            StringBuilder sb = new StringBuilder();
            sb.Append("依頼案件「");
            sb.Append(r.ID.ToString()).Append(" " + r.依頼元).Append("」は終了しました");

            MessageBox.Show(sb.ToString(),"確認",MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                mCnt++;
            }
            
            if (mCnt == 0)
            {
                MessageBox.Show("メール送信対象者がいません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // 実行確認
            if (MessageBox.Show(comboBox2.Text + "メールを" + mCnt.ToString() + "名の組合員に送信します。よろしいですか","送信確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
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
            if (MessageBox.Show(comboBox2.Text + "メールを組合員に送信します。よろしいですか", "再確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            return true;
        }
        
        ///-----------------------------------------------------------------------------
        /// <summary>
        ///     依頼メールを送信する </summary>
        /// <param name="toAddress">
        ///     </param>
        /// <param name="sSubject">
        ///     メール件名</param>
        /// <param name="sMailText">
        ///     メール本文</param>
        /// <param name="SubjectOp">
        ///     </param>
        /// <param name="toStatus">
        ///     true:宛名差し込みする、false:宛名差し込みしない</param>
        /// <param name="reStatus">
        ///     結果 1:選考、0:お断り</param>
        /// <param name="testFlg">
        ///     0:本送信、1:テスト送信</param>
        ///-----------------------------------------------------------------------------
        private void sendRequestMail(string toAddress, string sSubject, string sMailText, string SubjectOp, bool toStatus, int reStatus, int testFlg)
        {
            // メール設定情報
            guideDataSet.メール設定Row r = dts.メール設定.Single(a => a.ID == global.mailKey);
            
            // smtpサーバーを指定する
            SmtpClient client = new SmtpClient();
            client.Host = r.SMTPサーバー;
            client.Port = r.SMTPポート番号;
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

                        // 本文
                        if (toStatus)
                        {
                            // 宛名氏名を差し込む
                            string toSama = dg1[C_Name, i].Value.ToString() + "様" + Environment.NewLine + Environment.NewLine;
                            message.Body = toSama + sMailText;
                        }
                        else
                        {
                            message.Body = sMailText;
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
                        // メールアドレスの記入あり
                        if (dg1[C_MailAddress, i].Value != null)
                        {
                            string toAdd = "";
                            string toName = "";

                            // テスト？本送信？
                            if (testFlg == global.FLGOFF)
                            {
                                // 本送信
                                toAdd = dg1[C_MailAddress, i].Value.ToString();
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
                            
                            // 本文
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

                            // 本送信の時
                            if (testFlg == global.FLGOFF)
                            {
                                DateTime nDt = DateTime.Now;

                                // ガイド依頼名テーブルに選考、断り送信日時を書き込む
                                iraiMLSendDateUpdate(nDt, reStatus);

                                // 対象者データに結果送信日時を書き込む
                                guideSendUpdate(int.Parse(dg1[C_reqID, i].Value.ToString()), reStatus, nDt);

                                // 送信ログ書き込み
                                mllogUpdate(toName, toAdd, r.メール名称, r.メールアドレス, sSubject, sMailText, nDt);
                            }

                            // カウント
                            mCnt++;
                        }
                    }
                }
            }
            catch (SmtpException ex)
            {
                //エラーメッセージ
                MessageBox.Show(ex.Message, "メール送信エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                // 送信あり
                if (mCnt > 0)
                {
                    MessageBox.Show(mCnt.ToString() + "件の" + comboBox2.Text + "メールを送信しました");

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
        private void guideSendUpdate(int sID, int reStatus, DateTime nDt)
        {
            guideDataSet.ガイド依頼対象者Row r = dts.ガイド依頼対象者.Single(a => a.ID == sID);

            if (!r.HasErrors)
            {
                r.結果送信日時 = nDt;
                r.結果 = reStatus;
            }
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼名テーブルにメール送信日時を書き込み </summary>
        ///--------------------------------------------------------------------------
        private void iraiMLSendDateUpdate(DateTime nDt, int reStatus)
        {
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == _rm[0].iCode);

            if (reStatus == global.FLGON)
            {
                // 選考メール送信日時
                r.選考メール送信日時 = nDt;
            }
            else if (reStatus == global.FLGOFF)
            {
                // 断りメール送信日時
                r.断りメール送信日時 = nDt;
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                // 選考決定
                if (sendWarning(comboBox2.SelectedIndex))
                {
                    sendMailList(global.FLGON, _rm);
                }
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                // お断り
                if (sendWarning(comboBox2.SelectedIndex))
                {
                    sendMailList(global.FLGOFF, _rm);
                }
            }

            // メール文章クリア
            mailSendInitial();
        }

        private void mailSendInitial()
        {
            // メール文章クリア
            txtFrom.Text = string.Empty;
            txtCc.Text = string.Empty;
            txtBcc.Text = string.Empty;
            txtSubject.Text = string.Empty;
            checkBox1.Checked = false;
            txtText.Text = string.Empty;

            // メール送信ボタン
            linkLabel6.Visible = false;
            linkLabel5.Visible = false;
        }


        ///--------------------------------------------------------------------
        /// <summary>
        ///     送信済みメールの確認 </summary>
        /// <param name="cmbSel">
        ///     メール選択コンボボックスselectIndex</param>
        /// <returns>
        ///     送信する:true, 送信しない:false</returns>
        ///--------------------------------------------------------------------
        private bool sendWarning(int cmbSel)
        {
            bool rtn = true;
            DateTime dt;
            
            if (cmbSel == global.FLGOFF && DateTime.TryParse(txtSenkouDate.Text, out dt))
            {
                // 選考決定メール
                if (MessageBox.Show("選考メールは既に送信済みです。再送しますか？","確認",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    rtn = false;
                }
            }
            else if (cmbSel == global.FLGON && DateTime.TryParse(txtKotowariDate.Text, out dt))
            {
                // 断りメール
                if (MessageBox.Show("お断りメールは既に送信済みです。再送しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    rtn = false;
                }
            }

            return rtn;
        }


        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            makeSendMail();
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

            // 結果
            int kekka = 0;
            if (comboBox2.SelectedIndex == 0)
            {
                kekka = global.FLGON;
            }
            else
            {
                kekka = global.FLGOFF;
            }

            // メール送信
            sendRequestMail("", txtSubject.Text, txtText.Text, "", checkBox1.Checked, kekka, global.FLGON);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!preMailSend())
            {
                return;
            }

            // 結果
            int kekka = 0;
            if (comboBox2.SelectedIndex == 0)
            {
                kekka = global.FLGON;
            }
            else
            {
                kekka = global.FLGOFF;
            }

            // メール送信
            sendRequestMail("", txtSubject.Text, txtText.Text, "", checkBox1.Checked, kekka, global.FLGOFF);

            // 依頼終了する？
            if (requestClose(_rm[0].iCode))
            {
                // 閉じる
                this.Tag = TAGOK;
                this.Close();
            }
            else
            {
                showIrai();

                // 送信対象者クリア
                dg1.Rows.Clear();

                // メール文章クリア
                mailSendInitial();
            }
        }
    }
}
