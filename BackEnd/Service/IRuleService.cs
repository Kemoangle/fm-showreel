using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service
{
    public interface IRuleService
    {
        IEnumerable<Building> GetDoNotPlay(int videoId);
        IEnumerable<Category> GetNoBackToback(int videoId);
        void UpdateDoNotPlay(int videoId, Building[] buildings);
        void UpdateNoBackToBack(int videoId, Category[] buildings);

    }
}