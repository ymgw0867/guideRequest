using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mailReceive.common;
using System.Net.Mail;
using System.Collections;
using System.Globalization;
using System.Net.Security;
using System.Text.RegularExpressions;

namespace mailReceive.mail
{
    public partial class frmMailReceive : Form
    {
        public frmMailReceive()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(this.timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 接続～メール受信～切断
            PopTest();
        }

        guideDataSetTableAdapters.メール設定TableAdapter msAdp = new guideDataSetTableAdapters.メール設定TableAdapter();
        guideDataSetTableAdapters.messageIDTableAdapter idAdp = new guideDataSetTableAdapters.messageIDTableAdapter();
        guideDataSetTableAdapters.メール送受信記録TableAdapter lAdp = new guideDataSetTableAdapters.メール送受信記録TableAdapter();
        guideDataSetTableAdapters.ガイド依頼名TableAdapter gAdp = new guideDataSetTableAdapters.ガイド依頼名TableAdapter();
        guideDataSetTableAdapters.ガイド依頼対象者TableAdapter rAdp = new guideDataSetTableAdapters.ガイド依頼対象者TableAdapter();
        guideDataSetTableAdapters.アサインTableAdapter asAdp = new guideDataSetTableAdapters.アサインTableAdapter();
        guideDataSetTableAdapters.会員情報TableAdapter kAdp = new guideDataSetTableAdapters.会員情報TableAdapter();

        guideDataSet dts = new guideDataSet();

        // カラム定義
        string cDate = "col1";
        string cFrom = "col2";
        string cSubject = "col3";

        string cIrai = "col4";
        string cYoteiDate = "col5";
        string cNaiyou = "col6";
        string cReCount = "col7";
        string cNewDate = "col8";

        string cCheck = "col9";
        string cSaNum = "col10";
        string cName = "col11";
        string cWorkDays = "col12";
        string cMailAddress = "col13";
        string cAddress = "col14";
        string cIraiNum = "col15";
        string cID = "col16";
        string cResult = "col17";
        string cReMail = "col18";
        string cBody = "col19";
        string cBtn = "col20";

        Timer timer1 = new Timer();

        #region メール受信設定
        string sSmtpServer;     // smtpサーバー
        string sSmtpPort;       // smtpポート番号
        string sPopServer;      // popサーバー
        string sPopPort;        // popポート番号
        string sAccount;        // アカウント
        string sPassword;       // パスワード
        string sMailAddress;    // メールアドレス
        int sReceiveSpan;    // 受信間隔・分
        #endregion

        //処理中ステータス
        int _mJob = global.JOBOUT;

        bool cellValStatus = false;

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
                tempDGV.Height = 140;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                //各列幅指定
                tempDGV.Columns.Add(cDate, "送信日時");
                tempDGV.Columns.Add(cFrom, "差出人");
                tempDGV.Columns.Add(cSubject, "件名");
                tempDGV.Columns.Add(cBody, "内容");

                tempDGV.Columns[cBody].Visible = false;

                tempDGV.Columns[cDate].Width = 160;
                tempDGV.Columns[cFrom].Width = 260;
                tempDGV.Columns[cSubject].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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

        ///---------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///---------------------------------------------------------------------
        private void dgSetting2(DataGridView tempDGV)
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
                tempDGV.Height = 140;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                //各列幅指定
                DataGridViewButtonColumn bc = new DataGridViewButtonColumn();
                bc.UseColumnTextForButtonValue = true;
                bc.Text = "終了する";
                tempDGV.Columns.Add(bc);
                tempDGV.Columns[0].Name = cBtn;
                tempDGV.Columns[0].HeaderText = "";
                
                tempDGV.Columns.Add(cDate, "受付日");
                tempDGV.Columns.Add(cIrai, "依頼元");
                tempDGV.Columns.Add(cYoteiDate, "稼働予定");
                tempDGV.Columns.Add(cReCount, "返信数");
                tempDGV.Columns.Add(cNewDate, "最新の返信");
                tempDGV.Columns.Add(cIraiNum, "依頼番号");

