
namespace Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }  
        public Guid SenderId { get; set; }
        public Guid TrainerId { get; set; }
        public string MessageText { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;  

        public virtual User Sender { get; set; }
        public virtual User Trainer { get; set; }
    }
}