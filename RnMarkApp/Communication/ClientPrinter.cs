using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RnMarkApp.Communication
{
    public static class ClientPrinter
    {
        // Printer Setting
        private static string clientIpAddress = ConfigurationManager.AppSettings["PrinterIpAddress"];
        // private static int clientPort = Int32.Parse(ConfigurationManager.AppSettings["PrinterPortAddress"]);

        private static int m_hPrinterID;
        private static int m_bPrinting = 0;

        public static void SetupConnection()
        {
            // Init SEngine64.dll
            SEngine64Dll.MInitEnv();

            // Connecting to Printer
            string IP = $"IP={clientIpAddress};protocol=0";
            m_hPrinterID = SEngine64Dll.pCreate(IP);

            Logger.Info("Printer connected successfully");
        }

        public static void ReleaseConnection()
        {
            m_hPrinterID = SEngine64Dll.pRelease(m_hPrinterID);
            Logger.Info("Printer disconnected successfully");
        }

        public static bool SendMessageSlot(string messageSlot)
        {
            m_bPrinting = 0;
            int iErr = SEngine64Dll.pStartExt(m_hPrinterID, messageSlot);
            if (1 == iErr)
            {
                SEngine64Dll.pIsPrinting(m_hPrinterID, ref m_bPrinting);
            }

            return true;
        }

        public static bool StopPrinting()
        {
            if (1 == SEngine64Dll.pStop(m_hPrinterID))
            {
                m_bPrinting = 0;
            }

            return true;
        }

        public static bool SetDynamicImage(List<string> dynamicImagePathList)
        {
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

            return true;
        }

        public static bool SetDynamicText(List<string> dynamicTextList)
        {
            foreach (string dynamicText in dynamicTextList)
            {
                int iLen = System.Text.Encoding.Default.GetByteCount(dynamicText);
                byte[] bText = new byte[iLen];
                bText = System.Text.Encoding.Default.GetBytes(dynamicText);//System.Text.Encoding.ASCII.GetBytes(strText);
                SEngine64Dll.pAppendLocalDynamicData(m_hPrinterID, 0, iLen, bText);
            }

            return true;
        }

        public static void SendData()
        {
            // Send data
            SEngine64Dll.pSendLocalDynamicDataToPrinter(m_hPrinterID);
        }

        public static bool ClearAppend()
        {
            // Clear Append
            SEngine64Dll.pClearUpLocalDynamicData(m_hPrinterID);
            return true;
        }

        public static bool ClearBuffer()
        {
            // Clear Buffer
            SEngine64Dll.pClearUpDynamicDataInPrinter(m_hPrinterID);
            return true;
        }

        /*

        private static TcpClient client;
        private static NetworkStream stream;

        public static void SetupConnection()
        {
            client = new TcpClient();
            // Setup Connection
            try
            {
                stream = InitStream(client, clientIpAddress, clientPort);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                Logger.Error($"Exception: {e}");
                throw new Exception($"Exception: {e}");
            }
        }

        private static NetworkStream InitStream(TcpClient client, string ipAddress, int port)
        {
            try
            {
                var result = client.BeginConnect(ipAddress, port, null, null);
                result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1.5));
                if (!client.Connected)
                {
                    Console.WriteLine("Failed to connect.");
                    Logger.Error("Failed to connect.");
                    throw new Exception("Failed to connect.");
                }
                Console.WriteLine("Printer connected successfully");
                Logger.Info("Printer connected successfully");
                return client.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during connecting: { e }");
                Logger.Error($"Error during connecting: { e }");
                throw new Exception($"Error during connecting: { e }");
            }
        }

        public static void CloseStream()
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (client != null)
                {
                    client.Close();
                    //client.Dispose();
                }
                Console.WriteLine("Printer disconnected successfully");
                Logger.Info("Printer disconnected successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during disconnecting: {e}");
                Logger.Error($"Error during disconnecting: {e}");
            }
        }

        public static void Reconnect()
        {
            try
            {
                Thread.Sleep(500);
                CloseStream();
                SetupConnection();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Printer reconnection failed: {e}");
                Logger.Error($"Printer reconnection failed: {e}");
                Reconnect();
            }

        }

        public static byte[] SendData(byte[] data)
        {
            try
            {
                Console.WriteLine("Sending data to printer");
                Logger.Info("Sending data to printer");
                string dataString = Encoding.ASCII.GetString(data);
                Console.WriteLine(dataString);
                Logger.Info(dataString);
                // send to printer
                Thread.Sleep(150);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Data sent");
                Logger.Info("Data sent");

                // String to store the response ASCII representation.
                byte[] responseByte = new Byte[20];
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Console.WriteLine("Reading response from printer");
                Logger.Info("Reading response from printer");
                Thread.Sleep(150);
                Int32 bytes = stream.Read(responseByte, 0, responseByte.Length);
                byte[] trimmedResponseByte = TrimEnd(responseByte);
                responseData = Encoding.ASCII.GetString(trimmedResponseByte);
                Console.WriteLine(responseData);
                Logger.Info(responseData);

                return trimmedResponseByte;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during sending data to printer: {e}");
                Logger.Error($"Error during sending data to printer: {e}");
                Reconnect();
                return SendData(data);
            }
        }

        private static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
        // */
    }

}
