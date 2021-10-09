using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Util
{
    public static class ByteConverter
    {
        public static byte[] Uint32ToBytes(UInt32 uint32) { return DigitToBytes(uint32, 4); }

        public static byte[] LongToBytes(ulong int64) { return DigitToBytes(int64, 8); }

        public static byte[] Uint16ToBytes(UInt16 uint16) { return DigitToBytes(uint16, 2); }

        public static byte[] DigitToBytes(ulong Digit, int ByteLength)
        {
            byte[] result = new byte[ByteLength];
            string strHex = Digit.ToString("x");
            while (strHex.Length < (ByteLength * 2))
            {
                strHex = strHex.Insert(0, "0");
            }

            for (int i = 0; i < ByteLength; i++)
            {
                int Index = ByteLength - 1 - i;
                //The low position is in the front, the high position is in the back        
                result[Index] = Convert.ToByte(strHex.Substring(i * 2, 2), 16);
            }
            return result;
        }
    }
}
