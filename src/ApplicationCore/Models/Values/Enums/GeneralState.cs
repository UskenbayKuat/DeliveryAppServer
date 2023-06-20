using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Values.Enums
{
    public enum GeneralState
    {
        [Display(Name = "Ожидает заказ")]
        WaitingOrder = 1,
        [Display(Name = "Ожидает рассмотрения")]
        WaitingOnReview = 2,
        [Display(Name = "На рассмотрении")]
        OnReview = 3,
        [Display(Name = "Ожидает передачи")]
        PendingForHandOver = 4,
        [Display(Name = "Передан")]
        ReceivedByDriver = 5,
        [Display(Name = "В пути")]
        InProgress = 6,
        [Display(Name = "Завершен")]
        Done = 7,
        [Display(Name = "Отложенный")]
        Delayed = 8,
        [Display(Name = "Отмененный")]
        Canceled = 9 
    }
}