namespace ApplicationCore.Entities.AppEntities
{
    public class Kit : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsUnlimited { get; set; }
    }
}