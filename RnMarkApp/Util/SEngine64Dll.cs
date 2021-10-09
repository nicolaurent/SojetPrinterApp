using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RnMarkApp.Util
{
    public static class SEngine64Dll
    {
        [DllImport("SEngine64.dll", EntryPoint = "MInitEnv", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int MInitEnv();

        [DllImport("SEngine64.dll", EntryPoint = "MFiniEnv", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int MFiniEnv();

        [DllImport("SEngine64.dll", EntryPoint = "pStart", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pStart(int iID, string strFilePath); //开启喷印

        [DllImport("SEngine64.dll", EntryPoint = "pStartExt", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pStartExt(int iID, string strMsgName); //开启喷印,不上传资料 

        [DllImport("SEngine64.dll", EntryPoint = "pStop", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pStop(int iID); //终止喷印

        [DllImport("SEngine64.dll", EntryPoint = "pAppendLocalDynamicData", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pAppendLocalDynamicData(int iID, int iType, int iDataLen, byte[] bDataBuf); //添加本地动态数据

        [DllImport("SEngine64.dll", EntryPoint = "pSendLocalDynamicDataToPrinter", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pSendLocalDynamicDataToPrinter(int iID); //发送本地动态数据到打印设备

        [DllImport("SEngine64.dll", EntryPoint = "pClearUpLocalDynamicData", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pClearUpLocalDynamicData(int iID); //清除本地动态数据 

        [DllImport("SEngine64.dll", EntryPoint = "pClearUpDynamicDataInPrinter", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pClearUpDynamicDataInPrinter(int iID); //清除喷印设备中的动态数据缓存

        //喷印控制
        [DllImport("SEngine64.dll", EntryPoint = "pCreate", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pCreate(string strArgStr);//申请资源，建立设备连接，返回设备ID

        [DllImport("SEngine64.dll", EntryPoint = "pRelease", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pRelease(int iID); //释放资源，喷印任务句柄

        [DllImport("SEngine64.dll", EntryPoint = "pIsPrinting", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int pIsPrinting(int iID, ref int iIsPrinting); //获取打印机喷印状态
    }
}
