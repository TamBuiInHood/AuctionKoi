using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.ISerivice;
using PRN231.AuctionKoi.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuctionModel> CreateAuction(AuctionModel model)
        {
            var auctionEntity = new Auction
            {
                AuctionName = model.AuctionName,
                AuctionDate = model.AuctionDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                MinIncrement = model.MinIncrement,
                Status = model.Status,
                Description = model.Description,
                CreateDate = DateTime.Now,
                AutionMethod = model.AutionMethod,  // Corrected property name from AutionMethod to AuctionMethod
                AuctionCode = model.AuctionCode,
                TimeSpan = model.TimeSpan,
                TypeId = model.TypeId
            };

            await _unitOfWork.AuctionRepository.Insert(auctionEntity);
            await _unitOfWork.SaveAsync();
            return MapToAuctionModel(auctionEntity); // Convert to AuctionModel
        }

        public async Task<bool> DeleteAuction(int id)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByCondition(x => x.AuctionId == id);
            if (auction == null)
            {
                throw new Exception("This auction does not exist to delete");
            }

            _unitOfWork.AuctionRepository.Delete(auction);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<AuctionModel> GetAuctionById(int id)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByCondition(x => x.AuctionId == id);
            if (auction == null)
            {
                throw new Exception("This auction does not exist");
            }
            return MapToAuctionModel(auction); // Convert to AuctionModel
        }

        public async Task<PageEntity<AuctionModel>> GetAllAuctions(string? searchKey, string? orderBy, int? pageIndex = null, int? pageSize = null)
        {
            var auctions = await _unitOfWork.AuctionRepository.GetAll(); // Lấy tất cả phiên đấu giá

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchKey))
            {
                auctions = auctions.Where(a => a.AuctionName.Contains(searchKey) || a.AuctionCode.Contains(searchKey));
            }

            // Sắp xếp
            if (!string.IsNullOrEmpty(orderBy))
            {
                auctions = orderBy.ToLower() switch
                {
                    "name" => auctions.OrderBy(a => a.AuctionName),
                    "date" => auctions.OrderBy(a => a.AuctionDate),
                    _ => auctions.OrderBy(a => a.AuctionId)
                };
            }

            // Phân trang
            var totalCount = auctions.Count();
            var items = auctions.Skip((pageIndex ?? 0) * (pageSize ?? 10))
                                .Take(pageSize ?? 10)
                                .Select(auction => MapToAuctionModel(auction)) // Chuyển sang AuctionModel
                                .ToList();

            return new PageEntity<AuctionModel>
            {
                TotalRecord = totalCount,
                List = items
            };
        }


        public async Task<AuctionModel> UpdateAuction(AuctionModel model)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByCondition(x => x.AuctionId == model.AuctionId);
            if (auction == null)
            {
                throw new Exception("This auction does not exist to update");
            }

            // Update information
            auction.AuctionName = model.AuctionName;
            auction.AuctionDate = model.AuctionDate;
            auction.StartTime = model.StartTime;
            auction.EndTime = model.EndTime;
            auction.MinIncrement = model.MinIncrement;
            auction.Status = model.Status;
            auction.Description = model.Description;
            auction.AutionMethod = model.AutionMethod; // Corrected property name
            auction.AuctionCode = model.AuctionCode;
            auction.TimeSpan = model.TimeSpan;
            auction.TypeId = model.TypeId;

            _unitOfWork.AuctionRepository.Update(auction);
            await _unitOfWork.SaveAsync();
            return MapToAuctionModel(auction); // Convert back to AuctionModel
        }

        private AuctionModel MapToAuctionModel(Auction auction)
        {
            return new AuctionModel
            {
                AuctionId = auction.AuctionId,
                AuctionName = auction.AuctionName,
                AuctionDate = auction.AuctionDate,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                MinIncrement = auction.MinIncrement,
                Status = auction.Status,
                Description = auction.Description,
                CreateDate = auction.CreateDate,
                AutionMethod = auction.AutionMethod, // Corrected property name
                AuctionCode = auction.AuctionCode,
                TimeSpan = auction.TimeSpan,
                TypeId = auction.TypeId
            };
        }
    }

}
