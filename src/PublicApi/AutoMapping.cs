using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using AutoMapper;
using ApplicationCore.Entities.Values;
using PublicApi.Endpoints.Clients.CalculateOrder;
using PublicApi.Endpoints.Clients.ClientPackage;
using PublicApi.Endpoints.Drivers.CreateCar;
using PublicApi.Endpoints.Drivers.CreateRouteTrip;
using PublicApi.Endpoints.Orders;
using PublicApi.Endpoints.RegisterApi.ConfirmRegister;
using PublicApi.Endpoints.RegisterApi.ProceedRegisterDriver;
using PublicApi.Endpoints.RegisterApi.Register;
using PublicApi.Endpoints.Shared.RefreshToken;

namespace PublicApi
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //TODO add forMember option for all maps
            CreateMap<RegisterCommand, RegistrationInfo>();
            CreateMap<ConfirmRegisterCommand, ConfirmRegistrationInfo>();
            CreateMap<ProceedRegisterCommand, ProceedRegistrationInfo>();
            CreateMap<CreateRouteTripCommand, RouteInfo>();
            CreateMap<CalculateOrderCommand, ClientPackageInfo>();
            CreateMap<ClientPackageCommand, ClientPackageInfo>();
            CreateMap<CreateCarCommand, CreateCarInfo>();
            CreateMap<OrderCommand, ClientPackageInfo>();
            CreateMap<ClientPackage, OrderInfo>().ForMember(o => o.OrderState, 
                    o => o.MapFrom(c => c.Order.OrderState.ToString()))
                .ForMember(o => o.Package, 
                    o => o.MapFrom(c => c.Package))
                .ForMember(o => o.Price, 
                    o => o.MapFrom(c => c.Price))
                .ForMember(o => o.IsSingle, 
                    o => o.MapFrom(c => c.IsSingle))
                .ForMember(o => o.StartCity, 
                    o => o.MapFrom(c => c.Route.StartCity))
                .ForMember(o => o.FinishCity, 
                    o => o.MapFrom(c => c.Route.FinishCity))
                .ForMember(o => o.CreatedAt, 
                    o => o.MapFrom(c => c.CreatedAt));;
            CreateMap<ClientPackage, ClientPackageInfo>()
                .ForMember(o => o.ClientPackageId, 
                    o => o.MapFrom(c => c.Id))
                .ForMember(o => o.Package, 
                    o => o.MapFrom(c => c.Package))
                .ForMember(o => o.Price, 
                    o => o.MapFrom(c => c.Price))
                .ForMember(o => o.IsSingle, 
                    o => o.MapFrom(c => c.IsSingle))
                .ForMember(o => o.StartCity, 
                    o => o.MapFrom(c => c.Route.StartCity))
                .ForMember(o => o.FinishCity, 
                    o => o.MapFrom(c => c.Route.FinishCity))
                .ForMember(o => o.CreateAt, 
                    o => o.MapFrom(c => c.CreatedAt))
                .ForMember(o => o.CarType, 
                    o => o.MapFrom(c => c.CarType));
            CreateMap<RouteTrip, RouteTripInfo>()
                .ForMember(o => o.StartCity,
                    o => o.MapFrom(c => c.Route.StartCity))
                .ForMember(o => o.FinishCity,
                    o => o.MapFrom(c => c.Route.FinishCity))
                .ForMember(o => o.DeliveryDate,
                    o => o.MapFrom(c => c.CreatedAt));
            CreateMap<RefreshRequest, RefreshTokenInfo>()
                .ForMember(o => o.RefreshToken, 
                    o => o.MapFrom(r => r.RefreshToken));
        }   
    }
}