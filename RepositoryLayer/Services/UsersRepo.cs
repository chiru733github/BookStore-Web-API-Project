using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model_Layer.Models;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class UsersRepo : IUsersRepo
    {
        readonly SqlConnection conn = new SqlConnection();
        readonly string connString;
        readonly IConfiguration config;
        public UsersRepo(IConfiguration configuration)
        {
            this.config = configuration;
            connString = configuration.GetConnectionString("BookStoreDBConn");
            conn.ConnectionString = connString;
        }
        public UserEntity SignIn(UserSignIn model)
        {
            try
            {
                conn.Open();
                SqlCommand Insertcmd = new SqlCommand("signIn", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Insertcmd.Parameters.AddWithValue("@FullName", model.FullName);
                Insertcmd.Parameters.AddWithValue("@Email", model.Email);
                Insertcmd.Parameters.AddWithValue("@password", EncodePassword(model.Password));
                Insertcmd.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                UserEntity user = new UserEntity();
                SqlDataReader read = Insertcmd.ExecuteReader();
                if (read.Read())
                {
                    user.Id = (int)read["userId"];
                    user.FullName = (string)read["FullName"];
                    user.Email = (string)read["EmailId"];
                    user.Password = DecodePassword((string)read["Password"]);
                    user.MobileNumber = (string)read["MobileNumber"];
                    user.CreatedAt = (DateTime)read["UpdatedAt"];
                    user.UpdatedAt = (DateTime)read["CreatedAt"];
                    return user;
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
        public string Login(LoginModel model)
        {
            try
            {
                conn.Open();
                SqlCommand loginCmd = new SqlCommand("UserLogin", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                loginCmd.Parameters.AddWithValue("@Email", model.Email);
                loginCmd.Parameters.AddWithValue("@password", EncodePassword(model.Password));
                SqlDataReader reader = loginCmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = (int)reader["userId"];
                    string Email = (string)reader["EmailId"];
                    return GenerateToken(Email, userId);
                }
                return "Login Failed";
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
        public bool EditDetails(int userid, UserSignIn user)
        {
            try 
            {
                conn.Open();
                SqlCommand updateCmd = new SqlCommand("EditProfile", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                updateCmd.Parameters.AddWithValue("@UserId", userid);
                updateCmd.Parameters.AddWithValue("@FullName", user.FullName);
                updateCmd.Parameters.AddWithValue("@Email", user.Email);
                updateCmd.Parameters.AddWithValue("@password", EncodePassword(user.Password));
                updateCmd.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                updateCmd.ExecuteNonQuery();
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
        public bool CheckEmail(string email)
        {
            try
            {
                conn.Open();
                SqlCommand CheckEmailCmd = new SqlCommand("checkEmail", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                CheckEmailCmd.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = CheckEmailCmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;
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
        public ForgotPasswordModel ForgotPassword(string Email)
        {
            try
            {
                if (CheckEmail(Email))
                {
                    ForgotPasswordModel forgotmodel = new ForgotPasswordModel();
                    conn.Open();
                    SqlCommand CheckEmailCmd = new SqlCommand("checkEmail", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    CheckEmailCmd.Parameters.AddWithValue("@Email", Email);
                    SqlDataReader reader = CheckEmailCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        forgotmodel.Email = (string)reader["EmailId"];
                        forgotmodel.UserId = (int)reader["userId"];
                    }
                    forgotmodel.Token = GenerateToken(forgotmodel.Email, forgotmodel.UserId);
                    return forgotmodel;
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
        public bool ResetUserPassword(string Email, ResetPasswordModel Reset)
        {
            try
            {
                if (Reset.Password == Reset.ConfirmPassword)
                {
                    conn.Open();
                    SqlCommand ResetPSWCmd = new SqlCommand("ResetPassword", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    ResetPSWCmd.Parameters.AddWithValue("@Email", Email);
                    ResetPSWCmd.Parameters.AddWithValue("@password", EncodePassword(Reset.Password));
                    ResetPSWCmd.ExecuteNonQuery();
                    return true;
                }
                return false;
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
        public string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string DecodePassword(string password)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(password);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public UserEntity FetchById(int UserId)
        {
            try
            {
                conn.Open();
                SqlCommand GetById = new SqlCommand("GetById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                GetById.Parameters.AddWithValue("@UserId", UserId);
                UserEntity user = new UserEntity();
                SqlDataReader reader = GetById.ExecuteReader();
                if (reader.Read())
                {
                    user.Id = (int)reader["userId"];
                    user.FullName = (string)reader["FullName"];
                    user.Email = (string)reader["EmailId"];
                    user.Password = DecodePassword((string)reader["Password"]);
                    user.MobileNumber = (string)reader["MobileNumber"];
                    user.CreatedAt = (DateTime)reader["UpdatedAt"];
                    user.UpdatedAt = (DateTime)reader["CreatedAt"];
                    return user;
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
        private string GenerateToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",email),
                new Claim("UserId",userId.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
