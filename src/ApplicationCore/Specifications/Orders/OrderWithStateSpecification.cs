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
                    o.State.StateValue == GeneralState.WaitingOnReview);
        }
        public OrderWithStateSpecification(int orderId)
        {
            Query.Include(o => o.State)
                .Where(o => o.Id == orderId);
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
                    o.State.StateValue == GeneralState.WaitingOnReview ||
                    o.State.StateValue == GeneralState.OnReview ||
                    o.State.StateValue == GeneralState.PendingForHandOver ||
                    o.State.StateValue == GeneralState.ReceivedByDriver);
        }
    }
}