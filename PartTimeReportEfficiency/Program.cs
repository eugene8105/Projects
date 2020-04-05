using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTimeReportEfficiency
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSetup fs = new FileSetup();
            fs.WriteToFile();
        }
    }
}
