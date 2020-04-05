using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.IOSystemDomain;
using ABB.Robotics.Controllers.RapidDomain;

namespace Pull_Info
{
    class MainApp
    {
      
        static void Main(string[] args)
        {
            Testing_Pull test_01 = new Testing_Pull();
            test_01.RobotTest();

        }

        
    }
}
