using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.ISerivice;
using KoiAuction.Service.Base;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.BussinessModels.DetailProposalModel;
using KoiAuction.Service.Services;
using PRN231.AuctionKoi.API.Payloads;
using KoiAuction.Common.Utils;
using Microsoft.AspNetCore.OData.Query;
using KoiAuction.Common;

namespace KoiAuction.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DetailProposalsController : ControllerBase
    {
        //private readonly Fa24Se1716Prn231G5KoiauctionContext _context;
        private readonly IDetailProposalService _detailProposalService;

        public DetailProposalsController(IDetailProposalService detailProposalService)
        {
            _detailProposalService = detailProposalService;
        }

        // GET: api/DetailProposals
        [EnableQuery]
        [HttpGet(APIRoutes.DetailProposal.Get, Name = "Get All Detail Proposal")]
        public async Task<IBusinessResult> GetDetailProposals(PaginationParameter paginationParameter)
        {
            return await _detailProposalService.Get(paginationParameter);
        }

        [EnableQuery]
        [HttpGet(APIRoutes.DetailProposal.GetOData, Name = "Get All Detail Proposal by Odata")]
        public async Task<IActionResult> GetProposalsByOData(PaginationParameter paginationParameter)
        {
            var proposals = await _detailProposalService.Get(paginationParameter);
            var queryableData = ODataResultConverter.ConvertDetailProposalToQueryable(proposals);
            return Ok(queryableData);
        }
        // GET: api/DetailProposals/5
        [HttpGet(APIRoutes.DetailProposal.GetByID, Name = "Get Detail Proposal By id")]
        public async Task<IBusinessResult> GetDetailProposal([FromRoute(Name = "detail-proposal-id")] int id)
        {
            var proposal = await _detailProposalService.GetByID(id);
            return proposal;

            //var detailProposal = await _context.DetailProposals.FindAsync(id);

            //if (detailProposal == null)
            //{
            //    return NotFound();
            //}

            //return detailProposal;
        }

        // PUT: api/DetailProposals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(APIRoutes.DetailProposal.Update, Name = "Update Detail Proposal")]
        public async Task<IBusinessResult> PutDetailProposal([FromRoute(Name = "detail-proposal-id")] int id, UpdateDetailProposalModel updateDetailProposalModel)
        {
            return await _detailProposalService.Update(id, updateDetailProposalModel);

            //if (id != detailProposal.FishId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(detailProposal).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!DetailProposalExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/DetailProposals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(APIRoutes.DetailProposal.Create, Name = "Create Detail Proposal")]
        public async Task<IBusinessResult> PostDetailProposal(CreateDetailProposalModel createDetailProposal)
        {
            return await _detailProposalService.Insert(createDetailProposal);
        }

        // DELETE: api/DetailProposals/5
        [HttpDelete(APIRoutes.DetailProposal.Delete, Name = "Delete Detail Proposal")]
        public async Task<IBusinessResult> DeleteDetailProposal([FromRoute(Name = "detail-proposal-id")] int id)
        {
            return await _detailProposalService.Delete(id);
        }

        // GET: api/Proposals
        [HttpGet(APIRoutes.DetailProposal.GetListDetailProposalType, Name = "Get Detail Proposal Type")]
        public async Task<IBusinessResult> GetAllDetailProposalsType()
        {
            return await _detailProposalService.GetAllType();
        }

        // GET: api/Proposals
        [HttpGet(APIRoutes.DetailProposal.GetListAuction, Name = "Get Detail Proposal Auction")]
        public async Task<IBusinessResult> GetDetailProposalAuction()
        {
            return await _detailProposalService.GetAllAuction();
        }

        // GET: api/Proposals
        [HttpGet(APIRoutes.DetailProposal.GetProposal, Name = "Get All Proposal For Detail Proposal")]
        public async Task<IBusinessResult> GetAllProposals()
        {
            return await _detailProposalService.GetAllProposal();
        }

        [HttpPost(APIRoutes.DetailProposal.UploadToFirebase, Name = "Upload File to Firebase Of DetailProposal")]
        public async Task<IBusinessResult> UploadToFirebase([FromQuery(Name = "detailProposalId")] int detailProposalId,
            [FromQuery(Name = "type")] int type,
            IFormFile file)
        {
            return await _detailProposalService.UploadToFirebase(type,file, detailProposalId);
        }

    }
}
