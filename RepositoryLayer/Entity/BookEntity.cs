using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class BookEntity
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public decimal MRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public float rating { get; set;}
        public int NoofRatings { get; set;}
        public string BookImg { get; set;}
        public int Quantity { get; set;}
    }
}
