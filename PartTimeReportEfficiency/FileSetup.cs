using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PartTimeReportEfficiency
{
    class FileSetup
    {
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        string folder = @"C:\Users\EugeneBo\Desktop\";   // @"C:\Temp\"
        public List<Data> cd { get; set; }
        public FileSetup()
        {
            cd = new List<Data>();
            FileName = "PartReport_2_27_20.csv";
            NewFileName = "PartReport.csv";
            ReadCSV();
        }
        public void ReadCSV()
        {
            try
            {
                using (var rd = new StreamReader(FileName))
                {
                    int counter = 0;
                    while (!rd.EndOfStream)
                    {
                        var splits = rd.ReadLine().Split(',');
                        
                        if (counter >= 1)
                        {
                            cd.Add(new Data
                            {
                                PartName = splits[0],
                                Time = splits[1],
                                PartEfficiency = Efficiency(Convert.ToDouble(splits[1]))
                            });
                        }
                        else
                        {
                            cd.Add(new Data
                            {
                                PartName = splits[0],
                                Time = splits[1]
                            });
                        }
                        counter++;
                        
                    }
                }
                Console.WriteLine("File was split successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem with the file: {e}");
            }

        }
        public double Efficiency(double cyTime)
        {
            double efficiencyProcent;
            return efficiencyProcent = cyTime / .8;
        }
        public void WriteToFile()
        {
            string fullPath = folder + NewFileName;
            
            using (TextWriter tw = new StreamWriter(fullPath))
            {
                foreach (var i in cd)
                {
                    tw.WriteLine($"{i.PartName}         {i.Time}         {i.PartEfficiency}");
                }
                //foreach (var s in cd)
                //    tw.WriteLine(s);
            }
        }
    } // end of FileSetup
} // end of PartTimeReportEfficiency
