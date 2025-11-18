namespace ITI.Pharmacy.API.DTOs
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<ZoneDTO> Zones { get; set; } = new();
    }

    public class ZoneDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryFee { get; set; }
        public bool IsActive { get; set; }
    }
}