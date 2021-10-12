using RnMarkApp.Data;
using RnMarkApp.Model;
using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RnMarkApp.Communication
{
    public class ClientPlc
    {
        // Printer Setting
        private static string clientIpAddress = ConfigurationManager.AppSettings["PlcIpAddress"];
        private static int clientPort = Int32.Parse(ConfigurationManager.AppSettings["PlcPortAddress"]);

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
                    Console.WriteLine("PLC Failed to connect.");
                    Logger.Error("PLC Failed to connect.");
                    throw new Exception("PLC Failed to connect.");
                }
                Console.WriteLine("PLC connected successfully");
                Logger.Info("PLC connected successfully");
                return client.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during connecting to PLC: { e }");
                Logger.Error($"Error during connecting to PLC: { e }");
                throw new Exception($"Error during connecting to PLC: { e }");
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
                Console.WriteLine("PLC disconnected successfully");
                Logger.Info("PLC disconnected successfully");
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
            catch (Exception e)
            {
                Console.WriteLine($"PLC reconnection failed: {e}");
                Logger.Error($"PLC reconnection failed: {e}");
                Reconnect();
            }
        }

        public static byte[] SendData(byte[] data)
        {
            try
            {
                Console.WriteLine("Sending data to PLC");
                Logger.Info("Sending data to PLC");
                string dataString = Encoding.ASCII.GetString(data);
                Console.WriteLine(dataString);
                Logger.Info(dataString);
                // send to printer
                Thread.Sleep(150);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Data sent");
                Logger.Info("Data sent");

                // String to store the response ASCII representation.
                byte[] responseByte = new Byte[30];
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Console.WriteLine("Reading response from PLC");
                Logger.Info("Reading response from PLC");
                Thread.Sleep(2000);
                Int32 bytes = stream.Read(responseByte, 0, responseByte.Length);
                byte[] trimmedResponseByte = TrimEnd(responseByte);
                responseData = Encoding.ASCII.GetString(trimmedResponseByte);
                Console.WriteLine(responseData);
                Logger.Info(responseData);

                return trimmedResponseByte;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during sending data to PLC: {e}");
                Logger.Error($"Error during sending data to PLC: {e}");
                Reconnect();
                return SendData(data);
            }
        }

        public static void GetRequest()
        {
            while (true)
            {
                try
                {
                    byte[] responseByte = SendData(Encoding.Default.GetBytes("GET/REQUEST"));
                    ParseResponse(responseByte);
                    Thread.Sleep(1000);
                }
                catch(CustomException e)
                {
                    Console.WriteLine($"PLC Response CustomException: {e.SkuName},{e.Side},{e.ErrorMessage}");
                    Logger.Error($"PLC Response CustomException: {e.SkuName},{e.Side},{e.ErrorMessage}");
                    SendData(Encoding.Default.GetBytes($"ERROR,{e.SkuName},{e.Side},{e.ErrorMessage}"));
                }
                catch(Exception e)
                {
                    Console.WriteLine($"PLC Response Exception: {e}");
                    Logger.Error($"PLC Response Exception: {e}");
                    SendData(Encoding.Default.GetBytes($"ERROR,,,,PLC Response Exception: {e}"));
                }
            }
        }

        private static void ParseResponse(byte[] responseByte)
        {
            string response = Encoding.ASCII.GetString(responseByte);
            string[] responseSplit = response.Split('/');
            // if (response.ToUpper() == "WAIT")
            if (response.ToUpper().Contains("WAIT"))
            {
                return;
            }
            //else if(responseSplit[0].ToUpper() == "PRINT")
            else if (responseSplit[0].ToUpper().Contains("PRINT"))
            {
                ParseResponsePrint(response);
            }
            else if (responseSplit[0].ToUpper() == "ERROR")
            {
                Console.WriteLine($"PLC Response Error: {response}");
                Logger.Error($"PLC Response Error: {response}");
                SendData(Encoding.Default.GetBytes($"ERROR,,,,PLC Response Error:{response}"));
            }
            else
            {
                Console.WriteLine($"PLC Response unknown: {response}");
                Logger.Error($"PLC Response unknown: {response}");
                SendData(Encoding.Default.GetBytes($"ERROR,,,,PLC Response unknown: {response}"));
            }
        }

        private static void ParseResponsePrint(string response)
        {
            string[] responseSplit = response.Split('/');

            if(responseSplit[1].ToUpper() == "START")
            {
                Console.WriteLine($"PLC start printing: {response}");
                Logger.Info($"PLC start printing: {response}");
            }
            else
            {
                ProcessPrinting(response);
            }
        }

        private static void ProcessPrinting(string response) 
        {
            SetupLibrary();
            string[] responseSplit = response.Split('/');
            string side = responseSplit[1];
            string skuList = responseSplit[2];
            string weight = responseSplit[3];
            string height = responseSplit[4];

            
            WorkOrderModel currentWo = WorkOrderLib.GetWorkorder(side, skuList, weight, height);
            WorkOrderParser.Parser(currentWo);

            byte[] readyByte = Encoding.Default.GetBytes($"READY/{side}/{skuList}/{weight}/{height}");
            byte[] responsePrint = SendData(readyByte);
            ParseResponse(responsePrint);

            /* For Testing
            byte[] readyByte = Encoding.Default.GetBytes($"READY");
            byte[] responsePrint = SendData(readyByte);
            ParseResponse(responsePrint);
            // */
        }

        private static void SetupLibrary()
        {
            //WorkOrderLib.Parse();
            SkuLib.Parse();
            ImageLib.ParseSymbol();
            //ImageLib.ParseSymbolPos();
            ImageLib.ParseSkuSymbol();
        }

        private static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
    }
}
