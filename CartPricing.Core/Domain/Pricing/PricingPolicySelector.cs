using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain.Pricing
{
    public static class PricingPolicySelector
    {
        private const decimal HighRevenueThreshold = 10_000_000m;

        public static IPricingPolicy For(Client client)
        {
            return client switch
            {
                IndividualClient => new IndividualPricingPolicy(),
                ProfessionalClient pro => pro.AnnualRevenue > HighRevenueThreshold
                    ? new ProfessionalHighRevenuePricingPolicy()
                    : new ProfessionalLowRevenuePricingPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(client), "Unsupported client type.")
            };
        }
    }

}
