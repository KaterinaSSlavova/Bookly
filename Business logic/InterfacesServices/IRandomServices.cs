using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookly.Data.Models;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<Book> GetUnreadBooks(int userId);
        Book RandomResult(int userId);
    }
}
