using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.ISerivice
{
    public interface IAuctionService
    {
        Task<AuctionModel> CreateAuction(AuctionModel model);
        Task<AuctionModel> GetAuctionById(int id);
        Task<PageEntity<AuctionModel>> GetAllAuctions(string? searchKey, string? orderBy, int? pageIndex = null, int? pageSize = null);
        Task<AuctionModel> UpdateAuction(AuctionModel model);
        Task<bool> DeleteAuction(int id);
    }
}
