using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests
{
    public sealed class CalculateCartTotalValidatorTests
    {
        private readonly CalculateCartTotalValidator _sut = new();

        [Fact]
        public void Valid_Individual_Passes()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-1",
                FirstName = "Kranthi",
                LastName = "Lazarus",
                HighEndPhones = 1,
                Laptops = 1
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.True(ok);
            Assert.Empty(errors);
        }

        [Fact]
        public void Valid_Professional_Passes()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = "Test Company",
                RegistrationNumber = "REG-9",
                AnnualRevenue = 1_000_000m,
                MidRangePhones = 2
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.True(ok);
            Assert.Empty(errors);
        }

        [Fact]
        public void Missing_Both_Shapes_Fails()
        {
            var cmd = new CalculateCartTotal { ClientId = "X", HighEndPhones = 1 };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("client", errors.Keys);
        }

        [Fact]
        public void Both_Shapes_Present_Fails()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "X",
                FirstName = "A",
                LastName = "B",
                CompanyName = "C",
                RegistrationNumber = "R",
                AnnualRevenue = 1m
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("client", errors.Keys);
        }

        [Fact]
        public void Missing_ClientId_Fails()
        {
            var cmd = new CalculateCartTotal { FirstName = "A", LastName = "B" };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("clientId", errors.Keys);
        }

        [Theory]
        [InlineData(-1, 0, 0, "highEndPhones")]
        [InlineData(0, -1, 0, "midRangePhones")]
        [InlineData(0, 0, -1, "laptops")]
        public void Negative_Quantities_Fail(int h, int m, int l, string expectedKey)
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-1",
                FirstName = "A",
                LastName = "B",
                HighEndPhones = h,
                MidRangePhones = m,
                Laptops = l
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains(expectedKey, errors.Keys);
        }

        [Fact]
        public void Quantity_Cap_Boundary_Accept_Exact_Cap()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-1",
                FirstName = "A",
                LastName = "B",
                HighEndPhones = 100_000
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.True(ok);
            Assert.Empty(errors);
        }

        [Fact]
        public void Professional_Negative_Revenue_Fails()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = "C",
                RegistrationNumber = "R",
                AnnualRevenue = -10m
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("annualRevenue", errors.Keys);
        }

        [Fact]
        public void Whitespace_Only_ClientId_Treated_As_Missing()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "   ",
                FirstName = "Kranthi",
                LastName = "Lazarus"
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("clientId", errors.Keys);
        }

        [Fact]
        public void Professional_Missing_Required_Fields_Fails()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = null,
                RegistrationNumber = null,
                AnnualRevenue = null
            };
            var (ok, errors) = _sut.Validate(cmd);
            Assert.False(ok);
            Assert.Contains("client", errors.Keys); 
        }
    }


}
