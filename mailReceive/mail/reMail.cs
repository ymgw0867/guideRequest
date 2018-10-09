using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mailReceive.mail
{
    ///-----------------------------------------------------
    /// <summary>
    ///     返信状況クラス </summary>
    ///-----------------------------------------------------
    class reMail
    {
        public DateTime cDate;      // 受付日
        public string iraimoto;     // 依頼元
        public DateTime wDate;      // 稼働予定日
        public string Naiyou;       // 内容
        public int reCount;         // 返信数
        public DateTime nDate;      // 最新返信日時
        public string iraiNum;      // 依頼番号
    }
}
