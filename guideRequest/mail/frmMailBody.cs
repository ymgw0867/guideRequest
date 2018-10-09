using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guideRequest.mail
{
    public partial class frmMailBody : Form
    {
        public frmMailBody(string sDate, string sFrom, string sSubject, string sBody)
        {
            InitializeComponent();

            _sBody = sBody;
            _sDate = sDate;
            _sFrom = sFrom;
            _sSubject = sSubject;
        }

        string _sBody = string.Empty;
        string _sDate = string.Empty;
        string _sFrom = string.Empty;
        string _sSubject = string.Empty;

        private void frmMailBody_Load(object sender, EventArgs e)
        {
            utility.WindowsMaxSize(this, this.Width, this.Height);
            utility.WindowsMinSize(this, this.Width, this.Height);
            
            lblDate.Text = _sDate;
            lblFrom.Text = _sFrom;
            lblSubject.Text = _sSubject;
            txtBody.Text = _sBody;
            txtBody.SelectionStart = 0;
        }

        private void frmMailBody_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            this.Dispose();
        }
    }
}
