using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IConfigLoader
    {
        Dictionary<string, Dictionary<string, string>> Config { get; }
        string FilePath { get; set; }

        bool Add(string configFileName, string key, string value);
        bool Delete(string configFileName, string key);
        string GetValue(string key, string configFileName);
        List<string> ListConfigFiles();
        void LoadConfig(string configFileName);
        void SaveConfig(string configFileName);
        bool Update(string configFileName, string key, string value);
    }
}