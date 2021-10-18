using RnMarkApp.Communication;
using RnMarkApp.Data;
using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace RnMarkApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //////////////////// REAL PROGRAM /////////////////////////
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            //*

            ClientPrinter.SetupConnection();

            ClientPlc.SetupConnection();

            ClientPlc.GetRequest();
            // */

            ////////////////////// PRINTER COMMAND TEST ////////////////////////////
            // byte[] data = MessageConverter.ProcessImage(ImageLib.SymbolDict["3"], ImageLib.SymbolPosDict["2"].Item1, ImageLib.SymbolPosDict["2"].Item2);

            /*
            ClientPrinter.SendData(data);
            Console.ReadLine();

            ClientPrinter.CloseStream();
            // */

            // WorkOrderLib.Parse();


            // ClientPlc.GetRequest();

            /*
            List<byte> dataList = new List<byte>
            {
                0x70,0xF8,
                0x03,0x00,
                0x07,0x03,
                0x0C,0x00,
                0x01,0x00,
                0x08,0x00,
                0x41,0x42,
                0x43,0x44,
                0x45,0x46,
                0x47,0x00
            };


            List<byte> checksumList = Checksum.GetChecksum(dataList.ToArray());


            dataList.InsertRange(2, checksumList);
            // */


            // ClientPrinter.SetupConnection();


            // Clear cache
            /*
            byte[] dataClearCache = MessageConverterSojet.ProcessClearCache();
            byte[] responseClearCache = ClientPrinter.SendData(dataClearCache);
            string responseString = System.Text.Encoding.Default.GetString(responseClearCache);
            Console.WriteLine(responseString);
            // */

            /*
            // Start Printing - Change Message Slot
            //byte[] data = MessageConverterSojet.ProcessStartPrinting("DYTEXT");
            //byte[] data = MessageConverterSojet.ProcessStartPrinting("MX20022");
            byte[] data = MessageConverterSojet.ProcessStartPrinting("DYSYMBOL");
            string dataString = System.Text.Encoding.Default.GetString(data);
            Console.WriteLine(dataString);
            byte[] response = ClientPrinter.SendData(data);
            responseString = System.Text.Encoding.Default.GetString(response);
            Console.WriteLine(responseString);
            Console.WriteLine("Change message slot");
            // */


            //byte[] bStopPrinting = MessageConverterSojet.ProcessStopPrinting();
            //byte[] responseStopPrinting = ClientPrinter.SendData(bStopPrinting);
            //Console.WriteLine("Stop printing");

            // Clear cache (again)
            /*
            responseClearCache = ClientPrinter.SendData(dataClearCache);
            responseString = System.Text.Encoding.Default.GetString(responseClearCache);
            Console.WriteLine(responseString);
            // */

            // change dynamic image
            /*
            string imagePath = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\00.bmp";
            byte[] bDynamicImage = MessageConverterSojet.ProcessImage(imagePath, 1);
            string sDynamicImage = System.Text.Encoding.Default.GetString(bDynamicImage);
            Console.WriteLine(sDynamicImage);
            byte[] responseDynamicImage = ClientPrinter.SendData(bDynamicImage);
            string sResponseDynamicImage = System.Text.Encoding.Default.GetString(responseDynamicImage);
            Console.WriteLine(sResponseDynamicImage);
            Console.WriteLine("set dynamic image");
            // */

            // Change dynamic text
            /*
            List<string> dynamicText = new List<string> { "one", "two" };
            byte[] bDynamicText = MessageConverterSojet.ProcessDynamicText(dynamicText);
            string sDynamicText = System.Text.Encoding.Default.GetString(bDynamicText);
            Console.WriteLine(sDynamicText);
            byte[] responseDynamicText = ClientPrinter.SendData(bDynamicText);
            string sResponse = System.Text.Encoding.Default.GetString(responseDynamicText);
            Console.WriteLine(sResponse);
            Console.WriteLine("set dynamic text");
            // */


            /////////////////////// DLL TEST ////////////////////////////////
            /* 
            // Init SEngine64.dll
            SEngine64Dll.MInitEnv();

            // Connecting to Printer
            string ipAddress = ConfigurationManager.AppSettings["PrinterIpAddress"];
            string IP = $"IP={ipAddress};protocol=0";
            int m_hPrinterID = SEngine64Dll.pCreate(IP);

            // Upload Message to Printer
            string message = "MX20022";
            int m_bPrinting = 0;
            int iErr = SEngine64Dll.pStartExt(m_hPrinterID, message);
            if (1 == iErr)
            {
                SEngine64Dll.pIsPrinting(m_hPrinterID, ref m_bPrinting);
            }

            while (true)
            {
                // Append Text
                List<string> dynamicTextList = new List<string> { "1234567890", "1234567890" };
                foreach (string dynamicText in dynamicTextList)
                {
                    int iLen = System.Text.Encoding.Default.GetByteCount(dynamicText);
                    byte[] bText = new byte[iLen];
                    bText = System.Text.Encoding.Default.GetBytes(dynamicText);//System.Text.Encoding.ASCII.GetBytes(strText);
                    SEngine64Dll.pAppendLocalDynamicData(m_hPrinterID, 0, iLen, bText);
                }

                // Append Image
                string blankImage = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\00.bmp";
                string image01 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\01.bmp";
                string image02 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\02.bmp";
                string image03 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\03.bmp";
                string image04 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\04.bmp";
                string image05 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\05.bmp";
                string image06 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\06.bmp";
                string image07 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\07.bmp";
                string image08 = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\08.bmp";
                string astarImage = @"C:\Users\nico.laurent\Documents\Nico\Side\E-Series Inkjet\Handling Marks\Symbols\astar.bmp";

                List<string> dynamicImagePathList = new List<string> { blankImage, blankImage, blankImage, blankImage, blankImage, blankImage };
                foreach (string path in dynamicImagePathList)
                {
                    FileStream BMPFile = File.OpenRead(path); //OpenRead
                    int iLen = 0;
                    iLen = (int)BMPFile.Length;    //获得文件长度 
                    byte[] bImage = new byte[iLen + 2]; //建立一个字节数组 
                    BMPFile.Read(bImage, 0, iLen);       //按字节流读取
                    SEngine64Dll.pAppendLocalDynamicData(m_hPrinterID, 1, iLen, bImage);
                    BMPFile.Close();
                }

                // Send data
                SEngine64Dll.pSendLocalDynamicDataToPrinter(m_hPrinterID);

                // Clear Append
                SEngine64Dll.pClearUpLocalDynamicData(m_hPrinterID);

                // Clear Buffer
                SEngine64Dll.pClearUpDynamicDataInPrinter(m_hPrinterID);
                // */

            // Printer testing
            /*
            ClientPrinter.SetupConnection();
            //string command1 = "PRINT/S1/90000001,90000003,90000025/2050/350";
            //string command2 = "PRINT/S2/90000001,90000003,90000025/2050/350";
            //string command3 = "PRINT/S3/90000001,90000003,90000025/2050/350";
            //string command4 = "PRINT/S4/90000001,90000003,90000025/2050/350";
            //byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command1);
            //
            //ClientPlc.ParseResponse(bytes);


            //*
            while (true)
            {
                Console.WriteLine("Please enter command:\r\n");
                string command = Console.ReadLine();

                if(command.ToLower() == "exit")
                {
                    ClientPrinter.ReleaseConnection();
                    break;
                }

                ClientPlc.ProcessPrinting(command);
                
            }

           
            // */
        }

        static void OnProcessExit(object sender, EventArgs e)
        {

            ClientPrinter.ReleaseConnection();
            ClientPlc.CloseStream();
        }
    }
}
