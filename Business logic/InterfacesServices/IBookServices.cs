using ViewModels.Model;
using Models.Entities;
using Business_logic.DTOs;
using System.Net;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        bool AddBook(Book book);
        List<Book> LoadBooks();
        Book GetBookById(int bookId);
        bool RemoveBook(int id);
    }
}
