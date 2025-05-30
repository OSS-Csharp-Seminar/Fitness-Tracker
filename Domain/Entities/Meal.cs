﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Meal 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  
        public int Tags { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }

        public virtual User User { get; set; }
    }
}
