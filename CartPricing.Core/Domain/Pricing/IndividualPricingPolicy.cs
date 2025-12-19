using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain.Pricing
{
    // Individuals: high-end €1500, mid-range €800, laptop €1200
    public sealed class IndividualPricingPolicy : IPricingPolicy
    {
        public decimal GetUnitPrice(ProductType product) => product switch
        {
            ProductType.HighEndPhone => 1500m,
            ProductType.MidRangePhone => 800m,
            ProductType.Laptop => 1200m,
            _ => throw new ArgumentOutOfRangeException(nameof(product))
        };
    }
}
