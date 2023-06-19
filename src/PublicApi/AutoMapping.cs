using AutoMapper;
using ApplicationCore.Entities.Values;
using PublicApi.Commands;
using PublicApi.Endpoints.Drivers.Car;
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
            CreateMap<RegisterCommand, RegistrationInfo>();
            CreateMap<ConfirmRegisterCommand, ConfirmRegistrationInfo>();
            CreateMap<ProceedRegisterCommand, ProceedRegistrationInfo>();
            CreateMap<RefreshCommand, RefreshTokenInfo>();
            CreateMap<CreateDeliveryCommand, RouteTripInfo>();
            CreateMap<CarCommand, CarInfo>();
            CreateMap<CreateOrderCommand, OrderInfo>();
            CreateMap<ConfirmHandOverCommand, ConfirmHandOverInfo>();
        }   
    }
}