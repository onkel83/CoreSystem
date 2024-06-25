using Core.Interfaces;

namespace Core.Models
{
    public abstract class BaseModel : IBaseModel
    {
        public int ID { get; set; }
    }
}
