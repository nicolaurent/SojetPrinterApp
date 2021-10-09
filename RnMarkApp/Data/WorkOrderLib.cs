using Microsoft.VisualBasic.FileIO;
using RnMarkApp.Model;
using RnMarkApp.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Data
{
    public static class WorkOrderLib
    {
        // Temporary Class for testing

        private static string workorderLibDir = ConfigurationManager.AppSettings["WorkorderLibraryDir"];
        private static List<WorkOrderModel> workorderList;

        /*
        public static void Parse()
        {
            workorderList = new List<WorkOrderModel>();
            using (TextFieldParser parser = new TextFieldParser(workorderLibDir))
            {
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;
                string[] category = parser.ReadLine().Split(',');

                while (!parser.EndOfData)
                {
                    string content = parser.ReadLine();
                    WorkOrderParser.Parser(content);
                }
            }
        }

        // */

        public static WorkOrderModel GetWorkorder(string boxId, string skuName, string side)
        {
            foreach (WorkOrderModel wo in workorderList)
            {
                if (wo.SkuName.ToLower() == skuName.ToLower() 
                    && wo.Side.ToLower() == side.ToLower() 
                    && wo.BoxId.ToLower() == boxId.ToLower())
                {
                    return wo;
                }
            }

            Logger.Error($"Workorder BoxId: {boxId}, SKU name: {skuName}, Side: {side} is not found!");

            return null;
        }

        public static void Parse()
        {
            workorderList = new List<WorkOrderModel>();
            using (TextFieldParser parser = new TextFieldParser(workorderLibDir))
            {
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;
                string[] category = parser.ReadLine().Split(',');

                while (!parser.EndOfData)
                {
                    var content = parser.ReadFields();
                    // Return null when CSV format is not valid
                    if (category.Length != content.Length)
                    {
                        Logger.Error("WorkOrderLib Category and content length are mismatch");
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "WorkOrderLib Category and content length are mismatch"
                        };
                    }

                    WorkOrderModel wo = ParseContent(content, category);

                    // Return null when Unrecognized category found
                    if (wo == null)
                    {
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "WorkOrderLib Unrecognized category found"
                        };
                    }

                    workorderList.Add(wo);
                }
            }
        }

        //*
        private static WorkOrderModel ParseContent(string[] content, string[] category)
        {
            WorkOrderModel result = new WorkOrderModel();
            result.Symbols = new List<string>();
            int index = 0;

            foreach (var item in category)
            {
                switch (item.Replace(" ", string.Empty).ToLower())
                {
                    case "boxid":
                        result.BoxId = content[index];
                        index++;
                        continue;
                    case "sku":
                        result.SkuName = content[index];
                        index++;
                        continue;
                    case "weight":
                        result.Weight = content[index];
                        index++;
                        continue;
                    case "side":
                        result.Side = content[index];
                        index++;
                        continue;
                    case "data1":
                        result.Data1 = content[index];
                        index++;
                        continue;
                    case "data2":
                        result.Data2 = content[index];
                        index++;
                        continue;
                    case "data3":
                        result.Data3 = content[index];
                        index++;
                        continue;
                    case "data4":
                        result.Data4 = content[index];
                        index++;
                        continue;
                    case "data5":
                        result.Data5 = content[index];
                        index++;
                        continue;
                    case "symbols":
                        if (String.IsNullOrEmpty(content[index]))
                        {
                            result.Symbols = new List<string>();
                        }
                        else
                        {
                            string[] symbolsArray = content[index].Split('&');
                            result.Symbols = symbolsArray.ToList();
                        }
                        index++;
                        continue;
                    default:
                        // Return null when category is undefined
                        Logger.Error($"WorkOrderLib Category '{item}' is not defined");
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = $"WorkOrderLib Category '{item}' is not defined"
                        };
                        // return null;
                }
            }

            return result;
        }
        // */
    }
}
