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
    public static class SkuLib
    {
        private static string skuLibDir = ConfigurationManager.AppSettings["SkuLibraryDir"];
        private static List<SkuModel> skuList;

        public static string GetMessageSlot(string skuName, string side)
        {
            foreach (SkuModel sku in skuList)
            {
                if(sku.Name.ToLower() == skuName.ToLower() && sku.Side.ToLower() == side.ToLower())
                {
                    return sku.MessageSlot;
                }
            }

            Logger.Error($"SkuLib SKU name: {skuName}, Side: {side} is not found!");

            throw new CustomException
            {
                BoxId = string.Empty,
                SkuName = string.Empty,
                Side = string.Empty,
                ErrorMessage = $"SkuLib SKU name: {skuName}, Side: {side} is not found!"
            };

            // return null;
        }

        public static void Parse()
        {
            skuList = new List<SkuModel>();
            using (TextFieldParser parser = new TextFieldParser(skuLibDir))
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
                        Logger.Error("SkuLib Parse CSV Category and content length are mismatch");
                        // return null;

                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "SkuLib Parse CSV Category and content length are mismatch"
                        };
                    }

                    SkuModel sku = ParseContent(content, category);

                    // Return null when Unrecognized category found
                    if (sku == null)
                    {
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "SkuLib Parse Unrecognized category found"
                        };
                    }

                    skuList.Add(sku);
                }
            }
        }

        private static SkuModel ParseContent(string[] content, string[] category)
        {
            SkuModel result = new SkuModel();
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
                        result.Name = content[index];
                        index++;
                        continue;
                    case "side":
                        result.Side = content[index];
                        index++;
                        continue;
                    case "messageslot":
                        result.MessageSlot = content[index];
                        index++;
                        continue;
                    default:
                        // Return null when category is undefined
                        Logger.Error($"SkuLib ParseContent Category '{item}' is not defined");

                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = $"SkuLib ParseContent Category '{item}' is not defined"
                        };

                        // return null;
                }
            }

            return result;
        }
    }
}
