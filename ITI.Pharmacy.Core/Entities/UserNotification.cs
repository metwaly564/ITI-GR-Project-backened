namespace ITI.Pharmacy.Core.Entities
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public string Type { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}
