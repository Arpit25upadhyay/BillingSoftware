using AaryanBillingApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AaryanBillingApp.Models.Dtos
{
    public class CustomerDetailsDto
    {
        public Customer Customer { get; set; } = new();
    }
}
