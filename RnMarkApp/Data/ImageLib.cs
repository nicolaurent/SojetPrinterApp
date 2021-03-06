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
    public static class ImageLib
    {
        public static Dictionary<string, string> SymbolDict;
        public static Dictionary<string, Tuple<ushort, ushort>> SymbolPosDict;
        public static Dictionary<string, List<string>> SkuSymbolDict;

        private static string symbolLibDir = ConfigurationManager.AppSettings["SymbolLibraryDir"];
        private static string symbolPosLibDir = ConfigurationManager.AppSettings["SymbolPosLibraryDir"];
        private static string skuSymbolDir = ConfigurationManager.AppSettings["SkuSymbolDir"];

        public static void ParseSymbol()
        {
            SymbolDict = new Dictionary<string, string>();
            using (TextFieldParser parser = new TextFieldParser(symbolLibDir))
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
                        Logger.Error("ParseSymbol CSV Category and content length are mismatch");
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbol CSV Category and content length are mismatch"
                        };
                    }

                    SymbolModel symbol = ParseContent(content, category);

                    // Return null when Unrecognized category found
                    if (symbol == null)
                    {
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbol Unrecognized category found"
                        };
                    }

                    SymbolDict.Add(symbol.Number, symbol.ImagePath);
                }
            }
        }

        public static void ParseSymbolPos()
        {
            SymbolPosDict = new Dictionary<string, Tuple<ushort, ushort>>();
            using (TextFieldParser parser = new TextFieldParser(symbolPosLibDir))
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
                        Logger.Error("ParseSymbolPos CSV Category and content length are mismatch");
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbolPos CSV Category and content length are mismatch"
                        };
                    }

                    SymbolPosModel symbolPos = ParsePosContent(content, category);

                    // Return null when Unrecognized category found
                    if (symbolPos == null)
                    {
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbolPos Unrecognized category found"
                        };
                    }

                    SymbolPosDict.Add(symbolPos.Idx, Tuple.Create(Convert.ToUInt16(symbolPos.XPos), Convert.ToUInt16(symbolPos.YPos)));
                }
            }
        }

        public static void ParseSkuSymbol()
        {
            SkuSymbolDict = new Dictionary<string, List<string>>();
            using (TextFieldParser parser = new TextFieldParser(skuSymbolDir))
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
                        Logger.Error("ParseSkuSymbol CSV Category and content length are mismatch");
                        // return null;
                        throw new CustomException
                        {
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbol CSV Category and content length are mismatch"
                        };
                    }

                    SkuSymbolModel skuSymbol = ParseContentSkuSymbol(content, category);

                    // Return null when Unrecognized category found
                    if (skuSymbol == null)
                    {
                        // return null;
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = "ParseSymbol Unrecognized category found"
                        };
                    }

                    SkuSymbolDict.Add(skuSymbol.SKU, skuSymbol.SymbolList);
                }
            }
        }

        private static SymbolModel ParseContent(string[] content, string[] category)
        {
            SymbolModel result = new SymbolModel();
            int index = 0;

            foreach (var item in category)
            {
                switch (item.Replace(" ", string.Empty).ToLower())
                {
                    case "symbolnumber":
                        result.Number = content[index];
                        index++;
                        continue;
                    case "imagedir":
                        result.ImagePath = content[index];
                        index++;
                        continue;
                    default:
                        // Return null when category is undefined
                        Logger.Error($"ParseContent Category '{item}' is not defined");
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = $"ParseContent Category '{item}' is not defined"
                        };
                        // return null;
                }
            }

            return result;
        }

        private static SymbolPosModel ParsePosContent(string[] content, string[] category)
        {
            SymbolPosModel result = new SymbolPosModel();
            int index = 0;

            foreach (var item in category)
            {
                switch (item.Replace(" ", string.Empty).ToLower())
                {
                    case "index":
                        result.Idx = content[index];
                        index++;
                        continue;
                    case "xpos":
                        result.XPos = content[index];
                        index++;
                        continue;
                    case "ypos":
                        result.YPos = content[index];
                        index++;
                        continue;
                    default:
                        // Return null when category is undefined
                        Logger.Error($"ParsePosContent Category '{item}' is not defined");

                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = $"ParsePosContent Category '{item}' is not defined"
                        };
                        // return null;
                }
            }

            return result;
        }

        private static SkuSymbolModel ParseContentSkuSymbol(string[] content, string[] category)
        {
            SkuSymbolModel result = new SkuSymbolModel();
            result.SymbolList = new List<string>();
            int index = 0;

            foreach (var item in category)
            {
                switch (item.Replace(" ", string.Empty).ToLower())
                {
                    case "sku":
                        result.SKU = content[index];
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
                    case "symbols":
                        if (String.IsNullOrEmpty(content[index]))
                        {
                            result.SymbolList = new List<string>();
                        }
                        else
                        {
                            string[] symbolsArray = content[index].Split('&');
                            result.SymbolList = symbolsArray.ToList();
                        }
                        index++;
                        continue;
                    default:
                        // Return null when category is undefined
                        Logger.Error($"SkuSymbol Category '{item}' is not defined");
                        throw new CustomException
                        {
                            BoxId = string.Empty,
                            SkuName = string.Empty,
                            Side = string.Empty,
                            ErrorMessage = $"SkuSymbol Category '{item}' is not defined"
                        };
                        // return null;
                }
            }

            return result;
        }

    }
}
