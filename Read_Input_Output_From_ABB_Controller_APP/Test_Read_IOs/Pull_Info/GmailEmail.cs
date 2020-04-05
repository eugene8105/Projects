using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Info
{
    class GmailEmail
    {
        SmtpClient client;

        private string fromAddress;
        private string toAddress;
        private string subject;
        private string message;
        
        const string hostName = "smtp.gmail.com";
        const int port = 587;

        private const string password = "";

        public void SetUpEmail()
        {
            fromAddress = "eugenebobalo@gmail.com";
            toAddress = "4142434526@messaging.sprintpcs.com";
            
            //  Carrier destinations
            //  ATT: Compose a new email and use the recipient's 10-digit wireless phone number, followed by @txt.att.net. For example, 5551234567@txt.att.net.
            //  Verizon: Similarly, ##@vtext.com
            //  Sprint: ##@messaging.sprintpcs.com
            //  TMobile: ##@tmomail.net

            subject = "Street 25 - robot support";
            message = "Signal is on.";

            client = new SmtpClient(hostName, port);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(fromAddress, password);

            try
            {
                client.Send(fromAddress, toAddress, subject, message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email did not sent.");
                Console.WriteLine(ex.Message.ToString());
            }

        } // end of SetUpEmail

    } // end of Email class

} // end of Pull_Info
