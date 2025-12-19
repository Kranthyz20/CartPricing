using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain.Pricing
{
    // Professionals with annual revenue > €10M: high-end €1000, mid-range €550, laptop €900
    public sealed class ProfessionalHighRevenuePricingPolicy : IPricingPolicy
    {
        public decimal GetUnitPrice(ProductType product) => product switch
        {
            ProductType.HighEndPhone => 1000m,
            ProductType.MidRangePhone => 550m,
            ProductType.Laptop => 900m,
            _ => throw new ArgumentOutOfRangeException(nameof(product))
        };
    }

}
