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

        public IEnumerable<Building> GetAllBuildings(string keySearch = "")
        {
            var query = _context.Buildings.AsQueryable();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.BuildingName.Contains(keySearch));
            }
            query = query.OrderByDescending(b => b.Id);

            return query.ToList();
        }

        public Building GetBuildingById(int id)
        {
            return _context.Buildings.SingleOrDefault((Building b) => b.Id == id);
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

        public IEnumerable<Building> GetBuildings()
        {
            return _context.Buildings.ToList();
        }
    }
}
