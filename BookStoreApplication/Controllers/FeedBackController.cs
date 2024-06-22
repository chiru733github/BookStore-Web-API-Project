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
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackBL feedBackBL;
        private readonly IDistributedCache _cache;
        public FeedBackController(IFeedBackBL feedBack, IDistributedCache cache)
        {
            this.feedBackBL = feedBack;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddFeedBack")]
        public IActionResult AddFeedBack(FeedBackModel feedBack)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                FeedBackEntity result = feedBackBL.AddFeedBack(UserId, feedBack);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding Feedback Failed", Data = "It will given null value" });
                }
                else
                {
                    return Ok(new ResponseModel<FeedBackEntity> { IsSuccess = true, Message = "FeedBack is Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Adding Feedback throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("EditFeedBack")]
        public ActionResult EditFeedBack(int FeedBackId,FeedBackModel feedBack)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                FeedBackEntity updateFeedBack = feedBackBL.EditFeedBack(UserId,FeedBackId,feedBack);
                if (updateFeedBack != null)
                {
                    return Ok(new ResponseModel<FeedBackEntity> { IsSuccess = true, Message = "user is updated FeedBack by FeedBack Id", Data = updateFeedBack });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given null value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Updating FeedBack throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("RemoveFeedBack")]
        public ActionResult RemoveFeedBack(int FeedBackId)
        {
            try
            {
                bool Remove = feedBackBL.RemoveFeedBack(FeedBackId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Remove FeedBack by FeedBack Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Removing FeedBack throws Exception", Data = ex.Message });
            }
        }
        [HttpGet("ViewAllFeedBacks")]
        public async Task<IActionResult> ViewAllFeedBacks()
        {
            string cacheKey = "FeedBackList";
            string serializationUserList;
            var FeedBackList = new List<FeedBackEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                FeedBackList = JsonConvert.DeserializeObject<List<FeedBackEntity>>(serializationUserList);
            }
            else
            {
                FeedBackList = feedBackBL.ViewAllFeedBack();
                serializationUserList = JsonConvert.SerializeObject(FeedBackList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(FeedBackList);
        }
    }
}
