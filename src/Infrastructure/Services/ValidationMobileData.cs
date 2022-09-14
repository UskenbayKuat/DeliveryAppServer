using System;
using System.Text.RegularExpressions;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ValidationMobileData : IValidation
    {
        private string _smsCodePattern = @"[0-9]{4}";
        private string _phoneNumberPattern = @"^\+?[78][-\(]?\d{3}\)?-?\d{3}-?\d{2}-?\d{2}$";
        private int _smsCodeLength = 4;
        
        public bool ValidationMobileNumber(string phoneNumber) => new Regex(_phoneNumberPattern).IsMatch(phoneNumber);
        public bool ValidationCode(string code) => new Regex(_smsCodePattern).IsMatch(code) && code.Length == _smsCodeLength;
        public bool ValidationDate(DateTime dateTime) => dateTime >= DateTime.Today;

    }
}