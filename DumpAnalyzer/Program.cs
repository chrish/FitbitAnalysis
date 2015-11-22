using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DumpAnalyzer
{
    /// <summary>
    /// This code is copyright (c) Christoffer Hafsahl 2015 and licensed under the BSD-license. 
    ///
    /// Basic tool to get some statistics from Fitbit device dumps. This will parse 
    /// all dump_timestamp_.txt files in a directory, gather header, data and footer 
    /// information and put everything into a nicely formatted csv-file. 
    /// 
    /// No warranties and use code at own risk. Have not tested this under Mono, but 
    /// I see no reason why it shouldn't compile and execute there as well. 
    /// 
    /// A big thanks for Axelle Aprville for additional information about the format.
    /// 
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Count() == 2 && Directory.Exists(args[0]) && Directory.Exists(args[1]))
            {
                new Program(args[0], args[1]);
            }
            else
            {
                Console.WriteLine("Please add path to analyze as one parameter and output path as the other.");
            } 
        }


        public Program(string analysisPath, string outputPath)
        {
            List<Dump> dumps  = new List<Dump>();

            var files = Directory.GetFiles(analysisPath, "dump*");

            foreach (string file in files)
            {
                Dump dp = new Dump(file);
                dumps.Add(dp);
            }

            if (!outputPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                outputPath += Path.DirectorySeparatorChar;
            }

            string charsep = "\t";

            string header = "Filename\t\t\t\tHeader\t\t\t\t\t\t\t\t\t\t\t\tFooter\t\t\t\t\t\t\tDeviceId\tHeaderSeq\t\tModelNo\t\t\t\t\tFooterInd\tDumpCRC\tDumpSize\tSize in Dec\t\tCalc size";

            File.AppendAllText(outputPath, header + Environment.NewLine);

            string outfile = outputPath + "outputFromFitbitDump.txt";

            foreach (Dump d in dumps)
            {
                string output = d.Filename + "\t\t";
                    output += string.Join(" ", d.Header) + "\t\t";
                output += string.Join(" ", d.Footer) + "\t\t";
                output += d.DeviceId + "\t\t\t";
                output += string.Join(" ", d.HeaderSequence)  + "\t\t";
                output += string.Join(" ", d.ModelNumber)  + "\t\t";
                output += string.Join(" ", d.FooterBegin)  + "\t";
                output += string.Join(" ", d.DumpChecksum)  + "\t";
                output += string.Join(" ", d.DumpSize)  + "\t\t";
                output += d.GetDecimal(d.DumpSize) + "\t\t\t\t";

                if ( d.GetDecimal(d.DumpSize) < 1000)
                {
                    output += "\t";
                }

                
                output += d.GetCalcLength();

                
                File.AppendAllText(outfile, output + Environment.NewLine);
            }
        }
    }
}
