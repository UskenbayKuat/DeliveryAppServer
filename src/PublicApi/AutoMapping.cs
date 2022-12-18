using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using AutoMapper;
using ApplicationCore.Entities.Values;
using PublicApi.Endpoints.Clients.Order;
using PublicApi.Endpoints.Delivery;
using PublicApi.Endpoints.Drivers.Car;
using PublicApi.Endpoints.Drivers.RouteTrip;
using PublicApi.Endpoints.RegisterApi.ConfirmRegister;
using PublicApi.Endpoints.RegisterApi.ProceedRegister;
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
            CreateMap<RouteTripCommand, RouteTripInfo>();
            CreateMap<OrderCommand, OrderInfo>();
            CreateMap<CarCommand, CarInfo>();
            CreateMap<DeliveryCommand, OrderInfo>();
            CreateMap<Order, DeliveryInfo>()
                .ForMember(o => o.Package, 
                    o => o.MapFrom(o => o.Package))
                .ForMember(o => o.Price, 
                    o => o.MapFrom(o => o.Price))
                .ForMember(o => o.IsSingle, 
                    o => o.MapFrom(o => o.IsSingle))
                .ForMember(o => o.StartCity, 
                    o => o.MapFrom(o => o.Route.StartCity))
                .ForMember(o => o.FinishCity, 
                    o => o.MapFrom(o => o.Route.FinishCity))
                .ForMember(o => o.DeliveryState, 
                    o => o.MapFrom(o => o.DeliveryDate))
                .ForMember(o => o.StateName, 
                    o => o.MapFrom(o => o.State.Name))
                .ForMember(o => o.CreatedAt, 
                    o => o.MapFrom(o => o.CreatedAt));

            CreateMap<Order, OrderInfo>()
                .ForMember(o => o.OrderId,
                    o => o.MapFrom(o => o.Id))
                .ForMember(o => o.Package,
                    o => o.MapFrom(o => o.Package))
                .ForMember(o => o.Price,
                    o => o.MapFrom(o => o.Price))
                .ForMember(o => o.IsSingle,
                    o => o.MapFrom(o => o.IsSingle))
                .ForMember(o => o.StartCity,
                    o => o.MapFrom(o => o.Route.StartCity))
                .ForMember(o => o.FinishCity,
                    o => o.MapFrom(o => o.Route.FinishCity))
                .ForMember(o => o.DeliveryDate,
                    o => o.MapFrom(o => o.DeliveryDate))
                .ForMember(o => o.CarType,
                    o => o.MapFrom(o => o.CarType))
                .ForMember(o => o.StateName,
                    o => o.MapFrom(o => o.State.Name));
            CreateMap<RouteTrip, RouteTripInfo>()
                .ForMember(r => r.StartCity,
                    r => r.MapFrom(r => r.Route.StartCity))
                .ForMember(r => r.FinishCity,
                    r => r.MapFrom(r => r.Route.FinishCity))
                .ForMember(r => r.DeliveryDate,
                    r => r.MapFrom(r => r.DeliveryDate));
            CreateMap<RefreshRequest, RefreshTokenInfo>()
                .ForMember(r => r.RefreshToken, 
                    r => r.MapFrom(r => r.RefreshToken));
        }   
    }
}