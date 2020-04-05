using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.EventLogDomain;
using ABB.Robotics.Controllers.IOSystemDomain;
using ABB.Robotics.Controllers.RapidDomain;

namespace Error_Notification
{
    class Connections
    {
        private NetworkScanner netScan = null;
        private ControllerInfo[] ctrlInfo = new ControllerInfo[1];
        private Controller aController = null;
        private IOFilterTypes currentFilter = IOFilterTypes.All;
        private Mastership _Mastership = null;
        private Rapid rapid = null;
        private ABB.Robotics.Controllers.RapidDomain.Task task = null;
        
        public Controller RAB_ConnectToRob()
        {
            // scanning the network for the controllers
            NetworkScanner netScan = new NetworkScanner();

            // getting all available controllers from the network
            ctrlInfo = netScan.GetControllers();

            // can be used like this, if you know which controller inside of ctrlInfo[] array.
            aController = new Controller(ctrlInfo[0]);
            
            return aController;
        }
    }
}
