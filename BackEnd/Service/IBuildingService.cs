using Showreel.Models;

namespace Showreel.Service
{
    public interface IBuildingService
    {    
        IEnumerable<Building> GetAllBuildings(string keySearch = "");
        Building GetBuildingById(int id);
        void AddBuilding(Building building);
        void UpdateBuilding(Building building);
        void DeleteBuilding(int id);
    }
}
