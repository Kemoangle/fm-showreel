using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service.impl
{
    public class LandlordAdsService : ILanlordAdsService
    {
        private readonly ShowreelContext _context;
        public LandlordAdsService(ShowreelContext context)
        {
            _context = context;
        }
        public void AddLandlordAds(Landlordad landlordad)
        {
            _context.Landlordads.Add(landlordad);
            _context.SaveChanges();
        }

        public void DeleteLandlordAds(int id)
        {
            var landlordadDelete = _context.Landlordads.Find(id);
            if (landlordadDelete != null)
            {
                _context.Landlordads.Remove(landlordadDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Landlordad> GetAllLandlordAds(int id)
        {
            var query = _context.Landlordads.Where(l => l.BuildingId == id);
            return query.ToList();
        }

        public IEnumerable<Landlordad> GetLandlordAdsBuilding(int id)
        {
            var query = _context.Landlordads.Where(l => l.BuildingId == id);
            return query.ToList();
        }

        public Landlordad GetLandlordad(int id)
        {
            return _context.Landlordads.FirstOrDefault(v => v.Id == id);
        }

        public void UpdateLandlordAds(Landlordad landlordad)
        {
            _context.Update(landlordad);
            _context.SaveChanges();
        }


    }
}