using AutoMapper;
using ApplicationCore.Entities.Values;
using PublicApi.Endpoints.Clients.Order;
using PublicApi.Endpoints.Delivery;
using PublicApi.Endpoints.Drivers.Car;
using PublicApi.Endpoints.Drivers.RouteTrip;
using PublicApi.Endpoints.RegisterApi.ConfirmRegister;
using PublicApi.Endpoints.RegisterApi.ProceedRegister;
using PublicApi.Endpoints.RegisterApi.Register;

namespace PublicApi
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterCommand, RegistrationInfo>();
            CreateMap<ConfirmRegisterCommand, ConfirmRegistrationInfo>();
            CreateMap<ProceedRegisterCommand, ProceedRegistrationInfo>();
            CreateMap<RouteTripCommand, RouteTripInfo>();
            CreateMap<OrderCommand, OrderInfo>();
            CreateMap<CarCommand, CarInfo>();
            CreateMap<DeliveryCommand, OrderInfo>();
        }   
    }
}