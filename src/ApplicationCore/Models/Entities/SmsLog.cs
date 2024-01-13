using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using System;

namespace ApplicationCore.Models.Entities
{
    public class SmsLog : BaseEntity
    {
        public DateTime LifeTime { get; private set; }
        public bool IsActual { get; private set; }
        public int TryCount { get; private set; }
        public int InputCount { get; private set; }
        public string SmsCode { get; private set; }
        public string ContentXml{ get; private set; }
        public User User { get; set; }

        public SmsLog(int minute, User user)
        {
            IsActual = true;
            TryCount = 1;
            InputCount = 0;
            LifeTime = DateTime.Now.AddMinutes(minute);
            User = user;
            SmsCode = GetSmsCode();
        }
        public SmsLog()
        {
            
        }
        public SmsLog SetTryCount(int minute)
        {
            TryCount++;
            InputCount = 0;
            LifeTime = DateTime.Now.AddMinutes(minute);
            return this;
        }

        public SmsLog SetSmsCode(bool isProd = true)
        {
            SmsCode = isProd ? GetSmsCode() : "0000";
            return this;
        }

        public SmsLog SetContentXml(string xml)
        {
            ContentXml = xml;
            return this;
        }

        public SmsLog SetNonActual()
        {
            IsActual = false;
            return this;
        }

        public SmsLog SetInputCount()
        {
            InputCount++;
            return this;
        }

        private string GetSmsCode() =>
            new Random(Guid.NewGuid().GetHashCode()).Next(1000, 10000).ToString();
    }
}