                tempDGV.Columns[cIraiNum].Visible = false;
                tempDGV.Columns[cBtn].Width = 70;
                tempDGV.Columns[cDate].Width = 90;
                tempDGV.Columns[cIrai].Width = 200;
                tempDGV.Columns[cYoteiDate].Width = 100;
                tempDGV.Columns[cReCount].Width = 70;
                //tempDGV.Columns[cNewDate].Width = 120;
                tempDGV.Columns[cNewDate].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[cReCount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

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

        ///---------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///---------------------------------------------------------------------
        private void dgSetting3(DataGridView tempDGV)
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
                tempDGV.Height = 240;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                //各列幅指定
                DataGridViewCheckBoxColumn cc = new DataGridViewCheckBoxColumn();
                cc.TrueValue = "True";
                cc.FalseValue = "False";
                tempDGV.Columns.Add(cc);
                tempDGV.Columns[0].Name = cCheck;
                tempDGV.Columns[0].HeaderText = "選考";

                tempDGV.Columns.Add(cResult, "結果");
                tempDGV.Columns.Add(cSaNum, "コード");
                tempDGV.Columns.Add(cName, "氏名");
                tempDGV.Columns.Add(cDate, "返信日時");
                tempDGV.Columns.Add(cWorkDays, DateTime.Today.Year.ToString() + "年稼働日数");
                tempDGV.Columns.Add(cMailAddress, "メールアドレス");
                tempDGV.Columns.Add(cAddress, "住所");
                tempDGV.Columns.Add(cID, "CID");

                tempDGV.Columns[cID].Visible = false;

                tempDGV.Columns[cResult].Width = 120;
                tempDGV.Columns[cCheck].Width = 50;
                tempDGV.Columns[cSaNum].Width = 70;
                tempDGV.Columns[cName].Width = 100;
                tempDGV.Columns[cDate].Width = 130;
                tempDGV.Columns[cWorkDays].Width = 120;
                tempDGV.Columns[cMailAddress].Width = 300;
                tempDGV.Columns[cAddress].Width = 300;

                tempDGV.Columns[cCheck].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[cDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[cSaNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                tempDGV.Columns[cWorkDays].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

                // 編集不可とする
                tempDGV.ReadOnly = false;
                foreach (DataGridViewColumn c in tempDGV.Columns)
                {
                    if (c.Name == cCheck)
                    {
                        c.ReadOnly = false;
                    } 
                    else
                    {
                        c.ReadOnly = true;
                    }
                }

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

        private void frmMailReceive_Load(object sender, EventArgs e)
        {
            //タイマーオフ
            timer1.Enabled = false;

            //ウィンドウ最大サイズ設定
            utility.WindowsMaxSize(this, this.Width, this.Height);

            //ウィンドウ最小サイズ設定
            utility.WindowsMinSize(this, this.Width, this.Height);

            //グリッド定義
            dgSetting(dg1);
            dgSetting2(dg2);
            dgSetting3(dg3);

            // メール設定読み込み
            this.Cursor = Cursors.WaitCursor;

            msAdp.Fill(dts.メール設定);
            lAdp.Fill(dts.メール送受信記録);
            gAdp.Fill(dts.ガイド依頼名);
            rAdp.Fill(dts.ガイド依頼対象者);
            asAdp.Fill(dts.アサイン);
            kAdp.Fill(dts.会員情報);

            // メール設定データ取得
            getMailData();

            //リストビュー初期化
            listviewInitial();

            //ボタン
            linkLabel1.Enabled = true;
            linkLabel2.Enabled = false;

            //警告ログ表示ラベル
            lblErrmsg.Text = string.Empty;

            // 結果メール送信ボタン
            linkLabel3.Visible = false;

            // 依頼返信状況表示
            reReqestSum();

            this.Cursor = Cursors.Default;
        }

        /// ------------------------------------------------------------
        /// <summary>
        ///     メール設定データ取得 </summary>
        /// ------------------------------------------------------------
        private void getMailData()
        {
            guideDataSet.メール設定Row r = dts.メール設定.Single(a => a.ID == global.mailKey);

            sSmtpServer = r.SMTPサーバー;
            sSmtpPort = r.SMTPポート番号.ToString();
            sPopServer = r.POPサーバー;
            sPopPort = r.POPポート番号.ToString();
            sAccount = r.ログイン名;
            sPassword = r.パスワード;
            sMailAddress = r.メールアドレス;
            sReceiveSpan = r.受信間隔;
        }

        /// ---------------------------------------------------
        /// <summary>
        ///     リストビュー初期化 </summary>
        /// ---------------------------------------------------
        private void listviewInitial()
        {
            listView1.View = View.Details;
            listView1.Clear();
            ColumnHeader cTime = new ColumnHeader();
            cTime.Text = "時刻";
            cTime.Width = 140;
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "処理";
            ch.Width = 480;

            ColumnHeader[] colHeaderRegValue = { cTime, ch };
            listView1.Columns.AddRange(colHeaderRegValue);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
        }


        private void PopTest()
        {
            //処理中
            _mJob = global.JOBIN;

            //マウスポインタを待機にする
            this.Cursor = Cursors.WaitCursor;
            linkLabel1.Text = "接続中...";
            linkLabel1.Enabled = false;

            //受信メールカウント
            int _mCount = 0;

            //ファイル名連番
            //int fNumber = 0;

            //message-ID
            string _msid;
            
            // POPサーバ、ユーザ名、パスワードを設定
            string hostname = sPopServer;

            ////////////////////////////////////デバッグステータス
            //////if (global.dStatus == 0) hostname = global.pblPopServerName;
            //////else hostname = "vspop.aaa.co,jp"; //デバッグ用

            string username = sAccount;
            string password = sPassword;
            int popPort = utility.StrtoZero(sPopPort);

            // POP サーバに接続します。
            addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "POPサーバに接続", Color.Black);
            PopClient pop = new PopClient(hostname, popPort);
            //POPサーバへの接続障害時は何もしないで待機状態へ戻る　2011/07/20
            if (global.Msglog.StartsWith("ERR"))
            {
                addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), global.Msglog, Color.Red);
                
                //非処理中ステータス
                _mJob = global.JOBOUT;

                //マウスポインタを戻す
                this.Cursor = Cursors.Default;

                ////////////////////////////////////デバッグステータス
                //////global.dStatus = 0;

                return;
            }

            //////////////////////////////////////デバッグステータス
            ////////global.dStatus = 1;

            addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), global.Msglog, Color.Black);

            // ログインします。
            pop.Login(username, password);

            // POP サーバに溜まっているメールのリストを取得します。
            ArrayList list = pop.GetList();

