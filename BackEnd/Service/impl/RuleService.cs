using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;
using Showreel.Service;

namespace BackEnd.Service.impl
{
    public class RuleService : IRuleService
    {
        private readonly ShowreelContext _context;
        private readonly IBuildingService _buildingService;
        private readonly IVideoCategoryService _categoryService;
        public RuleService(ShowreelContext context,IBuildingService buildingService,IVideoCategoryService categoryService)
        {   
            _context = context;
            _buildingService = buildingService;
            _categoryService = categoryService;
        }

        public IEnumerable<Building> GetDoNotPlay(int videoId)
        {
            var doNotPlayId = _context.Rules.Where(r => r.VideoId == videoId && r.DoNotPlay > 0).Select(r => r.DoNotPlay).ToList();
            var query = _context.Buildings
                        .Where(b => doNotPlayId.Contains(b.Id))
                        .ToList();
            return query;
        }

        public IEnumerable<Category> GetNoBackToback(int videoId)
        {
            var noBackToBackId = _context.Rules.Where(r => r.VideoId == videoId && r.NoBackToBack > 0).Select(r => r.NoBackToBack).ToList();
            var query = _context.Categories
                        .Where(b => noBackToBackId.Contains(b.Id))
                        .ToList();
            return query;
        }

        public void UpdateDoNotPlay(int videoId, Building[] buildings)
        {
            var doNotPlayExist = _context.Rules
                                            .Where(v => v.VideoId == videoId && v.DoNotPlay > 0)
                                            .ToList();
            if (doNotPlayExist.Any())
            {
                _context.Rules.RemoveRange(doNotPlayExist);
            }

            foreach (var building in buildings)
            {
                var newRule = new Rule
                {
                    VideoId = videoId,
                    DoNotPlay = building.Id
                };

                _context.Rules.Add(newRule);
            }
            _context.SaveChanges();
        }

        public void UpdateNoBackToBack(int videoId, Category[] categories)
        {
            var noBackToBackExist = _context.Rules
                                            .Where(v => v.VideoId == videoId && v.NoBackToBack > 0)
                                            .ToList();
            if (noBackToBackExist.Any())
            {
                _context.Rules.RemoveRange(noBackToBackExist);
            }

            foreach (var category in categories)
            {
                var newRule = new Rule
                {
                    VideoId = videoId,
                    NoBackToBack = category.Id
                };

                _context.Rules.Add(newRule);
            }
            _context.SaveChanges();
        }
    }
}