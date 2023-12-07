using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service
{
    public interface IRestrictionService
    {
        IEnumerable<BuildingRestriction> GetBuildingRestriction(int buildingId);
        IEnumerable<RestrictionExcept> GetRestrictionExcepts(int buildingRestrictionId);
        IEnumerable<VideoType> GetVideoExcept(int buildingRestrictionId);
        BuildingRestriction AddBuildingRestriction(BuildingRestriction buildingRestriction);
        BuildingRestriction UpdateBuildingRestriction(BuildingRestriction buildingRestriction);
        void DeleteRestriction(int id);
        void UpdateRestrictionExcept(VideoType[] videoTypes, int buildingRestrictionId);    
        BuildingRestriction GetBuildingRestrictionById(int id);    
    }
}