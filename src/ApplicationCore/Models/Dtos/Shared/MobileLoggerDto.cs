﻿namespace ApplicationCore.Models.Dtos.Shared
{
    public class MobileLoggerDto
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}