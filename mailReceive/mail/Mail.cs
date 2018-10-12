using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace mailReceive.mail
{
	/// <summary>
	/// ���[���w�b�_�����擾���邽�߂̃N���X�ł��B
	/// </summary>
	public class MailHeader
	{
		/// <summary>���[���w�b�_��</summary>
		private string mailheader;

		/// <summary>
		/// �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="mail">���[���{�́B</param>
		public MailHeader(string mail)
		{
			// ���[���̃w�b�_���ƃ{�f�B���� 1�ȏ�̋�s�ł킯���Ă��܂��B
			// ���K�\�����g���ăw�b�_���݂̂����o���܂��B
			Regex reg = new Regex(@"^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline);
			Match m = reg.Match(mail);
			this.mailheader = m.Groups["header"].Value;
		}

		/// <summary>
		/// �w�b�_���S�̂�Ԃ��܂��B
		/// </summary>
		public string Text
		{
			get { return this.mailheader; }
		}

		/// <summary>
		/// �w�b�_�̊e�s��Ԃ��܂��B
		/// name �� null�A�������́A�󕶎����n�����ꍇ�͂��ׂẴw�b�_��Ԃ��܂��B
		/// </summary>
		public string[] this[string name]
		{
			get
			{
				// Subject: line1
				//          line2
				// �̂悤�ɕ����s�ɕ�����Ă���w�b�_��
				// Subject: line1 line2
				// �ƂȂ�悤�� 1�s�ɂ܂Ƃ߂܂��B
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

				// name �Ɉ�v����w�b�_�݂̂𒊏o
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
		/// �f�R�[�h���܂��B
		/// </summary>
		/// <param name="encodedtext">�f�R�[�h���镶����B</param></param>
		/// <returns>�f�R�[�h�������ʁB</returns>
		public static string Decode(string encodedtext)
		{
			string decodedtext = "";
			while (encodedtext != "")
            {
                // ���s���폜�F2018/10/12
                var reg = new Regex("\r\n");
                string text = reg.Replace(encodedtext, string.Empty);

                Regex r = new
					Regex(@"^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$");
				//Match m = r.Match(encodedtext); // 2018/10/12
				Match m = r.Match(text);    // 2018/10/12

                if (m.Groups["charset"].Value == "" || m.Groups["encoding"].Value == "" || m.Groups["encodedtext"].Value == "")
				{
					// �G���R�[�h���ꂽ������͂Ȃ�
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

                        // Quoted-Printable�̃f�R�[�h�����@2018/10/12
                        decodedtext += qpDecode(m.Groups["encodedtext"].Value, enc);

                        // �ȉ��Q�s 2018/10/12 �R�����g��
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
						// ���T�|�[�g
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
        ///     Quoted-Printable�̃f�R�[�h�F2018/10/12 </summary>
        /// <param name="text">
        ///     �f�R�[�h�Ώە�����</param>
        /// <param name="encoding">
        ///     �����R�[�h</param>
        /// <returns>
        ///     �f�R�[�h���ʕ�����</returns>
        ///-----------------------------------------------------------------
        public static string qpDecode(string text, Encoding encoding)
        {
            var bytes = new List<Byte>();

            //// �\�t�g���s�����ׂč폜
            //var reg = new Regex("=[ \t]*\r\n");
            //text = reg.Replace(text, string.Empty);

            // ���ɓǂޕ����ʒu
            var index = 0;

            while (index < text.Length)
            {
                var c = text[index];

                if (c == '=')
                {
                    // = ���o���玟��2���������킹�ēǂ�
                    // �͈͊O�Q�ƂɂȂ�ꍇ�͕s����QP
                    var octetStr = text.Substring(index + 1, 2);

                    // 16�i��������Ƃ���Byte�^�ɕϊ�
                    var octet = Convert.ToByte(octetStr, 16);
                    bytes.Add(octet);

                    // �r�ݍ���2���������߂�
                    index += 2;
                }
                else
                {
                    // = �Ŏn�܂�Ȃ������͂��̂܂܏o��
                    // Ascii�����Ȃ̂�Byte�^�ɃL���X�g
                    bytes.Add((byte)c);
                }

                index++;
            }

            var result = encoding.GetString(bytes.ToArray());
            return result;
        }
    }

	/// <summary>
	/// ���[���{�f�B�����擾���邽�߂̃N���X�ł��B
	/// </summary>
	public class MailBody
	{
		/// <summary>���[���{�f�B��</summary>
		private string mailbody;

		/// <summary>�e�}���`�p�[�g���̃R���N�V����</summary>
		private MailMultipart[] multiparts;

		/// <summary>
		/// �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="mail">���[���{�́B</param>
		public MailBody(string mail)
		{
			// ���[���̃w�b�_���ƃ{�f�B���� 1�ȏ�̋�s�ł킯���Ă��܂��B
			// ���K�\�����g���ăw�b�_���A�{�f�B�������o���܂��B
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
		/// �{�f�B���S�̂�Ԃ��܂��B
		/// </summary>
		public string Text
		{
			get { return this.mailbody; }
		}

		/// <summary>
		/// �}���`�p�[�g���̃R���N�V������Ԃ��܂��B
		/// </summary>
		public MailMultipart[] Multiparts
		{
			get { return this.multiparts; }
		}
	}

	/// <summary>
	/// �ЂƂ̃}���`�p�[�g��������킷�N���X�ł��B
	/// </summary>
	public class MailMultipart
	{
		/// <summary>���[���{��</summary>
		private string mail;

		/// <summary>
		/// �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="mail">���[���{�́B</param>
		public MailMultipart(string mail)
		{
			this.mail = mail;
		}

		/// <summary>
		/// �w�b�_�����擾���܂��B
		/// </summary>
		public MailHeader Header
		{
			get { return new MailHeader(this.mail); }
		}

		/// <summary>
		/// �{�f�B�����擾���܂��B
		/// </summary>
		public MailBody Body
		{
			get { return new MailBody(this.mail); }
		}
	}

	/// <summary>
	/// ���[����\���N���X�ł��B
	/// </summary>
	public class Mail
	{
		/// <summary>���[���{��</summary>
		private string mail;

		/// <summary>
		/// �R���X�g���N�^�ł��B
		/// </summary>
		/// <param name="mail">���[���{�́B</param>
		public Mail(string mail)
		{
			// �s���̃s���I�h2���s���I�h1�ɕϊ�
			this.mail = Regex.Replace(mail, @"\r\n\.\.", "\r\n.");
		}

		/// <summary>
		/// �w�b�_�����擾���܂��B
		/// </summary>
		public MailHeader Header
		{
			get { return new MailHeader(this.mail); }
		}

		/// <summary>
		/// �{�f�B�����擾���܂��B
		/// </summary>
		public MailBody Body
		{
			get { return new MailBody(this.mail); }
		}
	}
}
