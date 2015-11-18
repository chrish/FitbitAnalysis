using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DumpAnalyzer;

namespace DumpAnalyzerTests
{
    [TestClass]
    public class DumpTest
    {
        string path = @"C:\Users\christoffer.hafsahl\Documents\Private\HiG\IMT4012 - Forensic Analysis\Dumps\";
        string file = "dump-1447361266.txt";

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestDump()
        {
            Dump d = new Dump(path + file);

            string Header = "2D 02 00 00 01 00 5F 01 00 00 EC 2B 18 32 4E 12";
            string Footer = "C0 42 0D 6A 05 93 03 00 00";         // Last 9 bytes
            string Data =
                "6C 1F 55 08 95 F7 57 6A 93 B6 E1 2D A3 46 F8 CB 88 E6 44 BE C7 03 9F 61 02 B4 D5 25 47 48 50 3C 1D EC 6D 5D 93 58 58 75 6E 0E 73 3C DE D9 99 74 48 B9 1B CB 00 40 10 54 12 A0 26 D8 AE 46 8D 89 5F 0C 10 A9 96 01 B5 6C 80 DD 6D B3 FD E5 56 1B 1E 32 18 CE 1C C0 E2 55 E2 24 6B 7C 18 3E 1D 37 6B 40 91 79 A0 F3 11 56 D2 65 42 A1 F5 BA A1 46 29 FB 10 22 34 94 60 EE A5 9D F2 B1 32 45 8C 0F 7C 14 3F 48 61 5F A9 B0 6A 2D 50 1A 67 18 B8 0C 61 65 C1 0B 84 BD FC 0D 42 16 02 09 74 C7 DC AD F5 07 37 13 D2 B7 EE DD 3D 40 17 F4 00 89 53 B4 DA EE 2F 60 25 E3 F8 D1 39 CA 96 F5 17 08 01 03 67 4B D2 90 FF 19 BE 44 94 32 FA 74 4E 83 07 47 68 67 FF 87 DA 5B 39 F3 EB 38 3D 03 F1 22 BA 32 C7 59 ED 9D 29 55 2C 8A F5 07 8E B8 ED 1E C6 50 83 A2 41 39 9D E5 45 84 23 0F C0 F9 48 76 1F C8 92 9B CB 81 C3 60 85 69 AA 85 BD 49 77 DF BA 75 31 57 B8 9E 49 08 D7 AD 82 76 AE B6 17 FC 03 DF 38 9F 73 B4 B7 8D B9 5F 87 0B EE F4 07 45 C6 1B A9 16 50 55 30 2C 18 3E A4 B5 C0 AF 2E 75 BE 87 83 B2 BD 99 C0 E3 58 4E 2D E2 CC E2 FB 7B E5 CB 04 04 52 BD E6 40 7F 75 05 2C AF 9A 37 8F D4 28 83 B2 AF 06 B6 C7 8F E5 2F EF 23 C5 F3 9E 8F 54 4D 11 9C 8B 84 90 DA 69 75 2D 81 DA D4 36 A1 40 E0 46 65 88 02 88 EA 35 21 48 AC 00 4C FE 5D 9C 90 AE 6A 2D 33 89 A1 30 5A D8 72 A8 18 83 72 96 EA 0D B2 3F BB FF 22 28 91 20 DC DE C7 BC 3C 49 64 D5 40 5A C1 95 D8 CC DD 65 E4 53 EF 08 F9 40 27 C8 1B 11 7B 22 21 46 08 12 73 2B 20 01 CD 39 93 FA CC 07 80 59 DD 53 EB 1B 78 E1 73 A0 77 A7 5D 31 64 06 8F F9 30 2C E9 39 40 E8 81 2B E6 27 7C 40 8A 4F 9E 82 EA 29 80 10 0D 03 48 F9 94 6B 69 A6 59 B7 5F E9 7D 59 FC 70 95 DB 32 40 7A EF 84 D5 97 BD 07 9E 65 1E 01 A4 8E D7 B7 11 02 0F FA CE 42 EA 4F 0F 8E 43 1A E9 04 08 9E C3 6C F8 FF 7B A6 BD 40 0F C6 BD EE 11 BF 08 F1 CD 38 FE 69 0C CF 1E 69 06 F6 DF B9 C4 75 98 12 D5 BA 24 DE 61 DA A1 70 C8 D3 52 E7 73 78 3C 5C 87 49 DF A3 FC 7A 21 44 A2 96 22 C4 56 22 E2 41 A2 AA F4 C4 07 38 42 03 29 83 B1 20 97 AA E1 0E 0B F5 23 83 FC 3F 13 87 2D 8A 48 7B 8F 27 E8 BC E1 D9 40 97 49 F3 8F 29 77 FC 6F 81 73 A7 03 69 79 FC 0E 48 29 9C 1E 18 F5 FF 6D 09 B5 F1 A1 34 7C 75 A5 C1 C8 63 68 81 DB C9 DE 40 0E 68 E9 EE 28 DD E5 3E 2F 6B 27 90 E6 29 6C D7 F9 3B D1 E7 43 43 E0 9F 83 C8 C2 B8 4C A1 B7 28 CC DB EE 9F B7 BB B5 1B 05 C4 38 A7 45 4E 93 39 6A 57 41 DC 02 D7 67 3F 85 F3 25 6D 00 85 AD 43 8C 9F 96 BA AC 92 36 03 2C 6E E0 CA 16 EC 21 46 E5 F2 9A C0 DE AE 8C 6A BA 5A 94 AA 61 2E CE F6 38 25 5D 37 A3 BF 2E 09 8E 61 59 67 3E 1F AB A8 2C FC 8F 41 4D 25 F4 AA A1 B3 6E 9E F7 61 10 32 C6 3A 1D EE 67 C9 48 E3 20 5B 9A 37 64 7E 3A 2B 97 68 01 60 F4 97 80 D7 72 F4 B6 CE 85 FA EF 97 78 FF 88 11 9F 3E EE CA C7 16 F0 5E FA D4 58 83 88 97 ED 93 E6 9A C4 11 04 4B 67 66 AD A4 5A 33 73 C9 35 57 47 28 49 64 03 00";           // Reminder

            string DeviceId = "2D";                     // First 2 bytes
            string HeaderSequence = "5F 01 00 00";      // 4 bytes; 7 thru 10
            string ModelNumber = "EC 2B 18 32 4E 12";   // Last 6 bytes of the header

            // All arrays in big endian
            string FooterBegin = "C0 42 0D";    // First 3 bytes of the footer: Tracker packets (count?), End dump response, Megadump end
            string DumpChecksum = "05 6A";      // Bytes 4+5
            string DumpSize = "03 93";          // Dump size; bytes 6+7

            Assert.AreEqual(DeviceId, d.DeviceId);
            string Header2 = String.Join(" ", d.Header);

            string Footer2 = String.Join(" ", d.Footer);
            
            string Data2 = String.Join(" ", d.Data);

            Assert.AreEqual(Header, Header2);
            Assert.AreEqual(Footer, Footer2);
            
            File.AppendAllText(@"c:\temp\datacomp.txt", "EXP: " + Data + Environment.NewLine);
            File.AppendAllText(@"c:\temp\datacomp.txt", "ACT: " + Data2 + Environment.NewLine);
            
            Assert.AreEqual(Data.Length, Data2.Length);
            Assert.AreEqual(Data, Data2);
            Assert.AreEqual(DeviceId, string.Join(" ", d.DeviceId));
            Assert.AreEqual(HeaderSequence, string.Join(" ", d.HeaderSequence));
            Assert.AreEqual(ModelNumber, string.Join(" ", d.ModelNumber));
            Assert.AreEqual(FooterBegin, string.Join(" ", d.FooterBegin));
            Assert.AreEqual(DumpChecksum, string.Join(" ", d.DumpChecksum));
            Assert.AreEqual(DumpSize, string.Join(" ", d.DumpSize));

        }
    }
}
