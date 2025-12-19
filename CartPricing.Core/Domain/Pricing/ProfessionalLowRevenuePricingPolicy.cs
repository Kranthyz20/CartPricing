using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain.Pricing
{
    // Professionals with annual revenue <= €10M: high-end €1150, mid-range €600, laptop €1000
    public sealed class ProfessionalLowRevenuePricingPolicy : IPricingPolicy
    {
        public decimal GetUnitPrice(ProductType product) => product switch
        {
            ProductType.HighEndPhone => 1150m,
            ProductType.MidRangePhone => 600m,
            ProductType.Laptop => 1000m,
            _ => throw new ArgumentOutOfRangeException(nameof(product))
        };
    }

}
