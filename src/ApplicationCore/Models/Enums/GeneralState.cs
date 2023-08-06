using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Enums
{
    public enum GeneralState
    {
        [Display(Name = "Ожидает заказ")]
        WAITING_ORDER = 1,
        [Display(Name = "Ожидает рассмотрения")]
        WAITING_ON_REVIEW = 2,
        [Display(Name = "На рассмотрении")]
        ON_REVIEW = 3,
        [Display(Name = "Ожидает передачи")]
        PENDING_For_HAND_OVER = 4,
        [Display(Name = "Передан")]
        RECEIVED_BY_DRIVER = 5,
        [Display(Name = "В пути")]
        INPROGRESS = 6,
        [Display(Name = "Завершен")]
        DONE = 7,
        [Display(Name = "Отложенный")]
        DELAYED = 8,
        [Display(Name = "Отмененный")]
        CANCALED = 9,
        [Display(Name = "Ожидание передачу заказа")]
        AWAITING_TRANSFER_TO_CUSTOMER = 10,
        [Display(Name = "Доставлено")]
        DELIVERED = 11,
    }
}