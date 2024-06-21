using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Model_Layer.Models;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersBL userBL;
        private readonly IBus _bus;
        public UsersController(IUsersBL users, IBus bus) 
        {
            this.userBL = users;
            this._bus = bus;
        }

        [HttpPost("SignIn")]
        public ActionResult SignIn(UserSignIn SignIn)
        {
            try
            {
                var result = userBL.SignIn(SignIn);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Register Failed", Data = "Email Already Exists" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "Register successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Register Failed", Data = ex.Message });
            }
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginModel loginModel)
        {
            if (userBL.CheckEmail(loginModel.Email))
            {
                var result = userBL.Login(loginModel);
                if (result == "Login Failed")
                {

                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Login Failed", Data = "Login Failed" });
                }
                else
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User login Successfull", Data = result });
                }
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email doesn't exist", Data = "try to add the email first" });
            }
        }
        [Authorize]
        [HttpPut("UpdateDetails")]
        public ActionResult UpdateDetails(UserSignIn model)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool updateUser = userBL.EditDetails(UserId, model);
                if (updateUser)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "user is updated by Id", Data = true });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "UserId did not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ForgotPasswordModel result = userBL.ForgotPassword(email);
            if (result != null)
            {
                Send send = new Send();
                send.SendMail(result.Email, result.Token);
                Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(result);
                return Ok(new ResponseModel<ForgotPasswordModel> { IsSuccess = true, Message = "execution successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Email doesn't exist" });
            }
        }
        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel Model)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            if (Email != null)
            {
                if (userBL.ResetUserPassword(Email, Model))
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Updated Password", Data = "This is the Updated password : " + Model.ConfirmPassword + " for Email of" + Email });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Password is not equal to confirm Password" });
                }
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Email doesn't exist." });
            }
        }
        [HttpGet("FetchById")]
        public ActionResult FetchById(int id)
        {
            try
            {
                var result = userBL.FetchById(id);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "select by Id successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "UserId did not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
