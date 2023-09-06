using System;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithStateSpecification : Specification<Order>
    {
        public OrderWithStateSpecification(int routeId, DateTime date)
        {
            Query.Include(o => o.State)
                .Where(o =>
                    !o.IsDeleted &&
                    o.Route.Id == routeId &&
                    o.DeliveryDate <= date &&
                    o.State.StateValue == GeneralState.WAITING_ON_REVIEW);
        }
        public OrderWithStateSpecification(int orderId)
        {
            Query.Include(o => o.State)
                .Where(o => o.Id == orderId && !o.IsDeleted);
        }
        public OrderWithStateSpecification(int orderId, string userId)
        {
            Query.Include(o => o.State)
                .Include(o => o.Client)
                .Where(o => o.Id == orderId && !o.IsDeleted && o.Delivery.Driver.UserId == userId);
        }
        public OrderWithStateSpecification(int orderId, GeneralState state)
        {
            Query.Include(o => o.State)
                .Where(o => o.Id == orderId && !o.IsDeleted && o.State.StateValue == state);
        }
        public OrderWithStateSpecification(string userId)
        {
            Query.Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.Delivery)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.Client)
                .Include(o => o.Delivery.Driver.Car)
                .Include(o => o.Delivery.State)
                .Include(o => o.Location)
                .Where(o => o.Client.UserId == userId && !o.IsDeleted)
                .Where(o =>
                    o.State.StateValue == GeneralState.WAITING_ON_REVIEW ||
                    o.State.StateValue == GeneralState.ON_REVIEW ||
                    o.State.StateValue == GeneralState.PENDING_For_HAND_OVER ||
                    o.State.StateValue == GeneralState.AWAITING_TRANSFER_TO_CUSTOMER ||
                    o.State.StateValue == GeneralState.DELIVERED ||
                    o.State.StateValue == GeneralState.RECEIVED_BY_DRIVER);
        }
    }
}