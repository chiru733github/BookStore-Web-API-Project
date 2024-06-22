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
    public class OrdersRepo:IOrdersRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public OrdersRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }

        public OrderEntity AddOrder(OrderModel orderModel)
        {
            try
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand("AddOrder", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Insertcmd.Parameters.AddWithValue("@CartId", orderModel.CartId);
                Insertcmd.Parameters.AddWithValue("@AddressId", orderModel.AddressId);
                OrderEntity order = new OrderEntity();
                SqlDataReader reader = Insertcmd.ExecuteReader();
                if (reader.Read())
                {
                    order.OrderId = (int)reader["OrderId"];
                    order.BookImg = (string)reader["BookImg"];
                    order.BookName = (string)reader["BookName"];
                    order.AuthorName = (string)reader["AuthorName"];
                    order.MRP = (decimal)reader["TotalMRP"];
                    order.DiscountPrice = (decimal)reader["ActualPrice"];
                    order.orderedDate = (DateTime)reader["OrderedDateTime"];
                    order.Quantity = (int)reader["Quantity"];
                    order.Address = (string)reader["Address"];
                    order.IsDeleted = (bool)reader["IsDeleted"];
                    order.AddressId = (int)reader["AddressId"];
                    order.UserId = (int)reader["UserId"];
                    order.BookId = (int)reader["BookId"];
                    return order;
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

        public List<OrderEntity> GetAllOrdersByUserId(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAllOrdersByUserIdcmd = new SqlCommand("GetAllOrdersByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAllOrdersByUserIdcmd.Parameters.AddWithValue("@userId", UserId);
                List<OrderEntity> orders = new List<OrderEntity>();
                SqlDataReader reader = GetAllOrdersByUserIdcmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderEntity order = new OrderEntity();
                    order.OrderId = (int)reader["OrderId"];
                    order.BookImg = (string)reader["BookImg"];
                    order.BookName = (string)reader["BookName"];
                    order.AuthorName = (string)reader["AuthorName"];
                    order.MRP = (decimal)reader["TotalMRP"];
                    order.DiscountPrice = (decimal)reader["ActualPrice"];
                    order.orderedDate = (DateTime)reader["OrderedDateTime"];
                    order.Quantity = (int)reader["Quantity"];
                    order.Address = (string)reader["Address"];
                    order.IsDeleted = (bool)reader["IsDeleted"];
                    order.AddressId = (int)reader["AddressId"];
                    order.UserId = (int)reader["UserId"];
                    order.BookId = (int)reader["BookId"];
                    orders.Add(order);
                }
                return orders;

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

        public bool CancellingOrder(int orderId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveOrder", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@OrderId", orderId);
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

        public List<OrderEntity> ViewAllOrders()
        {
            try
            {
                conn.Open();
                SqlCommand GetAllOrderscmd = new SqlCommand("ViewAllOrder", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                List<OrderEntity> orders = new List<OrderEntity>();
                SqlDataReader reader = GetAllOrderscmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderEntity order = new OrderEntity();
                    order.OrderId = (int)reader["OrderId"];
                    order.BookImg = (string)reader["BookImg"];
                    order.BookName = (string)reader["BookName"];
                    order.AuthorName = (string)reader["AuthorName"];
                    order.MRP = (decimal)reader["TotalMRP"];
                    order.DiscountPrice = (decimal)reader["ActualPrice"];
                    order.orderedDate = (DateTime)reader["OrderedDateTime"];
                    order.Quantity = (int)reader["Quantity"];
                    order.Address = (string)reader["Address"];
                    order.IsDeleted = (bool)reader["IsDeleted"];
                    order.AddressId = (int)reader["AddressId"];
                    order.UserId = (int)reader["UserId"];
                    order.BookId = (int)reader["BookId"];
                    orders.Add(order);
                }
                return orders;

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
