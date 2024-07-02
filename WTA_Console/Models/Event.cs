using Core.Models;
using System;
using System.Collections.Generic;

namespace WTA_Console.Models
{
    public class Event : BaseModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> Workers { get; set; } = new List<int>();
        public string Category { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name} ({StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}), Kategorie: {Category}";
        }

        public void AddWorker(int workerId)
        {
            if (!Workers.Contains(workerId))
            {
                Workers.Add(workerId);
            }
        }

        public void RemoveWorker(int workerId)
        {
            if (Workers.Contains(workerId))
            {
                Workers.Remove(workerId);
            }
        }
    }
}
