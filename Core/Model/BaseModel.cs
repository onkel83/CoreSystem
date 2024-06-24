using System;

namespace Core.Model
{
    public abstract class BaseModel
    {
        private static int nextId = 1;

        public int ID { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public BaseModel()
        {
            ID = nextId++;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}