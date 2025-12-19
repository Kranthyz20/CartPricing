using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartPricing.Core.Domain
{
    public sealed class ShoppingCart
    {
        public int HighEndPhoneQty { get; }
        public int MidRangePhoneQty { get; }
        public int LaptopQty { get; }

        public ShoppingCart(int highEndPhoneQty, int midRangePhoneQty, int laptopQty)
        {
            if (highEndPhoneQty < 0) throw new ArgumentOutOfRangeException(nameof(highEndPhoneQty), "Quantity cannot be negative.");
            if (midRangePhoneQty < 0) throw new ArgumentOutOfRangeException(nameof(midRangePhoneQty), "Quantity cannot be negative.");
            if (laptopQty < 0) throw new ArgumentOutOfRangeException(nameof(laptopQty), "Quantity cannot be negative.");

            HighEndPhoneQty = highEndPhoneQty;
            MidRangePhoneQty = midRangePhoneQty;
            LaptopQty = laptopQty;
        }

        public IEnumerable<(ProductType Product, int Quantity)> Lines()
        {
            if (HighEndPhoneQty > 0) yield return (ProductType.HighEndPhone, HighEndPhoneQty);
            if (MidRangePhoneQty > 0) yield return (ProductType.MidRangePhone, MidRangePhoneQty);
            if (LaptopQty > 0) yield return (ProductType.Laptop, LaptopQty);
        }
    }

}
