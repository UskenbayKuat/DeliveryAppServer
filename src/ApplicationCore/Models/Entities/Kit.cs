using ApplicationCore.Models.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Kit : BaseEntity
    {
        public Kit(KitEnum kitEnum, int quantity, double price, bool isUnlimited = false)
        {
            Name = kitEnum.ToString();
            KitName = kitEnum.ToString();
            KitValue = kitEnum;
            Quantity = quantity;
            IsUnlimited = isUnlimited;
            Price = price;
        }
        public Kit()
        {
        }
        public string Name { get; private set; }
        public KitEnum KitValue { get; private set; }
        public string KitName { get; private set; }
        public int Quantity { get; private set; }
        public bool IsUnlimited { get; private set; }
        public double Price { get; private set; }
    }
}