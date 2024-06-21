using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class BookModel
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public float MRP { get; set; }
        public float DiscountPercentage { get; set; }
        public string BookImg { get; set; }
        public int Quantity { get; set; }
    }
}
