using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests.BusinessLayer
{
    public sealed class ClientInferenceTests
    {
        private readonly CalculateCartTotalValidator _validator = new();

        [Fact]
        public void Infers_Individual_When_FirstName_And_LastName_Present()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "I-1",
                FirstName = "Kranthi",
                LastName = "Lazarus",
                HighEndPhones = 1
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.True(ok);
            Assert.Empty(errors);
        }

        [Fact]
        public void Infers_Professional_When_Company_Registration_Revenue_Present()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = "Test Company",
                RegistrationNumber = "REG-123",
                AnnualRevenue = 2_500_000m,
                MidRangePhones = 2
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.True(ok);
            Assert.Empty(errors);
        }

        [Fact]
        public void Rejects_Both_Shapes_Provided()
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

            var (ok, errors) = _validator.Validate(cmd);

            Assert.False(ok);
            Assert.True(errors.ContainsKey("client"));
        }

        [Fact]
        public void Rejects_Neither_Shape_Provided()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "X",
                HighEndPhones = 1
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.False(ok);
            Assert.True(errors.ContainsKey("client"));
        }

        [Fact]
        public void ClientId_Is_Required()
        {
            var cmd = new CalculateCartTotal
            {
                // No ClientId
                FirstName = "Kranthi",
                LastName = "Lazarus",
                HighEndPhones = 1
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.False(ok);
            Assert.True(errors.ContainsKey("clientId"));
        }

        [Fact]
        public void Professional_Shape_Requires_NonNegative_AnnualRevenue()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "P-1",
                CompanyName = "Test Company",
                RegistrationNumber = "REG-9",
                AnnualRevenue = -5m
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.False(ok);
            Assert.True(errors.ContainsKey("annualRevenue"));
        }

        [Fact]
        public void Whitespace_Only_Values_Are_Treated_As_Missing()
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = "   ",
                FirstName = "   ",
                LastName = "   ",
                HighEndPhones = 0
            };

            var (ok, errors) = _validator.Validate(cmd);

            Assert.False(ok);
            Assert.True(errors.ContainsKey("clientId"));
            Assert.True(errors.ContainsKey("client"));
        }
    }
}