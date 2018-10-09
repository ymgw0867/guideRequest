using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace mailReceive
{
    class utility
    {
        /// -------------------------------------------------------------------------
        /// <summary> 
        ///     任意のディレクトリのファイルを削除する </summary>
        /// <param name="sPath">
        ///     指定するディレクトリ</param>
        /// <param name="sFileType">
        ///     ファイル名及び形式</param>
        /// -------------------------------------------------------------------------
        public static void FileDelete(string sPath,string sFileType)
        {
            //sFileTypeワイルドカード"*"は、すべてのファイルを意味する
            foreach(string files in System.IO.Directory.GetFiles(sPath,sFileType))
            {
                // ファイルを削除する
                System.IO.File.Delete(files);
            }
        }


        /// -------------------------------------------------------------------------
        /// <summary>
        ///     文字列の値が数字かチェックする </summary>
        /// <param name="tempStr">
        ///     検証する文字列</param>
        /// <returns>
        ///     数字:true,数字でない:false</returns>
        /// -------------------------------------------------------------------------
        public static bool NumericCheck(string tempStr)
        {
            double d;

            if (tempStr == null) return false;

            if (double.TryParse(tempStr, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) == false)
                return false;

            return true;
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     Nullをstring.Empty("")に置き換える </summary>
        /// <param name="tempStr">
        ///     stringオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのときそのまま値を返す</returns>
        /// -------------------------------------------------------------------------
        public static string NulltoStr(string tempStr)
        {
            if (tempStr == null)
            {
                return string.Empty;
            }
            else
            {
                return tempStr;
            }
        }

        ///-----------------------------------------------------------------------
        /// <summary>
        ///     ウィンドウ最小サイズの設定 </summary>
        /// <param name="tempFrm"> 
        ///     対象とするウィンドウオブジェクト </param>
        /// <param name="wSize">
        ///     width </param>
        /// <param name="hSize">
        ///     Height </param>
        ///-----------------------------------------------------------------------
        public static void WindowsMinSize(Form tempFrm, int wSize, int hSize)
        {
            tempFrm.MinimumSize = new Size(wSize, hSize);
        }

        ///-----------------------------------------------------------------------
        /// <summary>
        ///     ウィンドウ最小サイズの設定 </summary>
        /// <param name="tempFrm">
        ///     対象とするウィンドウオブジェクト</param>
        /// <param name="wSize">
        ///     width</param>
        /// <param name="hSize">
        ///     height</param>
        ///-----------------------------------------------------------------------
        public static void WindowsMaxSize(Form tempFrm, int wSize, int hSize)
        {
            tempFrm.MaximumSize = new Size(wSize, hSize);
        }
        
        //処理モード
        public class formMode
        {
            public int Mode { get; set; }
            public int ID { get; set; }
            public Int64 ID64 { get; set; }
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     Nullをstring.Empty("")に置き換える <summary>
        /// <param name="tempStr">
        ///     stringオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのときそのまま値を返す</returns>
        /// -------------------------------------------------------------------------
        public static string NulltoStr(object tempStr)
        {
            DateTime dt;
            double val;

            if (tempStr == null)
            {
                return string.Empty;
            }
            else
            {
                if (tempStr == DBNull.Value)
                {
                    return string.Empty;
                }
                else
                {
                    if (double.TryParse(tempStr.ToString(), out val))
                    {
                        return tempStr.ToString();
                    }
                    else if (GetRegexDateType(tempStr.ToString())) // 日付の正規表現に一致するか？　2013/09/15
                    {
                        dt = DateTime.Parse(tempStr.ToString());
                        return dt.ToShortDateString();
                    }
                    // カンマの削除は行わない 2013/03/29
                    //else return tempStr.ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(",",string.Empty).Replace(@"\", string.Empty);
                    else return tempStr.ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(@"\", string.Empty);
                }
            }
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     日付の正規表現に一致するか調べます </summary>
        /// <param name="d">
        ///     対象の文字列</param>
        /// -------------------------------------------------------------------------
        public static bool GetRegexDateType(string d)
        {
            Regex r1 = new Regex(@"^\d{4}/\d{1}/\d{1}\s\d{1}:\d{2}:\d{2}$");        // 2012/4/5 0:00:00
            Regex r12 = new Regex(@"^\d{4}/\d{1}/\d{1}\s\d{2}:\d{2}:\d{2}$");       // 2012/4/5 00:00:00
            Regex r13 = new Regex(@"^\d{4}/\d{1}/\d{1}$");                          // 2012/4/5
            Regex r2 = new Regex(@"^\d{4}/\d{1}/\d{2}\s\s\d{1}:\d{2}:\d{2}$");      // 2012/4/15 0:00:00
            Regex r22 = new Regex(@"^\d{4}/\d{1}/\d{2}\s\d{2}:\d{2}:\d{2}$");       // 2012/4/15 00:00:00
            Regex r23 = new Regex(@"^\d{4}/\d{1}/\d{2}$");                          // 2012/4/15
            Regex r3 = new Regex(@"^\d{4}/\d{2}/\d{1}\s\d{1}:\d{2}:\d{2}$");        // 2012/11/5 0:00:00
            Regex r32 = new Regex(@"^\d{4}/\d{2}/\d{1}\s\d{2}:\d{2}:\d{2}$");       // 2012/11/5 00:00:00
            Regex r33 = new Regex(@"^\d{4}/\d{2}/\d{1}$");                          // 2012/11/5
            Regex r4 = new Regex(@"^\d{4}/\d{2}/\d{2}\s\d{1}:\d{2}:\d{2}$");        // 2012/11/15 0:00:00,2012/04/09 0:00:00
            Regex r42 = new Regex(@"^\d{4}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2}$");       // 2012/11/15 00:00:00,2012/04/09 00:00:00
            Regex r43 = new Regex(@"^\d{4}/\d{2}/\d{2}$");                          // 2012/11/15,2012/04/09

            if (Regex.IsMatch(d, r1.ToString())) return true;
            if (Regex.IsMatch(d, r12.ToString())) return true;
            if (Regex.IsMatch(d, r13.ToString())) return true;
            if (Regex.IsMatch(d, r2.ToString())) return true;
            if (Regex.IsMatch(d, r22.ToString())) return true;
            if (Regex.IsMatch(d, r23.ToString())) return true;
            if (Regex.IsMatch(d, r3.ToString())) return true;
            if (Regex.IsMatch(d, r32.ToString())) return true;
            if (Regex.IsMatch(d, r33.ToString())) return true;
            if (Regex.IsMatch(d, r4.ToString())) return true;
            if (Regex.IsMatch(d, r42.ToString())) return true;
            if (Regex.IsMatch(d, r43.ToString())) return true;

            return false;
        }

        //public static string NulltoStr(Excel.Range tempStr)
        //{
        //    if (tempStr == null)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return tempStr.Text;
        //    }
        //}

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     日付型データがNullのときstring.Empty("")に置き換える </summary>
        /// <param name="tempStr">
        ///     datetimeオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのときShortDateString値を返す</returns>
        /// -------------------------------------------------------------------------
        public static string NulltoStr(DateTime dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }
            else
            {
                return dt.ToShortDateString();
            }
        }
        
        ///--------------------------------------------------------------------
        /// <summary>
        ///     string型データが数字以外のとき"０"を返す </summary>
        /// <param name="tempStr">
        ///     int</param>
        /// <returns>
        ///     数字以外のとき"0"、数字のとき値をそのまま返す</returns>
        ///--------------------------------------------------------------------
        public static int StrtoZero(string dt)
        {
            if (!NumericCheck(dt))
            {
                return 0;
            }
            else
            {
                return int.Parse(dt);
            }
        }
    }
}
