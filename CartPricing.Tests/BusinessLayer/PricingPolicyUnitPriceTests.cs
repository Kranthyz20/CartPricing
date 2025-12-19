using CartPricing.Core.Domain;
using CartPricing.Core.Domain.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CartPricing.Tests.BusinessLayer
{

    public sealed class PricingPolicyUnitPriceTests
    {
        [Fact]
        public void Individual_Policy_Unit_Prices_Are_Correct()
        {
            var policy = new IndividualPricingPolicy();

            Assert.Equal(1500m, policy.GetUnitPrice(ProductType.HighEndPhone));
            Assert.Equal(800m, policy.GetUnitPrice(ProductType.MidRangePhone));
            Assert.Equal(1200m, policy.GetUnitPrice(ProductType.Laptop));
        }

        [Fact]
        public void Professional_HighRevenue_Policy_Unit_Prices_Are_Correct()
        {
            var policy = new ProfessionalHighRevenuePricingPolicy();

            Assert.Equal(1000m, policy.GetUnitPrice(ProductType.HighEndPhone));
            Assert.Equal(550m, policy.GetUnitPrice(ProductType.MidRangePhone));
            Assert.Equal(900m, policy.GetUnitPrice(ProductType.Laptop));
        }

        [Fact]
        public void Professional_LowRevenue_Policy_Unit_Prices_Are_Correct()
        {
            var policy = new ProfessionalLowRevenuePricingPolicy();

            Assert.Equal(1150m, policy.GetUnitPrice(ProductType.HighEndPhone));
            Assert.Equal(600m, policy.GetUnitPrice(ProductType.MidRangePhone));
            Assert.Equal(1000m, policy.GetUnitPrice(ProductType.Laptop));
        }
    }
}
