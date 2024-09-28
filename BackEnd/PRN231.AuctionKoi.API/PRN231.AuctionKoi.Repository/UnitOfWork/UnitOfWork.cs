using PRN231.AuctionKoi.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using KoiAuction.Repository.Entities;
using KoiAuction.Repository.Repositories;


namespace PRN231.AuctionKoi.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private Fa24Se1716Prn231G5KoiauctionContext _context;

        private PaymentRepository _paymentRepo;
        private ProposalRepository _proposalRepo;
        private UserAuctionRepository _userAuctionRepo;
        private AutionRepository _AuctionRepository;
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
                    this._paymentRepo = new PaymentRepository(_context);
                }
                return _paymentRepo;
            }
        }

        public ProposalRepository ProposalRepository
        {
            get
            {
                if(_proposalRepo == null)
                {
                    this._proposalRepo = new ProposalRepository(_context);
                }
                return _proposalRepo;
            }
        }
        public AutionRepository AuctionRepository  
        {
            get
            {
                return _AuctionRepository ??= new AutionRepository(_context);
            }
        }
        public UserAuctionRepository UserAuctionRepository
        {
            get
            {
                if (_userAuctionRepo == null)
                {
                    this._userAuctionRepo = new UserAuctionRepository(_context);
                }
                return _userAuctionRepo;
            }
        }

        //GenericRepository<Category> IUnitOfWork.CategoryRepository
        //{
        //    get
        //    {
        //        if (_categoryRepo == null)
        //        {
        //            this._categoryRepo = new GenericRepository<Category>(_context);
        //        }
        //        return _categoryRepo;
        //    }
        //}

    }
}
