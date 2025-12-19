
# 🛒 CartPricing – Solution Overview

## ✅ Intention & Understanding

Goal: **Calculate the total price of a shopping cart for a given client, based on client type and pricing rules.**

The company sells **three product categories**:
- High-end phone
- Mid-range phone
- Laptop

There are **two client types**:
- **Individual**: `clientId`, `firstName`, `lastName`
- **Professional**: `clientId`, `companyName`, `registrationNumber`, optional `vatNumber`, and `annualRevenue`

### Pricing Rules
- **Individual**:  
  High-end = €1500, Mid-range = €800, Laptop = €1200  
- **Professional**:  
  - Revenue > €10M → High-end = €1000, Mid-range = €550, Laptop = €900  
  - Revenue ≤ €10M → High-end = €1150, Mid-range = €600, Laptop = €1000  

The cart can contain **multiple quantities** of each product.  
The total must be calculated using these rules and returned in **EUR**.

---

## ✅ How I Approached It (as a Senior Developer / Technical Lead)

- **Lean Controller**: No business logic in the controller. It only maps the request to a command and delegates to the handler.
- **Application Layer**:  
  - Handles **validation**, **client type inference**, and **domain mapping**.
  - Uses a **pure calculator** for totals.
- **Domain Layer**:  
  - Explicit models: `IndividualClient`, `ProfessionalClient`, `ShoppingCart`.
  - Pricing encapsulated in **policies** with a **selector** for revenue tiers.
- **Error Handling**:  
  - Lightweight middleware ensures **consistent JSON responses** for all errors (400 for validation, 500 for unexpected exceptions).
- **Precision**:  
  - Used `decimal` for money.
  - Explicit rounding (`MidpointRounding.ToEven`) for deterministic results.
- **Validation**:  
  - Required fields per client type.
  - Quantities ≥ 0 (with a sanity cap).
  - Annual revenue ≥ 0 for professional.
  - Clear error messages for invalid input.

---

## ✅ Key Highlights

✔ Correct totals for all tiers  
✔ Client type inferred from payload   
✔ Clean separation of concerns  
✔ Robust validation and global error handling  
✔ Comprehensive tests (positive, negative, edge cases)  
✔ Future-proof design (easy to add products, discounts, VAT)  
✔ Standards: DI, immutability, domain-first, testability  

---

## ✅ Future Scope

- **Config-driven pricing** (read from DB or config instead of hardcoding).
- **Discounts & Promotions** via `IDiscountPolicy`.
- **VAT/Tax calculation** via `ITaxPolicy`.
- **API Versioning** for backward compatibility.
- **Swagger/OpenAPI** for better documentation.
- **CI/CD pipeline** with automated tests and coverage reports.

---

## ✅ Tests Included

- **Business Logic**:
  - Pricing policy unit prices.
  - Policy selection for all tiers.
  - Client inference rules.
  - Command → Domain mapping.
- **Calculator**:
  - Individual, professional high/low revenue totals.
  - Threshold edge cases.
  - Zero quantities.
  - Negative guards.
- **Validator**:
  - Missing/both Client Types.
  - Missing clientId.
  - Negative quantities.
   - Whitespace handling.
  - Missing annualRevenue for professional.

---

## ✅ How to Run

### Run API
```bash
dotnet run --project CartPricing.Api
```
### Endpoint
```bash
POST /api/cart/total
```
## Sample Request

### Individual Client
```bash
{
    "client": {
        "clientId": "I-100",
        "firstName" : "Kranthi",
        "lastName" : "Lazarus"
    },
    "highEndPhones": 1,
    "midRangePhones": 1,
    "laptops": 5
}
```
### Professional Client
```bash
{
  "client": {
    "clientId": "P-200",
    "companyName": "Test Company",
    "registrationNumber": "REG-2025-111",
    "annualRevenue": 2500000
  },
  "highEndPhones": 1,
  "midRangePhones": 1,
  "laptops": 1
}
```

## Sample Response
```bash
{
  "currency": "EUR",
  "total": 2750.00
}
```

## Run Tests
```bash
dotnet test
```
