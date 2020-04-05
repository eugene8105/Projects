using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.EventLogDomain;
using System;
using System.Text.RegularExpressions;

namespace Error_Notification
{
    class LogEditor
    {
        //Connections cnn_01;
        EmailSetup emailPreparation = new EmailSetup();

        private string outputLine = null;
        public string OutputLine
        {
            get { return outputLine; }
            set { outputLine = value; }
        }

        private string description = null;
        public string Description
        {
            get { return description; }
        }
        private string errorNumber = null;
        public string ErrorNumber
        {
            get { return errorNumber; }
        }
        private string errorTitle = null;
        public string ErrorTitle
        {
            get { return errorTitle; }
        }
        private string errorType = null;
        public string ErrorType
        {
            get { return errorType; }
        }
        private string errorTime = null;
        public string ErrorTime
        {
            get { return errorTime; }
        }
        private string errorBody = null;
        public string ErrorBody
        {
            get { return errorBody; }
            set { errorBody = value; }
        }
        private string bodyText = null;
        public string BodyText
        {
            get { return BodyText; }
        }

        public string StName { get; set; }

        private ControllerInfo[] ctrlInfo;
        private Controller aController = null;
        private ControllerInfoCollection cntInfoColl;
        private EventLog log;
        
        public LogEditor()
        {

            NetworkScanner netScan = new NetworkScanner();
            netScan.Scan();

            ctrlInfo = netScan.GetControllers();

            foreach (var i in ctrlInfo)
            {
                if (i.SystemName.ToString() == "2600-109425") // ABB IRC5 Controller number 26-00268, 4600-800484
                {
                    aController = new Controller(i);
                }
            }
            // can be used like this, if you know which controller inside of ctrlInfo[] array.
            //aController = new Controller(ctrlInfo[0]);

            // using ControllerInfoCollection cntInfoColl - 
            //cntInfoColl = netScan.Controllers;

            log = aController.EventLog;

            Console.WriteLine($"Application is running.");
            aController.EventLog.MessageWritten += _errorEventHappened;

            Console.ReadLine();

        }

        /// <summary>
        ///  _errorEventHappened method - triggered if event happened.
        ///  In this case if message was written to the Event log (like an error)
        ///  it will record information about that error to the variables inside this method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _errorEventHappened(object sender, MessageWrittenEventArgs e)
        {
            
            var test = e.Message;
            if ((e.Message.Type.ToString() == "Error") && (aController.OperatingMode.ToString() == "Auto"))
            {
                description = "Description";
                errorNumber = test.Number.ToString();
                errorTitle = test.Title.ToString();
                errorType = test.Type.ToString();
                errorTime = test.Timestamp.ToString();
                bodyText = test.Body.ToString();

                // what does - Dispose method do?????????? Why I need to dispose???
                test.Dispose();

                // formating a string from all HTML/XML tags
                TextEditor();
                emailPreparation.SendThroughOUTLOOK();
            }
            
            
        } // end of _errorEventHappened method

        /// <summary>
        /// In TextEditor method information from _errorEventHappened method is getting processed
        /// and set up for an email
        /// </summary>
        public void TextEditor()
        {
            if (aController.SystemName.ToString() == "26-00268")
            {
                emailPreparation.StreetName = "W04";
            }
            outputLine = Regex.Replace(bodyText, "<.*?>", System.String.Empty);
            //ar = Regex.Split(outputLine, pt);
            //Console.ReadKey();
            outputLine = Regex.Replace(outputLine, @"\.(?! |$)", ". ");
            outputLine = outputLine.Replace("1)", "\n\n\bActions \n1) ");
            outputLine = outputLine.Replace("2)", "\n2) ");
            outputLine = outputLine.Replace("3)", "\n3) ");
            errorBody = outputLine;
            emailPreparation.Subject = $"{errorTime} - {errorTitle}";
            emailPreparation.Massage = $"{errorTime} \n{errorNumber}: {errorTitle}\n\n{description} \n{errorBody}";
            Console.WriteLine($"massage from ~Subject {emailPreparation.Subject}");
            Console.WriteLine($"massage from ~Massage {emailPreparation.Massage}");

        } // end of TextEditor method

    } // end EventLogs class
}
