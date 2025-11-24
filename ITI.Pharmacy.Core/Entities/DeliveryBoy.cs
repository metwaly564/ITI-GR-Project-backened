namespace ITI.Pharmacy.Core.Entities
{
    public class DeliveryBoy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string VehicleNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
