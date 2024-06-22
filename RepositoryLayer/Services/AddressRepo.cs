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
    public class AddressRepo:IAddressRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public AddressRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }

        public AddressEntity AddToAddress(int UserId, AddressModel address)
        {
            try
            {
                conn.Open();
                SqlCommand InsertAddresscmd = new SqlCommand("AddAddress", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                InsertAddresscmd.Parameters.AddWithValue("@UserId", UserId);
                InsertAddresscmd.Parameters.AddWithValue("@FullName", address.FullName);
                InsertAddresscmd.Parameters.AddWithValue("@MobileNumber", address.MobileNumber);
                InsertAddresscmd.Parameters.AddWithValue("@Address", address.Address);
                InsertAddresscmd.Parameters.AddWithValue("@City", address.City);
                InsertAddresscmd.Parameters.AddWithValue("@State", address.State);
                InsertAddresscmd.Parameters.AddWithValue("@Type", address.Type);
                AddressEntity addressEntity = new AddressEntity();
                SqlDataReader reader = InsertAddresscmd.ExecuteReader();
                if (reader.Read())
                {
                    addressEntity.AddressId = (int)reader["AddressId"];
                    addressEntity.FullName = (string)reader["FullName"];
                    addressEntity.MobileNumber = (string)reader["MobileNumber"];
                    addressEntity.Address = (string)reader["Address"];
                    addressEntity.City = (string)reader["City"];
                    addressEntity.State = (string)reader["State"];
                    addressEntity.Type = (string)reader["Type"];
                    addressEntity.UserId = (int)reader["userId"];
                    addressEntity.IsDeleted = (bool)reader["IsDeleted"];
                    return addressEntity;
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

        public List<AddressEntity> GetAllAddressByUserId(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAllAddressByUserIdcmd = new SqlCommand("GetAllAddressByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAllAddressByUserIdcmd.Parameters.AddWithValue("@userId", UserId);
                List<AddressEntity> addresses = new List<AddressEntity>();
                SqlDataReader reader = GetAllAddressByUserIdcmd.ExecuteReader();
                while (reader.Read())
                {
                    AddressEntity addressEntity = new AddressEntity();
                    addressEntity.AddressId = (int)reader["AddressId"];
                    addressEntity.FullName = (string)reader["FullName"];
                    addressEntity.MobileNumber = (string)reader["MobileNumber"];
                    addressEntity.Address = (string)reader["Address"];
                    addressEntity.City = (string)reader["City"];
                    addressEntity.State = (string)reader["State"];
                    addressEntity.Type = (string)reader["Type"];
                    addressEntity.UserId = (int)reader["userId"];
                    addressEntity.IsDeleted = (bool)reader["IsDeleted"];
                    addresses.Add(addressEntity);
                }
                return addresses;

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

        public AddressEntity EditAddress(int AddressId,int UserId,AddressModel address)
        {
            try
            {
                conn.Open();
                SqlCommand UpdateAddresscmd = new SqlCommand("EditAddress", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                UpdateAddresscmd.Parameters.AddWithValue("@AddressId", AddressId);
                UpdateAddresscmd.Parameters.AddWithValue("@UserId", UserId);
                UpdateAddresscmd.Parameters.AddWithValue("@FullName", address.FullName);
                UpdateAddresscmd.Parameters.AddWithValue("@MobileNumber", address.MobileNumber);
                UpdateAddresscmd.Parameters.AddWithValue("@Address", address.Address);
                UpdateAddresscmd.Parameters.AddWithValue("@City", address.City);
                UpdateAddresscmd.Parameters.AddWithValue("@State", address.State);
                UpdateAddresscmd.Parameters.AddWithValue("@Type", address.Type);
                AddressEntity UpdatedAddress = new AddressEntity();
                SqlDataReader reader = UpdateAddresscmd.ExecuteReader();
                if (reader.Read())
                {
                    UpdatedAddress.AddressId = (int)reader["AddressId"];
                    UpdatedAddress.FullName = (string)reader["FullName"];
                    UpdatedAddress.MobileNumber = (string)reader["MobileNumber"];
                    UpdatedAddress.Address = (string)reader["Address"];
                    UpdatedAddress.City = (string)reader["City"];
                    UpdatedAddress.State = (string)reader["State"];
                    UpdatedAddress.Type = (string)reader["Type"];
                    UpdatedAddress.UserId = (int)reader["userId"];
                    UpdatedAddress.IsDeleted = (bool)reader["IsDeleted"];
                    return UpdatedAddress;
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

        public bool RemoveAddress(int userId, int AddressId)
        {
            try
            {
                conn.Open();
                SqlCommand Removecmd = new SqlCommand("RemoveFromAddress", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Removecmd.Parameters.AddWithValue("@UserId", userId);
                Removecmd.Parameters.AddWithValue("@AddressId", AddressId);
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

        public AddressEntity GetAddressById(int UserId,int AddressId)
        {
            try
            {
                conn.Open();
                SqlCommand GetAddressById = new SqlCommand("GetAddressByAddressId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetAddressById.Parameters.AddWithValue("@userId", UserId);
                GetAddressById.Parameters.AddWithValue("@AddressId",AddressId);
                AddressEntity address = new AddressEntity();
                SqlDataReader reader = GetAddressById.ExecuteReader();
                if (reader.Read())
                {
                    address.AddressId = (int)reader["AddressId"];
                    address.FullName = (string)reader["FullName"];
                    address.MobileNumber = (string)reader["MobileNumber"];
                    address.Address = (string)reader["Address"];
                    address.City = (string)reader["City"];
                    address.State = (string)reader["State"];
                    address.Type = (string)reader["Type"];
                    address.UserId = (int)reader["userId"];
                    address.IsDeleted = (bool)reader["IsDeleted"];
                    return address;
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
    }
}
