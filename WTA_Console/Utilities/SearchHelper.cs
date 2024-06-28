using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WTA_Console.Models;

namespace WTA_Console.Utilities
{
    public static class SearchHelper
    {
        public static WorkTime? Find(Predicate<WorkTime> predicate, List<WorkTime> list)
        {
            return list.Find(predicate);
        }

        public static List<WorkTime>? FindAll(Predicate<WorkTime> predicate, List<WorkTime> list)
        {
            return list.FindAll(predicate);
        }
        public static Worker? Find(Predicate<Worker> predicate, List<Worker> list)
        {
            return list.Find(predicate);
        }

        public static List<Worker>? FindAll(Predicate<Worker> predicate, List<Worker> list)
        {
            return list.FindAll(predicate);
        }
        
    }
}
