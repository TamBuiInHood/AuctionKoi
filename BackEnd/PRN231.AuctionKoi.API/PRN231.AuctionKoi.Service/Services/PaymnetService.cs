using AutoMapper;
using KoiAuction.Common;
using KoiAuction.Common.Constants;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using KoiAuction.BussinessModels.Pagination;
using Microsoft.IdentityModel.Tokens;
using PRN231.AuctionKoi.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.BussinessModels.PaymentModels;
using KoiAuction.Repository.Entities;
using KoiAuction.Common.Utils;


namespace KoiAuction.Service.Services
{
    public class PaymnetService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymnetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IBusinessResult> Delete(int id)
        {
            var paymnet = await _unitOfWork.PaymentRepository.GetByCondition(x => x.PaymentId == id);
            if (paymnet == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            _unitOfWork.PaymentRepository.Delete(paymnet);
            var result = await _unitOfWork.SaveAsync() > 0 ? true : false;
            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, true);
        }

        public async Task<IBusinessResult> Get(string? searchKey, string? orderBy, int pageIndex = PageDefault.PAGE_INDEX, int pageSize = PageDefault.PAGE_SIZE)
        {
            try
            {
                Expression<Func<Payment, bool>> filter = null!;
                Func<IQueryable<Payment>, IOrderedQueryable<Payment>> sortBy = null!;
                var validInt = 0;
                var validDate = DateTime.Now;
                if (int.TryParse(searchKey, out validInt))
                {
                    //filter = x => x.OrderId == validInt || x.TransactionId == validInt;
                }
                else if (DateTime.TryParse(searchKey, out validDate))
                {
                    filter = x => x.PaymentDate == validDate;
                }
                else if (!string.IsNullOrEmpty(searchKey))
                {
                    filter = x => x.Status!.ToLower().Contains(searchKey!.ToLower());
                }
                switch (orderBy)
                {
                    case "PaymentId":
                        sortBy = x => x.OrderByDescending(a => a.PaymentId);

                        break;
                    case "PaymentAmount":
                        sortBy = x => x.OrderByDescending(a => a.PaymentAmount);

                        break;
                    case "PaymentDate":
                        sortBy = x => x.OrderByDescending(x => x.PaymentDate);
                        break;
                    case "Status":
                        sortBy = x => x.OrderByDescending(x => x.Status);
                        break;
                    default:
                        sortBy = x => x.OrderByDescending(a => a.PaymentId);
                        break;
                }

                string includeProperties = "Order";
                var result = await _unitOfWork.PaymentRepository.Get(filter!, sortBy, includeProperties, pageIndex, pageSize);
                var pagin = new PageEntity<PaymentModel>();
                pagin.List = _mapper.Map<IEnumerable<PaymentModel>>(result);
                pagin.TotalRecord = await _unitOfWork.ProposalRepository.Count();
                pagin.TotalPage = PaginHelper.PageCount(pagin.TotalRecord, pageSize);
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagin);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetByID(string? idKey)
        {
            var id = 0;
            Expression<Func<Payment, bool>> filter = null!;
            if (int.TryParse(idKey, out id))
            {
                filter = x => x.PaymentId == id || x.OrderId == id;
            }
            else
            {
                //filter = x => x.TransactionId == id;
            }
            string includeProperties = "Order";
            var payment = await _unitOfWork.PaymentRepository.GetByCondition(filter, includeProperties);
            if (payment == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, payment);

        }

        public async Task<IBusinessResult> Insert(PaymentModel entityInsert)
        {
            entityInsert.PaymentDate = DateTime.Now;
            var mapEntity = _mapper.Map<Payment>(entityInsert);
            await _unitOfWork.PaymentRepository.Insert(mapEntity);
            var result = await _unitOfWork.SaveAsync() > 0 ? true : false;
            if (result)
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        }

        public async Task<IBusinessResult> Update(PaymentModel entityUpdate)
        {
            Expression<Func<Payment, bool>> filter = x => x.PaymentId == entityUpdate.PaymentId
                                                            || x.OrderId == entityUpdate.OrderId
                                                            || x.TransactionId == entityUpdate.TransactionId;
            var entity = await _unitOfWork.PaymentRepository.GetByCondition(filter);
            if (entity == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            if (entityUpdate.PaymentAmount.HasValue)
            {
                entity.PaymentAmount = entityUpdate.PaymentAmount.Value;
            }
            if (!entityUpdate.Status.IsNullOrEmpty())
            {
                entity.Status = entityUpdate.Status;
            }
            if (entityUpdate.PaymentDate.HasValue)
            {
                entity.PaymentDate = entityUpdate.PaymentDate.Value;
            }
            if (!entityUpdate.PaymentMethod.IsNullOrEmpty())
            {
                entity.PaymentMethod = entityUpdate.PaymentMethod;
            }
            _unitOfWork.PaymentRepository.Update(entity);
            var result = await _unitOfWork.SaveAsync() > 0 ? true : false;
            if (result)
            {
                return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        }
    }
}
