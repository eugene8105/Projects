using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.IOSystemDomain;
using ABB.Robotics.Controllers.RapidDomain;
using System.Windows.Forms;


namespace Pull_Info
{
    class Testing_Pull : INwScann
    {
        public Task t { get; set; }
        public Controller Cn { get; set; }
        public NetworkScanner NetScan { get; set; }
        public ControllerInfo[] CtrlInfo { get; set; }
        public string[] StreetName { get; set; }
        public ProgramPosition P { get; set; }
        public int LinePointer { get; set; }

        public Signal Sig { get; set; }

        public void ScannNow()
        {
            NetScan = new NetworkScanner();
            NetScan.Scan();
            CtrlInfo = NetScan.GetControllers();
        } // end of ScannNow method

        public void ControllerSetup()
        {
            foreach (var i in CtrlInfo)
            {
                for (int s = 0; s < StreetName.Length; s++)
                {
                    if (StreetName[s] == i.SystemName.ToString())
                    {
                        //ControlerStreet();
                        Cn = new Controller(i);
                    }
                }

            }

        } // end of ControllerSetup method

        public void RobotTest()
        {
            StreetNamesSetup();
            ScannNow();
            ControllerSetup();

            Console.WriteLine("Operating mode: \t" + Cn.OperatingMode.ToString());
            Console.WriteLine("State:\t\t\t" + Cn.State.ToString());
            Console.WriteLine("Execution status:\t" + Cn.Rapid.ExecutionStatus.ToString());

            Console.WriteLine("");
            if (Cn.Rapid.ExecutionStatus == ExecutionStatus.Running)
                Console.WriteLine("Program is running");

            if (Cn.Rapid.ExecutionStatus == ExecutionStatus.Stopped)
                Console.WriteLine("Program is stopped");


            Console.WriteLine("");
            //Book events to get info if an status changes

            //Cn.OperatingModeChanged += _ctrl_OperatingModeChanged;
            //Cn.StateChanged += _ctrl_StateChanged;
            //Cn.Rapid.ExecutionStatusChanged += Rapid_ExecutionStatusChanged;

            // signal change event.
            Sig = Cn.IOSystem.GetSignal("doMOFF_STATE"); // signal from W30 - diCallSupportTest

            Sig.Changed += new EventHandler<SignalChangedEventArgs>(SignalChangedEven);

            using (t = Cn.Rapid.GetTask("T_ROB1"))
            {
                //Use event for program pointer changed to get the information which routine is executed
                //t.ProgramPointerChanged += Tsk_ProgramPointerChanged;

                //Position of the program pointer
                P = t.ProgramPointer;
                Console.WriteLine($"Program pointer:   \t\tModule: {P.Module}, Routine: {P.Routine}");
                Console.WriteLine("");

                Console.ReadLine();
            }
        } // end of RobotTest method

        public void StreetNamesSetup()
        {
            StreetName = new string[3];
            StreetName[0] = "4600-800484";
            StreetName[1] = ""; // 2600-104546 W25
            StreetName[2] = "2600-109425";

        } // end of streetNamesSetup method

        public void ControlerStreet()
        {
            Console.WriteLine();
            Console.WriteLine("Controller Name: {0}", Cn.SystemName.ToString());
            // Console.WriteLine("Street Name: {0}", street);
            Console.WriteLine();
        } // end of ControlerStreet method

        private void Tsk_ProgramPointerChanged(object sender, ProgramPositionEventArgs e)
        {
            Console.WriteLine($"Tsk_ProgramPointerChanged:  \tModule: {e.Position.Module}, Routine: {e.Position.Routine}");
            Console.WriteLine($"Program Pointer on line          {e.Position.Range.Begin.Row}");
            Console.WriteLine("");

        } // end of Tsk_ProgramPointerChanged method

        GmailEmail gmEmail;
        OutlookEmail outEmail;

        // method to do something if IO is on or off.
        private void SignalChangedEven(Object sender, SignalChangedEventArgs e)
        {
            DigitalSignal digitalSig = (DigitalSignal)Sig;

            int val = digitalSig.Get();

            if (val == 1)
            {
                Console.WriteLine();
                Console.WriteLine($"Signal {Sig.Name.ToString()} is changed");
                Console.WriteLine("Text message was sent.");

                //gmEmail = new GmailEmail();
                //gmEmail.SetUpEmail();

                outEmail = new OutlookEmail();
                outEmail.SendThroughOUTLOOK();
            }

        } // end of SignalChangedEven method
       
        private void Rapid_ExecutionStatusChanged(object sender, ExecutionStatusChangedEventArgs e)
        {
            Console.WriteLine("Rapid_ExecutionStatusChanged: \t" + e.Status.ToString());
            if (e.Status.ToString() == "Stopped")
            {
                t = this.Cn.Rapid.GetTask("T_ROB1");
                P = t.ProgramPointer;
                LinePointer = P.Range.Begin.Row;

                // new Form(){TopMost = true} - message box will pop up on top of everything.
                MessageBox.Show(new Form() { TopMost = true }, $"{Cn.SystemName} Stopped - Line {LinePointer}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Console.WriteLine("");
        }

        private void _ctrl_StateChanged(object sender, StateChangedEventArgs e)
        {
            Console.WriteLine("_ctrl_StateChanged: \t\t" + e.NewState.ToString());
            Console.WriteLine("");
        }

        private void _ctrl_OperatingModeChanged(object sender, OperatingModeChangeEventArgs e)
        {
            Console.WriteLine("_ctrl_OperatingModeChanged: \t" + e.NewMode.ToString());

            Console.WriteLine("");

        }

    } // end of Testing_Pull class
} // end of Pull_Info namespace
