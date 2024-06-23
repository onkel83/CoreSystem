namespace Core.Interfaces
{
    public interface IConfigObserver
    {
        void UpdateConfig(string key, string value);
    }
}
