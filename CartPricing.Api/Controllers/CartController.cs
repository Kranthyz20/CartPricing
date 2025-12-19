using CartPricing.Api.Models;
using CartPricing.Core.Application;
using CartPricing.Core.Application.Commands;
using CartPricing.Core.Application.Handlers;
using CartPricing.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartPricing.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public sealed class CartController : ControllerBase
    {
        private readonly CalculateCartTotalHandler _handler;
        public CartController(CalculateCartTotalHandler handler) => _handler = handler;

        [HttpPost("total")]
        public ActionResult<CartTotalResponse> GetTotal([FromBody] CartTotalRequest request)
        {
            var cmd = new CalculateCartTotal
            {
                ClientId = request.Client.ClientId,
                FirstName = request.Client.FirstName,
                LastName = request.Client.LastName,
                CompanyName = request.Client.CompanyName,
                VatNumber = request.Client.VatNumber,
                RegistrationNumber = request.Client.RegistrationNumber,
                AnnualRevenue = request.Client.AnnualRevenue,

                HighEndPhones = request.HighEndPhones,
                MidRangePhones = request.MidRangePhones,
                Laptops = request.Laptops
            };

            var (ok, errors, total) = _handler.Handle(cmd);
            if (!ok)
            {
                return BadRequest(new
                {
                    code = "VALIDATION_ERROR",
                    message = "Request validation failed. Fix the indicated fields.",
                    traceId = HttpContext.TraceIdentifier,
                    errors
                });
            }

            return Ok(new CartTotalResponse { Currency = "EUR", Total = total!.Value });
        }
    }
}
