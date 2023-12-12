using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service
{
    public interface IRestrictionService
    {
        IEnumerable<Restriction> GetBuildingRestriction(int buildingId);
        IEnumerable<Category> GetRestrictionExcepts(int restrictionId);
        void DeleteRestriction(int id);
        Restriction GetRestrictionById(int id);
        void AddRestriction(Restriction restriction );
        void UpdateRestriction(Restriction restriction );
    }
}