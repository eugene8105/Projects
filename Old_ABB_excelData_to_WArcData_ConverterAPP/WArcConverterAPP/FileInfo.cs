using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace WArcConverterAPP
{
    public class FileInfo
    {
        /// <summary>
        ///   InvSched.: 0 - CV
        ///            128 - Pulls
        ///   Wire in InvSched is in m/min
        ///        in WArc is mm/sec
        ///        1 m/min = 16.7 mm/sec
        /// </summary>

        private string weldSpeed = "speed";
        public string wireFeed = "";
        public string startWire = "";
        public string endWire = "";

        private string wArcCVSchedul = "5";
        private string wArcPullsSchedul = "272";

        private string wArcPullsStartVolt = ".98";
        private string wArcPullEndVolt = ".98";
        private string wArcPullsVolt = ".98";

        private const string program = "5";

        private const string weaveLenght = "weaveLenght";
        private const string weaveWidth = "weaveWidth";
        private const string purgeTime = "0.2";
        private const string preflowTime = "0.1";
        private const string scrapeTime = "0";
        private const string moveDelay = "0.2";
        private const string postflowTime = "0.05";

        double mmSec = 16.7;
        
        string[] fileLines;
        string[] splitLine;

        string path = "Awc_Data.csv";
        List<WeldingData> weldDataList = new List<WeldingData>();
        WeldingData wd = new WeldingData();
        // bead data
        string wBD = $"TASK PERS arcbeaddata bd0:=[11,[2,0.2,0,0],[11,5,26.5,270,0,0,0,0,0],210];";

        // [[purge_time,preflow_time,scrape_type,start_move_delay,[welder_mode,welder_prog,voltage,wirefeed,current,ctrl_1,ctrl_2,ctrl_3,ctrl_4,.......[cool_time,fill_time,[welder_mode,welder_prog,voltage,wirefeed,current,ctrl_1,ctrl_2,ctrl_3,ctrl_4,burnback.............
        string wSE = "TASK PERS arcstartenddata se0:=[[0.2,0.1,0,0.2,[5,5,1,200,0,0,0,0,0],0,0,[0,0,0,0,0,0,0,0,0]]," +
                     "[0,0.5,[5,5,0.89,200,0,0,0,0,0],0.04,[0,0,0.89,150,0,0,0,0,0],0,0.05]];";

        string wTr = "TASK PERS arctrkdata trkData:=[0,30,50,0,0,15];";

        public FileInfo()
        {
            FileScanner();
            FileCreation();

        } // end of FileInfo constructor
        
        /// <summary>
        ///  FileScanner method is scanning file and spiting by lines.
        ///  and creating List of WeldingData objects and populates all data.
        /// </summary>
        public void FileScanner()
        {   
            fileLines = File.ReadAllLines(path);
            // test
            for (int i = 3; i < fileLines.Length; i++)
            {
                splitLine = fileLines[i].Split(',');


                // equations for converting miters/minuets to milliliters/second
                wireFeed = Convert.ToString(Math.Round(Convert.ToDouble(splitLine[13]) * Convert.ToDouble(mmSec)));
                startWire = Convert.ToString(Math.Round(Convert.ToDouble(splitLine[3]) * Convert.ToDouble(mmSec)));
                endWire = Convert.ToString(Math.Round(Convert.ToDouble(splitLine[8]) * Convert.ToDouble(mmSec)));

                /// <summary>
                ///  checking if schedule is CV or Pulls
                /// </summary>
                /// 0 - CV set up
                if (splitLine[15] == "0")
                {
                    weldDataList.Add(new WeldingData
                    {
                        WArcSchedul = wArcCVSchedul,
                        Program = program,
                        WeldSpeed = weldSpeed, // WeldSpeed = weldSpeed,
                        WeaveLenght = weaveLenght,
                        WeaveWidth = weaveWidth,
                        PurgeTime = purgeTime,
                        PreflowTime = preflowTime,
                        ScrapeTime = scrapeTime,
                        MoveDelay = moveDelay,
                        PostflowTime = postflowTime,
                        Wire = wireFeed,
                        //
                        S4CSchedule = splitLine[0],
                        StartVolt = splitLine[2],
                        StartWire = startWire,
                        StartTime = splitLine[4],
                        EndVolt = splitLine[7],
                        EndWire = endWire,
                        FillTime = splitLine[9],
                        BurnBack = splitLine[10],
                        Volt = splitLine[11],
                        Amperage = splitLine[12],
                        AdptVoltOff = splitLine[14],
                        GainY = splitLine[17],
                        GainZ = splitLine[18],
                        MinWeave = splitLine[21],
                        MaxWeave = splitLine[22],
                        MinSpeed = splitLine[23],
                        MaxSpeed = splitLine[24],
                        MCY = splitLine[25],
                        MCZ = splitLine[26]
                    });
                }

                // 128 - Pulls set up
                else
                {
                    weldDataList.Add(new WeldingData
                    {
                        //WArcCVSchedul = wArcCVSchedul,
                        WArcSchedul = wArcPullsSchedul,
                        Program = program,
                        WeldSpeed = weldSpeed,
                        WeaveLenght = weaveLenght,
                        WeaveWidth = weaveWidth,
                        PurgeTime = purgeTime,
                        PreflowTime = preflowTime,
                        ScrapeTime = scrapeTime,
                        MoveDelay = moveDelay,
                        PostflowTime = postflowTime,
                        Wire = wireFeed,
                        //
                        S4CSchedule = splitLine[0],
                        StartVolt = wArcPullsStartVolt,
                        StartWire = startWire,
                        StartTime = splitLine[4],
                        EndVolt = wArcPullEndVolt,
                        EndWire = endWire,
                        FillTime = splitLine[9],
                        BurnBack = splitLine[10],
                        Volt = wArcPullsVolt,
                        Amperage = splitLine[12],
                        AdptVoltOff = splitLine[14],
                        GainY = splitLine[17],
                        GainZ = splitLine[18],
                        MinWeave = splitLine[21],
                        MaxWeave = splitLine[22],
                        MinSpeed = splitLine[23],
                        MaxSpeed = splitLine[24],
                        MCY = splitLine[25],
                        MCZ = splitLine[26]
                    });
                }
                //  Wire = Convert.ToInt32(splitLine[13]) * Convert.ToInt32(mmSec),
                //splitLine[13] = (Convert.ToInt32(splitLine[13]) * Convert.ToInt32(mmSec)).ToString();
                
            }
            Console.WriteLine();
            Console.WriteLine("File scanning and data separation is completed.");
            Console.WriteLine();
        } // end of FileScanner method

        /// <summary>
        ///  FileCeation method - creates a new file and creates new string with new data from List of WeldingData objects
        /// </summary>
        public void FileCreation()
        {
            // creates a file with all data
            string path = @"TestWArcData.mod";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var dt in weldDataList)
                    {
                        sw.WriteLine($"S4C Schedule - {dt.S4CSchedule} data:");
                        sw.WriteLine(wBD = $"TASK PERS arcbeaddata bd{dt.S4CSchedule}:=[{dt.WeldSpeed},[{dt.WeaveLenght},{dt.WeaveWidth},0,0],[{dt.WArcSchedul},{dt.Program},{dt.Volt},{dt.Wire.ToString()},0,0,0,0,0],{dt.Amperage}];");
                        sw.WriteLine(wSE = $"TASK PERS arcstartenddata se{dt.S4CSchedule}:=[[{dt.PurgeTime},{dt.PreflowTime},{dt.ScrapeTime},{dt.MoveDelay},[{dt.WArcSchedul},{dt.Program},{dt.StartVolt},{dt.StartWire},0,0,0,0,0],0,0,[0,0,0,0,0,0,0,0,0]]," +
                                           $"[0,{dt.FillTime},[{dt.WArcSchedul},{dt.Program},{dt.StartVolt},{dt.StartWire},0,0,0,0,0],{dt.BurnBack},[0,0,{dt.EndVolt},{dt.EndWire},0,0,0,0,0],0,{dt.PostflowTime}]];");
                        sw.WriteLine(wTr = $"TASK PERS arctrkdata trk{dt.S4CSchedule}:=[0,{dt.GainY},{dt.GainZ},0,0,15];");
                        sw.WriteLine("");
                    }
                    Console.WriteLine("File with WArc data was created.");
                }
            }
        } // end of CreateFile method
        
    } // end of FileInfo class
}
