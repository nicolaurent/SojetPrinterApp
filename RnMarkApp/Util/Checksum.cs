using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Util
{
    public static class Checksum
    {
        public static List<byte> GetChecksum(byte[] bytes)
        {
            UInt16 checksum = CalculateCheckSum(bytes);
            byte[] checksumByte = ByteConverter.Uint16ToBytes(checksum);
            return checksumByte.ToList();
        }

        private static UInt16 CalculateCheckSum(byte[] bytes)
        {
            Int64 Sum = 0;
            if ((bytes.Length) % 2 == 0)
            {
                for (int i = 0; i < bytes.Length - 1; i = i + 2)
                {
                    Int64 bit = bytes[i + 1] << 8 | bytes[i];
                    Sum += bit;
                }

            }
            else
            {
                for (int i = 0; i < bytes.Length - 1; i = i + 2)
                {
                    Int64 bit = bytes[i + 1] << 8 | bytes[i];
                    Sum += bit;
                }
                Sum += bytes[bytes.Length - 1];
            }

            return (UInt16)(Sum & 0xFFFF);
        }

        
    }
}
