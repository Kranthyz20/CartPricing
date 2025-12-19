using CartPricing.Core.Domain;
using CartPricing.Core.Domain.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Application
{
    public sealed class CartCalculator
    {
        public decimal CalculateTotal(Client client, ShoppingCart cart)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (cart is null) throw new ArgumentNullException(nameof(cart));

            var policy = PricingPolicySelector.For(client);

            decimal total = 0m;

            foreach (var (product, qty) in cart.Lines())
            {
                var unitPrice = policy.GetUnitPrice(product);

                checked
                {
                    total += unitPrice * qty;
                }
            }

            return decimal.Round(total, 2, MidpointRounding.ToZero);
        }
    }

}
