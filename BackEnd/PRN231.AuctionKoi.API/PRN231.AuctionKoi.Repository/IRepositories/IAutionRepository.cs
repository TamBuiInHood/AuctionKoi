using KoiAuction.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Repository.IRepositories
{
    public interface IAutionRepository
    {
        Task<IEnumerable<Auction>> GetAllAuctionsAsync();
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task AddAuctionAsync(Auction auction);
        Task UpdateAuctionAsync(Auction auction);
        Task DeleteAuctionAsync(int id);
        bool AuctionExists(int id);
        Task<IEnumerable<AuctionType>> GetAuctionTypes();
    }
}
