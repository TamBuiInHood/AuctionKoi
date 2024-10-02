using KoiAuction.Repository.Entities;
using KoiAuction.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using PRN231.AuctionKoi.Repository.IRepositories;
using PRN231.AuctionKoi.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Repository.Repositories
{
   

    public class AutionRepository : GenericRepository<Auction>, IAutionRepository
    {
        private readonly Fa24Se1716Prn231G5KoiauctionContext _context;
        public AutionRepository(Fa24Se1716Prn231G5KoiauctionContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Auction>> GetAll()
        {
            return await _context.Auctions
                          .Include(a => a.Type) 
                          .ToListAsync();
        }
    

        public async Task<Auction> GetByCondition(System.Linq.Expressions.Expression<Func<Auction, bool>> predicate)
        {
            return await _context.Auctions.FirstOrDefaultAsync(predicate);
        }

        public async Task InsertAsync(Auction entity)
        {
            await _context.Auctions.AddAsync(entity);
        }

        public async Task UpdateAsync(Auction entity)
        {
            _context.Auctions.Update(entity);
        }

        public async Task DeleteAsync(Auction entity)
        {
            _context.Auctions.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Any(System.Linq.Expressions.Expression<Func<Auction, bool>> predicate)
        {
            return _context.Auctions.Any(predicate);
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            return await _context.Auctions.Include(a => a.Type).ToListAsync();
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.AuctionId == id);
        }

        public async Task AddAuctionAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuctionAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuctionAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AuctionType>> GetAuctionTypes()
        {
            return await _context.AuctionTypes.ToListAsync();
        }
        public bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.AuctionId == id);
        }
    }
}
