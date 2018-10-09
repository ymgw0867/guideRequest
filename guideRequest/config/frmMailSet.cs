using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using guideRequest.common;

namespace guideRequest
{
    public partial class frmMailSet : Form
    {
        guideDataSet dts = new guideDataSet();
        guideDataSetTableAdapters.メール設定TableAdapter adp = new guideDataSetTableAdapters.メール設定TableAdapter();

        utility.formMode f = new utility.formMode();

        public frmMailSet()
        {
            InitializeComponent();

            // データ読み込み
            adp.Fill(dts.メール設定);
        }

        private void frmSystem_Load(object sender, EventArgs e)
        {
            //ウィンドウ最小サイズ設定
            utility.WindowsMinSize(this, this.Width, this.Height);

            //ウィンドウ最大サイズ設定
            utility.WindowsMaxSize(this, this.Width, this.Height);

            txtMemo.ScrollBars = ScrollBars.Vertical;

            //登録データ表示
            DataShow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void frmSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void DataShow()
        {
            try
            {
                if (dts.メール設定.Count() > 0)
                {
                    foreach (var s in dts.メール設定.Where(a => a.ID == 1))
                    {
                        // データ表示
                        txtSmtpServerName.Text = s.SMTPサーバー;
                        txtSmtpPort.Text = s.SMTPポート番号.ToString();
                        txtPopServerName.Text = s.POPサーバー;
                        txtPopPort.Text = s.POPポート番号.ToString();
                        txtLogin.Text = s.ログイン名;
                        txtPassword.Text = s.パスワード;
                        txtFromAddress.Text = s.メールアドレス;
                        txtMemo.Text = s.署名;
                        txtMailRecSpan.Text = s.受信間隔.ToString();
                        txtMailName.Text = s.メール名称;
                        txtDataSaveMonth.Text = s.データ保存月数.ToString();

                        // 編集モード
                        f.Mode = global.FORM_EDITMODE;
                    }
                }
                else
                {
                    // 表示欄初期化
                    txtSmtpServerName.Text = string.Empty;
                    txtSmtpPort.Text = string.Empty;
                    txtPopServerName.Text = string.Empty;
                    txtPopPort.Text = string.Empty;
                    txtLogin.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtFromAddress.Text = string.Empty;
                    txtMemo.Text = string.Empty;
                    txtMailRecSpan.Text = string.Empty;
                    txtMailName.Text = string.Empty;
                    txtDataSaveMonth.Text = string.Empty;

                    // 新規登録モード
                    f.Mode = global.FORM_ADDMODE;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        /// -----------------------------------------------------------------
        /// <summary> 
        ///     入力項目のエラー検証 </summary>
        /// <returns>
        ///     true:エラー無し、false:エラー有り</returns>
        /// -----------------------------------------------------------------
        private bool ErrCheck()
        {
            // SMTPサーバー名
            if (txtSmtpServerName.Text == string.Empty)
            {
                MessageBox.Show("SMTPサーバー名が未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSmtpServerName.Focus();
                return false;
            }

            // SMTPポート番号
            if (txtSmtpPort.Text == string.Empty)
            {
                MessageBox.Show("SMTPポート番号が未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSmtpPort.Focus();
                return false;
            }

            if (utility.NumericCheck(txtSmtpPort.Text) == false)
            {
                MessageBox.Show("SMTPポート番号は数字で入力してください", "数字項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSmtpPort.Focus();
                return false;
            }

            // POPサーバー名
            if (txtPopServerName.Text == string.Empty)
            {
                MessageBox.Show("POPサーバー名が未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPopServerName.Focus();
                return false;
            }

            // POPポート番号
            if (txtPopPort.Text == string.Empty)
            {
                MessageBox.Show("POPポート番号が未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPopPort.Focus();
                return false;
            }

            if (utility.NumericCheck(txtPopPort.Text) == false)
            {
                MessageBox.Show("POPポート番号は数字で入力してください", "数字項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPopPort.Focus();
                return false;
            }

            // ログイン名
            if (txtLogin.Text == string.Empty)
            {
                MessageBox.Show("ログイン名が未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLogin.Focus();
                return false;
            }

            // パスワード
            if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("パスワードが未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Focus();
                return false;
            }

            // 送信者アドレス
            if (txtFromAddress.Text == string.Empty)
            {
                MessageBox.Show("送信者アドレスが未入力です", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFromAddress.Focus();
                return false;
            }
            
            // メール受信間隔
            if (utility.NumericCheck(txtMailRecSpan.Text) == false)
            {
                MessageBox.Show("メール受信間隔は数字で入力してください", "数字項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMailRecSpan.Focus();
                return false;
            }

            if (int.Parse(txtMailRecSpan.Text) < 1)
            {
                MessageBox.Show("メール受信間隔に０は登録出来ません", "入力値不正", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMailRecSpan.Focus();
                return false;
            }

            // データ保存期間
            if (utility.NumericCheck(txtDataSaveMonth.Text) == false)
            {
                MessageBox.Show("データ保存月数は数字で入力してください", "数字項目", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDataSaveMonth.Focus();
                return false;
            }

            return true;
        }

        private void DataAddNew()
        {
            try
            {
                guideDataSet.メール設定Row r = (guideDataSet.メール設定Row)dts.メール設定.Newメール設定Row();

                r.ID = global.mailKey;
                r.SMTPサーバー = txtSmtpServerName.Text;
                r.SMTPポート番号 = utility.StrtoZero(txtSmtpPort.Text);
                r.POPサーバー = txtPopServerName.Text;
                r.POPポート番号 = utility.StrtoZero(txtPopPort.Text);
                r.ログイン名 = txtLogin.Text;
                r.パスワード = txtPassword.Text;
                r.メールアドレス = txtFromAddress.Text;
                r.メール名称 = txtMailName.Text;
                r.受信間隔 = utility.StrtoZero(txtMailRecSpan.Text);
                r.署名 = txtMemo.Text;
                r.登録年月日 = DateTime.Now;
                r.更新年月日 = DateTime.Now;
                r.データ保存月数 = utility.StrtoZero(txtDataSaveMonth.Text);

                dts.メール設定.Addメール設定Row(r);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            finally
            {
            }

            MessageBox.Show("設定データが登録されました", "登録完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataUpdate()
        {
            try
            {
                foreach (var s in dts.メール設定.Where(a => a.ID == global.mailKey))
                {
                    s.SMTPサーバー = txtSmtpServerName.Text;
                    s.SMTPポート番号 = utility.StrtoZero(txtSmtpPort.Text);
                    s.POPサーバー = txtPopServerName.Text;
                    s.POPポート番号 = utility.StrtoZero(txtPopPort.Text);
                    s.ログイン名 = txtLogin.Text;
                    s.パスワード = txtPassword.Text;
                    s.メールアドレス = txtFromAddress.Text;
                    s.メール名称 = txtMailName.Text;
                    s.受信間隔 = utility.StrtoZero(txtMailRecSpan.Text);
                    s.署名 = txtMemo.Text;
                    s.更新年月日 = DateTime.Now;
                    s.データ保存月数 = utility.StrtoZero(txtDataSaveMonth.Text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            finally
            {
            }

            MessageBox.Show("メール設定データが更新されました", "更新完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtSmtpPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //エラーチェック
            if (!ErrCheck())
            {
                return;
            }

            //データ登録
            switch (f.Mode)
            {
                case global.FORM_ADDMODE:   //新規登録

                    if (MessageBox.Show("メール設定データを登録します。よろしいですか？", "登録確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    DataAddNew();
                    break;

                case global.FORM_EDITMODE:  //編集

                    if (MessageBox.Show("メール設定データを更新します。よろしいですか？", "登録確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    DataUpdate();

                    break;
            }

            // データ更新
            adp.Update(dts.メール設定);

            //終了
            this.Close();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("データを更新しないで終了します。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            //終了する
            this.Close();
        }
    }
}
