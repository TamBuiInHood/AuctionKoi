using AutoMapper;
using Firebase.Storage;
using KoiAuction.BussinessModels.DetailProposalModel;
using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Common;
using KoiAuction.Common.Utils;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using Microsoft.AspNetCore.Http;
using PRN231.AuctionKoi.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.Services
{
    public class DetailProposalService : IDetailProposalService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mappper;

        public DetailProposalService(IUnitOfWork unitOfWork, IMapper mappper)
        {
            _unitOfWork = unitOfWork;
            _mappper = mappper;
        }

        public async Task<IBusinessResult> Delete(int id)
        {

            try
            {
                var checkExist = await _unitOfWork.DetailProposalRepository.GetByID(id);
                if (checkExist != null)
                {
                    var result = await _unitOfWork.DetailProposalRepository.DeleteDetailProposal(id);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, result);
                    }
                }
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Can not find any detail proposal with that id");
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_DELETE_MSG, ex.Message.ToString());
            }
        }

        public async Task<IBusinessResult> Get(PaginationParameter paginationParameter)
        {
            try
            {
                Expression<Func<DetailProposal, bool>> filter = null!;
                Func<IQueryable<DetailProposal>, IOrderedQueryable<DetailProposal>> orderBy = null!;
                if (!(paginationParameter.Search == null || paginationParameter.Search.Equals("")))
                {
                    int validInt = 0;
                    double validDouble = 0;
                    var checkInt = int.TryParse(paginationParameter.Search, out validInt);
                    var checkDouble = double.TryParse(paginationParameter.Search, out validDouble);
                    var validDate = DateOnly.MinValue;
                    if (checkInt)
                    {
                        filter = x => x.FishId == validInt
                                    || x.Age == validInt
                                    || x.Rating == validInt
                                    || x.TimeSpan == validInt
                                    || x.Index == validInt
                                    || x.MinIncrement == validInt;
                    }
                    else if(DateOnly.TryParse(paginationParameter.Search, out validDate))
                    {
                        filter = x => x.CreateDate == validDate
                                      || x.UpdateDate == validDate;
                    }
                    else if (checkDouble)
                    {
                        filter = x => x.InitialPrice == validDouble
                                      || x.FinalPrice == validDouble
                                      || x.Length == validDouble
                                      || x.AuctionFee == validDouble
                                      || x.Weight == validDouble;
                    }
                    else
                    {
                        filter = x => x.FishCode.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.FishName.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Description.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Status.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Gender.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Color.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Farm.FarmName.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.Auction.AuctionName.ToLower().Contains(paginationParameter.Search.ToLower())
                                      || x.FishType.FishTypeName.ToLower().Contains(paginationParameter.Search.ToLower());
                    }
                }
                switch (paginationParameter.SortBy)
                {
                    case "fishid":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.FishId)
                                   : x => x.OrderBy(x => x.FishId) : x => x.OrderBy(x => x.FishId);
                        break;
                    case "fishcode":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.FishCode)
                                   : x => x.OrderBy(x => x.FishCode) : x => x.OrderBy(x => x.FishCode);
                        break;
                    case "fishname":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.FishName)
                                   : x => x.OrderBy(x => x.FishName) : x => x.OrderBy(x => x.FishName);
                        break;
                    case "gender":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Gender)
                                   : x => x.OrderBy(x => x.Gender) : x => x.OrderBy(x => x.Gender);
                        break;
                    case "age":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Age)
                                   : x => x.OrderBy(x => x.Age) : x => x.OrderBy(x => x.Age);
                        break;
                    case "length":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Length)
                                   : x => x.OrderBy(x => x.Length) : x => x.OrderBy(x => x.Length);
                        break;
                    case "weight":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Weight)
                                   : x => x.OrderBy(x => x.Weight) : x => x.OrderBy(x => x.Weight);
                        break;
                    case "rating":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Rating)
                                   : x => x.OrderBy(x => x.Rating) : x => x.OrderBy(x => x.Rating);
                        break;
                    case "status":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Status)
                                   : x => x.OrderBy(x => x.Status) : x => x.OrderBy(x => x.Status);
                        break;
                    case "createdate":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.CreateDate)
                                   : x => x.OrderBy(x => x.CreateDate) : x => x.OrderBy(x => x.CreateDate);
                        break;
                    case "updatedate":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.UpdateDate)
                                   : x => x.OrderBy(x => x.UpdateDate) : x => x.OrderBy(x => x.UpdateDate);
                        break;
                    case "description":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Description)
                                   : x => x.OrderBy(x => x.Description) : x => x.OrderBy(x => x.Description);
                        break;
                    case "color":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Color)
                                   : x => x.OrderBy(x => x.Color) : x => x.OrderBy(x => x.Color);
                        break;
                    case "initialprice":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.InitialPrice)
                                   : x => x.OrderBy(x => x.InitialPrice) : x => x.OrderBy(x => x.InitialPrice);
                        break;
                    case "finalprice":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.FinalPrice)
                                   : x => x.OrderBy(x => x.FinalPrice) : x => x.OrderBy(x => x.FinalPrice);
                        break;
                    case "auctionfee":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.AuctionFee)
                                   : x => x.OrderBy(x => x.AuctionFee) : x => x.OrderBy(x => x.AuctionFee);
                        break;
                    case "index":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Index)
                                   : x => x.OrderBy(x => x.Index) : x => x.OrderBy(x => x.Index);
                        break;
                    case "timespan":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.TimeSpan)
                                   : x => x.OrderBy(x => x.TimeSpan) : x => x.OrderBy(x => x.TimeSpan);
                        break;
                    case "minincrement":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.MinIncrement)
                                   : x => x.OrderBy(x => x.MinIncrement) : x => x.OrderBy(x => x.MinIncrement);
                        break;
                    case "fishtypename":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.FishType.FishTypeName)
                                   : x => x.OrderBy(x => x.FishType.FishTypeName) : x => x.OrderBy(x => x.FishType.FishTypeName);
                        break;
                    case "farmname":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Farm.FarmName)
                                   : x => x.OrderBy(x => x.Farm.FarmName) : x => x.OrderBy(x => x.Farm.FarmName);
                        break;
                    case "auctionname":
                        orderBy = !string.IsNullOrEmpty(paginationParameter.Direction)
                                    ? paginationParameter.Direction.ToLower().Equals("desc")
                                   ? x => x.OrderByDescending(x => x.Auction.AuctionName)
                                   : x => x.OrderBy(x => x.Auction.AuctionName) : x => x.OrderBy(x => x.Auction.AuctionName);
                        break;
                    default:
                        orderBy = x => x.OrderBy(x => x.FishId);
                        break;
                }
                string includeProperties = "Auction,Farm,FishType";
                var result = await _unitOfWork.DetailProposalRepository.Get(filter, orderBy, includeProperties, paginationParameter.PageIndex, paginationParameter.PageSize);
                var pagin = new PageEntity<DetailProposalModel>();
                pagin.List = _mappper.Map<IEnumerable<DetailProposalModel>>(result);
                pagin.TotalRecord = await _unitOfWork.DetailProposalRepository.Count();
                pagin.TotalPage = PaginHelper.PageCount(pagin.TotalRecord, paginationParameter.PageSize);
                if (pagin.List.Any())
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagin);
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new PageEntity<DetailProposalModel>());
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAllAuction()
        {
            try
            {
                var listAuctions = await _unitOfWork.DetailProposalRepository.ListAuctions();
                if (listAuctions != null && listAuctions.Any())
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, listAuctions);
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Auction>());
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAllProposal()
        {
            try
            {
                var listProposal = await _unitOfWork.DetailProposalRepository.ListProposals();
                if (listProposal != null && listProposal.Any())
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, listProposal);
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Proposal>());
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAllType()
        {
            try
            {
                var listFishType = await _unitOfWork.DetailProposalRepository.ListFishTypes();
                if (listFishType != null && listFishType.Any())
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, listFishType);
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<FishType>());
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetByID(int id)
        {
            try
            {
                string includeProperties = "Auction,Farm,FishType";
                var result = await _unitOfWork.DetailProposalRepository.GetByCondition(x => x.FishId == id, includeProperties: includeProperties);
                var finalResult = _mappper.Map<DetailProposalModel>(result);
                if (finalResult != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, finalResult);

                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new ProposalModel());
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> Insert(CreateDetailProposalModel entityinsert)
        {
            try
            {
                var data = _mappper.Map<DetailProposal>(entityinsert);
                data.FishCode = "Fish" + Guid.NewGuid().ToString();
                data.CreateDate = DateOnly.FromDateTime(DateTime.Now);
                await _unitOfWork.DetailProposalRepository.Insert(data);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);

                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, result);
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_CREATE_MSG, ex.Message.ToString());
            }
        }

        public async Task<IBusinessResult> Update(int id, UpdateDetailProposalModel entityUpdate)
        {
            try
            {
                var checkExist = await _unitOfWork.DetailProposalRepository.GetByID(id);
                if (checkExist != null)
                {
                    if (!string.IsNullOrEmpty(entityUpdate.FishName))
                    {
                        checkExist.FishName = entityUpdate.FishName;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.Color))
                    {
                        checkExist.Color = entityUpdate.Color;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.Description))
                    {
                        checkExist.Description = entityUpdate.Description;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.Gender))
                    {
                        checkExist.Gender = entityUpdate.Gender;
                    }
                    if (entityUpdate.CreateDate.HasValue)
                    {
                        checkExist.CreateDate = entityUpdate.CreateDate;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.Status))
                    {
                        checkExist.Status = entityUpdate.Status;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.ImageUrl))
                    {
                        checkExist.ImageUrl = entityUpdate.ImageUrl;
                    }
                    if (!string.IsNullOrEmpty(entityUpdate.VideoUrl))
                    {
                        checkExist.VideoUrl = entityUpdate.VideoUrl;
                    }
                    if (entityUpdate.UpdateDate.HasValue)
                    {
                        checkExist.UpdateDate = entityUpdate.UpdateDate;
                    }
                   
                    if (entityUpdate.Age > 0)
                    {
                        checkExist.Age = entityUpdate.Age;
                    }
                    if (entityUpdate.Length > 0)
                    {
                        checkExist.Length = entityUpdate.Length;
                    }
                    if (entityUpdate.Weight > 0)
                    {
                        checkExist.Weight = entityUpdate.Weight;
                    }
                    if (entityUpdate.Rating > 0)
                    {
                        checkExist.Rating = entityUpdate.Rating;
                    }
                    if (entityUpdate.InitialPrice > 0)
                    {
                        checkExist.InitialPrice = entityUpdate.InitialPrice;
                    }
                    if (entityUpdate.FinalPrice > 0)
                    {
                        checkExist.FinalPrice = entityUpdate.FinalPrice;
                    }
                    if (entityUpdate.Index > 0)
                    {
                        checkExist.Index = entityUpdate.Index;
                    }
                    if (entityUpdate.TimeSpan > 0)
                    {
                        checkExist.TimeSpan = entityUpdate.TimeSpan;
                    }
                    if (entityUpdate.MinIncrement > 0)
                    {
                        checkExist.MinIncrement = entityUpdate.MinIncrement;
                    }
                    if (entityUpdate.AuctionFee > 0)
                    {
                        checkExist.AuctionFee = entityUpdate.AuctionFee;
                    }
                    if (entityUpdate.FishTypeId > 0)
                    {
                        checkExist.FishTypeId = entityUpdate.FishTypeId;
                    }
                    if (entityUpdate.FarmId > 0)
                    {
                        checkExist.FarmId = entityUpdate.FarmId;
                    }
                    if (entityUpdate.AuctionId > 0)
                    {
                        checkExist.AuctionId = entityUpdate.AuctionId;
                    }
                    _unitOfWork.DetailProposalRepository.Update(checkExist);
                    var result = await _unitOfWork.SaveAsync();
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, result);
                    }
                }

                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Can not find any detail proposal to update with that id");

            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_UPDATE_MSG, ex.Message.ToString());
            }
        }

        public async Task<IBusinessResult> UploadToFirebase(int type, IFormFile file, int detailProposalId)
        {
            try
            {
                var existDetailProposal = await _unitOfWork.DetailProposalRepository.GetByID(detailProposalId);
                var firebaseStorage = new FirebaseStorage(FirebaseConfig.STORAGE_BUCKET);
                string fileName = Path.GetFileName(file.FileName);
                await firebaseStorage.Child("detailProposal").Child(fileName).PutAsync(file.OpenReadStream());
                var downloadUrl = await firebaseStorage.Child("detailProposal").Child(fileName).GetDownloadUrlAsync();
                if (existDetailProposal != null)
                {
                    if (!string.IsNullOrEmpty(existDetailProposal.ImageUrl) && type == 1)
                    {
                        // Parse the image URL to get the file name
                        var fileNameDelete = existDetailProposal.ImageUrl.Substring(existDetailProposal.ImageUrl.LastIndexOf('/') + 1);
                        fileNameDelete = fileNameDelete.Split('?')[0]; // Remove the query parameters
                        var encodedFileName = Path.GetFileName(fileNameDelete);
                        var fileNameOfficial = Uri.UnescapeDataString(encodedFileName);
                        // Delete the image from Firebase Storage
                        var fileRef = firebaseStorage.Child(fileNameOfficial);
                        await fileRef.DeleteAsync();
                       
                    }
                    if(!string.IsNullOrEmpty(existDetailProposal.VideoUrl) && type == 2)
                    {
                        // Parse the image URL to get the file name
                        var fileNameDelete = existDetailProposal.VideoUrl.Substring(existDetailProposal.VideoUrl.LastIndexOf('/') + 1);
                        fileNameDelete = fileNameDelete.Split('?')[0]; // Remove the query parameters
                        var encodedFileName = Path.GetFileName(fileNameDelete);
                        var fileNameOfficial = Uri.UnescapeDataString(encodedFileName);
                        // Delete the image from Firebase Storage
                        var fileRef = firebaseStorage.Child(fileNameOfficial);
                        await fileRef.DeleteAsync();
                    }
                    if(type == 1)
                    {
                        existDetailProposal.ImageUrl = downloadUrl;
                    }
                    else
                    {
                        existDetailProposal.VideoUrl = downloadUrl;
                    }
                }
                if (downloadUrl != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, downloadUrl);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, downloadUrl);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, Const.FAIL_UPDATE_MSG, ex.Message.ToString());
            }
        }
    }
}
