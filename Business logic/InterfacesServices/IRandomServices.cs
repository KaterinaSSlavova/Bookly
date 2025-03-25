using Models.Entities;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<Book> GetUnreadBooks(int userId);
        Book RandomResult(int userId);
    }
}
