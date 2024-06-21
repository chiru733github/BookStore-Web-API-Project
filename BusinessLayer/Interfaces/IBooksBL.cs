using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IBooksBL
    {
        BookEntity AddBook(BookModel book);
        BookEntity GetByBookId(int id);
        bool EditBookDetails(int BookId, BookModel model);
        List<BookEntity> GetAllBooks();
        bool RemoveBook(int BookId);
    }
}
