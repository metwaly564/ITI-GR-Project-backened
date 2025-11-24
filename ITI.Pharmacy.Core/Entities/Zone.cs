namespace ITI.Pharmacy.Core.Entities
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public decimal DeliveryFee { get; set; }
        public bool IsActive { get; set; } = true;


        public virtual City City { get; set; }
    }
}
