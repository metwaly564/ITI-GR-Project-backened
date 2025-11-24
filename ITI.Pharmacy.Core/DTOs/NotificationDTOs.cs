namespace ITI.Pharmacy.Core.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
    }
}
