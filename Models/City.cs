namespace ITI.Pharmacy.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
    }
}