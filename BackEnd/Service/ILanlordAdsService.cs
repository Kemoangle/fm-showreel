using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service
{
    public interface ILanlordAdsService
    {
        IEnumerable<Landlordad> GetAllLandlordAds(int id);
        IEnumerable<Video> GetLandlordAdsBuilding(int id);
        void AddLandlordAds(Landlordad landlordad);
        void UpdateLandlordAds(Landlordad landlordad);
        void DeleteLandlordAds(int id);
        Landlordad GetLandlordad(int id);
    }
}