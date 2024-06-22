using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class CartsRepo:ICartRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public CartsRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }

        public CartEntity AddToCart(int UserId, int bookId)
        {
            try
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand("AddTocart", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Insertcmd.Parameters.AddWithValue("@userId", UserId);
                Insertcmd.Parameters.AddWithValue("@BookId", bookId);
                CartEntity cart = new CartEntity();
                SqlDataReader reader = Insertcmd.ExecuteReader();
                if (reader.Read())
                {
                    cart.CartId = (int)reader["cartId"];
                    cart.BookImg = (string)reader["bookImg"];
                    cart.BookName = (string)reader["bookName"];
                    cart.AuthorName = (string)reader["AuthorName"];
                    cart.MRP = (decimal)reader["MRP"];
                    cart.DiscountPrice = (decimal)reader["Discountprice"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.UserId = (int)reader["userId"];
                    cart.BookId = (int)reader["BookId"];
                    return cart;
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

        public List<CartEntity> GetAllCarts(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAllCartByUserIdcmd = new SqlCommand("GetAllCartByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAllCartByUserIdcmd.Parameters.AddWithValue("@userId", UserId);
                List<CartEntity> carts = new List<CartEntity>();
                SqlDataReader reader = GetAllCartByUserIdcmd.ExecuteReader();
                while (reader.Read())
                {
                    CartEntity cart = new CartEntity();
                    cart.CartId = (int)reader["cartId"];
                    cart.BookImg = (string)reader["bookImg"];
                    cart.BookName = (string)reader["bookName"];
                    cart.AuthorName = (string)reader["AuthorName"];
                    cart.MRP = (decimal)reader["MRP"];
                    cart.DiscountPrice = (decimal)reader["Discountprice"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.UserId = (int)reader["userId"];
                    cart.BookId = (int)reader["BookId"];
                    carts.Add(cart);
                }
                return carts;

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

        public CartEntity EditCart(UpdateCartQuantity quantity)
        {
            try
            {
                conn.Open();
                SqlCommand Updatecmd = new SqlCommand("updateQuantity", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Updatecmd.Parameters.AddWithValue("@cartId", quantity.CartId);
                Updatecmd.Parameters.AddWithValue("@Quantity", quantity.Quantity);
                CartEntity cart = new CartEntity();
                SqlDataReader reader = Updatecmd.ExecuteReader();
                if (reader.Read())
                {
                    cart.CartId = (int)reader["cartId"];
                    cart.BookImg = (string)reader["bookImg"];
                    cart.BookName = (string)reader["bookName"];
                    cart.AuthorName = (string)reader["AuthorName"];
                    cart.MRP = (decimal)reader["MRP"];
                    cart.DiscountPrice = (decimal)reader["Discountprice"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.UserId = (int)reader["userId"];
                    cart.BookId = (int)reader["BookId"];
                    return cart;
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

        public bool RemoveCart(int cartId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveFromCart", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@cartId", cartId);
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

        public List<CartEntity> ViewAllCarts()
        {
            try
            {
                conn.Open();
                SqlCommand GetAllCartscmd = new SqlCommand("ViewAllCart", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                List<CartEntity> carts = new List<CartEntity>();
                SqlDataReader reader = GetAllCartscmd.ExecuteReader();
                while (reader.Read())
                {
                    CartEntity cart = new CartEntity();
                    cart.CartId = (int)reader["cartId"];
                    cart.BookImg = (string)reader["bookImg"];
                    cart.BookName = (string)reader["bookName"];
                    cart.AuthorName = (string)reader["AuthorName"];
                    cart.MRP = (decimal)reader["MRP"];
                    cart.DiscountPrice = (decimal)reader["Discountprice"];
                    cart.Quantity = (int)reader["Quantity"];
                    cart.UserId = (int)reader["userId"];
                    cart.BookId = (int)reader["BookId"];
                    carts.Add(cart);
                }
                return carts;

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

        public int CartCount(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAllCartByUserIdcmd = new SqlCommand("NoofCartsByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAllCartByUserIdcmd.Parameters.AddWithValue("@userId", UserId);
                int count = 0;
                SqlDataReader reader = GetAllCartByUserIdcmd.ExecuteReader();
                if (reader.Read())
                {
                    count= reader.GetInt32("CartCount");
                    return count;
                }
                return count;

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
