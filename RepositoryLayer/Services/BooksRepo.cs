using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class BooksRepo : IBooksRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public BooksRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }

        public BookEntity AddBook(BookModel model)
        {
            try
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand("AddBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Insertcmd.Parameters.AddWithValue("@bookName", model.BookName);
                Insertcmd.Parameters.AddWithValue("@AuthorName", model.AuthorName);
                Insertcmd.Parameters.AddWithValue("@Description", model.Description);
                Insertcmd.Parameters.AddWithValue("@MRP", model.MRP);
                Insertcmd.Parameters.AddWithValue("@DiscountPercentage", model.DiscountPercentage);
                Insertcmd.Parameters.AddWithValue("@BookImg", model.BookImg);
                Insertcmd.Parameters.AddWithValue("@quantity",model.Quantity);
                BookEntity Book = new BookEntity();
                SqlDataReader read = Insertcmd.ExecuteReader();
                if (read.Read())
                {
                    Book.BookId = (int)read["BookId"];
                    Book.BookName = (string)read["bookName"];
                    Book.AuthorName = (string)read["AuthorName"];
                    Book.Description = (string)read["Description"];
                    Book.MRP = (decimal)read["MRP"];
                    Book.DiscountPrice = (decimal)read["DiscountPrice"];
                    Book.rating = (float)(double)read["Rating"];
                    Book.NoofRatings = (int)read["NoofRating"];
                    Book.BookImg = (string)read["bookImg"];
                    Book.Quantity = (int)read["Quantity"];
                    
                }
                return Book;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public BookEntity GetByBookId(int id)
        {
            try
            {
                conn.Open();
                SqlCommand GetByBookId = new SqlCommand("GetByBookId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetByBookId.Parameters.AddWithValue("@BookId", id);
                BookEntity Book = new BookEntity();
                SqlDataReader reader = GetByBookId.ExecuteReader();
                if (reader.Read())
                {
                    Book.BookId = (int)reader["BookId"];
                    Book.BookName = (string)reader["bookName"];
                    Book.AuthorName = (string)reader["AuthorName"];
                    Book.Description = (string)reader["Description"];
                    Book.MRP = (decimal)reader["MRP"];
                    Book.DiscountPrice = (decimal)reader["DiscountPrice"];
                    Book.rating = (float)(double)reader["Rating"];
                    Book.NoofRatings = (int)reader["NoofRating"];
                    Book.BookImg = (string)reader["bookImg"];
                    Book.Quantity = (int)reader["Quantity"];
                    return Book;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool EditBookDetails(int BookId,BookModel model)
        {
            try
            {
                conn.Open();
                SqlCommand updateBookCmd = new SqlCommand("UpdateByBookId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                updateBookCmd.Parameters.AddWithValue("@BookId", BookId);
                updateBookCmd.Parameters.AddWithValue("@bookName", model.BookName);
                updateBookCmd.Parameters.AddWithValue("@AuthorName", model.AuthorName);
                updateBookCmd.Parameters.AddWithValue("@Description", model.Description);
                updateBookCmd.Parameters.AddWithValue("@MRP", model.MRP);
                updateBookCmd.Parameters.AddWithValue("@DiscountPercentage", model.DiscountPercentage);
                updateBookCmd.Parameters.AddWithValue("@BookImg", model.BookImg);
                updateBookCmd.Parameters.AddWithValue("@quantity", model.Quantity);
                updateBookCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<BookEntity> GetAllBooks()
        {
            try
            {
                conn.Open();
                SqlCommand GetAllBooks = new SqlCommand("GetAllBooks", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                List<BookEntity> Books = new List<BookEntity>();
                SqlDataReader reader = GetAllBooks.ExecuteReader();
                while (reader.Read())
                {
                    BookEntity Book = new BookEntity();
                    Book.BookId = (int)reader["BookId"];
                    Book.BookName = (string)reader["bookName"];
                    Book.AuthorName = (string)reader["AuthorName"];
                    Book.Description = (string)reader["Description"];
                    Book.MRP = (decimal)reader["MRP"];
                    Book.DiscountPrice = (decimal)reader["DiscountPrice"];
                    Book.rating = (float)(double)reader["Rating"];
                    Book.NoofRatings = (int)reader["NoofRating"];
                    Book.BookImg = (string)reader["bookImg"];
                    Book.Quantity = (int)reader["Quantity"];
                    Books.Add(Book);
                }
                return Books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool RemoveBook(int BookId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@BookId", BookId);
                Removecmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
