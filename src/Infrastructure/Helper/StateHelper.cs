
using System.Linq;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values.Enums;
using Infrastructure.AppData.DataAccess;


namespace Infrastructure.Helper
{
    public class StateHelper
    {
        private readonly AppDbContext _db;

        public StateHelper(AppDbContext db)
        {
            _db = db;
        }
        public State FindState(int id) =>
             _db.States.First(s => s.Id == id);
    }
}