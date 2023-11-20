using Showreel.DTO;
using Showreel.Models;

namespace Showreel.Service
{
    public interface IBuildingService
    {
        IEnumerable<Building> GetAllBuildings();
        Building GetBuildingById(int id);
        void AddBuilding(Building building);
        void UpdateBuilding(Building building);
        void DeleteBuilding(int id);
        IEnumerable<BuildingDTO> GetAllBuilding();
        void AddBuildingRestriction(Buildingrestriction buildingrestriction);
    }
}
