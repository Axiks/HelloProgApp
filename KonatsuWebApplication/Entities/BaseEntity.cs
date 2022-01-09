﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonatsuWebApplication.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
