using System;

namespace Core.Model
{
    public class MenueEntry
    {
        public char? Key { get; set; } = null;
        public string Description { get; set; } = string.Empty;
        public Action? Empfaenger { get; set; } = null;
    }
}
