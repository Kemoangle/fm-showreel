using Showreel.Models;

namespace Showreel.Service
{
    public interface IRestrictionService
    {
        IEnumerable<Restriction> GetAllRestrictions();
        Restriction GetRestrictionById(int id);
        void AddRestriction(Restriction restriction);
        void UpdateRestriction(Restriction restriction);
        void DeleteRestriction(int id);
        IEnumerable<Restriction> GetRestrictionByBuildingId(int id);
    }
}
