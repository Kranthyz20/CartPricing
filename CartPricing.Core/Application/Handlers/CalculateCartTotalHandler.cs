using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Validation;
using CartPricing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Application.Handlers
{

    public sealed class CalculateCartTotalHandler
    {
        private readonly CalculateCartTotalValidator _validator = new();
        private readonly CartCalculator _calculator = new();

        public (bool IsValid, Dictionary<string, string[]>? Errors, decimal? Total) Handle(CalculateCartTotal cmd)
        {
            var (ok, errors) = _validator.Validate(cmd);
            if (!ok) return (false, errors, null);

            var clientId = cmd.ClientId!.Trim();
            var firstName = cmd.FirstName?.Trim();
            var lastName = cmd.LastName?.Trim();
            var companyName = cmd.CompanyName?.Trim();
            var registrationNumber = cmd.RegistrationNumber?.Trim();
            var vatNumber = cmd.VatNumber?.Trim();
            var annualRevenue = cmd.AnnualRevenue;

            Client client;
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                client = new IndividualClient(clientId, firstName!, lastName!);
            }
            else
            {
                client = new ProfessionalClient(
                    clientId,
                    companyName!,
                    vatNumber,
                    registrationNumber!,
                    annualRevenue!.Value);
            }

            var cart = new ShoppingCart(cmd.HighEndPhones, cmd.MidRangePhones, cmd.Laptops);
            var total = _calculator.CalculateTotal(client, cart);

            return (true, null, total);
        }
    }
}
