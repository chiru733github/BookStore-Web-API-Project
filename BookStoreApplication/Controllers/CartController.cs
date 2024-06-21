using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Entity;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartsBL;
        private readonly IDistributedCache _cache;
        public CartController(ICartBL cartBL, IDistributedCache cache)
        {
            this.cartsBL = cartBL;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddBookToCart")]
        public IActionResult AddBookToCart(int bookId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                CartEntity result = cartsBL.AddToCart(UserId,bookId);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding cart Failed", Data = "It will given null value" });
                }
                else
                {
                    return Ok(new ResponseModel<CartEntity> { IsSuccess = true, Message = "Cart Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Add cart throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAllCartsByUserId")]
        public async Task<IActionResult> GetAllCartsByUserId()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string cacheKey = "CartsList";
            string serializationUserList;
            var CartsList = new List<CartEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                CartsList = JsonConvert.DeserializeObject<List<CartEntity>>(serializationUserList);
            }
            else
            {
                CartsList = cartsBL.GetAllCarts(UserId);
                serializationUserList = JsonConvert.SerializeObject(CartsList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(CartsList);
        }
        [Authorize]
        [HttpPut("UpdateQuantity")]
        public ActionResult UpdateQuantity(UpdateCartQuantity quantity)
        {
            try
            {
                CartEntity updateCart = cartsBL.EditCart(quantity);
                if (updateCart!=null)
                {
                    return Ok(new ResponseModel<CartEntity> { IsSuccess = true, Message = "user is updated quantity by cart Id", Data = updateCart });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given null value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Updating Cart throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("RemoveBookFromCart")]
        public ActionResult RemoveBookFromCart(int cartId)
        {
            try
            {
                bool Remove = cartsBL.RemoveCart(cartId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Remove book from cart by cart Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Removing Cart throws Exception", Data = ex.Message });
            }
        }
        [HttpGet("ViewAllCarts")]
        public async Task<IActionResult> ViewAllCarts()
        {
            string cacheKey = "CartsList";
            string serializationUserList;
            var CartsList = new List<CartEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                CartsList = JsonConvert.DeserializeObject<List<CartEntity>>(serializationUserList);
            }
            else
            {
                CartsList = cartsBL.ViewAllCarts();
                serializationUserList = JsonConvert.SerializeObject(CartsList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(CartsList);
        }
        [Authorize]
        [HttpGet("NoofCartCountByUserId")]
        public ActionResult NoofCartCountByUserId()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int CountofCart = cartsBL.CartCount(UserId);
                if (CountofCart>=1)
                {
                    return Ok(new ResponseModel<int> { IsSuccess = true, Message = "Count of cart by UserId", Data = CountofCart });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given zero value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Updating Cart throws Exception", Data = ex.Message });
            }
        }
    }
}
