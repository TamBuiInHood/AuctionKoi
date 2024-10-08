using KoiAuction.BussinessModels.DetailProposalModel;
using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Common
{
    public class ODataResultConverter
    {
        public static IQueryable<ProposalModel> ConvertToQueryable(IBusinessResult businessResult)
        {
            if (businessResult.Data is PageEntity<ProposalModel> list)
            {
                return list.List.AsQueryable();
            }

            throw new InvalidOperationException("Invalid business result type");
        }
        public static IQueryable<DetailProposalModel> ConvertDetailProposalToQueryable(IBusinessResult businessResult)
        {
            if (businessResult.Data is PageEntity<DetailProposalModel> list)
            {
                return list.List.AsQueryable();
            }

            throw new InvalidOperationException("Invalid business result type");
        }
    }
}
