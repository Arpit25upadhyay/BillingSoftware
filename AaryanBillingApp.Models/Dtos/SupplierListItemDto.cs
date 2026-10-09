using System;
using System.Collections.Generic;
using System.Text;

namespace AaryanBillingApp.Models.Dtos
{
    public class SupplierListItemDto
    {
        public int Id { get; set; }

        public string SupplierCode { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public decimal OpeningBalance { get; set; }

        public int PaymentDays { get; set; }

        public bool IsActive { get; set; }
    }
}
