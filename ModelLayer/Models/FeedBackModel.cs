using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelLayer.Models
{
    public class FeedBackModel
    {
        public string Comment { get; set; }
        public int rating { get; set; }
        public int BookId { get; set; }
    }
}
