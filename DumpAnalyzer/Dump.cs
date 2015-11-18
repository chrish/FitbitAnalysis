using System;
using System.IO;
using System.Linq;

namespace DumpAnalyzer
{
    public class Dump
    {
        /*
            Below are the fields we know that are defined in the data received from the Fitbit Charge HR:
         */
        public string[] Header { get; protected set; }          // First 16 bytes
        public string[] Footer { get; protected set; }          // Last 9 bytes
        public string[] Data { get; protected set; }            // Reminder

        public string DeviceId { get; protected set; }          // First 2 bytes
        public string[] HeaderSequence { get; protected set; }  // 4 bytes; 7 thru 10
        public string[] ModelNumber { get; protected set; }     // Last 6 bytes of the header

        // All arrays in big endian
        public string[] FooterBegin { get; protected set; }     // First 3 bytes of the footer: Tracker packets (count?), End dump response, Megadump end
        public string[] DumpChecksum { get; protected set; }    // Bytes 4+5
        public string[] DumpSize { get; protected set; }        // Size of dump in little endian. Header + data.

        public string Filename { get; protected set; }
        /*
            Then some additional data just for fun. Or something.
         */
        int DataLength;         // Length of data (no header/footer)


        public Dump(string filename)
        {
            Filename = Path.GetFileName(filename);

            string data = "";
            try
            {
                data = File.ReadAllText(filename).Replace('\n', ' ');
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);

            }
            string[] bytes = data.Split(' ');

            if (bytes.Length < 26)
            {
                throw new FileLoadException("File does not contain a valid dump. (Too short)");
            }

            Header = bytes.Take(16).ToArray();
            Footer = bytes.Skip(bytes.Length - 10).Take(9).ToArray();
            Data = bytes.Skip(16).Take(bytes.Length - 26).ToArray();

            DeviceId = Header[0];
            HeaderSequence = Header.Skip(6).Take(4).ToArray();
            ModelNumber = Header.Skip(10).Take(6).ToArray();

            FooterBegin = Footer.Take(3).ToArray();
            DumpChecksum = Footer.Skip(3).Take(2).ToArray();
            DumpSize = Footer.Skip(5).Take(2).ToArray();

            // Correcting endianess
            Array.Reverse(DumpChecksum);
             
        }

        public int GetDecimal(string[] from)
        {
            return GetDecimal(from, false);
        }

        public int GetDecimal(string[] from, bool littleEndian)
        {
            
            string fromData = string.Join("", from);
            
            byte[] output = Enumerable.Range(0, fromData.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(fromData.Substring(x, 2), 16))
                     .ToArray();



            int sum = BitConverter.ToInt16(output,0);
            
            return sum;
        }

        public int GetCalcLength()
        {
            return Header.Length + Data.Length;
        }
    }
}
