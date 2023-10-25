﻿namespace StoreManagement.Shared.Models
{
    public class MarkdownPlan
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int InitialInventory { get; set; }

        public DateTime? CurrentSaleDate { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public Decimal InitialReduction { get; set; }

        public Decimal IntermidiateReduction { get; set; }

        public Decimal FinalReduction { get; set; }

        public bool IsActive { get; set; }

        public bool SaleEnded { get; set; }

        public bool IntermediateCompleted { get; set; }

        public bool InitialCompleted { get; set; }

    }
}
