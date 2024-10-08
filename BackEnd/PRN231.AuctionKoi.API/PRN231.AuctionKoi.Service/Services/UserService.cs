using AutoMapper;
using KoiAuction.BussinessModels.UserAuctionModels;
using KoiAuction.BussinessModels.UserModels;
using KoiAuction.Common;
using KoiAuction.Common.Utils;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using PRN231.AuctionKoi.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Service.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IBusinessResult> CheckLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new BusinessResult(Const.WARNING_INVALID_LOGIN_CODE, Const.WARNING_INVALID_LOGIN_MSG);
            }
            Expression<Func<User, bool>> filter = null!;
            filter = x => x.Mail!.Equals(email) && x.Password!.Equals(PasswordHelper.ConvertToEncrypt(password));
            var userEntity = await _unitOfWork.UserRepository.GetByCondition(filter);
            if (userEntity == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<UserModel>(userEntity));
        }
    }
}
