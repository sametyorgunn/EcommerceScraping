﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentScraping.Model
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductBrand { get; set; }
        public string ProductName { get; set; }
        public string ProductRating { get; set; }
        public string ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public Dictionary<string,string> ProductProperties { get; set; }
        public List<Comment> Comment { get; set; }
    }
}
