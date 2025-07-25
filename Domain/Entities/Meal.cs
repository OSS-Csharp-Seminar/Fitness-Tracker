﻿
namespace Domain.Entities
{
    public class Meal 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  
        public int Tags { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}
