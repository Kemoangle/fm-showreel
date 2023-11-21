using Microsoft.EntityFrameworkCore;
using Showreel.DTO;
using Showreel.Models;

namespace Showreel.Service.impl
{
    public class BuildingService : IBuildingService
    {
        private readonly ShowreelContext _context;
        private readonly IRestrictionService _restrictionService;
        public BuildingService(ShowreelContext context, IRestrictionService restrictionService)
        {
            _context = context;
            _restrictionService = restrictionService;
        }

        public IEnumerable<Building> GetAllBuildings(string keySearch = "")
        {
            var query = _context.Buildings.AsQueryable();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.BuildingName.Contains(keySearch));
            }
            return query.ToList();
        }

        public Building GetBuildingById(int id)
        {
            return _context.Buildings.FirstOrDefault((Building b) =>b.Id == id);
        }

        public void AddBuilding(Building building)
        {
            _context.Buildings.Add(building);
            _context.SaveChanges();
        }

        public void UpdateBuilding(Building building)
        {
            _context.Update(building);
            _context.SaveChanges();
        }

        public void DeleteBuilding(int id)
        {
            var buildingToDelete = _context.Buildings.Find(id);
            if (buildingToDelete != null)
            {
                _context.Buildings.Remove(buildingToDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<BuildingDTO> GetAllBuilding()
        {
            var query = from b in _context.Buildings
                             select new BuildingDTO
                             {
                                 Id = b.Id,
                                 BuildingName = b.BuildingName,
                                 Address = b.Address,
                                 District = b.District,
                                 Remark = b.Remark,
                                 CreateTime = b.CreateTime,
                                 LastUpdateTime = b.LastUpdateTime,
                                 Zone = b.Zone,
                                 PostalCode = b.PostalCode,
                                 Restrictions = _restrictionService.GetRestrictionByBuildingId(b.Id)
                             };
    
            return query.ToList();
        }

        public void AddBuildingRestriction(Buildingrestriction buildingRestriction)
        {
            _context.Buildingrestrictions.Add(buildingRestriction);
            _context.SaveChanges();
        }
    }
}