            for (int i = 0; i < list.Count; ++i)
            {
                // メール本体を取得する
                string mailtext = pop.GetMail((string)list[i]);

                // Mail クラスを作成
                Mail mail = new Mail(mailtext);

                //受信対象メールの判定値
                int _mTarget = 0;

                //message-IDを取得
                if (mail.Header["Message-ID"].Length > 0)
                {
                    _msid = MailHeader.Decode(mail.Header["Message-ID"][0]).Replace("Message-ID: ", "");

                    // 2015/11/24
                    _msid = _msid.Replace("Message-Id: ", "");
                }
                else
                {
                    _msid = string.Empty;
                }

                // 重複メールは受信しない
                if (getMessageid(_msid))
                {
                    //Content-Typeがあるメール
                    if (mail.Header["Content-Type"].Length > 0)
                    {
                        _mTarget = 1;

                        ////差出人アドレスを調べる
                        //foreach (string add in reAddress)
                        //{
                        //    if (MailHeader.Decode(mail.Header["From"][0]).Replace("From: ", "").IndexOf(add) >= 0)
                        //    {
                        //        _mTarget = 1;
                        //        break;
                        //    }
                        //}
                    }
                }

                // 受信対象メールのとき以下の処理を実行する
                if (_mTarget == 1)
                {
                    // メールデータ
                    mailData md = new mailData();
                    mailDataInitial(md);    //メールデータ初期化
                    
                    string sStr = string.Empty;

                    // 送信日時を取得
                    if (mail.Header["Date"].Length > 0)
                    {
                        sStr = MailHeader.Decode(mail.Header["Date"][0]).Replace("Date: ", "").Trim();
                   
                        //タイムゾーン記号を消去
                        sStr = sStr.Replace("(JST)", "").Trim();
                        sStr = sStr.Replace("(UT)", "").Trim();
                        sStr = sStr.Replace("(EST)", "").Trim();
                        sStr = sStr.Replace("(CST)", "").Trim();
                        sStr = sStr.Replace("(MST)", "").Trim();
                        sStr = sStr.Replace("(PST)", "").Trim();
                        sStr = sStr.Replace("(EDT)", "").Trim();
                        sStr = sStr.Replace("(CDT)", "").Trim();
                        sStr = sStr.Replace("(MDT)", "").Trim();
                        sStr = sStr.Replace("(PDT)", "").Trim();
                        sStr = sStr.Replace("(GMT)", "").Trim();
                        sStr = sStr.Replace("(C)", "").Trim();
                        sStr = sStr.Replace("(UTC)", "").Trim();

                        sStr = sStr.Replace("JST", "").Trim();
                        sStr = sStr.Replace("UT", "").Trim();
                        sStr = sStr.Replace("EST", "").Trim();
                        sStr = sStr.Replace("CST", "").Trim();
                        sStr = sStr.Replace("MST", "").Trim();
                        sStr = sStr.Replace("PST", "").Trim();
                        sStr = sStr.Replace("EDT", "").Trim();
                        sStr = sStr.Replace("CDT", "").Trim();
                        sStr = sStr.Replace("MDT", "").Trim();
                        sStr = sStr.Replace("PDT", "").Trim();
                        sStr = sStr.Replace("GMT", "").Trim();
                        sStr = sStr.Replace("C", "").Trim();
                        sStr = sStr.Replace("UTC", "").Trim();

                        //dg1[0, dg1.Rows.Count - 1].Value = DateTime.Parse(sStr).ToString("yyyy/M/dd HH:mm:ss");

                        // 世界標準時(RFC2822表記)からDetaTime型に変換
                        try
                        {
                            string[] expectedFormats = { "ddd, d MMM yyyy HH':'mm':'ss", "ddd, d MMM yyyy HH':'mm':'ss zzz", "d MMM yyyy HH':'mm':'ss", "d MMM yyyy HH':'mm':'ss zzz", "d", "D", "f", "F", "g", "G", "m", "r", "R", "s", "t", "T", "u", "U", "y" };
                            DateTime myUtcDT3 = System.DateTime.ParseExact(sStr, expectedFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
                            md.sendTime = myUtcDT3.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        catch (Exception)
                        {
                            md.sendTime = sStr;
                        }
                    }
                    else
                    {
                        sStr = string.Empty;
                    }

                    //差出人
                    md.fromAddress = MailHeader.Decode(mail.Header["From"][0]).Replace("From: ", "");
                    md.fromAddress = md.fromAddress.Replace("'", " ");

                    //if (md.fromAddress.IndexOf(Properties.Settings.Default.interFaxAddress) >= 0)
                    //{
                    //    _fromInterFax = 1;
                    //}
                    //else
                    //{
                    //    _fromInterFax = 0;
                    //}

                    //宛先
                    if (mail.Header["To:"].Length > 0)
                    {
                        md.toAddress = MailHeader.Decode(mail.Header["To:"][0]).Replace("To: ", "");
                        md.toAddress = md.toAddress.Replace("'", " ");
                    }
                    else
                    {
                        md.toAddress = string.Empty;
                    }

                    //if (_fromInterFax == 1) //差出人がinterfaxのとき
                    //{
                    //    md.toAddress = MailHeader.Decode(mail.Header["X-Interfax-InterFAXNumber:"][0]).Replace("X-Interfax-InterFAXNumber: ", "");
                    //    md.toAddress = md.toAddress.Replace("'", " ");
                    //}
                    //else
                    //{
                    //    md.toAddress = MailHeader.Decode(mail.Header["To:"][0]).Replace("To: ", "");
                    //    md.toAddress = md.toAddress.Replace("'", " ");
                    //}

                    //件名
                    //件名がないとき 2011/06/27
                    if (mail.Header["Subject"].Length > 0)
                    {
                        md.subject = MailHeader.Decode(mail.Header["Subject"][0]).Replace("Subject: ", "");
                        md.subject = md.subject.Replace("'", " ");
                    }

                    //マルチパートの判定
                    int mp = mail.Body.Multiparts.Length;

                    //マルチパート　または multipart/alternativeの判断
                    if (mp == 0)    //マルチパートではない
                    {
                        //Content-Type
                        sStr = MailHeader.Decode(mail.Header["Content-Type"][0]).Replace("Content-Type:", "").Trim();

                        //charset
                        if (sStr.IndexOf("charset=") > -1)
                        {
                            string sCharset = sStr.Substring(sStr.IndexOf("charset=")).Replace("charset=", "");
                            sCharset = sCharset.Replace(@"""", "");

                            // 2015/11/21
                            int cs = sCharset.IndexOf(";");
                            if (cs > -1)
                            {
                                // 2015/11/21 utf-8; reply-type=originalの「; reply-type=original」部を消去する
                                string cc = sCharset.Substring(cs, sCharset.Length - cs);
                                sCharset = sCharset.Replace(cc, "");
                            }

                            // メール本文を取得する
                            // Content-Type の charset を参照してデコード
                            if (mail.Header["Content-Transfer-Encoding"].Length > 0)
                            {
                                sStr = MailHeader.Decode(mail.Header["Content-Transfer-Encoding"][0]).Replace("Content-Transfer-Encoding:", "").Trim();
                                byte[] bytes;
                                if (sStr == "base64" || sStr == "BASE64")
                                {
                                    bytes = Convert.FromBase64String(mail.Body.Text);
                                }
                                else
                                {
                                    bytes = Encoding.ASCII.GetBytes(mail.Body.Text);
                                }

                                // 2016/02/18
                                if (sCharset == "cp932")
                                {
                                    sCharset = "Shift_JIS";
                                }

                                string mailbody = Encoding.GetEncoding(sCharset).GetString(bytes);
                                md.message = mailbody.Replace("'", " ");
                            }
                            else
                            {
                                md.message = string.Empty;
                            }
                        }
                        else
                        {
                            md.message = string.Empty;
                        }
                    }
                    else   //マルチパートのとき
                    {
                        //本文の確認　2011/06/27
                        for (int ix = 0; ix < mp; ix++)
                        {
                            //Content-Type を検証する
                            MailMultipart part1 = mail.Body.Multiparts[ix];

                            //マルチパートの更に中のマルチパート数を取得する　2011/06/27
                            int mb = part1.Body.Multiparts.Length;

                            //マルチパートの中のマルチパート毎の"Content-Type"を検証する　2011/06/27
                            for (int n = 0; n < mb; n++)
                            {
                                //Content-Type を検証する
                                MailMultipart p = part1.Body.Multiparts[n];
                                sStr = MailHeader.Decode(p.Header["Content-Type"][0]).Replace("Content-Type:", "").Trim();

                                //本文（"text/plain"）か？　2011/06/27
                                if (sStr.IndexOf("text/plain") >= 0)
                                {
                                    //charset
                                    if (sStr.IndexOf("charset=") > -1)
                                    {
                                        string sCharset = sStr.Substring(sStr.IndexOf("charset=")).Replace("charset=", "");
                                        sCharset = sCharset.Replace(@"""", "");

                                        // 2015/11/21
                                        int cs = sCharset.IndexOf(";");
                                        if (cs > -1)
                                        {
                                            // 2015/11/21 utf-8; reply-type=originalの「; reply-type=original」部を消去する
                                            string cc = sCharset.Substring(cs, sCharset.Length - cs);
                                            sCharset = sCharset.Replace(cc, "");
                                        }

                                        //エンコード名以降の文字列を削除する
                                        int m = sCharset.IndexOf(";");
                                        if (m >= 0)
                                        {
                                            sCharset = sCharset.Remove(m);
                                        }

                                        // メール本文を取得する
                                        // Content-Type の charset を参照してデコード
                                        //Content-Transfer-Encodingを取得する
                                        if (mail.Header["Content-Transfer-Encoding"].Length > 0)
                                        {
                                            sStr = MailHeader.Decode(p.Header["Content-Transfer-Encoding"][0]).Replace("Content-Transfer-Encoding:", "").Trim();
                                            byte[] bytes;
                                            if (sStr == "base64" || sStr == "BASE64")
                                            {
                                                bytes = Convert.FromBase64String(p.Body.Text);
                                            }
                                            else
                                            {
                                                bytes = Encoding.ASCII.GetBytes(p.Body.Text);
                                            }

                                            // 2016/02/18
                                            if (sCharset == "cp932")
                                            {
                                                sCharset = "Shift_JIS";
                                            }
                                             
                                            string mailbody = Encoding.GetEncoding(sCharset).GetString(bytes);
                                            md.message = mailbody.Replace("'", " ");
                                        }
                                        else
                                        {
                                            md.message = string.Empty;
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        ////// 添付ファイルの確認 : 添付ファイルは確認しないので以下コメント
                        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                        
                        ////for (int ix = 1; ix < mp; ix++)
                        ////{
                        ////    //Content-Type を検証する
                        ////    MailMultipart part2 = mail.Body.Multiparts[ix];
                        ////    sStr = MailHeader.Decode(part2.Header["Content-Type"][0]).Replace("Content-Type:", "").Trim();

                        ////    //添付ファイルがTIF画像かExcelファイルならば保存する
                        ////    int fType = global.ANYFILE;

                        ////    //差出人がInterFAXならば画像ファイル
                        ////    if (_fromInterFax == 1)
                        ////    {
                        ////        fType = global.TIFFILE;
                        ////    }
                        ////    else if (sStr.IndexOf("image/tiff") >= 0) //画像ファイルか？
                        ////    {
                        ////        fType = global.TIFFILE;
                        ////    }   //Excelファイルか？
                        ////    else if (sStr.IndexOf("application/vnd.ms-excel") >= 0 || sStr.IndexOf("application/ms-excel") >= 0 ||
                        ////             sStr.IndexOf("application/msexcel") >= 0 || sStr.IndexOf("application/x-msexcel") >= 0 ||
                        ////             sStr.IndexOf("application/x-msexcel") >= 0)
                        ////    {
                        ////        fType = global.XLSFILE;
                        ////    }

                        ////    if (fType == global.TIFFILE || fType == global.XLSFILE)
                        ////    {
                        ////        //Content-Transfer-Encodingを取得する
                        ////        sStr = MailHeader.Decode(part2.Header["Content-Transfer-Encoding"][0]).Replace("Content-Transfer-Encoding:", "").Trim();

                        ////        if (sStr == "base64")
                        ////        {
                        ////            byte[] bytes = Convert.FromBase64String(part2.Body.Text);

                        ////            string fName;
                        ////            fName = string.Format("{0:0000}", DateTime.Today.Year) +
                        ////                    string.Format("{0:00}", DateTime.Today.Month) +
                        ////                    string.Format("{0:00}", DateTime.Today.Day) +
                        ////                    string.Format("{0:00}", DateTime.Now.Hour) +
                        ////                    string.Format("{0:00}", DateTime.Now.Minute) +
                        ////                    string.Format("{0:00}", DateTime.Now.Second);

                        ////            fNumber++;

                        ////            //画像ファイルのとき
                        ////            if (fType == global.TIFFILE)
                        ////            {
                        ////                //保存フォルダがあるか？なければ作成する（TIFフォルダ）
                        ////                if (System.IO.Directory.Exists(Properties.Settings.Default.tifFile) == false)
                        ////                {
                        ////                    System.IO.Directory.CreateDirectory(Properties.Settings.Default.tifFile);
                        ////                }

                        ////                //interfaxからのメールのとき宛先をFAX番号をキーにして受信アドレステーブルから取得する
                        ////                string atesaki = string.Empty;
                        ////                string sqlStr = string.Empty;
                        ////                OleDbCommand sCom = new OleDbCommand();
                        ////                sCom.Connection = Cn;

                        ////                if (md.fromAddress.IndexOf(Properties.Settings.Default.interFaxAddress) >= 0)
                        ////                {
                        ////                    sqlStr += "select 名前 from 受信アドレス ";
                        ////                    sqlStr += "where FAX番号 like '%" + md.toAddress.Trim() + "'";
                        ////                    sCom.CommandText = sqlStr;
                        ////                    OleDbDataReader dR;
                        ////                    dR = sCom.ExecuteReader();

                        ////                    if (dR.HasRows)
                        ////                    {
                        ////                        dR.Read();
                        ////                        atesaki = dR["名前"].ToString();
                        ////                        md.toAddress += ": " + dR["名前"].ToString();
                        ////                    }
                        ////                    dR.Close();
                        ////                }

                        ////                //画像ファイル名
                        ////                fName = Properties.Settings.Default.tifFile + fName + string.Format("{0:000}", fNumber);
                        ////                if (atesaki != string.Empty)
                        ////                {
                        ////                    fName += "_" + atesaki + ".tif";
                        ////                }
                        ////                else
                        ////                {
                        ////                    fName += ".tif";
                        ////                }

                        ////                addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "画像を受信しました", Color.Blue);
                        ////            }
                        ////            else if (fType == global.XLSFILE) //XLSファイルのとき
                        ////            {
                        ////                //保存フォルダがあるか？なければ作成する（XLSフォルダ）
                        ////                if (System.IO.Directory.Exists(Properties.Settings.Default.xlsFile) == false)
                        ////                {
                        ////                    System.IO.Directory.CreateDirectory(Properties.Settings.Default.xlsFile);
                        ////                }

                        ////                fName = Properties.Settings.Default.xlsFile + fName + string.Format("{0:000}", fNumber) + ".xls";
                        ////                addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Excelファイルを受信しました", Color.Blue);
                        ////            }

                        ////            using (Stream stm = File.Open(fName, FileMode.Create))
                        ////            using (BinaryWriter bw = new BinaryWriter(stm))
                        ////            {
                        ////                bw.Write(bytes);
                        ////            }

                        ////            //添付ファイル名
                        ////            md.addFilename = fName;
                        ////        }
                        ////    }
                        ////}
                    }

                    //// 確認用に取得したメールをそのままカレントディレクトリに書き出します。
                    //using (StreamWriter sw = new StreamWriter(DateTime.Now.ToString("yyyyMMddHHmmssfff") + i.ToString("0000") + ".txt"))
                    //{
                    //    sw.Write(mailtext);
                    //}

                    // メールを POP サーバから取得します。
                    // ★注意★
                    // 削除したメールを元に戻すことはできません。
                    // 本当に削除していい場合は以下のコメントをはずしてください。
                    //pop.DeleteMail((string)list[i]);

                    // messageid履歴書き込み
                    messageidUpDate(_msid);

                    // 通信ログ書き込み
                    mailLogUpDate(md);

                    // ガイド依頼対象者返信日時書き込み
                    pickupMemUpdate(md.fromAddress, md.subject);

                    // グリッド表示
                    dg1.Rows.Add();

                    // 通信時刻
                    dg1[cDate, dg1.Rows.Count - 1].Value = md.sendTime;
                    // 差出人
                    dg1[cFrom, dg1.Rows.Count - 1].Value = md.fromAddress;
                    // 件名
                    dg1[cSubject, dg1.Rows.Count - 1].Value = md.subject;
                    // 内容
                    dg1[cBody, dg1.Rows.Count - 1].Value = md.message;

                    dg1.CurrentCell = null;

                    dg1.Sort(dg1.Columns[cDate], ListSortDirection.Descending);

                    _mCount++;
                }
            }

            //受信結果表示
            if (_mCount == 0)
            {
                addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "受信メールはありませんでした", Color.Black);
            }
            else
            {
                addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), _mCount.ToString() + "件のメールを受信しました", Color.Blue);
            }

            // 切断する
            addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "POPサーバ切断", Color.Black);
            pop.Close();
            addListView(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), global.Msglog, Color.Black);

