using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.ISerivice
{
    public interface IAuctionService
    {
          Task<IBusinessResult> CreateAuction(Auction auction);
        Task<IBusinessResult> GetAuctionById(int id);
        Task<IBusinessResult> GetAllAuctions(string? searchKey, string? orderBy, int? pageIndex = null, int? pageSize = null);
        Task<IBusinessResult> UpdateAuction(Auction auction);
        Task<IBusinessResult> DeleteAuction(int id);

        // Thêm các phương thức còn thiếu
       Task<IBusinessResult> GetAuctionTypes(); // Lấy danh sách các loại đấu giá
    //    bool AuctionExists(int id); // Kiểm tra sự tồn tại của một phiên đấu giá
    }
}
