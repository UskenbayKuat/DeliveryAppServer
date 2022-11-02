using AutoMapper;
using ApplicationCore.Entities.ApiEntities;
using PublicApi.Endpoints.Clients.CalculateOrder;
using PublicApi.Endpoints.Clients.ConfirmOrder;
using PublicApi.Endpoints.Drivers.CreateCar;
using PublicApi.Endpoints.Drivers.CreateRouteTrip;
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
            CreateMap<ConfirmOrderCommand, ClientPackageInfo>();
            CreateMap<CreateCarCommand, CreateCarInfo>();

            CreateMap<RefreshRequest, RefreshTokenInfo>()
                .ForMember("RefreshToken", opt => opt.MapFrom(o => o.RefreshToken));
        }
    }
}