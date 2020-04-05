using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.ConfigurationDomain;
using ABB.Robotics.Controllers.RapidDomain;

namespace Pull_Info
{
    interface INwScann
    {
        Controller Cn { get; set; }
        NetworkScanner NetScan { get; set; }
        ControllerInfo[] CtrlInfo { get; set; }
        Task t { get; set; }
        ProgramPosition P { get; set; }
        int LinePointer { get; set; }


        void ScannNow();
    }
}
