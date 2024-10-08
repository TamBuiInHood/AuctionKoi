﻿using KoiAuction.Repository.Repositories;
using PRN231.AuctionKoi.Repository.Repositories;


namespace PRN231.AuctionKoi.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Task<int> SaveAsync();
        //public GenericRepository<Category> CategoryRepository { get; }
        public PaymentRepository PaymentRepository{ get; }
        public OrderRepository OrderRepository{ get; }
        public ProposalRepository ProposalRepository{ get; }
        public UserAuctionRepository UserAuctionRepository { get; }
        public UserRepository UserRepository { get; }
        public DetailProposalRepository DetailProposalRepository { get; }



    }
}
