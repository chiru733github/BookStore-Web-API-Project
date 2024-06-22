using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class WishListWithUser
    {
        public int WishListId { get; set; }
        public string BookImg { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public decimal MRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
