using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class WaitingClientPackage : BaseEntity
    {
        public ClientPackage ClientPackage { get; set;}
    }
}