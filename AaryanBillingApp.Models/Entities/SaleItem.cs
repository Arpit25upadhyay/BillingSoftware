using System;
using System.Collections.Generic;
using System.Text;

namespace AaryanBillingApp.Models.Entities;

public class SaleItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Barcode { get; set; } = string.Empty;

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public int Stock { get; set; }
}
