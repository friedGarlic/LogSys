using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class Security : Form
    {
        public Security()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendEmailNotification("monantonio09@gmail.com", "Log Notification From CCIT BUILDING", "You've forgot to Time out, duration of Log is running!");
        }
        private void SendEmailNotification(string recipient, string subject, string body)
        {
            string senderMail = "remcarl2121@gmail.com";
            string senderPass = "pyppozalwxgjesqc";

            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.To.Add(new MailAddress(recipient));
            message.From = new MailAddress(senderMail); // Specify the From address
            message.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderMail, senderPass),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
    }
}
