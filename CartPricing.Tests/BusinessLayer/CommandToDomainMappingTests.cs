using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests.BusinessLayer
{

    public sealed class CommandToDomainMappingTests
    {
        private readonly CalculateCartTotalHandler _handler = new();

        [Fact]
        public void Maps_Individual_Command_To_IndividualClient_And_Computes_Total()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-42",
                FirstName = "Kranthi",
                LastName = "Lazarus",
                HighEndPhones = 1,
                MidRangePhones = 0,
                Laptops = 1
            };

            var (ok, errors, total) = _handler.Handle(cmd);

            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(2700m, total); // 1500 + 1200
        }

        [Fact]
        public void Maps_Professional_Command_To_ProfessionalClient_With_Vat_And_Computes_Total()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-99",
                CompanyName = "Test Company",
                VatNumber = "BE0123.456.789",
                RegistrationNumber = "REG-777",
                AnnualRevenue = 2_500_000m,
                HighEndPhones = 1,
                MidRangePhones = 1,
                Laptops = 1
            };

            var (ok, errors, total) = _handler.Handle(cmd);

            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(2750m, total);
        }

        [Fact]
        public void Trims_Whitespace_Before_Mapping_To_Domain()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "  I‑1  ",
                FirstName = "  Kranthi ",
                LastName = " Lazarus  ",
                HighEndPhones = 1,
                MidRangePhones = 2,
                Laptops = 1
            };

            var (ok, errors, total) = _handler.Handle(cmd);

            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(4300m, total); // 1500 + 1600 + 1200
        }

        [Fact]
        public void Rejects_Invalid_Command_Before_Mapping()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "Z",
                HighEndPhones = 1
            };

            var (ok, errors, total) = _handler.Handle(cmd);

            Assert.False(ok);
            Assert.NotNull(errors);
            Assert.Null(total);
            Assert.True(errors!.ContainsKey("client"));
        }
    }
}
