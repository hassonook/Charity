namespace Charity.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public LocationType LocationType { get; set; }
        public int? ParentId { get; set; }
        public Location? Parent { get; set; }

    }

    public enum LocationType
    {
        Country,
        Locality,
        City,
        Area
    }
}
