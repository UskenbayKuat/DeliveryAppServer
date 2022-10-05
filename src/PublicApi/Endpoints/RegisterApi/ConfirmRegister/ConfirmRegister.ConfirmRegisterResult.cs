﻿namespace PublicApi.Endpoints.RegisterApi.ConfirmRegister
{
    public class ConfirmRegisterResult : ConfirmRegisterCommand
    {
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}