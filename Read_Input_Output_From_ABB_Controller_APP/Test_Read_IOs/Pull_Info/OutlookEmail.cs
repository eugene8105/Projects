using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Info
{
    class OutlookEmail
    {

        Application oApp;
        
        private string subject;
        private string message;

        string[] recipients = { "sprintPhoneNumber@messaging.sprintpcs.com" };
        //  Carrier destinations
        //  ATT: Compose a new email and use the recipient's 10-digit wireless phone number, followed by @txt.att.net. For example, 5551234567@txt.att.net.
        //  Verizon: Similarly, ##@vtext.com
        //  Sprint: ##@messaging.sprintpcs.com
        //  TMobile: ##@tmomail.net
        
        // more info on https://www.c-sharpcorner.com/article/send-text-message-to-cell-phones-from-a-C-Sharp-application/

        public void SendThroughOUTLOOK()
        {
            subject = "Street 30 - robot support";
            message = "Need Robotic Support on W30.";

            try
            {
                // Create the Outlook application.
                oApp = new Application();
                // Create a new mail item.
                MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
                // Set HTMLBody. 
                //massage = "RCS, test massage!!";
                oMsg.HTMLBody = message;

                //Subject line
                //subject = "Test - Ignore.";
                oMsg.Subject = $"{subject}";
                
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

                Console.WriteLine("Text massage sent successfully.");
            }//end of try block
            catch (System.Exception ex)
            {
                Console.WriteLine("Text massage did not sent.");
                Console.WriteLine(ex.Message.ToString());
            }//end of catch
        }//end of Email Method

    } // end of OutlookEmail class
}
