using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Syslog_Test_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Syslogger.Syslog s = new Syslogger.Syslog();
            s.server = textBox1.Text;
            s.port = Int32.Parse(textBox3.Text);
            if (checkBox1.Checked) { s.throw_errors = true; }
            bool result = s.SendMessage(textBox2.Text);

            if (result)
            {
                label5.Text = "Success!";
            }
            else
            {
                label5.Text = "Failure";
            }

            // s.server = "127.0.0.1";
            // label1.Text = s.SendMessage("syslogger FTW!").ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
