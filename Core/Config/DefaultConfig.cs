using System.Collections.Generic;

namespace Core.Config
{
    public static class DefaultConfig
    {
        public static readonly Dictionary<string, string> Settings = new Dictionary<string, string>
        {
            { "LogLevel", "Info" },
            { "MaxConnections", "10" },
            { "TimeoutInSeconds", "30" },
            { "DatabaseFileName", "database.json" },
            { "DatabaseFolderName", "Data" }
        };
    }
}
