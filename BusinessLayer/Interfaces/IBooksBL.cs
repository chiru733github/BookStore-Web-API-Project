using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
        BookEntity FindBook(string BookName,string AuthorName);
        BookEntity EditOrInsertBookDetails(int BookId, BookModel model);
        List<WishListWithUser> WishListWithUser();
    }
}
