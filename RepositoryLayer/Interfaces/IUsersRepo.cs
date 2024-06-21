using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IUsersRepo
    {
        UserEntity SignIn(UserSignIn model);
        string Login(LoginModel model);
        bool EditDetails(int userid, UserSignIn user);
        bool CheckEmail(string Email);
        ForgotPasswordModel ForgotPassword(string Email);
        bool ResetUserPassword(string Email, ResetPasswordModel Model);
        UserEntity FetchById(int UserId);
    }
}
