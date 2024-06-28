using Core.Models;
using System;

namespace WTA_Console.Models
{
    public class WorkTime : BaseModel
    {
        private string _Worker = string.Empty;
        private string _Event = string.Empty;
        private DateTime _Start;
        private DateTime _Ende;
        private double _Pause;

        public string Worker { get => _Worker; set => _Worker = value; }
        public string Event { get => _Event; set => _Event = value; }
        public string Start { get => _Start.ToString(); set => _Start = DateTime.Parse(value); }
        public string Ende { get => _Ende.ToString(); set => _Ende = DateTime.Parse(value); }
        public string Pause { get => _Pause.ToString(); set => _Pause = double.Parse(value); }

        public double TotalWorkTime { get => Math.Round((_Ende - _Start).TotalHours - _Pause, 2); }

        public static DateTime StringToDate(string value)
        {
            return DateTime.ParseExact(value, "dd.MM.yyyy", null);
        }
        public static DateTime StringToDateTime(string value)
        {
            return DateTime.ParseExact(value, "HH:mm  dd.MM.yyyy", null);
        }
    }
}
