using KoiAuction.BussinessModels.Pagination;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.ISerivice;
using PRN231.AuctionKoi.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiAuction.Service.Base;
using KoiAuction.Common;

namespace KoiAuction.Service.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> CreateAuction(Auction auction)
        {
            try
            {
                auction.CreateDate = DateTime.Now;

                await _unitOfWork.AuctionRepository.Insert(auction);
                await _unitOfWork.SaveAsync();

                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, auction);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteAuction(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByCondition(x => x.AuctionId == id);
                if (auction == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "This auction does not exist to delete.");
                }

                _unitOfWork.AuctionRepository.Delete(auction);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                }

                return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAuctionById(int id)
        {
            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetAuctionByIdAsync(id);
                if (auction == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Auction not found.");
                }

                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, auction);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAllAuctions(string? searchKey, string? orderBy, int? pageIndex = null, int? pageSize = null)
        {
            try
            {
                var auctions = await _unitOfWork.AuctionRepository.GetAll();

                if (!string.IsNullOrEmpty(searchKey))
                {
                    auctions = auctions.Where(a => a.AuctionName.Contains(searchKey) || a.AuctionCode.Contains(searchKey));
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    auctions = orderBy.ToLower() switch
                    {
                        "name" => auctions.OrderBy(a => a.AuctionName),
                        "date" => auctions.OrderBy(a => a.AuctionDate),
                        _ => auctions.OrderBy(a => a.AuctionId)
                    };
                }

                var totalCount = auctions.Count();
                var items = auctions.Skip((pageIndex ?? 0) * (pageSize ?? 10))
                                    .Take(pageSize ?? 10)
                                    .ToList();

                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PageEntity<Auction>
                {
                    TotalRecord = totalCount,
                    List = items
                });
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> UpdateAuction(Auction auction)
        {
            try
            {
                var existingAuction = await _unitOfWork.AuctionRepository.GetByCondition(x => x.AuctionId == auction.AuctionId);
                if (existingAuction == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "This auction does not exist to update.");
                }

                existingAuction.AuctionName = auction.AuctionName ?? existingAuction.AuctionName;
                existingAuction.AuctionDate = auction.AuctionDate ?? existingAuction.AuctionDate;
                existingAuction.StartTime = auction.StartTime ?? existingAuction.StartTime;
                existingAuction.EndTime = auction.EndTime ?? existingAuction.EndTime;
                existingAuction.MinIncrement = auction.MinIncrement ?? existingAuction.MinIncrement;
                existingAuction.Status = auction.Status ?? existingAuction.Status;
                existingAuction.Description = auction.Description ?? existingAuction.Description;
                existingAuction.AutionMethod = auction.AutionMethod ?? existingAuction.AutionMethod;
                existingAuction.AuctionCode = auction.AuctionCode ?? existingAuction.AuctionCode;
                existingAuction.TimeSpan = auction.TimeSpan ?? existingAuction.TimeSpan;
                existingAuction.TypeId = auction.TypeId;

                _unitOfWork.AuctionRepository.Update(existingAuction);
                await _unitOfWork.SaveAsync();

                return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, existingAuction);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAuctionTypes()
        {
            try
            {
                var auctionTypes = await _unitOfWork.AuctionTypeRepository.GetAll();

                if (auctionTypes == null || !auctionTypes.Any())
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "No auction types found.");
                }

                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, auctionTypes);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

    }
}
