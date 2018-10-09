using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mailReceive.common
{
    class global
    {
        // フォームモード
        public const int FORM_ADDMODE = 0;     // 新規追加
        public const int FORM_EDITMODE = 1;    // 編集

        // ログインステータス
        public static bool loginStatus;

        // コード表表題
        public const string CODELIST_CHIIKI = "0";
        public const string CODELIST_IINKAI = "1";
        public const string CODELIST_MAILLIST = "2";
        public const string CODELIST_ISHI = "3";
        public const string CODELIST_KAIINREKI = "4";
        public const string CODELIST_GYOKEN = "5";
        public const string CODELIST_IHCSA = "6";
        public const string CODELIST_JICAJICE = "7";
        public const string CODELIST_DOUJI = "8";
        public const string CODELIST_NATIVE = "9";
        public const string CODELIST_IRAI = "10";

        // 0 or 1
        public const int FLGON = 1;
        public const int FLGOFF = 0;

        // メール設定テーブルキー
        public const int mailKey = 1;

        // 送受信ログ区分
        public const string MLLOG_SEND = "S"; 
        public const string MLLOG_REC = "R";

        // メール定型文種別配列
        public static string[] shArray = { "依頼", "決定通知", "お断り通知" };
        public const int MLCOMMENT_IRAI = 1; 
        public const int MLCOMMENT_KETTEI = 2; 
        public const int MLCOMMENT_KOTOWARI = 3;

        //通信処理ステータス
        public const int JOBOUT = 0;                //非送受信中
        public const int JOBIN = 1;                 //送受信中
        public static string Msglog;                //送受信コマンド

        // エラー表示
        public const string ERR_1 = "???"; 
        public const string ERR_2 = "??????";

        // メール定型文変換文字列
        public const string TO_NAME = "$NAME$";
    }
}
