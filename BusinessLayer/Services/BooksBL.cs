using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class BooksBL : IBooksBL
    {
        private readonly IBooksRepo _booksRepo;
        public BooksBL(IBooksRepo repo) 
        {
            this._booksRepo = repo;
        }

        public BookEntity AddBook(BookModel book)
        {
            return _booksRepo.AddBook(book);
        }

        public BookEntity GetByBookId(int id)
        {
            return _booksRepo.GetByBookId(id);
        }

        public bool EditBookDetails(int BookId, BookModel model)
        {
            return _booksRepo.EditBookDetails(BookId, model);
        }

        public List<BookEntity> GetAllBooks()
        {
            return _booksRepo.GetAllBooks();
        }

        public bool RemoveBook(int BookId)
        {
            return _booksRepo.RemoveBook(BookId);
        }
    }
}
