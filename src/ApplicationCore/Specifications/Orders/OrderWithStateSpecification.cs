using System;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithStateSpecification : Specification<Order>
    {
        public OrderWithStateSpecification(Guid routeId, DateTime date)
        {
            Query.Include(o => o.State)
                .Where(o =>
                    !o.IsDeleted &&
                    o.Route.Id == routeId &&
                    o.DeliveryDate <= date &&
                    o.State.StateValue == GeneralState.WAITING_ON_REVIEW);
        }
        public OrderWithStateSpecification(Guid orderOrUserId, bool isOrderId)
        {
            if (isOrderId)
            {
                Query.Include(o => o.State)
                .Where(o => o.Id == orderOrUserId && !o.IsDeleted);
            }
            else
            {
                Query.Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.Delivery)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.Delivery.Driver.User)
                .Include(o => o.Client)
                .Include(o => o.Client.User)
                .Include(o => o.Delivery.Driver.Cars)
                .Include(o => o.Delivery.State)
                .Include(o => o.Location)
                .Where(o => o.Client.User.Id == orderOrUserId && !o.IsDeleted)
                .Where(o =>
                    o.State.StateValue == GeneralState.WAITING_ON_REVIEW ||
                    o.State.StateValue == GeneralState.ON_REVIEW ||
                    o.State.StateValue == GeneralState.PENDING_For_HAND_OVER ||
                    o.State.StateValue == GeneralState.AWAITING_TRANSFER_TO_CUSTOMER ||
                    o.State.StateValue == GeneralState.DELIVERED ||
                    o.State.StateValue == GeneralState.RECEIVED_BY_DRIVER);
            }
        }
        public OrderWithStateSpecification(Guid orderId, Guid userId)
        {
            Query.Include(o => o.State)
                .Include(o => o.Client).ThenInclude(o => o.User)
                .Where(o => o.Id == orderId && !o.IsDeleted && o.Delivery.Driver.User.Id == userId);
        }
        public OrderWithStateSpecification(Guid orderId, GeneralState state)
        {
            Query.Include(o => o.State)
                .Where(o => o.Id == orderId && !o.IsDeleted && o.State.StateValue == state);
        }
    }
}