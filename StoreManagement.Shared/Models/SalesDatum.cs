﻿using StoreManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Shared.Models
{
    public class SalesDatum
    {
        public int Id { get; set; } 

        public MarkdownPlan MarkdownPlan { get; set; }

        public int MarkdownPlanId { get; set; }

        public DateOnly SalesDate { get; set; }

        public int TotalSold { get; set; }

    }
}
