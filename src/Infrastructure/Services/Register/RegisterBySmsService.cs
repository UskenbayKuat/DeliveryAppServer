using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Entities;
using ApplicationCore.Models.Entities.Dictionaries;
using ApplicationCore.Models.Values;
using ApplicationCore.Specifications.Sms;
using ApplicationCore.Specifications.Users;
using Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Register
{

    public class RegisterBySmsService : IRegistration
    {
        private readonly IGenerateToken _generateToken;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<DicSmsLog> _dicSmsRepository;
        private readonly IAsyncRepository<SmsLog> _smsRepository;
        private readonly SmsOptions _smsOptions;
        public RegisterBySmsService(
            IGenerateToken generateToken,
            IAsyncRepository<User> userRepository,
            IAsyncRepository<DicSmsLog> dicSmsRepository,
            IAsyncRepository<SmsLog> smsRepository,
            IOptions<SmsOptions> options)
        {
            _generateToken = generateToken;
            _userRepository = userRepository;
            _dicSmsRepository = dicSmsRepository;
            _smsRepository = smsRepository;
            _smsOptions = options.Value;
        }
        public async Task RegisterAsync(RegistrationDto dto)
        {
            var userSpec = new UserForRegisterSpecification(dto.PhoneNumber, dto.IsDriver);
            var user = await _userRepository.FirstOrDefaultAsync(userSpec) ?? await AddUserAsync(dto);
            await SendMessageAsync(user);
        }

        public async Task<ConfirmRegistrationDto> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken)
        {
            var smsSpec = new SmsForUserSpecification(dto.PhoneNumber, dto.IsDriver);
            var smsLog = await _smsRepository.FirstOrDefaultAsync(smsSpec, cancellationToken)
                ?? throw new NotExistUserException("Пользователь не найден");

            var dicSms = await _dicSmsRepository.FirstOrDefaultAsync(x => !x.IsDeleted);

            if (smsLog.LifeTime < DateTime.Now)
                throw new ArgumentException(dicSms.LifeTimeErrorMessage);

            if (dicSms.InputCount <= smsLog.InputCount)
                throw new ArgumentException(dicSms.InputCountMessage);

            if (smsLog.SmsCode == dto.SmsCode)
            {
                smsLog.User = smsLog.User.AddRefreshToken(
                    _generateToken.CreateRefreshToken(),
                    _generateToken.GetLifeTimeRefreshToken());

                await _smsRepository.UpdateAsync(smsLog.SetNonActual().SetInputCount());
            }
            else
            {
                await _smsRepository.UpdateAsync(smsLog.SetInputCount());
                throw new ArgumentException(dicSms.CodeErrorMessage);
            }

            return new()
            {
                AccessToken = _generateToken.CreateAccessToken(smsLog.User),
                RefreshToken = smsLog.User.RefreshToken,
                UserName = smsLog.User.UserName,
                Surname = smsLog.User.Surname,
                IsValid = smsLog.User.IsValid,
                Email = smsLog.User.Email
            };
        }

        private async Task SendMessageAsync(User user)
        {

            var dicSms = await _dicSmsRepository.FirstOrDefaultAsync(x => !x.IsDeleted);
            var smsLog = await _smsRepository.FirstOrDefaultAsync(new SmsForUserSpecification(user.Id));

            if (smsLog == null)
            {
                smsLog = new(minute: dicSms.MinuteLife,
                             user: user);
                await _smsRepository.AddAsync(smsLog);

                await SendSmsAsync(smsLog, dicSms);
                return;
            }

            if (smsLog.LifeTime > DateTime.Now)
                return;

            smsLog = dicSms.TryCount > smsLog.TryCount
                ? await _smsRepository.UpdateAsync(smsLog.SetTryCount(dicSms.MinuteLife).SetSmsCode())
                : throw new ArgumentException(dicSms.TryCountMessage);

            await SendSmsAsync(smsLog, dicSms);
        }

        private async Task SendKazInfoTehkzAsync(SmsLog smsLog, DicSmsLog dicSms)
        {
            var smsMessageForUser = string.Format(dicSms.Message, smsLog.SmsCode);
            var query = _smsOptions.GetQuery(smsLog.User.PhoneNumber, smsMessageForUser);

            using var client = new HttpClient()
            {
                BaseAddress = new Uri(_smsOptions.Url)
            };
            var _ = await client.GetAsync(query);
            var content = await _.Content.ReadAsStringAsync();
            await _smsRepository.UpdateAsync(smsLog.SetContentXml(content));

            if (!_.IsSuccessStatusCode)
            {
                throw new ArgumentException(dicSms.KazInfoErrorMessage);
            }
        }

        private Task SendSmsAsync(SmsLog smsLog, DicSmsLog dicSms) =>
            _smsOptions.IsProd
                    ? SendKazInfoTehkzAsync(smsLog, dicSms)
                    : _smsRepository.UpdateAsync(smsLog.SetSmsCode(isProd: false));

        private Task<User> AddUserAsync(RegistrationDto dto) => 
            _userRepository.AddAsync(new User(dto.PhoneNumber, dto.IsDriver));


    }
}