            /////以下デバッグ用でコメント化 2011/07/11 //////////////////////////////////////////////////////
            /////↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            ////Excelファイルから汎用データを作成する
            //ExcelToData(Cn);
            /////↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            // 依頼案件返信状況表示
            if (_mCount > 0)
            {
                reReqestSum();
            }

            //非処理中ステータス
            _mJob = global.JOBOUT;

            //マウスポインタを戻す
            this.Cursor = Cursors.Default;
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     ガイド依頼対象者テーブルに返信日時を書き込む </summary>
        /// <param name="sAddress">
        ///     メールアドレス</param>
        /// <param name="sSubject">
        ///     件名</param>
        ///-------------------------------------------------------------------------
        private void pickupMemUpdate(string sAddress, string sSubject)
        {
            string mlad = string.Empty;
            Regex reg = new Regex(@"<.*>$");

            foreach (var t in dts.ガイド依頼名
                .Where(a => a.依頼終了 == global.FLGOFF).OrderBy(a => a.ID))
            {
                if (sSubject.Contains(t.ID.ToString()))
                {
                    // メールアドレスを取得します
                    Match m = reg.Match(sAddress);
                    if (m.Success)
                    {
                        mlad = m.Value.Replace("<", "").Replace(">", "");
                    }
                    else
                    {
                        mlad = sAddress;
                    }

                    if (dts.ガイド依頼対象者.Any(a => a.依頼番号 == t.ID && a.メールアドレス == mlad))
                    {
                        /*
                            返信日時が書き込み済みのデータは上書きしない 
                            ※返信を複数回行った場合は最初の返信のみ有効にする                            
                        */
                        foreach (var s in dts.ガイド依頼対象者
                                .Where(a => a.依頼番号 == t.ID && 
                                       a.メールアドレス == mlad && 
                                       a.Is返信受信日時Null()))
                        {
                            s.返信受信日時 = DateTime.Now;
                            rAdp.Update(dts.ガイド依頼対象者);
                        }

                        break;
                    }
                }
            }
        }


