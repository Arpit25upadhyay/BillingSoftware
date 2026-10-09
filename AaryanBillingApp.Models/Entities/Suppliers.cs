using System;
using System.Collections.Generic;
using System.Text;

namespace AaryanBillingApp.Models.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        public string SupplierCode { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string? AlternateMobile { get; set; }

        public string? Email { get; set; }

        public string? GSTNumber { get; set; }

        public string? PANNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Pincode { get; set; }

        public decimal OpeningBalance { get; set; }

        public int PaymentDays { get; set; }

        public string? Remarks { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
