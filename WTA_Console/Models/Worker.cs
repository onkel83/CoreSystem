using Core.Models;

namespace WTA_Console.Models
{
    public class Worker : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Vorname { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name}, {Vorname}";
        }
    }
}
