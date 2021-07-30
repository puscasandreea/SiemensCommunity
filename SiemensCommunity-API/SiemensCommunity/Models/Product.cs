﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiemensCommunity.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string User { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public bool IsAvailable { get; set; }

        public string ImagePath { get; set; }

        public double Rating { get; set; }

        public string Details { get; set; }

        public Photo Photo { get; set; }
    }
}
