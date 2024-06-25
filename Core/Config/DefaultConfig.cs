using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Config
{
    public abstract class DefaultConfig
    {
        private static Dictionary<string, Dictionary<string, string>> _defaultConfig = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> GetDefault()
        {
            _defaultConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "defaultConfig.json",
                    new Dictionary<string, string> {
                        {"appname","[WTA]_Core_TestApp" },
                        {"version","0.0.1"  }
                    }
                },
                {
                    "LoggerConf.json",
                    new Dictionary<string, string>(){
                        {"logToFile", true.ToString()},
                        {"logToConsole", true.ToString()},
                        {"logFilePath", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs") },
                        {"errorFilePath", "Error.txt"},
                        {"messageFilePath", "Info.txt"}
                    }
                },
                {
                    "DatabaseConfig.json",
                    new Dictionary<string, string>(){
                        { "dataFilePath",Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data") },
                        { "LastID", "000000" }
                    }
                },
                {
                    "MenuConfig.json",
                    new Dictionary<string, string> {
                        {"SubMenuMaxDepth", "3" },
                        {"MenuDelimiter", "#" },
                        {"TitleColor", "White" },
                        {"KeyColor", "White" },
                        {"DescriptionColor", "White" },
                        {"DelimiterColor", "White" }
                    }
                },
                {
                    "TableConfig.json",
                    new Dictionary<string, string> {
                        {"HeaderColor", "White" },
                        {"CellColor", "Gray" },
                        {"AlternateRowColor", "DarkGray" },
                        {"FrameColor", "White" }
                    }
                },
            };

            return _defaultConfig;
        }
    }
}
