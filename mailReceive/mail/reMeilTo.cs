using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mailReceive.mail
{
    ///----------------------------------------------------------
    /// <summary>
    ///     結果メール送信先クラス </summary>
    ///----------------------------------------------------------
    public class reMeilTo
    {
        public long iCode;          // 依頼番号
        public int mailStatus;      // メール種別
        public DateTime reDate;     // 返信日時
        public double saNum;        // 組合員番号
        public string name;         // 氏名
        public int wDays;           // 当年稼働日数
        public string mailAddress;  // メールアドレス
        public string address;      // 住所
        public int reqID;           // ガイド依頼対象者データID
    }
}
