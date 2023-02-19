using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryQuery
    {
        public Task<ActionResult> GetOnReviewOrdersForDriverAsync(string userDriverId);
        public Task<ActionResult> GetActiveOrdersForDriverAsync(string userDriverId);
        public Task<ActionResult> GetDeliveryIsActiveAsync(string driverUserId);
    }
}