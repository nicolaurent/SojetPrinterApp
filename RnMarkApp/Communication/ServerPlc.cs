using RnMarkApp.Data;
using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Communication
{
    public static class ServerPlc
    {
        /*
        // Printer Setting
        private static string serverIpAddress = ConfigurationManager.AppSettings["PlcIpAddress"];
        private static int serverPort = Int32.Parse(ConfigurationManager.AppSettings["PlcPortAddress"]);
        private static NetworkStream nwStream;
        private static TcpClient client;
        private static TcpListener listener;
        private static byte[] buffer;

        public static void SetupConnection()
        {
            try
            {
                //---listen at the specified IP and port no.---
                IPAddress localAdd = IPAddress.Parse(serverIpAddress);
                listener = new TcpListener(localAdd, serverPort);
                Console.WriteLine("TCP Server Listening...");
                Logger.Info("TCP Server Listening...");
                listener.Start();
            }
            catch (Exception e) { 

            }
        }

        public static void Listening()
        {
            try
            {
                while (true)
                {
                    //---incoming client connected---
                    client = listener.AcceptTcpClient();

                    //---get the incoming data through a network stream---
                    nwStream = client.GetStream();
                    buffer = new byte[client.ReceiveBufferSize];

                    while (true)
                    {
                        try
                        {
                            //---read incoming stream---
                            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine("Received : " + dataReceived);
                            Logger.Info("Received : " + dataReceived);

                            // Parse Data & Send to Printer
                            WorkOrderParser.Parser(dataReceived);

                            //---write back the acknowldegement to the PLC---
                            Console.WriteLine("Sending back : " + dataReceived);
                            Logger.Info("Sending back : " + dataReceived);
                            // nwStream.Write(dataReceived, 0, dataReceived.Length);
                        }
                        catch (System.IO.IOException)
                        {
                            // do nothing
                            Console.WriteLine("Client disconnected");
                            Logger.Info("Client disconnected");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            Logger.Info(e.ToString());
                            break;
                        }
                    }
                    client.Close();
                }

            }
            finally
            {
                listener.Stop();
            }
        }

        /*
        public static void Run()
        {
            try
            {
                //---listen at the specified IP and port no.---
                IPAddress localAdd = IPAddress.Parse(serverIpAddress);
                listener = new TcpListener(localAdd, serverPort);
                Console.WriteLine("Listening...");
                listener.Start();
                while (true)
                {
                    //---incoming client connected---
                    client = listener.AcceptTcpClient();

                    //---get the incoming data through a network stream---
                    nwStream = client.GetStream();
                    buffer = new byte[client.ReceiveBufferSize];

                    while (true)
                    {
                        try
                        {
                            //---read incoming stream---
                            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine("Received : " + dataReceived);

                            // Parse Data

                            // Send Data to Printer
                            ClientPrinter.SendData(buffer);

                            //---write back the acknowldegement to the PLC---
                            Console.WriteLine("Sending back : " + dataReceived);
                            nwStream.Write(buffer, 0, bytesRead);
                        }
                        catch (System.IO.IOException)
                        {
                            // do nothing
                            Console.WriteLine("Client disconnected");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            break;
                        }
                    }
                    client.Close();
                }
                
            }
            finally
            {
                listener.Stop();
                Console.ReadLine();
            }
        }
        // */
    }
}
