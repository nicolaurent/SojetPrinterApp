using RnMarkApp.Communication;
using RnMarkApp.Model;
using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Data
{
    public static class WorkOrderParser
    {
        public static void Parser(WorkOrderModel currentWo)
        {
            // WorkOrderModel currentWo = ParseContent(content);

            // Clear Message
            SetClearMessage(currentWo);

            // Set Message Slot
            SetMessageSlot(currentWo);

            // Set Dynamic Text
            SetDynamicText(currentWo);

            // Set Symbols
            SetSymbols(currentWo);

            // Send Data
            ClientPrinter.SendData();
        }

        private static void SetClearMessage(WorkOrderModel currentWo)
        {
            bool isResponseOkClearAppend = ClientPrinter.ClearAppend();
            bool isResponseOkClearBuffer = ClientPrinter.ClearBuffer();
            bool isResponseOk = isResponseOkClearAppend && isResponseOkClearBuffer;

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetClearMessage response is not OK"
                };
            }
        }

        private static void SetMessageSlot(WorkOrderModel currentWo)
        {
            string messageSlot = SkuLib.GetMessageSlot(currentWo.Side);
            bool isResponseOk = ClientPrinter.SendMessageSlot(messageSlot);

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetMessageSlot response is not OK"
                };
            }

        }

        private static void SetDynamicText(WorkOrderModel currentWo)
        {
            List<string> dynamicTextList = new List<string> { " ", " "};

            if (!String.IsNullOrEmpty(currentWo.Data1))
            {
                dynamicTextList[0] = currentWo.Data1;
            }
            if (!String.IsNullOrEmpty(currentWo.Data2))
            {
                dynamicTextList[1] = currentWo.Data2;
            }

            bool isResponseOk = ClientPrinter.SetDynamicText(dynamicTextList);

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetDynamicText is not OK"
                };
            }
        }

        private static void SetSymbols(WorkOrderModel currentWo)
        {
            List<string> dynamicImagePathList = new List<string> 
            { 
                ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"],
                ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"],
                ImageLib.SymbolDict["0"], ImageLib.SymbolDict["0"]
            };

            int idx = 0;
            foreach (string s in currentWo.Symbols)
            {
                string imageDir = ImageLib.SymbolDict[s];
                dynamicImagePathList[idx] = imageDir;
                idx += 1;
            }

            bool isResponseOk = ClientPrinter.SetDynamicImage(dynamicImagePathList);

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetSymbols response is not OK"
                };
            }
        }

        // Rn Mark Printer - OBSOLETE
        /*
        private static void SetClearMessage(WorkOrderModel currentWo)
        {
            byte[] command = MessageConverter.ClearMessage();
            byte[] responseByte = ClientPrinter.SendData(command);
            bool isResponseOk = ResponseParser(responseByte, "clearmessage");

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    BoxId = currentWo.BoxId,
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetClearMessage response is not OK: {Encoding.ASCII.GetString(responseByte)}"
                };
            }
        }

        private static void SetMessageSlot(WorkOrderModel currentWo)
        {
            string messageSlot = SkuLib.GetMessageSlot(currentWo.SkuName, currentWo.Side);
            byte[] command = MessageConverter.ProcessMessageSlot(messageSlot);
            byte[] responseByte = ClientPrinter.SendData(command);
            bool isResponseOk = ResponseParser(responseByte, "messageslot", messageSlot);

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    BoxId = currentWo.BoxId,
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetMessageSlot response is not OK: {Encoding.ASCII.GetString(responseByte)}"
                };
            }

        }

        private static void SetDynamicText(WorkOrderModel currentWo)
        {
            if (!String.IsNullOrEmpty(currentWo.Data1))
            {
                SendDynamicText("0", currentWo.Data1, currentWo);
            }
            if (!String.IsNullOrEmpty(currentWo.Data2))
            {
                SendDynamicText("1", currentWo.Data2, currentWo);
            }
            if (!String.IsNullOrEmpty(currentWo.Data3))
            {
                SendDynamicText("2", currentWo.Data3, currentWo);
            }
            if (!String.IsNullOrEmpty(currentWo.Data4))
            {
                SendDynamicText("3", currentWo.Data4, currentWo);
            }
            if (!String.IsNullOrEmpty(currentWo.Data5))
            {
                SendDynamicText("4", currentWo.Data5, currentWo);
            }
        }

        private static void SendDynamicText(string objectIndex, string data, WorkOrderModel currentWo)
        {
            byte[] command = MessageConverter.ProcessDynamicText(objectIndex, data);
            byte[] responseByte = ClientPrinter.SendData(command);
            bool isResponseOk = ResponseParser(responseByte, "dynamictext");

            if (!isResponseOk)
            {
                throw new CustomException
                {
                    BoxId = currentWo.BoxId,
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SendDynamicText response index {objectIndex} is not OK: {Encoding.ASCII.GetString(responseByte)}"
                };
            }
        }

        private static void SetSymbols(WorkOrderModel currentWo)
        {
            if(currentWo.Symbols.Count > ImageLib.SymbolPosDict.Count)
            {
                throw new CustomException
                {
                    BoxId = currentWo.BoxId,
                    SkuName = currentWo.SkuName,
                    Side = currentWo.Side,
                    ErrorMessage = $"SetSymbols: symbol given is more than total number defined in the library"
                };
            }

            int idx = 1;
            foreach (string s in currentWo.Symbols)
            {
                string imageDir = ImageLib.SymbolDict[s];
                Tuple<ushort, ushort> pos = ImageLib.SymbolPosDict[idx.ToString()];
                byte[] command = MessageConverter.ProcessImage(imageDir, pos.Item1, pos.Item2);
                byte[] responseByte = ClientPrinter.SendData(command);
                bool isResponseOk = ResponseParser(responseByte, "symbol");
                if (!isResponseOk)
                {
                    throw new CustomException
                    {
                        BoxId = currentWo.BoxId,
                        SkuName = currentWo.SkuName,
                        Side = currentWo.Side,
                        ErrorMessage = $"SetSymbols response is not OK: {Encoding.ASCII.GetString(responseByte)}"
                    };
                }
                idx += 1;
            }
        }


        private static WorkOrderModel ParseContent(string content)
        {
            string[] contentSplit = content.Split(',');
            WorkOrderModel workOrder = new WorkOrderModel
            {
                BoxId = contentSplit[0],
                SkuName = contentSplit[1],
                Side = contentSplit[2],
                Weight = contentSplit[3],
                Data1 = contentSplit[4],
                Data2 = contentSplit[5],
                Data3 = contentSplit[6],
                Data4 = contentSplit[7],
                Data5 = contentSplit[8],
                Symbols = new List<string>()
            };

            for(int i = 9; i<contentSplit.Length; i++)
            {
                if(String.IsNullOrEmpty(contentSplit[i]))
                {
                    continue;
                }

                workOrder.Symbols.Add(contentSplit[i]);
            }

            return workOrder;
        }

        private static bool ResponseParser(byte[] responseByte, string messageType, string messageSlot = "")
        {
            switch (messageType.ToLower())
            {
                case "messageslot":
                    if (responseByte[1] == Convert.ToByte(9) && responseByte[2].ToString() == messageSlot)
                    {
                        Console.WriteLine($"Message type: {messageType}, messageSlot: {messageSlot}, response OK");
                        Logger.Info($"Message type: {messageType}, messageSlot: {messageSlot}, response OK");
                        return true;
                    }
                    Console.WriteLine($"Message type: {messageType} , messageSlot: {messageSlot}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    Logger.Error($"Message type: {messageType}, messageSlot: {messageSlot}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    return false;

                case "symbol":
                    if (responseByte[1] == Convert.ToByte(6) && responseByte[2] == Convert.ToByte(63))
                    {
                        Console.WriteLine($"Message type: {messageType} response OK");
                        Logger.Info($"Message type: {messageType} response OK");
                        return true;
                    }
                    Console.WriteLine($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    Logger.Error($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    return false;

                case "dynamictext":
                    if (responseByte[1] == Convert.ToByte(6) && responseByte[2] == Convert.ToByte(49))
                    {
                        Console.WriteLine($"Message type: {messageType} response OK");
                        Logger.Info($"Message type: {messageType} response OK");
                        return true;
                    }
                    Console.WriteLine($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    Logger.Error($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    return false;

                case "clearmessage":
                    if (responseByte[1] == Convert.ToByte(6) && responseByte[2] == Convert.ToByte(52))
                    {
                        Console.WriteLine($"Message type: {messageType} response OK");
                        Logger.Info($"Message type: {messageType} response OK");
                        return true;
                    }
                    Console.WriteLine($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    Logger.Error($"Message type: {messageType}, responseByte[1]: {responseByte[1].ToString()}, responseByte[2]: {responseByte[2].ToString()}");
                    return false;

                default:
                    Console.WriteLine($"Message type: {messageType} is not defined");
                    Logger.Error($"Message type: {messageType} is not defined");
                    return false;

            }

        }
        // */

    }
}
