using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace mailReceive.mail
{
	/// <summary>
	/// メールヘッダ部を取得するためのクラスです。
	/// </summary>
	public class MailHeader
	{
		/// <summary>メールヘッダ部</summary>
		private string mailheader;

		/// <summary>
		/// コンストラクタです。
		/// </summary>
		/// <param name="mail">メール本体。</param>
		public MailHeader(string mail)
		{
			// メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
			// 正規表現を使ってヘッダ部のみを取り出します。
			Regex reg = new Regex(@"^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline);
			Match m = reg.Match(mail);
			this.mailheader = m.Groups["header"].Value;
		}

		/// <summary>
		/// ヘッダ部全体を返します。
		/// </summary>
		public string Text
		{
			get { return this.mailheader; }
		}

		/// <summary>
		/// ヘッダの各行を返します。
		/// name に null、もしくは、空文字列を渡した場合はすべてのヘッダを返します。
		/// </summary>
		public string[] this[string name]
		{
			get
			{
				// Subject: line1
				//          line2
				// のように複数行に分かれているヘッダを
				// Subject: line1 line2
				// となるように 1行にまとめます。
				string header = Regex.Replace(this.mailheader, @"\r\n\s+", " ");

				if (name != null && name != "")
				{
					if (!name.EndsWith(":"))
					{
						name += ":";
					}
					name = name.ToLower();
				}
				else
				{
					name = null;
				}

				// name に一致するヘッダのみを抽出
				ArrayList ary = new ArrayList();
				foreach (string line in header.Replace("\r\n", "\n").Split('\n'))
				{
					if (name == null || line.ToLower().StartsWith(name))
					{
						ary.Add(line);
					}
				}

				return (string[])ary.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// デコードします。
		/// </summary>
		/// <param name="encodedtext">デコードする文字列。</param></param>
		/// <returns>デコードした結果。</returns>
		public static string Decode(string encodedtext)
		{
			string decodedtext = "";
			while (encodedtext != "")
            {
                // 改行を削除：2018/10/12
                var reg = new Regex("\r\n");
                string text = reg.Replace(encodedtext, string.Empty);

                Regex r = new
					Regex(@"^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$");
				//Match m = r.Match(encodedtext); // 2018/10/12
				Match m = r.Match(text);    // 2018/10/12

                if (m.Groups["charset"].Value == "" || m.Groups["encoding"].Value == "" || m.Groups["encodedtext"].Value == "")
				{
					// エンコードされた文字列はない
					decodedtext += encodedtext;
					encodedtext = "";
				}
				else
				{
					decodedtext += m.Groups["preascii"].Value;
					if (m.Groups["encoding"].Value == "B")
					{
						char[] c = m.Groups["encodedtext"].Value.ToCharArray();
						byte[] b = Convert.FromBase64CharArray(c, 0, c.Length);
                        string s = Encoding.GetEncoding(m.Groups["charset"].Value).GetString(b);
						decodedtext += s;
                    }
                    else if (m.Groups["encoding"].Value == "Q")
                    {
                        Encoding enc = new UTF8Encoding(true, true);

                        // Quoted-Printableのデコード処理　2018/10/12
                        decodedtext += qpDecode(m.Groups["encodedtext"].Value, enc);

                        // 以下２行 2018/10/12 コメント化
                        //byte[] bytes = enc.GetBytes(m.Groups["encodedtext"].Value.ToCharArray());
                        //string value2 = enc.GetString(bytes);

                        //UTF8Encoding [] c = m.Groups["encodedtext"].Value.ToCharArray();
                        ////byte[] b = Convert.FromBase64CharArray(c, 0, c.Length);
                        ////string s = Encoding.GetEncoding(m.Groups["charset"].Value).GetString(b);

                        //string s = Encoding.UTF8.GetString(c);
                        //decodedtext += s;
                    }
                    else
					{
						// 未サポート
						decodedtext += "=?" + m.Groups["charset"].Value + "?" + m.Groups["encoding"].Value + "?" +
							m.Groups["encodedtext"].Value + "?=";
					}
					encodedtext = m.Groups["postascii"].Value;
				}
			}
			return decodedtext;
		}

        ///-----------------------------------------------------------------
        /// <summary>
        ///     Quoted-Printableのデコード：2018/10/12 </summary>
        /// <param name="text">
        ///     デコード対象文字列</param>
        /// <param name="encoding">
        ///     文字コード</param>
        /// <returns>
        ///     デコード結果文字列</returns>
        ///-----------------------------------------------------------------
        public static string qpDecode(string text, Encoding encoding)
        {
            var bytes = new List<Byte>();

            //// ソフト改行をすべて削除
            //var reg = new Regex("=[ \t]*\r\n");
            //text = reg.Replace(text, string.Empty);

            // 次に読む文字位置
            var index = 0;

            while (index < text.Length)
            {
                var c = text[index];

                if (c == '=')
                {
                    // = が出たら次の2文字を合わせて読む
                    // 範囲外参照になる場合は不正なQP
                    var octetStr = text.Substring(index + 1, 2);

                    // 16進数文字列としてByte型に変換
                    var octet = Convert.ToByte(octetStr, 16);
                    bytes.Add(octet);

                    // 詠み込んだ2文字すすめる
                    index += 2;
                }
                else
                {
                    // = で始まらない文字はそのまま出力
                    // Ascii文字なのでByte型にキャスト
                    bytes.Add((byte)c);
                }

                index++;
            }

            var result = encoding.GetString(bytes.ToArray());
            return result;
        }
    }

	/// <summary>
	/// メールボディ部を取得するためのクラスです。
	/// </summary>
	public class MailBody
	{
		/// <summary>メールボディ部</summary>
		private string mailbody;

		/// <summary>各マルチパート部のコレクション</summary>
		private MailMultipart[] multiparts;

		/// <summary>
		/// コンストラクタです。
		/// </summary>
		/// <param name="mail">メール本体。</param>
		public MailBody(string mail)
		{
			// メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
			// 正規表現を使ってヘッダ部、ボディ部を取り出します。
			Regex reg = new Regex(@"^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline);
			Match m = reg.Match(mail);
			string mailheader = m.Groups["header"].Value;
			this.mailbody = m.Groups["body"].Value;

			reg = new Regex(@"Content-Type:\s+multipart/mixed;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase);
			m = reg.Match(mailheader);
			if (m.Groups["boundary"].Value != "")
			{
				// multipart
				string boundary = m.Groups["boundary"].Value;
				reg = new Regex(@"^.*?--" + boundary + @"\r\n(?:(?<multipart>.*?)" + @"--" + boundary + @"-*\r\n)+.*?$", RegexOptions.Singleline);
				m = reg.Match(this.mailbody);
				ArrayList ary = new ArrayList();
				for (int i = 0; i < m.Groups["multipart"].Captures.Count; ++i)
				{
					if (m.Groups["multipart"].Captures[i].Value != "")
					{
						MailMultipart b = new MailMultipart(m.Groups["multipart"].Captures[i].Value);
						ary.Add(b);
					}
				}
				this.multiparts = (MailMultipart[])ary.ToArray(typeof(MailMultipart));
			}
			else
			{
                ////// single
                ////this.multiparts = new MailMultipart[0];


                reg = new Regex(@"Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase);
                m = reg.Match(mailheader);
                if (m.Groups["boundary"].Value != "")
                {
                    // multipart
                    string boundary = m.Groups["boundary"].Value;
                    reg = new Regex(@"^.*?--" + boundary + @"\r\n(?:(?<multipart>.*?)" + @"--" + boundary + @"-*\r\n)+.*?$", RegexOptions.Singleline);
                    m = reg.Match(this.mailbody);
                    ArrayList ary = new ArrayList();
                    for (int i = 0; i < m.Groups["multipart"].Captures.Count; ++i)
                    {
                        if (m.Groups["multipart"].Captures[i].Value != "")
                        {
                            MailMultipart b = new MailMultipart(m.Groups["multipart"].Captures[i].Value);
                            ary.Add(b);
                        }
                    }
                    this.multiparts = (MailMultipart[])ary.ToArray(typeof(MailMultipart));
                }
                else
                {
                    // single
                    this.multiparts = new MailMultipart[0];
                }
			}
		}

		/// <summary>
		/// ボディ部全体を返します。
		/// </summary>
		public string Text
		{
			get { return this.mailbody; }
		}

		/// <summary>
		/// マルチパート部のコレクションを返します。
		/// </summary>
		public MailMultipart[] Multiparts
		{
			get { return this.multiparts; }
		}
	}

	/// <summary>
	/// ひとつのマルチパート部をあらわすクラスです。
	/// </summary>
	public class MailMultipart
	{
		/// <summary>メール本体</summary>
		private string mail;

		/// <summary>
		/// コンストラクタです。
		/// </summary>
		/// <param name="mail">メール本体。</param>
		public MailMultipart(string mail)
		{
			this.mail = mail;
		}

		/// <summary>
		/// ヘッダ部を取得します。
		/// </summary>
		public MailHeader Header
		{
			get { return new MailHeader(this.mail); }
		}

		/// <summary>
		/// ボディ部を取得します。
		/// </summary>
		public MailBody Body
		{
			get { return new MailBody(this.mail); }
		}
	}

	/// <summary>
	/// メールを表すクラスです。
	/// </summary>
	public class Mail
	{
		/// <summary>メール本体</summary>
		private string mail;

		/// <summary>
		/// コンストラクタです。
		/// </summary>
		/// <param name="mail">メール本体。</param>
		public Mail(string mail)
		{
			// 行頭のピリオド2つをピリオド1つに変換
			this.mail = Regex.Replace(mail, @"\r\n\.\.", "\r\n.");
		}

		/// <summary>
		/// ヘッダ部を取得します。
		/// </summary>
		public MailHeader Header
		{
			get { return new MailHeader(this.mail); }
		}

		/// <summary>
		/// ボディ部を取得します。
		/// </summary>
		public MailBody Body
		{
			get { return new MailBody(this.mail); }
		}
	}
}
