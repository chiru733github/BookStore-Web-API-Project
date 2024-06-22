using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class FeedBackEntity
    {
        public int FeedBackId { get; set; }
        public string UserName { get; set;}
        public string Comment { get; set;}
        public int rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public bool IsDelete { get; set; }
    }
}
