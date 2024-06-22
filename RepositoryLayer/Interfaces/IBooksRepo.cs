using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IBooksRepo
    {
        BookEntity AddBook(BookModel book);
        BookEntity GetByBookId(int id);
        bool EditBookDetails(int BookId, BookModel model);
        List<BookEntity> GetAllBooks();
        BookEntity FindBook(string BookName, string AuthorName);
        bool RemoveBook(int BookId);
        BookEntity EditOrInsertBookDetails(int BookId, BookModel model);
        List<WishListWithUser> WishListWithUser();
    }
}
