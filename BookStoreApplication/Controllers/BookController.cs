using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using GreenPipes.Caching;
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
    public class BookController : ControllerBase
    {
        private readonly IBooksBL booksBL;
        private readonly IDistributedCache _cache;
        public BookController(IBooksBL booksBL, IDistributedCache cache)
        {
            this.booksBL = booksBL;
            this._cache = cache;
        }
        [Authorize]
        [HttpPost("AddBook")]
        public IActionResult AddBook(BookModel book)
        {
            try
            {
                BookEntity result = booksBL.AddBook(book);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<BookEntity> { IsSuccess = false, Message = "Adding book Failed", Data = result });
                }
                else
                {
                    return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "Book Added successfull", Data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Adding Book throw Exception", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetByBookId")]
        public ActionResult FetchByBookId(int id)
        {
            try
            {
                var result = booksBL.GetByBookId(id);
                if (result != null)
                {
                    return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "select by Book Id successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "BookId does not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("UpdateBookDetails")]
        public ActionResult UpdateBookDetails(int BookId,BookModel model)
        {
            try
            {
                bool updateUser = booksBL.EditBookDetails(BookId,model);
                if (updateUser)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "user is updated by book by using BookId", Data = true });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "BookId does not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooksWithRedis()
        {
            string cacheKey = "BooksList";
            string serializationUserList;
            var BooksList = new List<BookEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                BooksList = JsonConvert.DeserializeObject<List<BookEntity>>(serializationUserList);
            }
            else
            {
                BooksList = booksBL.GetAllBooks();
                serializationUserList = JsonConvert.SerializeObject(BooksList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(BooksList);
        }
        [Authorize]
        [HttpDelete("RemoveBook")]
        public ActionResult RemoveBook(int BookId)
        {
            try
            {
                bool Remove = booksBL.RemoveBook(BookId);
                if (Remove)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Remove book from Book List by book Id", Data = Remove });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "It will given false value" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "while Removing Book throws Exception", Data = ex.Message });
            }
        }

        //Review Questions
        [HttpGet("FindBook")]
        public ActionResult FindBook(string BookName,string AuthorName)
        {
            try
            {
                BookEntity result = booksBL.FindBook(BookName,AuthorName);
                if (result != null)
                {
                    return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "select by Book Id successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "BookId does not exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateOrInsert")]
        public ActionResult UpdateOrInsert(int BookId, BookModel model)
        {
            try
            {
                BookEntity updateUser = booksBL.EditOrInsertBookDetails(BookId, model);
                if (updateUser.BookId==BookId)
                {
                    return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "user is updated the book by using BookId", Data = updateUser });
                }
                else if(updateUser!=null)
                {
                    return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "user is Inserted the book", Data = updateUser });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "BookId does not exist" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = ex.Message });
            }
        }
        [HttpGet("WishListwithUser")]
        public async Task<IActionResult> WishListwithUser()
        {
            string cacheKey = "WishListWithUser";
            string serializationUserList;
            var WishListWithUser = new List<WishListWithUser>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                WishListWithUser = JsonConvert.DeserializeObject<List<WishListWithUser>>(serializationUserList);
            }
            else
            {
                WishListWithUser = booksBL.WishListWithUser();
                serializationUserList = JsonConvert.SerializeObject(WishListWithUser);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(WishListWithUser);
        }
    }
}
