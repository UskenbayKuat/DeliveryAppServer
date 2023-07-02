namespace ApplicationCore.Entities.AppEntities
{
    public class Kit : BaseEntity
    {
        public Kit(int id, string name, int quantity, bool isUnlimited, double price)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            IsUnlimited = isUnlimited;
            Price = price;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public bool IsUnlimited { get; private set; }
        public double Price { get; private set; }
    }
}