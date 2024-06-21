using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IUsersBL
    {
        UserEntity SignIn(UserSignIn model);
        string Login(LoginModel model);
        bool EditDetails(int userid, UserSignIn user);
        bool CheckEmail(string email);
        ForgotPasswordModel ForgotPassword(string Email);
        bool ResetUserPassword(string Email, ResetPasswordModel Model);
        UserEntity FetchById(int UserId);
    }
}
