using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using Model_Layer.Models;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UsersBL : IUsersBL
    {
        private readonly IUsersRepo _userRepo;
        public UsersBL(IUsersRepo Repo) 
        {
            this._userRepo = Repo;
        }
        public string Login(LoginModel model)
        {
            return _userRepo.Login(model);
        }

        public UserEntity SignIn(UserSignIn model)
        {
            return _userRepo.SignIn(model);
        }

        public bool EditDetails(int userid, UserSignIn user)
        {
            return _userRepo.EditDetails(userid, user);
        }

        public bool CheckEmail(string email)
        {
            return _userRepo.CheckEmail(email);
        }

        public ForgotPasswordModel ForgotPassword(string Email)
        {
            return _userRepo.ForgotPassword(Email);
        }

        public bool ResetUserPassword(string Email, ResetPasswordModel Model)
        {
            return _userRepo.ResetUserPassword(Email, Model);
        }

        public UserEntity FetchById(int UserId)
        {
            return _userRepo.FetchById(UserId);
        }
    }
}
