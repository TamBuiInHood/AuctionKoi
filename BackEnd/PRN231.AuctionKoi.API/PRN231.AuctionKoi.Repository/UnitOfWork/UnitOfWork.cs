using PRN231.AuctionKoi.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using KoiAuction.Repository.Entities;
using KoiAuction.Repository.Repositories;
using KoiAuction.Repository.IRepositories;

namespace PRN231.AuctionKoi.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private Fa24Se1716Prn231G5KoiauctionContext _context;

        private PaymentRepository _paymentRepo;
        private ProposalRepository _proposalRepo;
        private UserAuctionRepository _userAuctionRepo;
        private AutionRepository _auctionRepo;
        private AuctionTypeRepository _auctionTypeRepo;
        //private GenericRepository<Category> _categoryRepo;

        public UnitOfWork(Fa24Se1716Prn231G5KoiauctionContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public PaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepo == null)
                {
                    _paymentRepo = new PaymentRepository(_context);
                }
                return _paymentRepo;
            }
        }

        public ProposalRepository ProposalRepository
        {
            get
            {
                if (_proposalRepo == null)
                {
                    _proposalRepo = new ProposalRepository(_context);
                }
                return _proposalRepo;
            }
        }

        public AutionRepository AuctionRepository
        {
            get
            {
                return _auctionRepo ??= new AutionRepository(_context);
            }
        }

        public UserAuctionRepository UserAuctionRepository
        {
            get
            {
                if (_userAuctionRepo == null)
                {
                    _userAuctionRepo = new UserAuctionRepository(_context);
                }
                return _userAuctionRepo;
            }
        }

        public AuctionTypeRepository AuctionTypeRepository
        {
            get
            {
                if (_auctionTypeRepo == null)
                {
                    _auctionTypeRepo = new AuctionTypeRepository(_context);
                }
                return _auctionTypeRepo;
            }
        }

        // GenericRepository<Category> IUnitOfWork.CategoryRepository
        // {
        //     get
        //     {
        //         if (_categoryRepo == null)
        //         {
        //             _categoryRepo = new GenericRepository<Category>(_context);
        //         }
        //         return _categoryRepo;
        //     }
        // }
    }
}
