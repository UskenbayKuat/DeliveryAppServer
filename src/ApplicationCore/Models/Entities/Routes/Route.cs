using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class Route : BaseEntity
    {
        public Route(Guid startCityId, Guid finishCityId)
        {
            StartCityId = startCityId;
            FinishCityId = finishCityId;
        }
        
        public Guid StartCityId { get; private set; }

        public City StartCity { get; set;}
        public Guid FinishCityId { get; private set; }
        
        public City FinishCity { get; set;}
    }
}