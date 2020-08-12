using SalesWebMvc.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models.ViewModels
{
    public class SaleRecordViewModel
    {
        public ICollection<Seller> Sellers { get; set; }

        public SalesRecord SaleRecord { get; set; }

        public ICollection<SaleStatus> Status { get; set; }

    }
}
