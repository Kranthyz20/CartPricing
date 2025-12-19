using CartPricing.Core.Application;
using CartPricing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests
{

    public sealed class CartCalculatorPricingTests
    {
        [Fact]
        public void Individual_CorrectTotals()
        {
            var sut = new CartCalculator();
            var total = sut.CalculateTotal(
                new IndividualClient("I-1", "Kranthi", "Lazarus"),
                new ShoppingCart(1, 2, 1));
            Assert.Equal(4300m, total);
        }

        [Fact]
        public void Professional_HighRevenue_CorrectTotals()
        {
            var sut = new CartCalculator();
            var total = sut.CalculateTotal(
                new ProfessionalClient("P-1", "Test Company", null, "REG", 12_000_000m),
                new ShoppingCart(2, 1, 3));
            Assert.Equal(5250m, total);
        }

        [Fact]
        public void Professional_LowRevenue_CorrectTotals()
        {
            var sut = new CartCalculator();
            var total = sut.CalculateTotal(
                new ProfessionalClient("P-2", "Test Company", null, "REG", 2_500_000m),
                new ShoppingCart(1, 1, 1));
            Assert.Equal(2750m, total);
        }

        [Fact]
        public void Threshold_Strictly_Greater_Than_10M()
        {
            var sut = new CartCalculator();
            var cart = new ShoppingCart(1, 1, 1);

            var lowTier = sut.CalculateTotal(
                new ProfessionalClient("P-eq", "Test Company", null, "REG", 10_000_000m), cart);
            var highTier = sut.CalculateTotal(
                new ProfessionalClient("P-gt", "Test Company2", null, "REG", 10_000_001m), cart);

            Assert.Equal(2750m, lowTier);    // 1150 + 600 + 1000
            Assert.Equal(2450m, highTier);   // 1000 + 550 + 900
        }

        [Fact]
        public void Zero_Quantities_Returns_Zero()
        {
            var sut = new CartCalculator();
            var total = sut.CalculateTotal(
                new IndividualClient("I-2", "Kranthi", "Lazarus"),
                new ShoppingCart(0, 0, 0));
            Assert.Equal(0m, total);
        }

        [Fact]
        public void ShoppingCart_NegativeQuantity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ShoppingCart(-1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ShoppingCart(0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ShoppingCart(0, 0, -1));
        }

        [Fact]
        public void ProfessionalClient_NegativeRevenue_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                    new ProfessionalClient("P-5", "Test Company", null, "REG-5", -1m));
        }
    }

}
