namespace Business_logic.Interfaces
{
    public interface ISessionHelper
    {
        void SetSession(string key, string value);
        string GetSession(string key);
    }
}
