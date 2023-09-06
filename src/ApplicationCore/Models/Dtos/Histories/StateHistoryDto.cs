using System;

namespace ApplicationCore.Models.Dtos.Histories
{
    public class StateHistoryDto
    {
        public string PendingForHandOver { get; set; } = string.Empty;
        public string ReceivedByDriver { get; set; } = string.Empty;
        public string Delivered { get; set; } = string.Empty;
    }
}