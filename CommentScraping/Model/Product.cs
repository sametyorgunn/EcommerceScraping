using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentScraping.Model
{
    public class Product
    {
        public string ProductName { get; set; }
        public List<Comment> Comment { get; set; }
    }
}
