namespace PRN231.AuctionKoi.API.Payloads
{
    public static class APIRoutes
    {
        public const string Base = "auction-koi";

        public static class AuctionKoi
        {
            public const string GetAll = Base + "/weatherforecast/";
        }

        public static class Authentication
        {
            public const string Login = Base + "/authentication/login";

            public const string LoginMobile = Base + "/authentication/login-mobile";

            public const string RefreshToken = Base + "/authentication/refresh-token";

        }

        public static class Paymnet
        {
            public const string Get = Base + "/paymnets/";

            public const string GetByID = Base + "/paymnets/{search-id}";

            public const string Update = Base + "/paymnets/{payment-id}";

            public const string Delete = Base + "/paymnets/{payment-id}";

            public const string Create = Base + "/paymnets/";

            public const string GetNoPagin = Base + "/paymnets/";

        }

        public static class Proposal
        {
            public const string Get = Base + "/proposals/";

            public const string GetOData = Base + "/proposals/use-odata";

            public const string Create = Base + "/proposals/create-proposal";

            public const string GetByID = Base + "/proposals/{proposal-id}";

            public const string Update = Base + "/proposals/{proposal-id}";

            public const string Delete = Base + "/proposals/{proposal-id}";

            public const string GetNoPagin = Base + "/proposals/get-no-paging";

            public const string GetUser = Base + "/proposals/user";

            public const string UploadToFirebase = Base + "/proposals/upload";

        }

        public static class UserAuction
        {
            public const string Get = Base + "/userAuctions/";

            public const string GetByID = Base + "/userAuctions/{bidId}";

            public const string Create = Base + "/userAuctions/";

            public const string Update = Base + "/userAuctions/{bidId}";

            public const string Delete = Base + "/userAuctions/{bidId}";

        }

        public static class DetailProposal
        {
            public const string Get = Base + "/detailProposal/";

            public const string GetOData = Base + "/detailProposal/use-odata";

            public const string Create = Base + "/detailProposal/create-detail-proposal";

            public const string GetByID = Base + "/detailProposal/{detail-proposal-id}";

            public const string Update = Base + "/detailProposal/{detail-proposal-id}";

            public const string Delete = Base + "/detailProposal/{detail-proposal-id}";

            public const string GetNoPagin = Base + "/detailProposal/get-no-paging";

            public const string GetListAuction = Base + "/detailProposal/auction";

            public const string GetListDetailProposalType = Base + "/detailProposal/type";

            public const string GetProposal = Base + "/detailProposal/proposal";

            public const string UploadToFirebase = Base + "/detailProposals/upload";


        }
        public static class Order
        {
            public const string Get = Base + "/orders/";
            public const string Create = Base + "/orders/create-order";
            public const string GetByID = Base + "/orders/{search-id}";
            public const string Update = Base + "/orders/{update-order}";
            public const string Delete = Base + "/orders/{order-id}";
        }


    }
}
