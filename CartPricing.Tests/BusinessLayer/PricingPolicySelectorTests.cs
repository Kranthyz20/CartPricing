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
    public sealed class PricingPolicySelectorTests
    {
        [Fact]
        public void Selects_Individual_Policy_For_IndividualClient()
        {
            var client = new IndividualClient("I-1", "Kranthi", "Lazarus");

            var policy = PricingPolicySelector.For(client);

            Assert.IsType<IndividualPricingPolicy>(policy);
        }

        [Fact]
        public void Selects_Professional_LowRevenue_Policy_When_Revenue_Is_At_Or_Below_10M()
        {
            var low = new ProfessionalClient("P-1", "Test Company1", null, "REG-1", 2_500_000m);
            var equal = new ProfessionalClient("P-2", "Test Company2", null, "REG-2", 10_000_000m);

            var policyLow = PricingPolicySelector.For(low);
            var policyEqual = PricingPolicySelector.For(equal);

            Assert.IsType<ProfessionalLowRevenuePricingPolicy>(policyLow);
            Assert.IsType<ProfessionalLowRevenuePricingPolicy>(policyEqual);
        }

        [Fact]
        public void Selects_Professional_HighRevenue_Policy_When_Revenue_Is_Strictly_Greater_Than_10M()
        {
            var high = new ProfessionalClient("P-3", "Test Company3", null, "REG-3", 10_000_001m);

            var policy = PricingPolicySelector.For(high);

            Assert.IsType<ProfessionalHighRevenuePricingPolicy>(policy);
        }
    }
}
