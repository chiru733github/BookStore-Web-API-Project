using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text;
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
    public class WishListController : ControllerBase
    {
        private readonly IWishListBL wishListBL;
        private readonly IDistributedCache _cache;
        public WishListController(IWishListBL listBL, IDistributedCache cache)
        {
            this.wishListBL = listBL;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddBookToWishList")]
        public IActionResult AddBookToWishList(int bookId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                WishListEntity result = wishListBL.AddToWishList(UserId, bookId);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding WishList Failed", Data = "It will given null value" });
                }
                else
                {
                    return Ok(new ResponseModel<WishListEntity> { IsSuccess = true, Message = "WishList Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Add WishList throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAllWishListByUserId")]
        public async Task<IActionResult> GetAllWishListsByUserId()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string cacheKey = "WishListsByUserId";
            string serializationUserList;
            var WishListsByUserId = new List<WishListEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                WishListsByUserId = JsonConvert.DeserializeObject<List<WishListEntity>>(serializationUserList);
            }
            else
            {
                WishListsByUserId = wishListBL.GetAllWishListByUserId(UserId);
                serializationUserList = JsonConvert.SerializeObject(WishListsByUserId);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(WishListsByUserId);
        }
        [Authorize]
        [HttpDelete("RemoveBookFromWishList")]
        public ActionResult RemoveBookFromWishList(int WishListId)
        {
            try
            {
                bool Remove = wishListBL.RemoveWishList(WishListId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Remove book from WishList by WishList Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Removing WishList throws Exception", Data = ex.Message });
            }
        }
        [HttpGet("ViewAllWishList")]
        public async Task<IActionResult> ViewAllWishList()
        {
            string cacheKey = "WishLists";
            string serializationUserList;
            var WishLists = new List<WishListEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                WishLists = JsonConvert.DeserializeObject<List<WishListEntity>>(serializationUserList);
            }
            else
            {
                WishLists = wishListBL.ViewAllWishLists();
                serializationUserList = JsonConvert.SerializeObject(WishLists);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(WishLists);
        }
    }
}
