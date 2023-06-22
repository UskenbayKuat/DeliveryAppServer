using AutoMapper;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Values;
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
            CreateMap<CreateDeliveryCommand, CreateDeliveryDto>();
            CreateMap<CarCommand, CreateCarDto>();
            CreateMap<CreateOrderCommand, CreateOrderDto>();
            CreateMap<ConfirmHandOverCommand, ConfirmHandOverDto>();
        }   
    }
}