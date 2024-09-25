﻿using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Service.Base;
using KoiAuction.Service.ISerivice;
using Microsoft.AspNetCore.Mvc;
using PRN231.AuctionKoi.Common.Utils;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalsController : ControllerBase
    {
        //private readonly AuctionKoiOfficialContext _context;

        private readonly IProposalService _proposalService;

        public ProposalsController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        // GET: api/Proposals
        [HttpGet]
        public async Task<IBusinessResult> GetProposals(PaginationParameter paginationParameter)
        {
            return await _proposalService.Get(paginationParameter);
        }

        // GET: api/Proposals/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetProposal(int id)
        {
            var proposal = await _proposalService.GetByID(id);
            return proposal;
        }

        // PUT: api/Proposals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutProposal([FromRoute] int id, UpdateProposalModel updateProposalModel)
        {
            return await _proposalService.Update(id,updateProposalModel);
        }

        // POST: api/Proposals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostProposal(CreateProposalModel createProposalModel)
        {
            return  await _proposalService.Insert(createProposalModel);
        }

        // DELETE: api/Proposals/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteProposal(int id)
        {
            return await (_proposalService.Delete(id));
        }
    }
}
