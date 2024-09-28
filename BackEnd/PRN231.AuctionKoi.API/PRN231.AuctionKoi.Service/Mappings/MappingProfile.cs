using AutoMapper;
using KoiAuction.BussinessModels.PaymentModels;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.BussinessModels.UserAuctionModels;
using KoiAuction.Repository.Entities;

namespace KoiAuction.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Proposal, ProposalModel>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(x => x.User.FullName))
                //.ForMember(dest => dest.DetailProposals, opt => opt.MapFrom(x => x.DetailProposals))
                .ReverseMap();
            CreateMap<Payment, PaymentModel>()
                //.ForMember(dest => dest.Order, opt => opt.MapFrom(x => x.Order))
                .ReverseMap();
            CreateMap<Auction, AuctionModel>()
              .ReverseMap();

            CreateMap<UserAuction, UserAuctionModel>()
               .ForMember(dest => dest.FishCode, opt => opt.MapFrom(x => x.Fish.FishCode))
               .ForMember(dest => dest.FishName, opt => opt.MapFrom(x => x.Fish.FishName))
               .ForMember(dest => dest.FishTypeName, opt => opt.MapFrom(x => x.Fish.FishType.FishTypeName))
               .ForMember(dest => dest.FarmName, opt => opt.MapFrom(x => x.Fish.Farm!.FarmName))
               .ForMember(dest => dest.UserCode, opt => opt.MapFrom(x => x.User.UserCode))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.User.FullName))
               .ForMember(dest => dest.Mail, opt => opt.MapFrom(x => x.User.Mail));
            CreateMap<UserAuctionModel, UserAuction>()
                .ForMember(dest => dest.Fish, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
        }
    }
}
