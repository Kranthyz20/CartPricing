using CartPricing.Core.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Application.Validation
{
    public sealed class CalculateCartTotalValidator
    {
        public (bool IsValid, Dictionary<string, string[]> Errors) Validate(CalculateCartTotal cmd)
        {
            var errors = new Dictionary<string, string[]>();

            var clientId = cmd.ClientId?.Trim();
            var firstName = cmd.FirstName?.Trim();
            var lastName = cmd.LastName?.Trim();
            var companyName = cmd.CompanyName?.Trim();
            var registrationNumber = cmd.RegistrationNumber?.Trim();
            var vatNumber = cmd.VatNumber?.Trim();
            var annualRevenue = cmd.AnnualRevenue;

            if (cmd.HighEndPhones < 0) errors["highEndPhones"] = new[] { "Quantity cannot be negative." };
            if (cmd.MidRangePhones < 0) errors["midRangePhones"] = new[] { "Quantity cannot be negative." };
            if (cmd.Laptops < 0) errors["laptops"] = new[] { "Quantity cannot be negative." };

            if (string.IsNullOrWhiteSpace(clientId))
                errors["clientId"] = new[] { "clientId is required." };

            // Identify Client Type 
            bool hasIndividual = !string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName);
            bool hasProfessional = !string.IsNullOrWhiteSpace(companyName)
                                   && !string.IsNullOrWhiteSpace(registrationNumber)
                                   && annualRevenue.HasValue;

            if (!hasIndividual && !hasProfessional)
                errors["client"] = new[] {
                    "Provide either individual (clientId, firstName, lastName) " +
                    "or professional (clientId, companyName, registrationNumber, annualRevenue) details."
                };

            if (hasIndividual && hasProfessional)
                errors["client"] = new[] { "Do not provide both individual and professional details." };

            if (hasProfessional && annualRevenue.HasValue && annualRevenue.Value < 0)
                errors["annualRevenue"] = new[] { "annualRevenue cannot be negative." };

            return (errors.Count == 0, errors);
        }
    }

}
