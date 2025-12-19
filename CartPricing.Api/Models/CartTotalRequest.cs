using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CartPricing.Api.Models
{
    public sealed class ClientDto
    {
        public string? ClientId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? CompanyName { get; init; }
        public string? VatNumber { get; init; }
        public string? RegistrationNumber { get; init; }
        public decimal? AnnualRevenue { get; init; }
    }

    public sealed class CartTotalRequest
    {
        public ClientDto Client { get; init; } = new();
        public int HighEndPhones { get; init; }
        public int MidRangePhones { get; init; }
        public int Laptops { get; init; }
    }

    public sealed class CartTotalResponse
    {
        public string Currency { get; init; } = "EUR";
        public decimal Total { get; init; }
    }
}

