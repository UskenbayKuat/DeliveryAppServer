namespace ApplicationCore.Entities.AppEntities
{
    public class WaitingList : BaseEntity
    {
        public ClientPackage ClientPackage { get; private set;}

        public WaitingList AddClientPackage(ClientPackage clientPackage)
        {
            ClientPackage = clientPackage;
            return this;
        }
    }
}