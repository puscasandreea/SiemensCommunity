﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class BorrowedProductDTO
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string User { get; set; }

        public string Name { get; set; }

        public bool IsAvailable { get; set; }

        public string ImagePath { get; set; }

        public double Rating { get; set; }

        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}