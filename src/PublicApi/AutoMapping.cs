using AutoMapper;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using PublicApi.Commands;
using PublicApi.Commands.Deliveries;
using PublicApi.Commands.Orders;
using PublicApi.Commands.Register;
using PublicApi.Endpoints.Shared.RefreshToken;

namespace PublicApi
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterCommand, RegistrationDto>();
            CreateMap<ConfirmRegisterCommand, ConfirmRegistrationDto>();
            CreateMap<ProceedRegisterCommand, ProceedRegistrationDto>();
            CreateMap<RefreshCommand, RefreshTokenDto>();
            CreateMap<CreateDeliveryCommand, CreateDeliveryDto>();
            CreateMap<CreateCarCommand, CreateCarDto>();
            CreateMap<CreateOrderCommand, CreateOrderDto>();
            CreateMap<ConfirmHandOverCommand, ConfirmHandOverDto>();
        }   
    }
}