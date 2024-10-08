using KoiAuction.API.Payloads.Requests.PaymentRequest;
using KoiAuction.API.Payloads.Requests.UserAuctionRequest;
using KoiAuction.BussinessModels.PaymentModels;
using KoiAuction.BussinessModels.UserAuctionModels;
using KoiAuction.Common.Constants;
using KoiAuction.Common.Utils;
using KoiAuction.Common.Utils.Filters;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using KoiAuction.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231.AuctionKoi.API.Payloads;

namespace KoiAuction.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserAuctionController : ControllerBase
    {
        private readonly IUserAuctionService _userAuctionService;

        public UserAuctionController(IUserAuctionService userAuctionService)
        {
            _userAuctionService = userAuctionService;
        }

        //[Authorize(Roles = )]
        [HttpGet(APIRoutes.UserAuction.Get, Name = "GetUserAuctionsAsync")]
        public async Task<IActionResult> GetAllAsync(PaginationParameter paginationParameter, UserAuctionFilters userAuctionFilters)
        {
            try
            {
                var result = await _userAuctionService.Get(paginationParameter, userAuctionFilters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = )]
        [HttpGet(APIRoutes.UserAuction.GetByID, Name = "GetUserAuctionByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string bidId)
        {
            try
            {
                var result = await _userAuctionService.GetByID(bidId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = )]
        [HttpPost(APIRoutes.UserAuction.Create, Name = "CreateUserAuctionAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] UserAuctionRequest reqObj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var insertEntity = new UserAuctionModel
                {
                    BidCode = CodeHelper.GenerateCode(),
                    CreateDate = DateTime.Now,
                    IsWinner = reqObj.IsWinner,
                    Price = reqObj.Price,
                    UserId = reqObj.UserId,
                    FishId = reqObj.FishId,
                    AuctionId = reqObj.AuctionId
                };

                var result = await _userAuctionService.Insert(insertEntity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = )]
        [HttpPut(APIRoutes.UserAuction.Update, Name = "UpdateUserAuctionAsync")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int bidId,
            [FromBody] UserAuctionRequest reqObj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updateEntity = new UserAuctionModel
                {
                    BidId = bidId,
                    IsWinner = reqObj.IsWinner,
                    Price = reqObj.Price,
                    UserId = reqObj.UserId,
                    FishId = reqObj.FishId,
                    AuctionId = reqObj.AuctionId
                };

                var result = await _userAuctionService.Update(updateEntity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = )]
        [HttpDelete(APIRoutes.UserAuction.Delete, Name = "DeleteUserAuctionAsync")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int bidId)
        {
            try
            {
                var result = await _userAuctionService.Delete(bidId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
