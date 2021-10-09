using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Data
{
    public static class MessageConverter
    {
        static byte[] BitReverseTable = new byte[] {
0x0, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
0x8, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
0x4, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
0xC, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
0x2, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
0xA, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
0x6, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
0xE, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
0x1, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
0x9, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,
0x5, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
0xD, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
0x3, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3,
0xB, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
0x7, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7,
0xF, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF};


        public static byte[] ProcessImage(string imageDir, ushort X, ushort Y)
        {
            // suppose input.png is the QR-Code barcode you already generated
            Bitmap bmInput = new Bitmap(imageDir);

            // hight of the output image is in Bytes; output image hight will be a multiple of 8 pixels;
            int byteHight = bmInput.Height / 8;
            // output image width is in pixels
            int pxWidth = bmInput.Width;

            // generate output image format from input bitmap
            bmInput.RotateFlip(RotateFlipType.Rotate90FlipX);
            System.Drawing.Imaging.BitmapData sourceData = bmInput.LockBits(new Rectangle(0, 0, bmInput.Width, bmInput.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
            byte[] bOutput = new byte[pxWidth * byteHight]; // this byte-array will be sent to the printer
            byte[] bInput = new byte[sourceData.Stride * sourceData.Height];
            System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, bInput, 0, bInput.Length);
            bmInput.UnlockBits(sourceData);
            for (int j = 0; j <= pxWidth - 1; j++)
            {
                for (int i = 0; i < byteHight; i++)
                {
                    // bOutput[j * byteHight + i] = bInput[j * sourceData.Stride + i].ReverseBitOrder ^ System.Convert.ToByte(255);
                    bOutput[j * byteHight + i] = Convert.ToByte(BitReverseTable[bInput[(j * sourceData.Stride) + i]] ^ Convert.ToByte(255));
                }
            }

            List<byte> dataList = new List<byte>
            {
                Convert.ToByte(1),
                Convert.ToByte(63)
            };
            dataList.AddRange(BitConverter.GetBytes(X).Reverse()); // Left (X-coordinate) represented in pixel units; two byte unsigned short integer (big-endian)
            dataList.Add(Convert.ToByte(Y / 8)); // Top (Y-Coordinate) represented in byte unit (equal to Y / 8)

            UInt16 UInt16pxWidth = Convert.ToUInt16(pxWidth);
            dataList.AddRange(BitConverter.GetBytes(UInt16pxWidth).Reverse()); // Width represented in pixel units; two byte unsigned short integer (big-endian)
            dataList.Add(Convert.ToByte(byteHight)); // Height represented in byte unit (equal to input height / 8)
            dataList.AddRange(bOutput.ToList()); // image stream
            dataList.Add(Convert.ToByte(4)); // 4 is EOH

            byte[] data = dataList.ToArray();

            return data;
        }

        public static byte[] ProcessMessageSlot(string messageSlot)
        {
            List<byte> dataList = new List<byte>
            {
                Convert.ToByte(1),
                Convert.ToByte(48),
                Convert.ToByte(Convert.ToUInt16(messageSlot)),
                Convert.ToByte(4)
            };

            byte[] data = dataList.ToArray();

            return data;
        }

        public static byte[] ProcessDynamicText(string objectIndex, string dynamicText)
        {
            int defaultDynamicTextLength = Int32.Parse(ConfigurationManager.AppSettings["DynamicTextLength"]);
            List<byte> dataList = new List<byte>
            {
                Convert.ToByte(1),
                Convert.ToByte(49),
                Convert.ToByte(Convert.ToUInt16(objectIndex)),
                Convert.ToByte(1)
            };

            for(int i = 0; i < defaultDynamicTextLength; i++)
            {
                byte unicode = 20; // put space as default

                if (i < dynamicText.Length)
                {
                    unicode = Convert.ToByte(dynamicText[i]);
                }

                dataList.Add(unicode);
            }


            dataList.Add(Convert.ToByte(4));

            byte[] data = dataList.ToArray();

            return data;
        }

        public static byte[] ProcessOnOffPrinter(bool isOn = true)
        {
            List<byte> dataList = new List<byte>
            {
                Convert.ToByte(1),
                Convert.ToByte(53),
            };

            if (isOn)
            {
                dataList.Add(Convert.ToByte(1));
            }
            else
            {
                dataList.Add(Convert.ToByte(0));
            }

            dataList.Add(Convert.ToByte(4));

            byte[] data = dataList.ToArray();

            return data;
        }

        public static byte[] ClearMessage()
        {
            List<byte> dataList = new List<byte>
            {
                Convert.ToByte(1),
                Convert.ToByte(52),
                Convert.ToByte(4)
            };

            byte[] data = dataList.ToArray();

            return data;
        }
    }
}
