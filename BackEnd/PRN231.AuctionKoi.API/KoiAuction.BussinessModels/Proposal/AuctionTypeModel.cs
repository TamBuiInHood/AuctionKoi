using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.BussinessModels.Proposal
{
    public class AuctionTypeModel
    {
        public int TypeId { get; set; }
        public string? TypeCode { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public bool? IsActive { get; set; }
        public int? EndAfter { get; set; }
        public bool? AutoExtend { get; set; }
    }
}
