using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class FeedBackRepo:IFeedBackRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public FeedBackRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }

        public FeedBackEntity AddFeedBack(int userId,FeedBackModel feedBack)
        {
            try
            {
                conn.Open();
                SqlCommand InsertFeedBackcmd = new SqlCommand("AddFeedBack", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                InsertFeedBackcmd.Parameters.AddWithValue("@Comment", feedBack.Comment);
                InsertFeedBackcmd.Parameters.AddWithValue("@rating", feedBack.rating);
                InsertFeedBackcmd.Parameters.AddWithValue("@userId", userId);
                InsertFeedBackcmd.Parameters.AddWithValue("@BookId", feedBack.BookId);
                FeedBackEntity review = new FeedBackEntity();
                SqlDataReader reader = InsertFeedBackcmd.ExecuteReader();
                if (reader.Read())
                {
                    review.FeedBackId = (int)reader["FeedBackId"];
                    review.UserName = (string)reader["UserName"];
                    review.Comment = (string)reader["Comment"];
                    review.rating = (int)reader["rating"];
                    review.IsDelete = (bool)reader["IsDelete"];
                    review.UserId = (int)reader["UserId"];
                    review.BookId = (int)reader["BookId"];
                    return review;
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

        public FeedBackEntity EditFeedBack(int UserId,int FeedbackId,FeedBackModel feedBack)
        {
            try
            {
                conn.Open();
                SqlCommand EditFeedBackcmd = new SqlCommand("EditFeedBack", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                EditFeedBackcmd.Parameters.AddWithValue("@FeedBackId", FeedbackId);
                EditFeedBackcmd.Parameters.AddWithValue("@Comment", feedBack.Comment);
                EditFeedBackcmd.Parameters.AddWithValue("@rating", feedBack.rating);
                EditFeedBackcmd.Parameters.AddWithValue("@userId", UserId);
                EditFeedBackcmd.Parameters.AddWithValue("@BookId", feedBack.BookId);
                FeedBackEntity review = new FeedBackEntity();
                SqlDataReader reader = EditFeedBackcmd.ExecuteReader();
                if (reader.Read())
                {
                    review.FeedBackId = (int)reader["FeedBackId"];
                    review.UserName = (string)reader["UserName"];
                    review.Comment = (string)reader["Comment"];
                    review.rating = (int)reader["rating"];
                    review.IsDelete = (bool)reader["IsDelete"];
                    review.UserId = (int)reader["UserId"];
                    review.BookId = (int)reader["BookId"];
                    return review;
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

        public bool RemoveFeedBack(int FeedbackId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveFeedBack", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@FeedBackId", FeedbackId);
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

        public List<FeedBackEntity> ViewAllFeedBack()
        {
            try
            {
                conn.Open();
                SqlCommand GetAllOrderscmd = new SqlCommand("ViewAllFeedBack", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                List<FeedBackEntity> feedBacks = new List<FeedBackEntity>();
                SqlDataReader reader = GetAllOrderscmd.ExecuteReader();
                while (reader.Read())
                {
                    FeedBackEntity review = new FeedBackEntity();
                    review.FeedBackId = (int)reader["FeedBackId"];
                    review.UserName = (string)reader["UserName"];
                    review.Comment = (string)reader["Comment"];
                    review.rating = (int)reader["rating"];
                    review.IsDelete = (bool)reader["IsDelete"];
                    review.UserId = (int)reader["UserId"];
                    review.BookId = (int)reader["BookId"];
                    feedBacks.Add(review);
                }
                return feedBacks;

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
