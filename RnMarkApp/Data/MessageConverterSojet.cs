using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Data
{
    public static class MessageConverterSojet
    {
        /*
        private static List<byte> PrinterDeviceNoList = GetDeviceNo();
        
        public static byte[] ProcessImage(string imageDir, int noImages)
        {
            List<byte> datapack = new List<byte>();

            // find count
            UInt16 count = Convert.ToUInt16(noImages);
            List<byte> bCount = ByteConverter.Uint16ToBytes(count).ToList();
            datapack.AddRange(bCount);

            // find total packs
            List<byte> bTotalPack = new List<byte> { 0x01, 0x00 };
            datapack.AddRange(bTotalPack);

            // find pack index
            List<byte> bPackIndex = new List<byte> { 0x00, 0x00 };
            datapack.AddRange(bPackIndex);

            // Read Image
            FileStream BMPFile = File.OpenRead(imageDir);
            ushort imageLength = (ushort)BMPFile.Length;
            byte[] bImage_array = new byte[imageLength];
            BMPFile.Read(bImage_array, 0, imageLength);

            // find file size
            List<byte> bFileSize = ByteConverter.Uint16ToBytes(imageLength).ToList();
            datapack.AddRange(bFileSize);

            // find size
            List<byte> bSize = ByteConverter.Uint16ToBytes(imageLength).ToList();
            datapack.AddRange(bSize);

            // find logo (image byte)
            List<byte> bImage = bImage_array.ToList();
            datapack.AddRange(bImage);

            // find datapack length
            UInt16 length = Convert.ToUInt16(datapack.Count);
            List<byte> bLength = ByteConverter.Uint16ToBytes(length).ToList();

            // add all together
            List<byte> dataList = new List<byte>();
            dataList.AddRange(PrinterDeviceNoList); // SN
            dataList.AddRange(new List<byte> { 0x08, 0x03 }); // command
            dataList.AddRange(bLength); // length
            dataList.AddRange(datapack); // count + size N + string N

            List<byte> finalData = FinalizeData(dataList);

            return finalData.ToArray();
        }
        

        public static byte[] ProcessDynamicText(List<string> dynamicTextList)
        {
            List<byte> datapack = new List<byte>();

            // find count
            UInt16 count = Convert.ToUInt16(dynamicTextList.Count);
            List<byte> bCount = ByteConverter.Uint16ToBytes(count).ToList();
            datapack.AddRange(bCount);

            foreach (string dynamicText in dynamicTextList)
            {
                // find size
                UInt16 size = Convert.ToUInt16(dynamicText.Length + 1);
                List<byte> bSize = ByteConverter.Uint16ToBytes(size).ToList();
                datapack.AddRange(bSize);

                // find string
                List<byte> bDynamicText = Encoding.ASCII.GetBytes(dynamicText).ToList();
                datapack.AddRange(bDynamicText);
                datapack.Add(0x00);
            }

            // find datapack length
            UInt16 length = Convert.ToUInt16(datapack.Count);
            List<byte> bLength = ByteConverter.Uint16ToBytes(length).ToList();

            // add all together
            List<byte> dataList = new List<byte>();
            dataList.AddRange(PrinterDeviceNoList); // SN
            dataList.AddRange(new List<byte> { 0x07, 0x03 }); // command
            dataList.AddRange(bLength); // length
            dataList.AddRange(datapack); // count + size N + string N

            List<byte> finalData = FinalizeData(dataList);

            return finalData.ToArray();
        }

        public static byte[] ProcessStartPrinting(string messageName)
        {
            List<byte> datapack = new List<byte>();

            // find size
            UInt16 size = Convert.ToUInt16(messageName.Length + 1);
            List<byte> bSize = ByteConverter.Uint16ToBytes(size).ToList();
            datapack.AddRange(bSize);

            // find string
            List<byte> bDynamicText = Encoding.ASCII.GetBytes(messageName).ToList();
            datapack.AddRange(bDynamicText);
            datapack.Add(0x00);


            // find datapack length
            UInt16 length = Convert.ToUInt16(datapack.Count);
            List<byte> bLength = ByteConverter.Uint16ToBytes(length).ToList();

            // add all together
            List<byte> dataList = new List<byte>();
            dataList.AddRange(PrinterDeviceNoList); // SN
            dataList.AddRange(new List<byte> { 0x05, 0x03 }); // command
            dataList.AddRange(bLength); // length
            dataList.AddRange(datapack); // count + size N + string N

            List<byte> finalData = FinalizeData(dataList);

            return finalData.ToArray();
        }

        public static byte[] ProcessStopPrinting()
        {
            // add all together
            List<byte> dataList = new List<byte>();
            dataList.AddRange(PrinterDeviceNoList); // SN
            dataList.AddRange(new List<byte> { 0x06, 0x03 }); // command
            dataList.AddRange(new List<byte> { 0x00, 0x00 }); // length

            List<byte> finalData = FinalizeData(dataList);

            return finalData.ToArray();
        }

        public static byte[] ProcessClearCache()
        {
            // add all together
            List<byte> dataList = new List<byte>();
            dataList.AddRange(PrinterDeviceNoList); // SN
            dataList.AddRange(new List<byte> { 0x09, 0x03 }); // command
            dataList.AddRange(new List<byte> { 0x00, 0x00 }); // length

            List<byte> finalData = FinalizeData(dataList);

            return finalData.ToArray();
        }


        private static List<byte> FinalizeData (List<byte> datalist)
        {
            List<byte> checksum = Checksum.GetChecksum(datalist.ToArray());
            List<byte> SOC = new List<byte>() { 0x53, 0x43 };
            List<byte> EOC = new List<byte>() { 0x45, 0x43 };

            List<byte> finalData = SOC;
            finalData.AddRange(checksum);
            finalData.AddRange(datalist); // SN + command + length + datapack
            finalData.AddRange(EOC);

            return finalData;
        }

        private static List<byte> GetDeviceNo()
        {
            string printerDeviceNo = ConfigurationManager.AppSettings["PrinterDeviceNo"];
            return ToByteArray(printerDeviceNo);
        }

        private static List<byte> ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes.ToList();
        }
        // */
    }
}
