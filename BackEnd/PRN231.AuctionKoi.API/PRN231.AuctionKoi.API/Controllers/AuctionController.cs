using KoiAuction.Common;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IBusinessResult> GetAllAuctions([FromQuery] string? searchKey, [FromQuery] string? orderBy, [FromQuery] int? pageIndex = null, [FromQuery] int? pageSize = null)
        {
            return await _auctionService.GetAllAuctions(searchKey, orderBy, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetAuctionById(int id)
        {
            return await _auctionService.GetAuctionById(id);
        }

        [HttpPost]
        public async Task<IBusinessResult> CreateAuction([FromBody] Auction auction)
        {
            return await _auctionService.CreateAuction(auction);
        }

        [HttpPut("{id}")]
        public async Task<IBusinessResult> UpdateAuction(int id, [FromBody] Auction auction)
        {
            auction.AuctionId = id; // Ensure the correct ID is set
            return await _auctionService.UpdateAuction(auction);
        }
        [HttpGet("types")]
        public async Task<IBusinessResult> GetAuctionTypes()
        {

            return await _auctionService.GetAuctionTypes();
        }
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteAuction(int id)
        {
            return await _auctionService.DeleteAuction(id);
        }
    }
}
