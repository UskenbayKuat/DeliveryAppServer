﻿using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class Client : BaseEntity
    {
        public User User { get; set;}
        public Guid UserId { get; set; }
    }
}