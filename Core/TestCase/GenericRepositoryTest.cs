﻿using Core.Config;
using Core.Interfaces;
using Core.Logging;
using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;

namespace Core.TestCase
{
    public class GenericRepositoryTest : TestBase
    {
        private readonly Logger _log = Logger.Instance;
        private readonly ConfigLoader _conf = ConfigLoader.Instance;

        private static GenericRepositoryTest _Instance = new GenericRepositoryTest();
        public static GenericRepositoryTest Instance { get => _Instance ?? new GenericRepositoryTest(); private set => _Instance = value; }

        private GenericRepositoryTest()
        {
        }

        public override void Start()
        {
            Console.WriteLine("Start des GenericRepository Tests");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des GenericRepository Tests, bitte drücken Sie [Enter]");
            Console.ReadLine();
        }

        public void RunGenericRepositoryTests()
        {
            try
            {
                var repository = new GenericRepository<TestModel>();

                var testItem = new TestModel { Name = "Test Item" };
                repository.Create(testItem);
                repository.ProcessQueue();

                var readItem = repository.Read(testItem.ID);
                Console.WriteLine($"Read item: {readItem?.Name}");

                testItem.Name = "Updated Test Item";
                repository.Update(testItem);
                repository.ProcessQueue();

                var allItems = repository.GetAll();
                Console.WriteLine($"All items count: {allItems.Count}");

                repository.Delete(testItem.ID);
                repository.ProcessQueue();

                allItems = repository.GetAll();
                Console.WriteLine($"All items count after delete: {allItems.Count}");

                repository.ResetIds();
                repository.ProcessQueue();
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Crit, ex.Message, $"[{nameof(GenericRepository<TestModel>)}::{nameof(RunGenericRepositoryTests)}]");
            }
        }
    }

    public class TestModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
