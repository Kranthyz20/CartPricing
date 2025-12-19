using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain.Pricing
{
    public interface IPricingPolicy
    {
        decimal GetUnitPrice(ProductType product);
    }
}
