using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Service.ISerivice;
using KoiAuction.Service.Responses;
using Microsoft.AspNetCore.Mvc;
using PRN231.AuctionKoi.API.Payloads;

namespace KoiAuction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet(APIRoutes.Auction.GetAll, Name = "Get All Auctions")]
        public async Task<IActionResult> GetAllAuctions([FromQuery] string? searchKey, [FromQuery] string? orderBy, [FromQuery] int? pageIndex = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                var result = await _auctionService.GetAllAuctions(searchKey, orderBy, pageIndex, pageSize);
                if (result != null)
                {
                    return Ok(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Get All Auctions Success",
                        Data = result,
                        IsSuccess = true
                    });
                }
                return NotFound(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Cannot Get Any Auction",
                    IsSuccess = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    IsSuccess = false
                });
            }
        }
        [HttpGet(APIRoutes.Auction.Type, Name = "Get AuctionType")]
        public async Task<IActionResult> GetAuctionTypes()
        {
            try
            {
                var result = await _auctionService.GetAuctionTypes();
                if (result != null && result.Any())
                {
                    return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Auction types retrieved successfully",
                        Data = result,
                        IsSuccess = true
                    });
                }

                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No auction types found",
                    IsSuccess = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    IsSuccess = false
                });
            }
        }

        [HttpPost(APIRoutes.Auction.Create, Name = "Create Auction")]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionModel model)
        {
            if (model == null)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Auction model cannot be null",
                    Data = null,
                    IsSuccess = false
                });
            }

            try
            {
                var createdAuction = await _auctionService.CreateAuction(model);
                return CreatedAtAction(nameof(GetAuctionById), new { id = createdAuction.AuctionId }, new BaseResponse()
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Auction created successfully",
                    Data = createdAuction,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = null,
                    IsSuccess = false
                });
            }
        }

        [HttpGet("{id:int}", Name = "Get Auction By Id")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            try
            {
                var auction = await _auctionService.GetAuctionById(id);
                if (auction == null)
                {
                    return NotFound(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Auction not found",
                        Data = null,
                        IsSuccess = false
                    });
                }
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Auction retrieved successfully",
                    Data = auction,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = null,
                    IsSuccess = false
                });
            }
        }

        [HttpPut("{id:int}", Name = "Update Auction")]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] AuctionUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Auction model cannot be null",
                    Data = null,
                    IsSuccess = false
                });
            }

            try
            {
                model.AuctionId = id; // Set the ID for the auction being updated
                var updatedAuction = await _auctionService.UpdateAuction(model);
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Auction updated successfully",
                    Data = updatedAuction,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = null,
                    IsSuccess = false
                });
            }
        }

        [HttpDelete("{id:int}", Name = "Delete Auction")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                var result = await _auctionService.DeleteAuction(id);
                if (!result)
                {
                    return NotFound(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Cannot delete this auction",
                        Data = null,
                        IsSuccess = false
                    });
                }
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Auction deleted successfully",
                    Data = null,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = null,
                    IsSuccess = false
                });
            }
        }
    }

}