        /// ----------------------------------------------------------------------
        /// <summary>
        ///     通信ログを書き込む </summary>
        /// ----------------------------------------------------------------------
        private void mailLogUpDate(mailData md)
        {
            guideDataSet.メール送受信記録Row r = dts.メール送受信記録.Newメール送受信記録Row();

            try
            {
                if (md.sendTime.Length > 50) md.sendTime = md.sendTime.Substring(0, 50);
                if (md.fromAddress.Length > 255) md.fromAddress = md.fromAddress.Substring(0, 255);
                if (md.toAddress.Length > 255) md.toAddress = md.toAddress.Substring(0, 255);
                if (md.subject.Length > 255) md.subject = md.subject.Substring(0, 255);
                if (md.addFilename.Length > 255) md.addFilename = md.addFilename.Substring(0, 255);
                if (md.memo.Length > 255) md.memo = md.memo.Substring(0, 255);

                DateTime dt;

                if (DateTime.TryParse(md.sendTime, out dt))
                {
                    r.日時 = dt;
                }
                else
                {
                    r.日時 = DateTime.Now;
                }

                r.送受信区分 = global.MLLOG_REC;
                r.受信者 = "";
                r.受信アドレス = md.toAddress;
                r.送信者 = "";
                r.送信アドレス = md.fromAddress;
                r.件名 = md.subject;
                r.本文 = md.message;
                r.登録年月日 = DateTime.Now;

                dts.メール送受信記録.Addメール送受信記録Row(r);

                // メール送受信記録更新
                lAdp.Update(dts.メール送受信記録);

                lAdp.Fill(dts.メール送受信記録);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        
        /// ------------------------------------------------------------------------
        /// <summary>
        ///     依頼案件返信状況表示 </summary>
        /// ------------------------------------------------------------------------
        private void reReqestSum()
        {
            // データ読み込み
            lAdp.Fill(dts.メール送受信記録);
            gAdp.Fill(dts.ガイド依頼名);
            rAdp.Fill(dts.ガイド依頼対象者);

            int ir = 0;

            reMail[] rem = null;

            foreach (var t in dts.ガイド依頼名.Where(a => a.依頼終了 == global.FLGOFF))
            {
                // 返信数を取得
                int s = dts.メール送受信記録
                        .Where(a => a.送受信区分 == global.MLLOG_REC)
                        .Count(a => a.件名.Contains(t.ID.ToString()));

                // 配列にセット
                ir++;
                Array.Resize(ref rem, ir);
                rem[ir - 1] = new reMail();

                rem[ir - 1].cDate = t.受付日時;
                rem[ir - 1].iraimoto = t.依頼元;
                rem[ir - 1].wDate = t.依頼日;
                rem[ir - 1].Naiyou = t.依頼内容;
                rem[ir - 1].reCount = s;
                rem[ir - 1].iraiNum = t.ID.ToString();

                if (s != 0)
                {
                    var f = dts.メール送受信記録
                        .Where(a => a.送受信区分 == global.MLLOG_REC &&
                                        a.件名.Contains(t.ID.ToString()))
                                        .Max(a => a.日時);

                    rem[ir - 1].nDate = f;
                }
                else
                {
                    rem[ir - 1].nDate = DateTime.Parse("1900/01/01");
                }
            }

            // グリッドに表示
            if (rem != null)
            {
                int i = 0;

                dg2.Rows.Clear();
                
                foreach (var ss in rem.OrderByDescending(a => a.nDate))
                {
                    //グリッド表示
                    dg2.Rows.Add();

                    // 受付日
                    dg2[cDate, i].Value = ss.cDate.ToShortDateString();

                    // 依頼元
                    dg2[cIrai, i].Value = ss.iraimoto;

                    // 稼働予定日
                    dg2[cYoteiDate, i].Value = ss.wDate.ToString("yyyy/MM/dd (ddd)");

                    // 返信数
                    dg2[cReCount, i].Value = ss.reCount.ToString();

                    // 最新返信
                    if (ss.reCount > 0)
                    {
                        dg2[cNewDate, i].Value = ss.nDate;
                    }
                    else
                    {
                        dg2[cNewDate, i].Value = string.Empty;
                    }

                    // 依頼番号
                    dg2[cIraiNum, i].Value = ss.iraiNum;

                    dg2.CurrentCell = null;

                    i++;
                }
            }

            // 返信詳細グリッドクリア
            dg3Clear();

            // ボタン
            linkLabel3.Visible = false;
        }

        private void dg3Clear()
        {
            txtIraiNum.Text = string.Empty;
            txtIrai.Text = string.Empty;
            txtNaiyou.Text = string.Empty;
            txtDate.Text = string.Empty;

            dg3.Rows.Clear();
        }



        ///-------------------------------------------------------------------------
        /// <summary>
        ///     MessageID履歴を書き込む </summary>
        ///-------------------------------------------------------------------------
        private void messageidUpDate(string tempid)
        {
            // messageID読み込み
            idAdp.Fill(dts.messageID);

            // messageIDテーブル更新
            guideDataSet.messageIDRow r = dts.messageID.NewmessageIDRow();
            r.受信日時 = DateTime.Now;
            r.message = tempid;

            dts.messageID.AddmessageIDRow(r);

            // テーブル更新
            idAdp.Update(dts.messageID);
        }

        /// ---------------------------------------------------------------
        /// <summary>
        ///     処理状況表示 </summary>
        /// <param name="col1">
        ///     列１</param>
        /// <param name="col2">
        ///     列２</param>
        /// ---------------------------------------------------------------
        private void addListView(string col1, string col2, Color c)
        {
            string[] item1 = { col1, col2 };
            listView1.Items.Add(new ListViewItem(item1));
            listView1.Items[(listView1.Items.Count - 1)].ForeColor = c;

            // 2015/11/24
            listView1.EnsureVisible(listView1.Items.Count - 1);
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     過去のmessageidを参照する </summary>
        /// <param name="stemp">
        ///     検証するmessageid:文字列</param>
        /// <returns>
        ///     過去になし：true,過去にあり：false</returns>
        ///---------------------------------------------------------
        private bool getMessageid(string stemp)
        {
            Boolean tf = false;

            try
            {
                // messageID読み込み
                idAdp.Fill(dts.messageID);

                if (dts.messageID.Count(a => a.message == stemp) > 0)
                {
                    tf = false;
                }
                else
                {
                    tf = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return tf;
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     メールデータ初期化 </summary>
        /// <param name="md">
        ///     メールデータインスタンス</param>
        /// <returns>
        ///     メールデータ</returns>
        ///--------------------------------------------------------
        private void mailDataInitial(mailData md)
        {
            md.sendTime = string.Empty;
            md.toAddress = string.Empty;
            md.fromAddress = string.Empty;
            md.subject = string.Empty;
            md.message = string.Empty;
            md.memo = string.Empty;
            md.addFilename = string.Empty;
        }

        private void frmMailReceive_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            this.Dispose();
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
        }

        private void dg2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string iNum = dg2[cIraiNum, e.RowIndex].Value.ToString();
            //showReMail(iNum, dg3);
            showReMailnew(iNum, dg3);
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     返信者詳細表示 </summary>
        /// <param name="iNum">
        ///     依頼番号</param>
        /// <param name="d">
        ///     データグリッドビュー </param>
        /// -------------------------------------------------------------------------
        private void showReMail(string iNum, DataGridView d)
        {
            cellValStatus = false;

            // 依頼案件情報
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == long.Parse(iNum));

            txtIraiNum.Text = iNum;
            txtIrai.Text = r.依頼元;
            txtDate.Text = r.依頼日.ToString("yyyy/MM/dd (ddd)");
            txtNaiyou.Text = r.依頼内容;

            // 返信者情報
            int iX = 0;

            d.Rows.Clear();

            // 返信データを取得
            foreach (var t in dts.メール送受信記録.Where(a => a.送受信区分 == global.MLLOG_REC && a.件名.Contains(iNum))
                                                .OrderBy(a => a.日時))
            {
                // メールアドレスを取得します
                string mlad = t.送信アドレス; 
                Regex reg = new Regex(@"<.*>$");
                Match m = reg.Match(t.送信アドレス);
                if (m.Success)
                {
                    mlad = m.Value.Replace("<", "").Replace(">", "");
                }

                // グリッドに表示します
                d.Rows.Add();
                
                d[cCheck, iX].Value = false;
                d[cDate, iX].Value = t.日時;
                
                d[cSaNum, iX].Value = global.ERR_1; // 初期値
                d[cName, iX].Value = global.ERR_1;  // 初期値

                // 返信者氏名取得
                foreach (var f in dts.ガイド依頼対象者.Where(a => a.依頼番号.ToString() == iNum && a.メールアドレス == mlad))
                {
                    if (f.結果 == global.FLGON)
                    {
                        d[cCheck, iX].Value = true;
                    }
                    else if (f.結果 == global.FLGOFF)
                    {
                        d[cCheck, iX].Value = false;
                    }

                    d[cSaNum, iX].Value = f.会員番号;

                    if (f.会員情報Row != null)
                    {
                        d[cName, iX].Value = f.会員情報Row.氏名;
                        d[cAddress, iX].Value = f.会員情報Row.都道府県 + f.会員情報Row.住所1.Trim().Replace(" ", "") + f.会員情報Row.住所2.Trim().Replace(" ", "") + f.会員情報Row.住所3.Trim().Replace(" ", "");
                    }
                    else
                    {
                        d[cName, iX].Value = global.ERR_1;
                        d[cAddress, iX].Value = global.ERR_1;
                    }

                    // 稼働日数
                    d[cWorkDays, iX].Value = getWorkDays(f.会員番号);

                    // ガイド依頼対象者ID
                    d[cID, iX].Value = f.ID.ToString();

                    // 結果メール
                    if (!f.Is結果送信日時Null())
                    {
                        if (f.結果 == global.FLGON)
                        {
                            d[cResult, iX].Value = "選考通知送信済み";
                        }
                        else if (f.結果 == global.FLGOFF)
                        {
                            d[cResult, iX].Value = "お断り通知送信済み";
                        }
                    }
                }

                // メールアドレス
                d[cMailAddress, iX].Value = mlad;

                // 行表示色
                if (d[cSaNum, iX].Value.ToString() == global.ERR_1)
                {
                    d.Rows[iX].DefaultCellStyle.ForeColor = Color.Red;
                }

                iX++;
            }

            if (iX > 0)
            {
                dg3.CurrentCell = null;
            }

            cellValStatus = true;

            // 結果メール送信ボタン
            bool bON = false;

            // 
            for (int i = 0; i < dg3.RowCount; i++)
            {
                // エラーデータはネグる
                if (dg3[cSaNum, i].Value.ToString() != global.ERR_1)
                {
                    bON = true;
                    break;
                }
            }

            // エラー以外の正常データが存在する場合、結果メール送信ボタン使用可とする
            if (bON)
            {
                linkLabel3.Visible = true;
            }
            else
            {
                linkLabel3.Visible = false;
            }

            // 案件終了ボタン
            //if (!r.Is依頼メール送信日時Null() && !r.Is断りメール送信日時Null())
            //{
            //    btnClose.Visible = true;
            //}
            //else
            //{
            //    btnClose.Visible = false;
            //}
        }


        private void showReMailnew(string iNum, DataGridView d)
        {
            cellValStatus = false;

            // 依頼案件情報
            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == long.Parse(iNum));

            txtIraiNum.Text = iNum;
            txtIrai.Text = r.依頼元;
            txtDate.Text = r.依頼日.ToString("yyyy/MM/dd (ddd)");
            txtNaiyou.Text = r.依頼内容;

            // 返信者情報
            int iX = 0;

            d.Rows.Clear();

            foreach (var t in dts.ガイド依頼対象者
                    .Where(a => a.依頼番号 == long.Parse(iNum) && !a.Is返信受信日時Null())
                    .OrderBy(a => a.返信受信日時))
            {
                // グリッドに表示します
                d.Rows.Add();

                d[cCheck, iX].Value = false;
                d[cDate, iX].Value = t.返信受信日時;

                d[cSaNum, iX].Value = global.ERR_1; // 初期値
                d[cName, iX].Value = global.ERR_1;  // 初期値

                // 返信者氏名取得
                if (t.結果 == global.FLGON)
                {
                    d[cCheck, iX].Value = true;
                }
                else if (t.結果 == global.FLGOFF)
                {
                    d[cCheck, iX].Value = false;
                }

                d[cSaNum, iX].Value = t.会員番号;

                if (t.会員情報Row != null)
                {
                    d[cName, iX].Value = t.会員情報Row.氏名;
                    d[cAddress, iX].Value = t.会員情報Row.都道府県 + t.会員情報Row.住所1.Trim().Replace(" ", "") + t.会員情報Row.住所2.Trim().Replace(" ", "") + t.会員情報Row.住所3.Trim().Replace(" ", "");
                }
                else
                {
                    d[cName, iX].Value = global.ERR_1;
                    d[cAddress, iX].Value = global.ERR_1;
                }

                // 稼働日数
                d[cWorkDays, iX].Value = getWorkDays(t.会員番号);

                // ガイド依頼対象者ID
                d[cID, iX].Value = t.ID.ToString();

                // 結果メール
                if (!t.Is結果送信日時Null())
                {
                    if (t.結果 == global.FLGON)
                    {
                        d[cResult, iX].Value = "選考通知送信済み";
                    }
                    else if (t.結果 == global.FLGOFF)
                    {
                        d[cResult, iX].Value = "お断り通知送信済み";
                    }
                }
                
                // メールアドレス
                d[cMailAddress, iX].Value = t.メールアドレス;

                // 行表示色
                if (d[cSaNum, iX].Value.ToString() == global.ERR_1)
                {
                    d.Rows[iX].DefaultCellStyle.ForeColor = Color.Red;
                }

                iX++;
            }

            if (iX > 0)
            {
                dg3.CurrentCell = null;
            }

            cellValStatus = true;

            // 結果メール送信ボタン
            bool bON = false;

            // 
            for (int i = 0; i < dg3.RowCount; i++)
            {
                // エラーデータはネグる
                if (dg3[cSaNum, i].Value.ToString() != global.ERR_1)
                {
                    bON = true;
                    break;
                }
            }

            // エラー以外の正常データが存在する場合、結果メール送信ボタン使用可とする
            if (bON)
            {
                linkLabel3.Visible = true;
            }
            else
            {
                linkLabel3.Visible = false;
            }

            // 案件終了ボタン
            //if (!r.Is依頼メール送信日時Null() && !r.Is断りメール送信日時Null())
            //{
            //    btnClose.Visible = true;
            //}
            //else
            //{
            //    btnClose.Visible = false;
            //}
            
        }

        /// -----------------------------------------------------------------------
        /// <summary>
        ///     当年稼働日数取得 </summary>
        /// <param name="sID">
        ///     カード番号</param>
        /// <returns>
        ///     稼働日数</returns>
        /// -----------------------------------------------------------------------
        private int getWorkDays(double sID)
        {
            DateTime dt = DateTime.Parse(DateTime.Today.Year.ToString() + "/01/01");

            // 当年稼働日数取得
            var s = dts.アサイン.Where(a => a.カード番号 == sID && !a.Is稼働日1Null() && a.稼働日1 >= dt)
                               .GroupBy(a => a.カード番号)
                               .Select(g => new
                               {
                                   cNum = g.Key,
                                   nissu = g.Sum(a => a.日数)
                               });

            int wDays = 0;

            foreach (var t in s)
            {
                wDays = t.nissu;
            }

            return wDays;
        }

        private void dg3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dg3.CurrentCellAddress.X == 0)
            {
                if (dg3.IsCurrentCellDirty)
                {
                    dg3.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void dg3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cellValStatus)
            {
                if (e.ColumnIndex == 0)
                {
                    //// 結果メール送信ボタン
                    ////btnSendMail.Enabled = false;

                    //// 選考チェックのとき
                    //if (dg3[e.ColumnIndex, e.RowIndex].Value.ToString() == "True")
                    //{
                    //    // 選考チェック行以外の行チェックをはずす
                    //    for (int i = 0; i < dg3.RowCount; i++)
                    //    {
                    //        if (i != e.RowIndex)
                    //        {
                    //            if (dg3[e.ColumnIndex, i].Value.ToString() == "True")
                    //            {
                    //                dg3[e.ColumnIndex, i].Value = false;
                    //            }
                    //        }
                    //    }

                    //    // 結果メール送信ボタン
                    //    //btnSendMail.Enabled = true;
                    //}
                }
            }
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
        }

        ///--------------------------------------------------------------------------------
        /// <summary>
        ///     結果メール送信対象者の配列を作成：（結果メール送信先クラス作成）</summary>
        /// <param name="dg">
        ///     データグリッドオブジェクト</param>
        /// <returns>
        ///     結果メール送信先クラスの配列</returns>
        ///--------------------------------------------------------------------------------
        private reMeilTo[] reMailSendto(DataGridView dg)
        {
            this.Cursor = Cursors.WaitCursor;   // カーソルを待機にする

            reMeilTo[] rm = null;
            int iX = 0;

            for (int i = 0; i < dg.RowCount; i++)
            {
                // エラーデータはネグる
                if (dg[cSaNum, i].Value.ToString() == global.ERR_1)
                {
                    continue;
                }

                Array.Resize(ref rm, iX + 1);

                rm[iX] = new reMeilTo();

                rm[iX].iCode = long.Parse(txtIraiNum.Text);

                if (dg[cCheck, i].Value.ToString() == "True")
                {
                    rm[iX].mailStatus = global.FLGON;
                }
                else
                {
                    rm[iX].mailStatus = global.FLGOFF;
                }

                rm[iX].reDate = DateTime.Parse(dg[cDate, i].Value.ToString());
                rm[iX].saNum = double.Parse(dg[cSaNum, i].Value.ToString());
                rm[iX].name = dg[cName, i].Value.ToString();
                rm[iX].wDays = int.Parse(dg[cWorkDays, i].Value.ToString());
                rm[iX].mailAddress = dg[cMailAddress, i].Value.ToString();
                rm[iX].address = dg[cAddress, i].Value.ToString();
                rm[iX].reqID = int.Parse(dg[cID, i].Value.ToString());

                iX++;
            }

            // カーソルを戻す
            this.Cursor = Cursors.Default;

            return rm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     依頼終了処理 </summary>
        ///--------------------------------------------------------------
        /// <returns></returns>
        private bool iraiCloseUpdate(long sID)
        {
            string msg = string.Empty;
            string msg2 = string.Empty;
            string sDate = string.Empty;
            string kDate = string.Empty;

            guideDataSet.ガイド依頼名Row r = dts.ガイド依頼名.Single(a => a.ID == sID);

            msg = r.ID.ToString() + " " + r.依頼元 + Environment.NewLine;
            msg += "受付日：" + r.受付日時.ToShortDateString();

            // 結果メール送信状況
            if (!r.Is選考メール送信日時Null() && !r.Is断りメール送信日時Null())
            {
                // 選考、断り両方送信済み
                msg2 = "選考メール、お断りメール共に送信済みです。この依頼案件を終了してよろしいですか？";
                sDate = r.選考メール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました";
                kDate = r.断りメール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました";
            }

            if (!r.Is選考メール送信日時Null() && r.Is断りメール送信日時Null())
            {
                // 選考のみ送信済み
                msg2 = "選考メールは送信済みですが、お断りメールは未送信です。この依頼案件を終了してよろしいですか？";
                sDate = r.選考メール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました";
                kDate = "未送信";
            }

            if (r.Is選考メール送信日時Null() && !r.Is断りメール送信日時Null())
            {
                // 断りのみ送信済み
                msg += "お断りメールは送信済みですが、選考メールは未送信です。この依頼案件を終了してよろしいですか？";
                sDate = "未送信";
                kDate = r.断りメール送信日時.ToString("yyyy/MM/dd HH:mm:ss") + " に送信されました";
            }

            if (r.Is選考メール送信日時Null() && r.Is断りメール送信日時Null())
            {
                // 選考、断り共に未送信
                msg2 = "選考、お断り共に未送信です。この依頼案件を終了してよろしいですか？";
                sDate = "未送信";
                kDate = "未送信";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(msg).Append(Environment.NewLine);
            sb.Append("選考メール： ").Append(sDate).Append(Environment.NewLine);
            sb.Append("お断りメール： ").Append(kDate).Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append(msg2).Append(Environment.NewLine);

            if (MessageBox.Show(sb.ToString(), "修了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            // 依頼終了処理
            r.依頼終了 = global.FLGON;

            // メッセージ
            sb.Clear();
            sb.Append("依頼案件「");
            sb.Append(r.ID.ToString()).Append(" " + r.依頼元).Append("」は終了しました");

            MessageBox.Show(sb.ToString(), "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // データベース更新
            gAdp.Update(dts.ガイド依頼名);
            gAdp.Fill(dts.ガイド依頼名);

            return true;
        }

        private void dg1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            showMailBody(e.RowIndex);
        }

        private void showMailBody(int rIndex)
        {
            string sDate = dg1[cDate, rIndex].Value.ToString();
            string sFrom = dg1[cFrom, rIndex].Value.ToString();
            string sSubject = dg1[cSubject, rIndex].Value.ToString();
            string sBody = dg1[cBody, rIndex].Value.ToString();

            frmMailBody frm = new frmMailBody(sDate, sFrom, sSubject, sBody);
            frm.ShowDialog();
        }

        private void dg2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                long iNum = long.Parse(dg2[cIraiNum, e.RowIndex].Value.ToString());

                if (iraiCloseUpdate(iNum))
                {
                    // 依頼返信状況表示
                    reReqestSum();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //接続
            PopTest();

            //インターバルセット
            //timer1.Interval = sReceiveSpan * 60000; // 分単位
            timer1.Interval = sReceiveSpan * 1000; // 秒単位
            timer1.Enabled = true;

            //受信ボタン
            linkLabel2.Enabled = true;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 受信
            PopTest();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 選考チェック行があるか
            bool bOn = false;
            for (int i = 0; i < dg3.RowCount; i++)
            {
                if (dg3[cCheck, i].Value.ToString() == "True")
                {
                    bOn = true;
                    break;
                }
            }

            // 選考通知者がいない場合の確認メッセージ
            if (!bOn)
            {
                if (MessageBox.Show("選考通知対象者がありません。全員がお断りメールとなります。" + Environment.NewLine + "よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (reMailSendto(dg3) != null)
            {
                // 結果メール送信画面へ遷移します
                frmReMailSend frm = new frmReMailSend(reMailSendto(dg3));
                frm.ShowDialog();

                /* 結果メール送信でガイド依頼データの「依頼終了」情報が変更されるので
                返信状況を再表示する */
                this.Cursor = Cursors.WaitCursor;

                lAdp.Fill(dts.メール送受信記録);
                gAdp.Fill(dts.ガイド依頼名);
                rAdp.Fill(dts.ガイド依頼対象者);

                reReqestSum();

                this.Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("送信対象者がありません", "返信メール送信", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
