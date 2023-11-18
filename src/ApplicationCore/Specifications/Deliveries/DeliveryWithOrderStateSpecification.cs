using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithOrderStateSpecification : Specification<Delivery>
    {
        public DeliveryWithOrderStateSpecification(Guid userId)
        {
            Query
                .Include(d => d.State)
                .Include(d => d.Orders).ThenInclude(o => o.State)
                .Where(d => d.Driver.User.Id == userId)
                .Where(d => d.State.StateValue == GeneralState.INPROGRESS ||
                            d.State.StateValue == GeneralState.WAITING_ORDER);
        }
    }
}
