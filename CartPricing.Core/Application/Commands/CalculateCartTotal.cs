using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Application.Commands
{
    public sealed class CalculateCartTotal
    {
        public string? ClientId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? CompanyName { get; init; }
        public string? VatNumber { get; init; }  
        public string? RegistrationNumber { get; init; }
        public decimal? AnnualRevenue { get; init; }
        public int HighEndPhones { get; init; }
        public int MidRangePhones { get; init; }
        public int Laptops { get; init; }

    }
}
