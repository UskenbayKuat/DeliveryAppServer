using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using AutoMapper;
using ApplicationCore.Entities.Values;
using PublicApi.Endpoints.Clients.CalculateOrder;
using PublicApi.Endpoints.Clients.ClientPackage;
using PublicApi.Endpoints.Clients.Order;
using PublicApi.Endpoints.Delivery;
using PublicApi.Endpoints.Drivers.Car;
using PublicApi.Endpoints.Drivers.RouteTrip;
using PublicApi.Endpoints.Orders;
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
            CreateMap<CalculateOrderCommand, OrderInfo>();
            CreateMap<OrderCommand, OrderInfo>();
            CreateMap<CarCommand, CarInfo>();
            CreateMap<DeliveryCommand, OrderInfo>();
            CreateMap<Order, DeliveryInfo>()
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
                    o => o.MapFrom(c => c.CreatedAt));
            
            //TODO .ForMember(o => o.DeliveryState, o => o.MapFrom(c => c.Delivery.State))
            
            CreateMap<Order, OrderInfo>()
                .ForMember(o => o.OrderId, 
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
                .ForMember(o => o.DeliveryDate, 
                    o => o.MapFrom(c => c.DeliveryDate))
                .ForMember(o => o.CarType, 
                    o => o.MapFrom(c => c.CarType));
            CreateMap<RouteTrip, RouteTripInfo>()
                .ForMember(o => o.StartCity,
                    o => o.MapFrom(c => c.Route.StartCity))
                .ForMember(o => o.FinishCity,
                    o => o.MapFrom(c => c.Route.FinishCity))
                .ForMember(o => o.DeliveryDate,
                    o => o.MapFrom(c => c.DeliveryDate));
            CreateMap<RefreshRequest, RefreshTokenInfo>()
                .ForMember(o => o.RefreshToken, 
                    o => o.MapFrom(r => r.RefreshToken));
        }   
    }
}