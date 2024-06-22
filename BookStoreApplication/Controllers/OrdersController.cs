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
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersBL ordersBL;
        private readonly IDistributedCache _cache;
        public OrdersController(IOrdersBL orderBL, IDistributedCache cache)
        {
            this.ordersBL = orderBL;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddOrder")]
        public IActionResult AddOrder(OrderModel order)
        {
            try
            {
                OrderEntity result = ordersBL.AddOrder(order);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding order Failed", Data = "It will given null value" });
                }
                else
                {
                    return Ok(new ResponseModel<OrderEntity> { IsSuccess = true, Message = "order Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Add order throws Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAllOrdersByUserId")]
        public async Task<IActionResult> GetAllOrdersByUserId()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string cacheKey = "OrdersList";
            string serializationUserList;
            var OrdersList = new List<OrderEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                OrdersList = JsonConvert.DeserializeObject<List<OrderEntity>>(serializationUserList);
            }
            else
            {
                OrdersList = ordersBL.GetAllOrdersByUserId(UserId);
                serializationUserList = JsonConvert.SerializeObject(OrdersList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(OrdersList);
        }
        [HttpDelete("CancellingOrder")]
        public ActionResult CancellingOrder(int orderId)
        {
            try
            {
                bool Remove = ordersBL.CancellingOrder(orderId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Cancelling order by using order Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while cancelling order throws Exception", Data = ex.Message });
            }
        }
        [HttpGet("ViewAllOrders")]
        public async Task<IActionResult> ViewAllOrders()
        {
            string cacheKey = "AllOrdersList";
            string serializationUserList;
            var AllOrdersList = new List<OrderEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                AllOrdersList = JsonConvert.DeserializeObject<List<OrderEntity>>(serializationUserList);
            }
            else
            {
                AllOrdersList = ordersBL.ViewAllOrders();
                serializationUserList = JsonConvert.SerializeObject(AllOrdersList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(AllOrdersList);
        }
    }
}
