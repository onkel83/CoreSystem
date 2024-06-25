using Core.Config;
using System;
using System.Collections.Generic;

namespace Core.TestCase
{
    public class ConfigLoaderTest : TestBase
    {
        private readonly ConfigLoader _conf = ConfigLoader.Instance;

        private static ConfigLoaderTest _Instance = new ConfigLoaderTest();
        public static ConfigLoaderTest Instance { get => _Instance ?? new ConfigLoaderTest(); private set => _Instance = value; }

        private ConfigLoaderTest()
        {
            base.Run(() => RunConfigLoaderTests());
        }

        public override void Start()
        {
            Console.WriteLine("Start des ConfigLoader Tests");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des ConfigLoader Tests, bitte drücken Sie [Enter]");
            Console.ReadLine();
        }

        private void RunConfigLoaderTests()
        {
            string testConfigFileName = "testConfig.json";

            _conf.Add(testConfigFileName, "testKey", "testValue");
            string value = _conf.GetValue("testKey", testConfigFileName);
            Console.WriteLine($"Retrieved value: {value}");

            _conf.Update(testConfigFileName, "testKey", "updatedValue");
            value = _conf.GetValue("testKey", testConfigFileName);
            Console.WriteLine($"Updated value: {value}");

            _conf.Delete(testConfigFileName, "testKey");
            value = _conf.GetValue("testKey", testConfigFileName);
            Console.WriteLine($"Value after delete: {value}");

            _conf.SaveConfig(testConfigFileName);
            List<string> configFiles = _conf.ListConfigFiles();
            Console.WriteLine($"Config files: {string.Join(", ", configFiles)}");
        }
    }
}
