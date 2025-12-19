
using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests
{
    public sealed class CalculateCartTotalHandlerTests
    {
        private readonly CalculateCartTotalHandler _sut = new();

        [Fact]
        public void Infers_Individual_And_Computes_Total()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-1",
                FirstName = "Kranthi",
                LastName = "Lazarus",
                HighEndPhones = 1,
                Laptops = 1
            };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(2700m, total);
        }

        [Fact]
        public void Infers_Professional_LowRevenue_And_Computes_Total()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = "Test Company",
                RegistrationNumber = "REG-1",
                AnnualRevenue = 2_500_000m,
                HighEndPhones = 1,
                MidRangePhones = 1,
                Laptops = 1
            };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(2750m, total);
        }

        [Fact]
        public void Infers_Professional_HighRevenue_And_Computes_Total()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-2",
                CompanyName = "Test Company",
                RegistrationNumber = "REG-2",
                AnnualRevenue = 12_000_000m,
                HighEndPhones = 2,
                MidRangePhones = 1,
                Laptops = 3
            };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(5250m, total);
        }

        [Fact]
        public void Rejects_Both_Shapes()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "X",
                FirstName = "A",
                LastName = "B",
                CompanyName = "C",
                RegistrationNumber = "R",
                AnnualRevenue = 1_000_000m
            };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.False(ok);
            Assert.NotNull(errors);
            Assert.True(errors!.ContainsKey("client"));
            Assert.Null(total);
        }

        [Fact]
        public void Rejects_Missing_Shape()
        {
            var cmd = new CalculateCartTotal { ClientId = "X", HighEndPhones = 1 };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.False(ok);
            Assert.True(errors!.ContainsKey("client"));
            Assert.Null(total);
        }

        [Fact]
        public void Trims_Whitespace_In_Mapping()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "  I-1  ",
                FirstName = "  Kranthi ",
                LastName = " Lazarus  ",
                HighEndPhones = 1,
                MidRangePhones = 2,
                Laptops = 1
            };
            var (ok, errors, total) = _sut.Handle(cmd);
            Assert.True(ok);
            Assert.Null(errors);
            Assert.Equal(4300m, total);
        }
    }


}
