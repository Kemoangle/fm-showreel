using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service.impl
{
    public class RestrcitionService : IRestrictionService
    {
        private readonly ShowreelContext context;
        public RestrcitionService(ShowreelContext _context)
        {
            context = _context;
        }

        public BuildingRestriction AddBuildingRestriction(BuildingRestriction buildingRestriction)
        {
            context.Add(buildingRestriction);
            context.SaveChanges();
            return buildingRestriction;
        }

        public void DeleteRestriction(int id)
        {
            var restrictionDelete = context.BuildingRestrictions.Find(id);
            if (restrictionDelete != null)
            {
                context.BuildingRestrictions.Remove(restrictionDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<BuildingRestriction> GetBuildingRestriction(int buildingId)
        {
            return context.BuildingRestrictions.Where(b => b.BuildingId == buildingId).ToList();
        }

        public BuildingRestriction GetBuildingRestrictionById(int id)
        {
            return context.BuildingRestrictions.FirstOrDefault(br => br.Id == id);
        }

        public IEnumerable<RestrictionExcept> GetRestrictionExcepts(int buildingRestrictionId)
        {
            return context.RestrictionExcepts.Where(r => r.BuildingRestrictionId == buildingRestrictionId).ToList();
        }

        public IEnumerable<VideoType> GetVideoExcept(int buildingRestrictionId)
        {
            var query = from br in context.BuildingRestrictions.Where(b => b.Id == buildingRestrictionId)
                        join rx in context.RestrictionExcepts on br.Id equals rx.BuildingRestrictionId
                        join vt in context.VideoTypes on rx.VideoTypeId equals vt.Id
                        select new VideoType{
                            Id = vt.Id,
                            Name = vt.Name
                        };
            return query.ToList();
        }

        public BuildingRestriction UpdateBuildingRestriction(BuildingRestriction buildingRestriction)
        {
            context.Update(buildingRestriction);
            context.SaveChanges();
            return buildingRestriction;
        }

        public void UpdateRestrictionExcept(VideoType[] videoTypes, int buildingRestrictionId)
        {
             var existingExcept = context.RestrictionExcepts
                                    .Where(v => v.BuildingRestrictionId == buildingRestrictionId)
                                    .ToList();
            if (existingExcept.Any())
            {
                context.RestrictionExcepts.RemoveRange(existingExcept);
            }
            foreach (var item in videoTypes)
            {
                var newExcept = new RestrictionExcept
                {
                    VideoTypeId = item.Id,
                    BuildingRestrictionId = buildingRestrictionId
                };
                context.RestrictionExcepts.Add(newExcept);
            }
            context.SaveChanges();
        }
    }
}