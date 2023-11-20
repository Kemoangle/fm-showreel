using Showreel.Models;

namespace Showreel.DTO
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public string? BuildingName { get; set; }

        public string? Address { get; set; }

        public string? District { get; set; }

        public string? Remark { get; set; }

        public DateOnly? CreateTime { get; set; }

        public DateOnly? LastUpdateTime { get; set; }

        public string? Zone { get; set; }
        public IEnumerable<Restriction> Restrictions { get; set; }
    }
}
