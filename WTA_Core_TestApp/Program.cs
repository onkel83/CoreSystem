using Core.Config;
using Core.ConsoleHelper;
using Core.Interfaces;
using Core.Logging;
using Core.Models;
using Core.Repositories;
using Core.TestCase;
internal class Program
{
    private static void Main(string[] args)
    {
        var logger = Logger.Instance;
        var configLoader = ConfigLoader.Instance;
        logger.RegisterObserver(new ConsoleLogObserver());

        logger.Log(LogLevel.Info, "Application started.");

        TableTest test = TableTest.Instance;
        //MenuTest menu = MenuTest.Instance;
    }
}