using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain
{
    public abstract class Client
    {
        public string ClientId { get; }

        protected Client(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("ClientId is required.", nameof(clientId));
            ClientId = clientId.Trim();
        }
    }

    public sealed class IndividualClient : Client
    {
        public string FirstName { get; }
        public string LastName { get; }

        public IndividualClient(string clientId, string firstName, string lastName) : base(clientId)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.", nameof(lastName));
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }
    }

    public sealed class ProfessionalClient : Client
    {
        public string CompanyName { get; }
        public string? VatNumber { get; } 
        public string RegistrationNumber { get; }
        public decimal AnnualRevenue { get; }

        public ProfessionalClient(
            string clientId,
            string companyName,
            string? vatNumber,
            string registrationNumber,
            decimal annualRevenue) : base(clientId)
        {
            if (string.IsNullOrWhiteSpace(companyName)) throw new ArgumentException("Company name is required.", nameof(companyName));
            if (string.IsNullOrWhiteSpace(registrationNumber)) throw new ArgumentException("Registration number is required.", nameof(registrationNumber));
            if (annualRevenue < 0) throw new ArgumentOutOfRangeException(nameof(annualRevenue), "Annual revenue cannot be negative.");

            CompanyName = companyName.Trim();
            VatNumber = string.IsNullOrWhiteSpace(vatNumber) ? null : vatNumber.Trim();
            RegistrationNumber = registrationNumber.Trim();
            AnnualRevenue = annualRevenue;
        }
    }

}
