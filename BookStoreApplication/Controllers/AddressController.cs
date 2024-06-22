using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using BusinessLayer.Services;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL addressBL;
        private readonly IDistributedCache _cache;
        public AddressController(IAddressBL address, IDistributedCache cache)
        {
            this.addressBL = address;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddAddress")]
        public IActionResult AddAddress(AddressModel address)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                AddressEntity result = addressBL.AddToAddress(UserId, address);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding Address Failed", Data = "It will given null value" });
                }
                else
                {
                    return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "Address Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Adding Address throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAllAddressByUserId")]
        public async Task<IActionResult> GetAllAddressByUserId()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string cacheKey = "AddressesList";
            string serializationUserList;
            var AddressesList = new List<AddressEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                AddressesList = JsonConvert.DeserializeObject<List<AddressEntity>>(serializationUserList);
            }
            else
            {
                AddressesList = addressBL.GetAllAddressByUserId(UserId);
                serializationUserList = JsonConvert.SerializeObject(AddressesList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(AddressesList);
        }
        [Authorize]
        [HttpPut("EditAddress")]
        public ActionResult EditAddress(int AddressId,AddressModel address)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                AddressEntity updateAddress = addressBL.EditAddress(AddressId,UserId,address);
                if (updateAddress != null)
                {
                    return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "user is updated Address by Address Id", Data = updateAddress });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given null value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Updating Address throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("RemoveAddress")]
        public ActionResult RemoveAddress(int addressId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool Remove = addressBL.RemoveAddress(UserId,addressId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Remove Address by Address Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Removing Address throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAddressById")]
        public ActionResult GetAddressById(int addressId)
        {
            try
            {
                int UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                AddressEntity result = addressBL.GetAddressById(UserId,addressId);
                if (result != null)
                {
                    return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "select Address by Address Id", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "AddressId does not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
