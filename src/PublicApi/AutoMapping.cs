using AutoMapper;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
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
            CreateMap<RegisterCommand, Driver>();
            CreateMap<Driver, RegisterDriverResult>();
            CreateMap<ConfirmRegisterCommand, Driver>();
            CreateMap<Driver, ConfirmRegisterResult>();
            CreateMap<RegisterCommand, RegistrationToken>();
            CreateMap<ConfirmRegisterCommand, ConfirmRegistrationToken>();
            CreateMap<ProceedRegisterCommand, ProceedRegistrationInfo>();
            CreateMap<CreateRouteTripCommand, RouteInfo>();
            CreateMap<CalculateOrderCommand, ClientPackageInfo>();
            CreateMap<ConfirmOrderCommand, ClientPackageInfo>();
            CreateMap<RouteInfo, CreateRouteTripResult>();
            CreateMap<CreateCarCommand, CreateCarInfo>();
            CreateMap<CreateCarInfo, CreateCarResult>();


            CreateMap<RefreshRequest, RefreshTokenInfo>()
                .ForMember("AccessToken", opt => opt.MapFrom(o => o.AccessToken))
                .ForMember("RefreshToken", opt => opt.MapFrom(o => o.RefreshToken));
        }
    }
}