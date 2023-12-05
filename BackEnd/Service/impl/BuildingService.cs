﻿using Microsoft.EntityFrameworkCore;
using Showreel.Models;

namespace Showreel.Service.impl
{
    public class BuildingService : IBuildingService
    {
        private readonly ShowreelContext _context;
        public BuildingService(ShowreelContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetAllBuildings(string keySearch = "", bool isGetLandlord = false)
        {
            var query = _context.Buildings.AsQueryable();
            if (isGetLandlord)
            {
                var newQuery = query
                .Select(x => new Building
                {
                    Id = x.Id,
                    BuildingName = x.BuildingName,
                    Address = x.Address,
                    Zone = x.Zone,
                    Landlordads = x.Landlordads,
                    PostalCode = x.PostalCode,
                    District = x.District
                });

                return newQuery;
            }

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.BuildingName.Contains(keySearch));
            }
            query = query.OrderByDescending(b => b.Id);

            return query.ToList();
        }

        public Building GetBuildingById(int id)
        {
            return _context.Buildings.FirstOrDefault((Building b) => b.Id == id);
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
    }
}
