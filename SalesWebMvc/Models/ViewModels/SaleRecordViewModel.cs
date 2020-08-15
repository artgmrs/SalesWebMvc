using SalesWebMvc.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebMvc.Models.ViewModels
{
    public class SaleRecordViewModel
    {
        public SalesRecord SalesRecord { get; set; }
        public ICollection<Seller> Seller { get; set; }

        [NotMapped]
        public ICollection<SaleStatus> SaleStatus { get; set; }

    }
}
