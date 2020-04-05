using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace Error_Notification
{
    class EmailSetup
    {
        //MailMessage obj_mail;
        /// <summary>
        /// SmtpClient - it's a class - Simple Mail Transfer Protocol (SMTP) class
        /// </summary>
        //SmtpClient client;

        private string toAddress = null;
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }
        private string subject = null;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string massage = null;
        public string Massage
        {
            get { return massage; }
            set { massage = value; }
        }

        private const string password = "";
        public string Password
        {
            get { return password; }
        }

        private const string userName = "";
        public string UserName
        {
            get { return userName; }
        }

        public string StreetName { get; set; }

        Application oApp;
        string[] recipients;
        
        public void SendThroughOUTLOOK()
        {
            try
            {
                // Create the Outlook application.
                oApp = new Application();
                // Create a new mail item.
                MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
                // Set HTMLBody. 
                //massage = "RCS, test massage!!";
                oMsg.HTMLBody = massage;
                
                //Subject line
                //subject = "Test - Ignore.";
                oMsg.Subject = $"{StreetName} {subject}";
                // Add a recipient.
                //oMsg.To = "email address 1"; // several recipients: "email address 1; email address 2"
                recipients = new string[] { "email address here" };
                // another way it can be done with string of arrays for several recipients:
                Recipients oRecips = (Recipients)oMsg.Recipients;
                foreach (var recipient in recipients)
                {
                    Recipient oRecip = (Recipient)oRecips.Add(recipient);
                    oRecip.Resolve();
                }
                oRecips = null;
                // Send.
                oMsg.Send();
                // Clean up.
                oMsg = null;
                oApp = null;
                
                Console.WriteLine("Email sent successfully.");
            }//end of try block
            catch (System.Exception ex)
            {
                Console.WriteLine("Email did not sent.");
                Console.WriteLine(ex.Message.ToString());
            }//end of catch
        }//end of Email Method
        
    } // end of EmailSetup
}


