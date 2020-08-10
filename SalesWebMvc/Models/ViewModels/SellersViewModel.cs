using SalesWebMvc.Models.Enums;
using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellersViewModel
    {
        public ICollection<Seller> Sellers { get; set; }

        public SalesRecord SalesRecord { get; set; }

        public ICollection<SaleStatus> Status { get; set; }

    }
}
