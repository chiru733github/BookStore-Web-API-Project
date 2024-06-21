using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class WishListRepo : IWishListRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public WishListRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }
        public WishListEntity AddToWishList(int UserId, int bookId)
        {
            try
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand("AddToWishList", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Insertcmd.Parameters.AddWithValue("@userId", UserId);
                Insertcmd.Parameters.AddWithValue("@BookId", bookId);
                WishListEntity wishList = new WishListEntity();
                SqlDataReader reader = Insertcmd.ExecuteReader();
                if (reader.Read())
                {
                    wishList.WishListId = (int)reader["WishListId"];
                    wishList.BookImg = (string)reader["bookImg"];
                    wishList.BookName = (string)reader["bookName"];
                    wishList.AuthorName = (string)reader["AuthorName"];
                    wishList.MRP = (decimal)reader["MRP"];
                    wishList.DiscountPrice = (decimal)reader["Discountprice"];
                    wishList.UserId = (int)reader["userId"];
                    wishList.BookId = (int)reader["BookId"];
                    return wishList;
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

        public List<WishListEntity> GetAllWishListByUserId(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAllWishListByUserIdcmd = new SqlCommand("GetAllWishListByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAllWishListByUserIdcmd.Parameters.AddWithValue("@userId", UserId);
                List<WishListEntity> WishLists = new List<WishListEntity>();
                SqlDataReader reader = GetAllWishListByUserIdcmd.ExecuteReader();
                while (reader.Read())
                {
                    WishListEntity wishList = new WishListEntity();
                    wishList.WishListId = (int)reader["WishListId"];
                    wishList.BookImg = (string)reader["bookImg"];
                    wishList.BookName = (string)reader["bookName"];
                    wishList.AuthorName = (string)reader["AuthorName"];
                    wishList.MRP = (decimal)reader["MRP"];
                    wishList.DiscountPrice = (decimal)reader["Discountprice"];
                    wishList.UserId = (int)reader["userId"];
                    wishList.BookId = (int)reader["BookId"];
                    WishLists.Add(wishList);
                }
                return WishLists;

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

        public bool RemoveWishList(int WishListId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveFromWishList", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@WishListId", WishListId);
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

        public List<WishListEntity> ViewAllWishLists()
        {
            try
            {
                conn.Open();
                SqlCommand GetAllWishListcmd = new SqlCommand("ViewAllWishCart", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                List<WishListEntity> WishLists = new List<WishListEntity>();
                SqlDataReader reader = GetAllWishListcmd.ExecuteReader();
                while (reader.Read())
                {
                    WishListEntity wishList = new WishListEntity();
                    wishList.WishListId = (int)reader["WishListId"];
                    wishList.BookImg = (string)reader["bookImg"];
                    wishList.BookName = (string)reader["bookName"];
                    wishList.AuthorName = (string)reader["AuthorName"];
                    wishList.MRP = (decimal)reader["MRP"];
                    wishList.DiscountPrice = (decimal)reader["Discountprice"];
                    wishList.UserId = (int)reader["userId"];
                    wishList.BookId = (int)reader["BookId"];
                    WishLists.Add(wishList);
                }
                return WishLists;

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
